using CISOServer.Net.Packets.Clientbound;
using System.Collections.Generic;

public class ClientDTO
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Avatar { get; set; }
	public ClientState State { get; set; }
	public int Health { get; set; }
	public HashSet<Character> Characters { get; }
	public int CardCount { get; set; }
}
