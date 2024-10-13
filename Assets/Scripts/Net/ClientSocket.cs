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
	[DllImport("__Internal")]
	private static extern void StartSocket();

	[DllImport("__Internal")]
	private static extern void SendSocket(string message);

	[DllImport("__Internal")]
	private static extern void ConsoleLog(string message);

#if UNITY_EDITOR
	public string Hostname = "localhost";
	public int Port = 8887;
	private TcpClient tcpClient;
	private StreamReader streamReader;
	private StreamWriter streamWriter;
	private ConcurrentQueue<string> messageQueue = new();
	private bool socketClosing;
#endif

	void Start()
	{
#if UNITY_EDITOR
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

#if UNITY_EDITOR
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
#if UNITY_EDITOR
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
		}
	}

	public void SendPacket(IPacket packet)
	{
		var message = JsonUtility.ToJson(packet);
#if UNITY_EDITOR
		streamWriter.WriteLine(message);
		Debug.Log($"Client: {message}");
#else
		SendSocket(message);
		ConsoleLog($"Client: {message}");
#endif
	}
}
