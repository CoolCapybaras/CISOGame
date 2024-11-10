﻿namespace CISOServer.Net.Packets.Clientbound
{
	public class ClientPlayedCardPacket : IPacket
	{
		public int id = 22;

		public int clientId;
		public Card card;
		public int targetId;

		public ClientPlayedCardPacket(int clientId, Card card, int targetId)
		{
			this.clientId = clientId;	
			this.card = card;
			this.targetId = targetId;
		}
	}
}