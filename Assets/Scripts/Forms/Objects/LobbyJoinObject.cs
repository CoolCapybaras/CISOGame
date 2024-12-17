using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyJoinObject : MonoBehaviour
{
    public int id;

    public void OnPressed()
    {
        JoinGameForm.Instance.OnJoinButtonPressed(id);
    }
}
