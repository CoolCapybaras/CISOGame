using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CISOServer.Net.Packets.Clientbound;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameForm : MonoBehaviour, IForm
{
    public static GameForm Instance;
    public PlayerLayout playerLayout;
    private Lobby _currentLobby;
    
    [Serializable]
    public struct Form
    {
        public GameObject localClientObj;
        
    }

    public Form form;
    
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
        playerLayout.SetupPlayers(_currentLobby.MaxClients);
        var players = _currentLobby.Players;

        var playerObjects = playerLayout.players;

        if (players.Exists(x => x.Id == GameManager.localClient.Id))
            players.RemoveAll(x => x.Id == GameManager.localClient.Id);

        var localObj = form.localClientObj.transform;

        StartCoroutine(Utils.LoadTexture(GameManager.localClient.Avatar, localObj.GetChild(0).GetComponent<RawImage>()));
        localObj.GetChild(1).GetComponent<TMP_Text>().text = GameManager.localClient.Name;
        
        for (int i = 0; i < players.Count; ++i)
        {
            var player = players[i];
            var obj = playerObjects[i];
            
            obj.GetComponent<Image>().sprite = null;
            StartCoroutine(Utils.LoadTexture(player.Avatar, obj.transform.GetChild(0).GetComponent<RawImage>()));
            obj.transform.GetChild(0).GetComponent<RawImage>().DOFade(1, 0.25f);
            obj.transform.GetChild(1).GetComponent<TMP_Text>().text = player.Name;
        }
    }

    public void OnLobbyJoined(LobbyJoinedPacket packet)
    {
        FormManager.Instance.ChangeForm("game");
        _currentLobby = packet.lobby;
    }
}