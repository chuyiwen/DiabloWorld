using UnityEngine;
using System.Collections;

public class SelectRoleView : MonoBehaviour {
	
	public Transform[]	roleTrans = new Transform[3];
	public UISprite[]	bigImageSpirte = new UISprite[3];
	public string[]		sDesc = new string[3];
	public UILabel		labelDesc;
	public UISprite		spriteFocus;
	
	private int			m_iSelectRoleID;
	
	// Use this for initialization
	void Start () {
		onSelect1();
	}
	
	void onSelect1 (){
		if (m_iSelectRoleID == 1) return;
		m_iSelectRoleID = 1;
		_Refresh();
	}
	
	void onSelect2 (){
		if (m_iSelectRoleID == 2) return;
		m_iSelectRoleID = 2;
		_Refresh();
	}
	
	void onSelect3 (){
		if (m_iSelectRoleID == 3) return;
		m_iSelectRoleID = 3;
		_Refresh();
	}
	
	void onOK (){
		Globals.It.ShowInput(setRoleName);
	}
	
	void _Refresh (){
		int iPos = m_iSelectRoleID-1;
		labelDesc.text = sDesc[iPos];
		Vector3 rolePos = roleTrans[iPos].localPosition;
		rolePos.z = -2;
		spriteFocus.transform.localPosition = rolePos;
		for (int i = 0; i < bigImageSpirte.Length; i++) {
			bigImageSpirte[i].gameObject.SetActive(i == iPos);
		}
	}
	
	void setRoleName (string sRoleName){
		if (string.IsNullOrEmpty(sRoleName) || sRoleName.Length < 3) {
			Globals.It.ShowWarn(7, 8, null);
		}
		else {
			Globals.It.ShowWaiting();
			
			Data_CreateRole mode = new Data_CreateRole();
			mode.profession = m_iSelectRoleID;
			mode.rolename = sRoleName;
			Globals.It.SendMsg(mode, Const_ICommand.CreateRole);
		}
	}
}
