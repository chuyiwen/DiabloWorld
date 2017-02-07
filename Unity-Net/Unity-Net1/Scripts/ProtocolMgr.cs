using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProtocolMgr : MonoBehaviour {
	
	private Dictionary<int, List<IProtocol>> m_Protocols;
	
	void Awake (){
		m_Protocols = new Dictionary<int, List<IProtocol>>();
	}
	
	void Start (){
		Register(new DengluProtocol());
		Register(new CreateRoleProtocol());
		Register(new RoleEnterGameProtocol());
		Register(new GetMapDataProtocol());
		Register(new MapBattleProtocol());
		Register(new PlayerStatProtocol());
	}
	
	public void Register(IProtocol protocol) {
		if (protocol == null) return;
		if(m_Protocols.ContainsKey(protocol.iCommand)) {
			m_Protocols[protocol.iCommand].Add(protocol);
		}
		else{
			List<IProtocol> vars = new List<IProtocol>();
			vars.Add(protocol);
			m_Protocols.Add(protocol.iCommand, vars);
		}
	}
	
	public List<IProtocol> GetProtocol (int iCommand){
		if (m_Protocols.ContainsKey(iCommand)) return m_Protocols[iCommand];
		return null;
	}
	
	public void Process (Message_Body body) {
		Debug.LogWarning(string.Format(":::{0}:{1}", body.iCommand, System.Text.Encoding.UTF8.GetString(body.body)));
		List<IProtocol> vars = GetProtocol(body.iCommand);
		if (vars != null && vars.Count > 0) {
			vars.ForEach(protocol=>{ protocol.Process(body); });
		}
	}
}
