using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System;
public class SocketManager : MonoBehaviour {
	
	public string host = "127.0.0.1";
	public int port = 55555;
	
	public static SocketManager Instance = null;
	private Dictionary<int, Action<JsonData>> _FuncList;
	private NetMgr _NetMgr;
	void Awake(){
		Instance = this;
		_NetMgr = new NetMgr ();
		_NetMgr.Start (host,port);
	}
	// Use this for initialization
	void Start () {
		_FuncList = new Dictionary<int, Action<JsonData>> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
	}
	public void RecvMessage(int commandId,JsonData jData)
	{
		bool isFunc = _FuncList.ContainsKey (commandId);
		if ((int)jData ["State"] == 1) {
			if (isFunc) {
				_FuncList [commandId] (jData);
			} else {
				PushMessage(commandId,jData);
			}
		} else {
			Debug.Log (jData["Message"]);
		}
	}
	public void SendMessage(byte[] msg, int commandId,Action<JsonData> Func)
	{
		_NetMgr.SendMsg (msg,commandId);
		bool isFunc = _FuncList.ContainsKey (commandId);
		if(!isFunc)
		{
			_FuncList.Add(commandId,Func);
		}
	}
	void PushMessage(int commandId,JsonData jData)
	{
		Debug.Log (jData["Data"]);
	}
}
