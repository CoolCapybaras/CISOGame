using System.Collections;
using System.Collections.Generic;
using CISOServer.Net.Packets.Serverbound;
using Models;
using UnityEngine;

public static class GameManager
{
    public static ClientDTO localClient;

    public static bool isInLobby;

    public static void EnsureLeavedLobby()
    {
        if (!isInLobby)
            return;
        
        ClientSocket.Instance.SendPacket(new LeaveLobbyPacket());
        isInLobby = false;
    }
}
