using System;
using System.Collections.Generic;

namespace CISOServer.Net.Packets.Clientbound
{
	[Serializable]
	public class SearchLobbyResultPacket : IPacket
	{
		public int id = 9;

		public List<Lobby> lobbies = new();
	}
}
