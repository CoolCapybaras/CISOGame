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
		public int id = 1;

		public AuthType type;
		public string data;

		public AuthPacket(AuthType type, string data)
		{
			this.type = type;
			this.data = data;
		}
	}
}
