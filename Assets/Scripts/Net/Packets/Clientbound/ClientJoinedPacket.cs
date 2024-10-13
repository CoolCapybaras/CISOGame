namespace CISOServer.Net.Packets.Clientbound
{
	public class ClientJoinedPacket : IPacket
	{
		public int id = 7;

		public ClientDTO client;

		public ClientJoinedPacket(ClientDTO client)
		{
			this.client = client;
		}
	}
}
