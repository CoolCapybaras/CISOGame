using System;
using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;

public class OverlayManager : MonoBehaviour
{
    public static OverlayManager Instance;
    
    [Serializable]
    public class Settings
    {
        public bool needProfileButton;
        public bool needSettingsButton;
        public bool needLeaveButton;
        public bool needBackButton;
    }
    
    [Serializable]
    public struct Form
    {
        public GameObject profileButton;
        public GameObject settingsButton;
        public GameObject leaveButton;
        public GameObject backButton;
    }

    public Form form;
    private void Awake()
    {
        Instance = this;
    }

    public void ApplySettings(Settings settings)
    {
        form.profileButton.SetActive(settings.needProfileButton);
        form.settingsButton.SetActive(settings.needSettingsButton);
        form.leaveButton.SetActive(settings.needLeaveButton);
        form.backButton.SetActive(settings.needBackButton);
    }

    public void OnBackButtonPressed()
    {
        FormManager.Instance.ChangeFormToLast();
        GameManager.EnsureLeavedLobby();
    }
}
