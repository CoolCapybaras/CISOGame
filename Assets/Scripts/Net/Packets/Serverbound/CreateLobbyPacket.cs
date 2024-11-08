namespace CISOServer.Net.Packets.Serverbound
{
	public class CreateLobbyPacket : IPacket
	{
		public int id = 3;

		public int maxClients;

		public CreateLobbyPacket(int maxClients)
		{
			this.maxClients = maxClients;
		}
	}
}
