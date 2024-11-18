using System;
using System.Collections;
using System.Collections.Generic;
using CISOServer.Net.Packets.Clientbound;
using CISOServer.Net.Packets.Serverbound;
using TMPro;
using UnityEngine;

public class JoinGameForm : BaseForm, IForm
{
    public static JoinGameForm Instance;

    private List<Lobby> _lobbies = new();
    [Serializable]
    public struct Form
    {
        public Transform lobbiesParent;
        public GameObject lobbyPrefab;
    }

    public Form form;
    
    private void Awake()
    {
        Instance = this;
    }

    public void OnActive()
    {
        Initialize();
        gameObject.SetActive(true);
    }

    public void OnDisable()
    {
        gameObject.SetActive(false);
    }

    private void Initialize()
    {
        Utils.DestroyChildren(form.lobbiesParent);
        _lobbies.Clear();
        ClientSocket.Instance.SendPacket(new SearchLobbyPacket(SearchLobbyType.Clear, 0));
        ClientSocket.Instance.SendPacket(new SearchLobbyPacket(SearchLobbyType.Search, 10));
    }

    public void OnSearchLobbyResult(SearchLobbyResultPacket packet)
    {
        if ( packet.lobbies == null || packet.lobbies.Count == 0)
            return;
        
        _lobbies.AddRange(packet.lobbies);
        InstantiateLobbyButtons();
    }

    private void InstantiateLobbyButtons()
    {
        Utils.DestroyChildren(form.lobbiesParent);

        foreach (var lobby in _lobbies)
        {
            var obj = Instantiate(form.lobbyPrefab, form.lobbiesParent);
            obj.transform.GetChild(0).GetComponent<TMP_Text>().text = $"Лобби {lobby.Id}";
            obj.transform.GetChild(1).GetComponent<TMP_Text>().text = $"{lobby.ClientsCount}/{lobby.MaxClients}";
            obj.GetComponent<LobbyJoinObject>().id = lobby.Id;
        }
    }

    public void OnJoinButtonPressed(int id)
    {
        ClientSocket.Instance.SendPacket(new JoinLobbyPacket(id));
    }
}
