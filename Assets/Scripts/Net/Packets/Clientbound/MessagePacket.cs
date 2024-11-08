﻿namespace CISOServer.Net.Packets.Clientbound
{
	public class MessagePacket : IPacket
	{
		public int id = 1;

		public int type;
		public string text;

		public MessagePacket(int type, string text)
		{
			this.type = type;
			this.text = text;
		}
	}
}
