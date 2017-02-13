using UnityEngine;
using System.Collections;

public class TestAnimation : MonoBehaviour {
	
	public string 	sFormat = "0241-a9b78cf6-000{0}{1}";
	public int 		iStart = 0;
	public int 		iCount = 7;
	public float	fWaitSecond = 0.15f;
	
	private UISprite 	m_Sprite;
	private int 		m_iCur = 0;
	
	// Use this for initialization
	void Start () {
		m_Sprite = GetComponent<UISprite>();
		StartCoroutine("runAction");
	}

	IEnumerator runAction (){
		while (true) {
			if(m_iCur < iCount + iStart) {
				string sName = string.Format(sFormat, m_iCur / 10, m_iCur % 10);
				m_Sprite.spriteName = sName;
				m_Sprite.MakePixelPerfect();
				m_iCur++;
			}
			else{
				m_iCur = iStart;
				runAction();
			}
			yield return new WaitForSeconds(fWaitSecond);
		}
		//yield return null;
	}
}
