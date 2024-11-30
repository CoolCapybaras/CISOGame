using CISOServer.Net.Packets;
using CISOServer.Net.Packets.Clientbound;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine;

public class ClientSocket : MonoBehaviour
{
	public static ClientSocket Instance;
	
	[DllImport("__Internal")]
	private static extern void StartSocket();

	[DllImport("__Internal")]
	private static extern void SendSocket(string message);

	[DllImport("__Internal")]
	private static extern void ConsoleLog(string message);

#if UNITY_EDITOR || PLATFORM_STANDALONE_WIN
	public string Hostname = "localhost";
	public int Port = 8887;
	private TcpClient tcpClient;
	private StreamReader streamReader;
	private StreamWriter streamWriter;
	private ConcurrentQueue<string> messageQueue = new();
	private bool socketClosing;
#endif

	void Awake()
	{
		Instance = this;
#if UNITY_EDITOR || PLATFORM_STANDALONE_WIN
		Task.Run(async () =>
		{
			Debug.Log("Connecting to server...");
			while (true)
			{
				while (true)
				{
					try
					{
						tcpClient = new TcpClient(Hostname, Port);
					}
					catch (SocketException)
					{
						if (socketClosing)
							return;
						Debug.Log($"Socket is closed. Reconnect will be attempted in 1 second.");
						await Task.Delay(1000);
						continue;
					}
					if (socketClosing)
						return;
					break;
				}
				streamReader = new StreamReader(tcpClient.GetStream());
				streamWriter = new StreamWriter(tcpClient.GetStream());
				streamWriter.AutoFlush = true;
				messageQueue.Enqueue("{\"id\":-1}");
				try
				{
					while (true)
						messageQueue.Enqueue(await streamReader.ReadLineAsync());
				}
				catch (Exception ex) when (ex.InnerException is SocketException)
				{
					if (socketClosing)
						return;
					Debug.LogException(ex);
				}
			}
		});
#else
		StartSocket();
#endif
	}

#if UNITY_EDITOR || PLATFORM_STANDALONE_WIN
	void Update()
	{
		while (messageQueue.TryDequeue(out string message))
			OnMessage(message);
	}

	void OnApplicationQuit()
	{
		tcpClient?.Close();
		socketClosing = true;
	}
#endif

	public void OnMessage(string message)
	{
#if UNITY_EDITOR || PLATFORM_STANDALONE_WIN
		Debug.Log($"Server: {message}");
#else
		ConsoleLog($"Server: {message}");
#endif
		switch (JsonUtility.FromJson<BasePacket>(message).id)
		{
			case -1:
				// OnOpen
				break;
			case 0:
				var packet = JsonUtility.FromJson<MessagePacket>(message);
				Debug.Log(packet.text);
				break;
			case 1:
				//var packet = JsonUtility.FromJson<AuthPacket>(message);
				break;
			case 8:
				// AuthResultPacket
				var authResult = JsonUtility.FromJson<AuthResultPacket>(message);
				AuthForm.Instance.OnAuthResult(authResult);
				break;
			case 9:
				var lobbySearchResult = JsonUtility.FromJson<SearchLobbyResultPacket>(message);
				JoinGameForm.Instance.OnSearchLobbyResult(lobbySearchResult);
				break;
			case 10:
				var lobbyJoinedPacket = JsonUtility.FromJson<LobbyJoinedPacket>(message);
				GameForm.Instance.OnLobbyJoined(lobbyJoinedPacket);
				break;
			case 11:
				var clientJoinedPacket = JsonUtility.FromJson<ClientJoinedPacket>(message);
				GameForm.Instance.OnClientJoinedPacket(clientJoinedPacket);
				break;
			case 12:
				var clientLeavedPacket = JsonUtility.FromJson<ClientLeavedPacket>(message);
				GameForm.Instance.OnClientLeavedPacket(clientLeavedPacket);
				break;
			case 19:
				var syncHandPacket = JsonUtility.FromJson<SyncHandPacket>(message);
				GameForm.Instance.OnSyncHandPacket(syncHandPacket);
				break;
			case 20:
				var clientsGotCardsPacket = JsonUtility.FromJson<ClientsGotCardsPacket>(message);
				GameForm.Instance.OnClientsGotCards(clientsGotCardsPacket);
				break;
			case 21:
				var clientTurnPacket = JsonUtility.FromJson<ClientTurnPacket>(message);
				GameForm.Instance.OnClientTurnPacket(clientTurnPacket);
				break;
			case 13:
				GameForm.Instance.OnBecomeHostPacket();
				break;
			case 17:
				GameForm.Instance.OnGameStarted();
				break;
			case 23:
				GameForm.Instance.OnDiscardCardsPacket();
				break;
		}
	}

	public void SendPacket(IPacket packet)
	{
		var message = JsonUtility.ToJson(packet);
#if UNITY_EDITOR || PLATFORM_STANDALONE_WIN
		streamWriter.WriteLine(message);
		Debug.Log($"Client: {message}");
#else
		SendSocket(message);
		ConsoleLog($"Client: {message}");
#endif
	}
}
