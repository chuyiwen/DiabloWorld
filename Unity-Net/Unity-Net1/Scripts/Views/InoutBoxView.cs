using UnityEngine;
using System.Collections;

public class InoutBoxView : MonoBehaviour {
	
	public UIInput	inputText;
	private System.Action<string> m_Handler;
	
	public void onShow (System.Action<string> handler){
		m_Handler = handler;
	}
	
	public void onClick (){
		if (m_Handler != null) {
			m_Handler(inputText.text);
		}
	}
	
}
