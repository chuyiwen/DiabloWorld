using UnityEngine;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;
using LitJson;
using System.Threading;
public class BufferObject
{
	public Socket _tcpSock = null;
	public int BufferSize;
	public byte[] buffer;
	public int commandId;
	public void CreateBuffer()
	{
		buffer = new byte[BufferSize];
	}
	public StringBuilder Data = new StringBuilder ();
}
public class NetMgr{
	//包头长度
	private const int RecviceHeadLength = 17;
	//是否接收数据
	private static bool m_bIsReceive = false;
	//当前接收大小
	private static int m_nReceiveLength = 0;
	//当前创建的socket
	private static Socket _tcpSock = null;
	// 超时事件
	private static ManualResetEvent timeoutobject = new ManualResetEvent(false);
	//是否连接
	private static bool m_bIsConnect = false;
	//Socket错误信息
	private static Exception m_socketexception;
	public void Start (string host,int port) {
		_tcpSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		IPAddress ip = IPAddress.Parse (host);
		IPEndPoint ipe = new IPEndPoint (ip, port);
		Connect (ipe,5);
	}
	private void Connect(IPEndPoint ipe,int timeoutmsec)
	{
		timeoutobject.Reset ();
		IAsyncResult ResultType = _tcpSock.BeginConnect (ipe,new AsyncCallback(CallbackConnect),_tcpSock);
		if (timeoutobject.WaitOne (timeoutmsec, false)) {
			if (m_bIsConnect) {
				Receive (_tcpSock);
			}
		} else {
			_tcpSock.Close();
			Debug.Log("Connect not to Server!");
		}
		//connectDone.WaitOne ();
		//sendDone.WaitOne ();

	}
	private void CallbackConnect(IAsyncResult ar)
	{
		try
		{
			m_bIsConnect = false;
			var socket = ar.AsyncState as Socket;
			
			if (socket != null)
			{
				socket.EndConnect(ar); 
				m_bIsConnect = true;
			}
		}
		catch (Exception ex)
		{
			m_bIsConnect = false;
			m_socketexception = ex;
		}
		finally
		{
			timeoutobject.Set();
		}
	}
	public void Receive(Socket _tcpSock)
	{
		BufferObject ReceiveBuffer = new BufferObject ();
		ReceiveBuffer._tcpSock = _tcpSock;
		ReceiveBuffer.BufferSize = RecviceHeadLength;
		ReceiveBuffer.CreateBuffer();
		m_nReceiveLength = ReceiveBuffer.BufferSize;
		m_bIsReceive = false;
		_tcpSock.BeginReceive (ReceiveBuffer.buffer,0,ReceiveBuffer.BufferSize,0,new AsyncCallback(ReceiveCallback),ReceiveBuffer);
	}
	public void ReceiveCallback(IAsyncResult ar)
	{
		try
		{
			BufferObject ReceiveBuffer = (BufferObject)ar.AsyncState;
			Socket m_Socket = ReceiveBuffer._tcpSock;
			int bytesRead = ReceiveBuffer._tcpSock.EndReceive(ar);
			//如果没有接收完毕则继续接收
			if(m_nReceiveLength > 0 ){
				ReceiveBuffer.Data.Append(Encoding.Default.GetString(ReceiveBuffer.buffer, 0, bytesRead));
				m_nReceiveLength -= bytesRead;
				if(m_nReceiveLength <= 0)
				{
					if(m_bIsReceive){
						if (ReceiveBuffer.Data.Length == ReceiveBuffer.BufferSize){
							JsonReader jsonR = new JsonReader(ReceiveBuffer.Data.ToString());
							JsonData jData = JsonMapper.ToObject(jsonR);
							SocketManager.Instance.RecvMessage(ReceiveBuffer.commandId,jData);
							Receive(m_Socket);
							return;
						}
					}else{
						if (ReceiveBuffer.Data.Length == ReceiveBuffer.BufferSize){
							int commandId, size;
							unPackHead(ReceiveBuffer.buffer, out commandId, out size);
							m_bIsReceive = true;
							BufferObject RecvDataBuffer = new BufferObject();
							RecvDataBuffer._tcpSock = ReceiveBuffer._tcpSock;
							RecvDataBuffer.BufferSize = size - 4;
							RecvDataBuffer.commandId = commandId;
							m_nReceiveLength = RecvDataBuffer.BufferSize;
							RecvDataBuffer.CreateBuffer();
							_tcpSock.BeginReceive (RecvDataBuffer.buffer,0,RecvDataBuffer.BufferSize,0,new AsyncCallback(ReceiveCallback),RecvDataBuffer);
							return;
						}
					}
				}
				_tcpSock.BeginReceive (ReceiveBuffer.buffer,0,m_nReceiveLength,0,new AsyncCallback(ReceiveCallback),ReceiveBuffer);
			}else
			{

			}
		}
		catch(Exception ex)
		{
			Console.WriteLine(ex.Message);
		}
	}
	private void unPackHead(byte[] head, out int commandId, out int size){
		int head0 = head [0];
		int head1 = head [1];
		int head2 = head [2];
		int head3 = head [3];
		int protoVersion = head [4];
		int serverVersion = System.BitConverter.ToInt32 (head, 5);
		// todo check the head, version
		System.Array.Reverse(head, 9, 4);
		size = System.BitConverter.ToInt32(head, 9);

		System.Array.Reverse(head, 13, 4);
		commandId = System.BitConverter.ToInt32(head, 13);
	}

	private byte[] packMsg(byte[] msg, int commandId){
		byte[] head = new byte[17];
		head [0] = 0;
		head [1] = 1;
		head [2] = 2;
		head [3] = 3;
		head [4] = 4; // protoVersion

		Array.Copy (System.BitConverter.GetBytes (0), 0, head, 5, 4); // serverVersion

		byte[] lengthByte = new byte[4];
		lengthByte = System.BitConverter.GetBytes (msg.Length + 4);
		Array.Reverse (lengthByte);
		Array.Copy (lengthByte, 0, head, 9, 4);

		byte[] commandIdByte = new byte[4];
		commandIdByte = System.BitConverter.GetBytes (commandId);
		Array.Reverse (commandIdByte);
		Array.Copy (commandIdByte, 0, head, 13, 4);

		byte[] ret = new byte[head.Length + msg.Length];
		Array.Copy (head, 0, ret, 0, head.Length);
		Array.Copy (msg, 0, ret, head.Length, msg.Length);
		return ret;
	}

	public void SendMsg(byte[] msg, int commandId){
		byte[] data = packMsg (msg, commandId);
		_tcpSock.Send(data, data.Length, 0);//发送信息
	}
}