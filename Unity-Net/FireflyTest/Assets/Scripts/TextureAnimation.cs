using UnityEngine;
using System.Collections;
// 图片动画 用于跳转
public class TextureAnimation : MonoBehaviour {
	
	public UISprite 	target;
	public float		fWaitSecond = 0.15f;
	public bool 		bRepeart;
	public string 		sFormat;
	public int 			iStart;  // 0
	public int 			iCount;  // 渐变次数
	
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

	IEnumerator _runAction (){  // 由小到大效果   这里可以考虑使用dotween类的插件
		while (bRepeart || (!bRepeart && m_iCur <= iCount + iStart)) {
			if(m_iCur < iCount + iStart) {
				string sName = string.Format(sFormat, m_iCur);
				target.spriteName = sName;
				target.MakePixelPerfect();  // 调整大小
				m_iCur++;  // 增加次数  m_iCur < iCount + iStart
                yield return new WaitForSeconds(fWaitSecond / Time.timeScale);  // 渐变
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
