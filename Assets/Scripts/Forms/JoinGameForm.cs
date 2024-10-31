using System;
using System.Collections;
using System.Collections.Generic;
using CISOServer.Net.Packets.Serverbound;
using TMPro;
using UnityEngine;

public class JoinGameForm : BaseForm, IForm
{
    public static JoinGameForm Instance;

    [Serializable]
    public struct Form
    {
        public TMP_InputField tempInputField;
    }

    public Form form;
    
    private void Awake()
    {
        Instance = this;
    }

    public void OnActive()
    {
        gameObject.SetActive(true);
    }

    public void OnDisable()
    {
        gameObject.SetActive(false);
    }

    public void OnTempJoinButtonPressed()
    {
        ClientSocket.Instance.SendPacket(new JoinLobbyPacket(int.Parse(form.tempInputField.text)));
    }
}
