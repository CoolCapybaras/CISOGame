using System;
using System.Collections;
using System.Collections.Generic;
using CISOServer.Net.Packets.Clientbound;
using CISOServer.Net.Packets.Serverbound;
using TMPro;
using UnityEngine;

public class AuthForm : BaseForm, IForm
{
    public static AuthForm Instance;
    
    [Serializable]
    public struct Form
    {
        public TMP_InputField LoginInputField;
    }
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("auth_token"))
            ClientSocket.Instance.SendPacket(new AuthPacket(AuthType.Token, PlayerPrefs.GetString("auth_token")));
    }

    public Form form;

    public void OnAnonymousLoginPressed()
    {
        var nickname = form.LoginInputField.text;
        
        ClientSocket.Instance.SendPacket(new AuthPacket(AuthType.Anonymous, nickname));
    }

    public void OnVkLoginPressed()
    {
        ClientSocket.Instance.SendPacket(new AuthPacket(AuthType.VK, string.Empty));
    }

    public void OnTelegramLoginPressed()
    {
        ClientSocket.Instance.SendPacket(new AuthPacket(AuthType.Telegram, string.Empty));
    }

    public void OnAuthResult(AuthResultPacket packet)
    {
        if (packet.flags.HasFlag(AuthResultFlags.Ok))
        {
            OnAuthSuccessful();
        }

        if (packet.flags.HasFlag(AuthResultFlags.HasToken))
        {
            PlayerPrefs.SetString("auth_token", packet.token);
        }

        if (packet.flags.HasFlag(AuthResultFlags.HasUrl))
        {
            Application.OpenURL(packet.url);
        }
    }

    public void OnAuthSuccessful()
    {
        FormManager.Instance.ChangeForm("mainmenu");
    }
    
    public void OnActive()
    {
        gameObject.SetActive(true);
    }

    public void OnDisable()
    {
       gameObject.SetActive(false);
    }
}
