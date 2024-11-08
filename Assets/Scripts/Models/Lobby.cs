using System.Collections.Generic;

public class Lobby
{
	public int Id { get; }
	public string Name { get; set; }
	public int MaxClients { get; }
	public bool IsStarted { get; set; }
	public List<ClientDTO> Players { get; }
	public int ClientsCount { get; }
	public List<Card> Table { get; }
}
