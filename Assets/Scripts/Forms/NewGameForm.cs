using System;
using System.Collections;
using System.Collections.Generic;
using CISOServer.Net.Packets.Serverbound;
using UnityEngine;

public class NewGameForm : BaseForm, IForm
{
    public static NewGameForm Instance;
    
    public void OnActive()
    {
        gameObject.SetActive(true);
    }

    public void OnDisable()
    {
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        Instance = this;
    }

    public void OnCreateLobbyPressed()
    {
        ClientSocket.Instance.SendPacket(new CreateLobbyPacket(5));
    }
}
