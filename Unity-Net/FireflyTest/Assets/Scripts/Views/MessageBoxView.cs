using UnityEngine;
using System.Collections;
// 消息弹幕
public class MessageBoxView : MonoBehaviour {
	
	public UILabel		labelTitle, labelText;
	
	private System.Action	m_BtnCallback;
	// 显示 并且 设置回调函数
	public void show (string strTitle, string sText, System.Action callback){
		m_BtnCallback = callback;
		labelTitle.text = strTitle.Trim();
		labelText.text = sText.Trim();
	}
	
	public void onClick (){
		if (m_BtnCallback != null) {
			m_BtnCallback();
		}
		Globals.It.HideWarn();
	}
}
