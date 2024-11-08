using System.Text.RegularExpressions;

namespace CISOServer.Net.Packets.Serverbound
{
	public partial class UpdateProfilePacket : IPacket
	{
		public static Regex NameRegex { get; } = new Regex("^[A-Za-zА-Яа-я0-9_ ]{3,24}$");

		public int id = 7;

		public string name;
		public string image;

		public UpdateProfilePacket(string name, string image)
		{
			this.name = name;
			this.image = image;
		}
	}
}
