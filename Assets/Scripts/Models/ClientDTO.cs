using System;
using CISOServer.Net.Packets.Clientbound;
using System.Collections.Generic;

[Serializable]
public class ClientDTO
{
	public int Id;
	public string Name;
	public string Avatar;
	public ClientState State;
	public int Health;
	public HashSet<Character> Characters { get; }
	public int CardCount;
}
