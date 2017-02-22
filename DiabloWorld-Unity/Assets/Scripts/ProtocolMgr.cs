using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// 协议（命令）管理类
public class ProtocolMgr : MonoBehaviour {
	
	private Dictionary<int, List<IProtocol>> m_Protocols;
	
	void Awake (){
		m_Protocols = new Dictionary<int, List<IProtocol>>();
	}
	
	void Start (){  // 注册命令
		Register(new DengluProtocol());
		Register(new CreateRoleProtocol());
		Register(new RoleEnterGameProtocol());
		Register(new GetMapDataProtocol());
		Register(new MapBattleProtocol());
		Register(new PlayerStatProtocol());
	}
	
	public void Register(IProtocol protocol) {
		if (protocol == null) return;
		if(m_Protocols.ContainsKey(protocol.iCommand)) {  // 一个命令可以有多个回调方法
			m_Protocols[protocol.iCommand].Add(protocol);
		}
		else{  // 第一次添加
			List<IProtocol> vars = new List<IProtocol>();
			vars.Add(protocol);
			m_Protocols.Add(protocol.iCommand, vars);
		}
	}
	
    // 从字典取得回调方法
	public List<IProtocol> GetProtocol (int iCommand){
		if (m_Protocols.ContainsKey(iCommand)) return m_Protocols[iCommand];
		return null;
	}
	// 处理消息，取得回调方法并执行方法
	public void Process (Message_Body body) {
		Debug.LogWarning(string.Format(":::{0}:{1}", body.iCommand, System.Text.Encoding.UTF8.GetString(body.body)));
		List<IProtocol> vars = GetProtocol(body.iCommand);
		if (vars != null && vars.Count > 0) {
			vars.ForEach(protocol=>{ protocol.Process(body); });  // lambda
		}
	}
}
