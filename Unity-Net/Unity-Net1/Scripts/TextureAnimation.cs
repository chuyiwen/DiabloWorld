using UnityEngine;
using System.Collections;

public class TextureAnimation : MonoBehaviour {
	
	public UISprite 	target;
	public float		fWaitSecond = 0.15f;
	public bool 		bRepeart;
	public string 		sFormat;
	public int 			iStart;
	public int 			iCount;
	
	public GameObject 	eventReceiver;
	public string 		finishEvent;
		
	private int 		m_iCur = 0;
	
	void Start (){
		if (Globals.It.bTestView) {
			runAction();
		}
	}
	
	public void runAction () {
		if (target != null)
			StartCoroutine("_runAction");
	}

	IEnumerator _runAction (){
		while (bRepeart || (!bRepeart && m_iCur <= iCount + iStart)) {
			if(m_iCur < iCount + iStart) {
				string sName = string.Format(sFormat, m_iCur);
				target.spriteName = sName;
				target.MakePixelPerfect();
				m_iCur++;
				yield return new WaitForSeconds(fWaitSecond / Time.timeScale);
			}
			else{
				if (bRepeart) m_iCur = iStart;
				else {
					if (eventReceiver != null && !string.IsNullOrEmpty(finishEvent)) {
						eventReceiver.SendMessage(finishEvent, SendMessageOptions.DontRequireReceiver);
					}
					break;
				}
			}
		}
		//yield return null;
	}
}
