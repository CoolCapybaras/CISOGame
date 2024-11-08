namespace CISOServer.Net.Packets.Serverbound
{
	public class JoinLobbyPacket : IPacket
	{
		public int id = 5;

		public int lobbyId;

		public JoinLobbyPacket(int lobbyId)
		{
			this.lobbyId = lobbyId;
		}
	}
}
