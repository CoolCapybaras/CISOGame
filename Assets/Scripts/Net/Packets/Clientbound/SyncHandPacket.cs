using System.Collections.Generic;

namespace CISOServer.Net.Packets.Clientbound
{
	public class SyncHandPacket : IPacket
	{
		public int id = 19;

		public List<Card> cards;

		public SyncHandPacket(List<Card> cards)
		{
			this.cards = cards;
		}
	}
}
