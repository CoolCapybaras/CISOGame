namespace CISOServer.Net.Packets.Clientbound
{
	public class ClientLeavedPacket : IPacket
	{
		public int id = 8;

		public int clientId;

		public ClientLeavedPacket(int clientId)
		{
			this.clientId = clientId;
		}
	}
}
