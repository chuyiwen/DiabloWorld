using UnityEngine;
using System.Collections;
// 开场动画界面
public class KaiChangView : MonoBehaviour {
	
	public UISprite[] spriteLabels = new UISprite[4];
	public UILabel		labelWWW;
	public string		sText;
	// 1 TEXT1
	// 2 TEXT2
	// 3 TEXTURE2
	// 4 TEXT3
	// 5 NULL
	// 6 TEXTURE3
	// 7 TEXT4
	// 8 NULL
	// 9 TEXTURE4
	// 10 SHOWBTN
	
	void Start () {
		labelWWW.alpha = 0;
		for(int i = 0; i < spriteLabels.Length; i++) {
			spriteLabels[i].alpha = 0;
		}
	}
	
	void onIgnore (){
		this.gameObject.SetActive(false);
		Globals.It.ShowCreateRoleView();
		GameObject.DestroyObject(gameObject);
	}
	
	void showWWW (){
		labelWWW.text = sText;
		labelWWW.GetComponent<TypewriterEffect>().enabled = true;  // 打字效果
	}
}
