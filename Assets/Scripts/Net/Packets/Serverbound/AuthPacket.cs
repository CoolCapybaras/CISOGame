namespace CISOServer.Net.Packets.Serverbound
{
	public enum AuthType
	{
		Anonymous,
		Token,
		VK,
		Telegram
	}

	public class AuthPacket : IPacket
	{
		public int id = 2;

		public AuthType type;
		public string data;
		public int lobbyId;

		public AuthPacket()
		{

		}

		public AuthPacket(AuthType type, string data)
		{
			this.type = type;
			this.data = data;
		}

		public AuthPacket(AuthType type, string data, int lobbyId)
		{
			this.type = type;
			this.data = data;
			this.lobbyId = lobbyId;
		}
	}
}
