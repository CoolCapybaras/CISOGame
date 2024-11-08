namespace CISOServer.Net.Packets.Serverbound
{
	public enum SearchLobbyType
	{
		Search,
		Clear
	}

	public class SearchLobbyPacket : IPacket
	{
		public int id = 4;

		public SearchLobbyType type;
		public int count;

		public SearchLobbyPacket(SearchLobbyType type, int count)
		{
			this.type = type;
			this.count = count;
		}
	}
}
