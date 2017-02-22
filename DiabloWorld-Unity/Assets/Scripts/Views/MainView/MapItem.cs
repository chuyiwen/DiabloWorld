using UnityEngine;
using System.Collections;

public class MapItem : MonoBehaviour {
	
	const string SXingxing_Hui = "xingxinghui";  // 灰色星星
	const string SXingxing_Liang = "xingxingliang";	 // 亮星星
	string[] SBg = {"paizi", "paizi1", "paizi2", "paizi3"};  // 牌子
	
	public UISprite spriteBg,spriteStar1,spriteStar2,spriteStar3;
	public UILabel	labelTitle;
	public UIImageButton imageBtn;
	
	private MapJson m_mapJson;
	private int		m_mapIndex;
	private int		m_pingji = -1;
	
	public void SetData (MapJson json, int mapIndex){  // 设置成灰色星星
		m_mapJson = json;
		m_mapIndex = mapIndex;
		spriteStar1.spriteName = SXingxing_Hui;
		spriteStar2.spriteName = SXingxing_Hui;
		spriteStar3.spriteName = SXingxing_Hui;
		spriteStar1.enabled = false;
		spriteStar2.enabled = false;
		spriteStar3.enabled = false;
		labelTitle.text = json.name;
		imageBtn.GetComponent<BoxCollider>().enabled = false;
	}
	
	public void Refresh (int iPingJi){  // 更新 评级
		if (m_pingji == iPingJi) return;
		m_pingji = iPingJi;
		imageBtn.GetComponent<BoxCollider>().enabled = true;
		spriteBg.spriteName = SBg[iPingJi];
		imageBtn.normalSprite = SBg[iPingJi];
		switch (iPingJi) {
		case 0:  // 没星星
		{
			spriteBg.spriteName = SBg[1];
			imageBtn.normalSprite = SBg[1];
			break;
		}
		case 1:
		{
			spriteStar1.spriteName = SXingxing_Liang;
			spriteStar1.enabled = true;
			break;
		}
		case 2:
		{
			spriteStar1.spriteName = SXingxing_Liang;
			spriteStar2.spriteName = SXingxing_Liang;
			spriteStar1.enabled = true;
			spriteStar2.enabled = true;
			break;
		}
		case 3:
		{
			spriteStar1.spriteName = SXingxing_Liang;
			spriteStar2.spriteName = SXingxing_Liang;
			spriteStar3.spriteName = SXingxing_Liang;
			spriteStar1.enabled = true;
			spriteStar2.enabled = true;
			spriteStar3.enabled = true;
			break;
		}
		}
		UISprite imgSprite = imageBtn.GetComponentInChildren<UISprite>();
		if (imgSprite != null) {
			imgSprite.spriteName = imageBtn.normalSprite;
			imgSprite.MakePixelPerfect();
		}
	}
	
	public void onClick (){
		Globals.It.MainPlayer.proMain.iCurrentMapIndex = m_mapIndex;
		Globals.It.ShowRenwuView(m_mapJson);  // 进入战斗
	}
}
