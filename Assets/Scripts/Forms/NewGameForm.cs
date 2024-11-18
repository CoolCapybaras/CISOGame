using System;
using System.Collections;
using System.Collections.Generic;
using CISOServer.Net.Packets.Serverbound;
using UnityEngine;
using UnityEngine.UI;

public class NewGameForm : BaseForm, IForm
{
    public static NewGameForm Instance;
    
    [Serializable]
    public struct Form
    {
        public Slider maxPlayersSlider;
    }

    public Form form;
    
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
        ClientSocket.Instance.SendPacket(new CreateLobbyPacket((int)form.maxPlayersSlider.value));
    }
}
