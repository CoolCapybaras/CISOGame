using System.Collections.Generic;

namespace CISOServer.Net.Packets.Clientbound
{
	public class LobbyJoinedPacket : IPacket
	{
		public int id = 6;

		public List<ClientDTO> clients;

		public LobbyJoinedPacket(List<ClientDTO> clients)
		{
			this.clients = clients;
		}
	}
}
