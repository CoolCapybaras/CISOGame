using System.Collections.Generic;

namespace CISOServer.Net.Packets.Clientbound
{
	public class ClientsGotCardsPacket : IPacket
	{
		public int id = 20;

		public List<int> clientIds;

		public ClientsGotCardsPacket(List<int> clientIds)
		{
			this.clientIds = clientIds;
		}
	}
}
