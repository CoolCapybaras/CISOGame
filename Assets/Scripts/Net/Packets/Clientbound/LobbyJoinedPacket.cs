namespace CISOServer.Net.Packets.Clientbound
{
	public class LobbyJoinedPacket : IPacket
	{
		public int id = 10;

		public int clientId;
		public Lobby lobby;

		public LobbyJoinedPacket(int clientId, Lobby lobby)
		{
			this.clientId = clientId;
			this.lobby = lobby;
		}
	}
}
