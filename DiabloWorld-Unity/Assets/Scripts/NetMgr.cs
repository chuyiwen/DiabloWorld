using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetMgr : MonoBehaviour {
	
	public string 	sIP = "119.29.146.115";  // 实际服务器ip
	public int 		iPort = 11009;  // 实际服务器端口 firefly "net":{"netport":11009}

    private XTcpClient m_Client;
	//private Queue<byte[]> 		m_bufferManager;
	private System.Action       m_ConnectSuccessCallBack;  // 成功的回调方法
	private bool	m_bWarnLostConnect;
	
	void Awake (){
		//m_bufferManager = new Queue<byte[]>();
		_init();
	}
	
	void _init (){
		m_Client = new XTcpClient();
        // 将回调方法加入到委托中
		m_Client.OnConnected += HandleM_ClientOnConnected;
		m_Client.OnDisconnected += HandleM_ClientOnDisconnected;
		m_Client.OnError += HandleM_ClientOnError;
	}
	
	void HandleM_ClientOnError (object sender, DSCClientErrorEventArgs e)
	{
		Debug.LogWarning("::OnError");
	}

	void HandleM_ClientOnDisconnected (object sender, DSCClientConnectedEventArgs e)
	{
		Debug.LogWarning("::OnDisconnected");
	}
    // 通知成功连接的回调
	void HandleM_ClientOnConnected (object sender, DSCClientConnectedEventArgs e)
	{
		Debug.LogWarning("::OnConnected");
		if (Connected) {
			if (m_ConnectSuccessCallBack != null) {
				m_ConnectSuccessCallBack();  // 通知
				m_ConnectSuccessCallBack = null;
			}
		}
		else{
			m_bWarnLostConnect = true;
		}
	}
	
    // 连接丢失的时候调用
	void _ShowLostConnect (){
		Globals.It.HideWaiting();
		Globals.It.ShowWarn(2, 5, null);
	}

    // 把 取数据 放到每一固定帧调用
    void FixedUpdate (){
		if (m_Client != null && m_Client.Connected) {
			Globals.It.ProcessMsg(m_Client.Loop());  // 取数据
		}
		if (m_bWarnLostConnect) {  // 是否连接丢失
			m_bWarnLostConnect = false;
			_ShowLostConnect();
		}
	}
	
    // ###################################################### 以下为外部调用方法

	public void ReInit (){
		_init();
	}
	
	public void Connect ()  // 无回调的连接方法
	{
		m_Client.Connect(sIP, iPort);
	}
	
	public void Connect (System.Action callback)  // 有成功回调的连接方法
    {
		m_ConnectSuccessCallBack = callback;
		m_Client.Connect(sIP, iPort);
	}
	
	public void Send (byte[] buffer) {  // 外界调用的 发送数据方法
		if (buffer != null && Connected) {
			m_Client.Send(buffer);
		}
	}
	
	public void Close (){
		if (Connected){
			m_Client.Close();
		}
	}
	
	public bool Connected {
		get {
			return m_Client != null && m_Client.Connected;
		}
	}
}
