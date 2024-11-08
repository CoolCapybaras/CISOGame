namespace CISOServer.Net.Packets.Clientbound
{
	public class GameEndedPacket : IPacket
	{
		public int id = 18;

		public int clientId;

		public GameEndedPacket(int clientId)
		{
			this.clientId = clientId;
		}
	}
}
