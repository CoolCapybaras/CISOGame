namespace CISOServer.Net.Packets.Serverbound
{
	public enum GameAction
	{
		PlayCard,
		EndTurn
	}

	public class GameActionPacket : IPacket
	{
		public int id = 16;

		public GameAction action;
		public Card card;
		public int targetId;

		public GameActionPacket(GameAction action)
		{
			this.action = action;
		}

		public GameActionPacket(GameAction action, Card card, int targetId)
		{
			this.action = action;
			this.card = card;
			this.targetId = targetId;
		}
	}
}
