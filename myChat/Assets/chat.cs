using UnityEngine;
using System.Collections;
public class chat : MonoBehaviour {
	//定义远程服务器IP（这里为本地）
	private string ip = "127.0.0.1";
	//定义服务器端口
	private int port = 10001;
	//限制连接数量为15个用户
	private int connectCount = 15;
	//是否启用网络地址转换器
	private bool useNAT = false;
	//接收到的消息
	private string recMes = "";
	//要发送的消息
	private string sendMes = "";
	//@weijiand ScreenKeyboard
	private OnScreenKeyboard osk;
	// Use this for initialization
	void Start()
	{
	}
	void Awake() 
	{
		osk = FindObjectOfType(typeof(OnScreenKeyboard)) as OnScreenKeyboard;
	}
	// Update is called once per frame
	void Update()
	{/*
		// You can use input from the OSK just by asking for the most recent 
		// pressed key, which will be returned to you as a string, or null if 
		// no key has been pressed since you last checked. Note that if more 
		// than one key has been pressed you will only be given the most recent.
		string keyPressed = osk.GetKeyPressed();
		if (keyPressed != "" && keyPressed != "新建服务器" && keyPressed != "连接服务器" && keyPressed != "断开连接")
		{
			// Take different action depending on what key was pressed
			if (keyPressed == "Backspace" || keyPressed == "<<")
			{
				// Remove a character
				if (sendMes.Length > 0)
					sendMes = sendMes.Substring(0, sendMes.Length-1);
			}
			else if (keyPressed == "Space")
			{
				// Add a space
				sendMes += " ";	
			}
			else if (keyPressed == "Enter" || keyPressed == "Done" || keyPressed == "发送消息")
			{
				// Change screens, or do whatever you want to 
				// do when your user has finished typing :-)
				GUILayout.TextArea("ttt");
				GetComponent<NetworkView>().RPC("SendMes", RPCMode.All, Network.player + "Say:" + sendMes);
			}
			else if (keyPressed == "Caps")
			{
				// Toggle the capslock state yourself
				osk.SetShiftState(osk.GetShiftState() == ShiftState.CapsLock ? ShiftState.Off : ShiftState.CapsLock);
			}
			else if (keyPressed == "Shift")
			{
				// Toggle shift state ourselves
				osk.SetShiftState(osk.GetShiftState() == ShiftState.Shift ? ShiftState.Off : ShiftState.Shift);
			}
			else
			{
				// Add a letter to the existing string
				sendMes += keyPressed;	
			}
		}*/
	}
	void OnGUI()
	{
		switch (Network.peerType)
		{
		//服务器是否开启，没有与服务器连接时
		case NetworkPeerType.Disconnected:
			StartCreat();
			break;
			//启动服务器
		case NetworkPeerType.Server:
			OnServer();
			break;
			//启动客户端
		case NetworkPeerType.Client:
			OnClient();
			break;
			//尝试连接
		case NetworkPeerType.Connecting:
			Debug.Log("连接中");
			break;
		}
	}
	void StartCreat()
	{
		GUILayout.BeginVertical();
		//新建服务器连接
		if (GUILayout.Button("新建服务器"))
		{
			//初始化服务器端口，服务器创建成功后，Network.peerType变为NetworkPeerType.Server
			NetworkConnectionError error = Network.InitializeServer(connectCount, port, useNAT);
			Debug.Log(error);
		}
		//客户端是否连接服务器
		if (GUILayout.Button("连接服务器"))
		{
			//连接至服务器，与服务器连接成功后，Network.peerType变为NetworkPeerType.Client
			NetworkConnectionError error = Network.Connect(ip, port);
			Debug.Log(error);
		}
		GUILayout.EndVertical();
	}
	void OnServer()
	{
		GUILayout.Label("新建服务器成功，等待客户端连接");
		////得到的IP与端口
		//string ip = Network.player.ipAddress;
		//int port = Network.player.port;
		//GUILayout.Label("IP地址:" + ip + ".\n端口号:" + port);
		//连接到服务器的所有客户端
		int length = Network.connections.Length;
		//遍历所有客户端并获取IP与端口号
		for (int i = 0; i < length; i++)
		{
			GUILayout.Label("连接的IP:" + Network.connections[i].ipAddress);
			GUILayout.Label("连接的端口:" + Network.connections[i].port);
		}
		if (GUILayout.Button("断开连接"))
		{
			//从服务器上断开连接，断开连接后，Network.peerType变为NetworkPeerType.Disconnected
			Network.Disconnect();
		}
		GUILayout.TextArea(recMes);
		sendMes = GUILayout.TextField(sendMes);
		if (GUILayout.Button("发送消息"))
		{
			GetComponent<NetworkView>().RPC("SendMes", RPCMode.All, Network.player + " say:" + sendMes);
			sendMes = "";
		}

	}
	void OnClient()
	{
		GUILayout.Label("连接成功");
		if (GUILayout.Button("断开连接"))
		{   //断开连接后，Network.peerType变为NetworkPeerType.Disconnected
			Network.Disconnect();
		}
		GUILayout.TextArea(recMes);
		sendMes = GUILayout.TextField(sendMes);
		if (GUILayout.Button("发送消息"))
		{
			GetComponent<NetworkView>().RPC("SendMes", RPCMode.All, Network.player + " say:" + sendMes);
			sendMes = "";
		}
	}
	[RPC]
	void SendMes(string mes)
	{
		this.recMes += "\n";
		this.recMes += mes;
	}


}