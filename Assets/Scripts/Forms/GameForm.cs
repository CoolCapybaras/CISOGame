using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CISOServer.Net.Packets.Clientbound;
using CISOServer.Net.Packets.Serverbound;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameForm : MonoBehaviour, IForm
{
    public static GameForm Instance;
    public PlayerLayout playerLayout;
    private Lobby _currentLobby;

    private int localClientId;
    [Serializable]
    public struct Form
    {
        public GameObject localClientObj;

        public RectTransform tableArea;
        public GameObject cardPrefab;
        public Transform localCardsParent;

        public Sprite cardBack;

        public GameObject smallCardPrefab;

        public GameObject startGameButton;

        public Color32 defaultPlayerBorderColor;
        public Color32 greenPlayerBorderColor;

        public GameObject gameButtons;
    }
    
    public Form form;

    private List<Card> _clientCards = new();

    private Dictionary<int, GameObject> _clientObjects = new();

    private int _lastTurnId = -1;

    private bool _waitForPlayerPick;
    private DraggableCard _currentDroppedCard;
    
    public void OnActive()
    {
        gameObject.SetActive(true);
        Initialize();
    }

    public void OnDisable()
    {
       gameObject.SetActive(false);
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Initialize()
    {
        form.startGameButton.SetActive(_currentLobby.Players.Count == 1);
        RecreatePlayers();
    }

    public void OnClientJoinedPacket(ClientJoinedPacket packet)
    {
        _currentLobby.Players.Add(packet.client);
        RecreatePlayers();
    }
    
    public void OnClientLeavedPacket(ClientLeavedPacket packet)
    {
        _currentLobby.Players.RemoveAll(x => x.Id == packet.clientId);
        RecreatePlayers();
    }

    private void RecreatePlayers()
    {
        _clientObjects.Clear();
        playerLayout.SetupPlayers(_currentLobby.MaxClients);
        var players = _currentLobby.Players;

        var playerObjects = playerLayout.players;

        if (players.Exists(x => x.Id == localClientId))
            players.RemoveAll(x => x.Id == localClientId);

        var localObj = form.localClientObj.transform;
        
        StartCoroutine(Utils.LoadTexture(GameManager.localClient.Avatar, localObj.GetChild(0).GetComponent<RawImage>()));
        localObj.GetChild(1).GetComponent<TMP_Text>().text = GameManager.localClient.Name;
        
        _clientObjects.Add(localClientId, localObj.gameObject);
        
        for (int i = 0; i < players.Count; ++i)
        {
            var player = players[i];
            var obj = playerObjects[i];

            obj.GetComponent<PlayerObject>().clientId = player.Id;
            _clientObjects.Add(player.Id, obj);
            obj.GetComponent<Image>().sprite = null;
            StartCoroutine(Utils.LoadTexture(player.Avatar, obj.transform.GetChild(0).GetComponent<RawImage>()));
            obj.transform.GetChild(0).GetComponent<RawImage>().DOFade(1, 0.25f);
            obj.transform.GetChild(1).GetComponent<TMP_Text>().text = player.Name;
        }
    }

    public void OnLobbyJoined(LobbyJoinedPacket packet)
    {
        FormManager.Instance.ChangeForm("game");
        GameManager.isInLobby = true;
        _currentLobby = packet.lobby;
        localClientId = packet.clientId;
    }

    public void OnStartGamePressed()
    {
        ClientSocket.Instance.SendPacket(new StartGamePacket());
    }

    public void OnGameStarted()
    {
        form.startGameButton.SetActive(false);
    }

    public void OnSyncHandPacket(SyncHandPacket packet)
    {
        _clientCards.Clear();
        _clientCards.AddRange(packet.cards);
    }

    public void OnClientsGotCards(ClientsGotCardsPacket packet)
    {
        CoroutineManager.Instance.StartCoroutine(InstantiateCards(packet.clientIds));
    }

    private IEnumerator InstantiateCards(List<int> clientIds)
    {
        var localClientCardIdx = 0;
        foreach (var id in clientIds)
        {
            if (id == localClientId)
            {
                InstantiateLocalCard(_clientCards[localClientCardIdx]);
                localClientCardIdx++;
                yield return new WaitForSeconds(0.25f);
                continue;
            }
            
            InstantiateSmallCard(_clientObjects[id]);
            yield return new WaitForSeconds(0.25f);
        }
    }

    private void InstantiateLocalCard(Card card)
    {
        var obj = Instantiate(form.cardPrefab, form.localCardsParent);

        var cardObj = CardManager.Instance.GetCard(card.Type);

        obj.GetComponent<Image>().sprite = cardObj.cardFront;
        form.localCardsParent.gameObject.GetComponent<CardHandLayout>().UpdateLayout();
        obj.GetComponent<DraggableCard>().tableArea = form.tableArea;
        obj.GetComponent<DraggableCard>().card = card;
    }

    public void OnCardDropped(DraggableCard card)
    {
        _waitForPlayerPick = true;
        _currentDroppedCard = card;

        card.transform.parent = form.tableArea;
        card.canDrag = false;
        SetCardsDraggable(false);
    }

    public void OnPlayerPressed(int playerId)
    {
        if (_waitForPlayerPick)
        {
            _waitForPlayerPick = false;
            ClientSocket.Instance.SendPacket(new GameActionPacket(GameAction.PlayCard, _currentDroppedCard.card, playerId));
        }
    }

    public void OnDiscardCardsPacket()
    {
        Utils.DestroyChildren(form.tableArea);
    }

    private void SetCardsDraggable(bool canDrag)
    {
        for (int i = 0; i < form.localCardsParent.childCount; ++i)
        {
            form.localCardsParent.GetChild(i).GetComponent<DraggableCard>().canDrag = canDrag;
        }
    }

    private void InstantiateSmallCard(GameObject playerObj)
    {
        var obj = Instantiate(form.smallCardPrefab, playerObj.transform.GetChild(2));

        playerObj.transform.GetChild(2).GetComponent<CardHandLayout>().UpdateLayout();
    }

    public void OnClientTurnPacket(ClientTurnPacket packet)
    {
        if (_lastTurnId != -1)
        {
            var lastTurnObj = _lastTurnId == localClientId ? form.localClientObj : _clientObjects[_lastTurnId];
            lastTurnObj.GetComponent<Image>().color = form.defaultPlayerBorderColor;
        }

        _lastTurnId = packet.clientId;
        var playerObj = packet.clientId == localClientId ? form.localClientObj : _clientObjects[packet.clientId];
        playerObj.GetComponent<Image>().color = form.greenPlayerBorderColor;
        
        SetCardsDraggable(localClientId == packet.clientId);
        form.gameButtons.SetActive(localClientId == packet.clientId);
    }

    public void OnBecomeHostPacket()
    {
        if (!_currentLobby.IsStarted)
            form.startGameButton.SetActive(true);
    }

    public void OnEndTurnPressed()
    {
        ClientSocket.Instance.SendPacket(new GameActionPacket(action: GameAction.EndTurn));
    }
}