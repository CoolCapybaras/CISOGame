using System.Collections.Generic;

namespace CISOServer.Net.Packets.Clientbound
{
	public class SearchLobbyResultPacket : IPacket
	{
		public int id = 9;

		public List<Lobby> lobbies;
	}
}
