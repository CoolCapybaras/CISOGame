using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    public int clientId;

    public void OnPressed()
    {
        GameForm.Instance.OnPlayerPressed(clientId);
    }
}
