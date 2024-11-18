using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Lobby
{
	public int Id;
	public string Name;
	public int MaxClients;
	public bool IsStarted;
	public List<ClientDTO> Players;
	public int ClientsCount;
	public List<Card> Table;
}
