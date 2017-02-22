using UnityEngine;
using System.Collections;
// 战斗卡牌  主角
public class BattlePlayer : MonoBehaviour {
	
	public UIAtlas[] 	atlas = new UIAtlas[7]; 
	public UISprite		spriteKapai;
	public UISlider		bloodSlider;
	
	[HideInInspector]
	public Data_MapBattle_R.StartData playerData;
	
	public void show (Data_MapBattle_R.StartData data) {
		playerData = data;
		UIAtlas atlasUse = null;
		if (data.chaIcon <= 3 ) {  // 根据
			atlasUse = atlas[0];
		}
		else if (data.chaIcon <= 6003) {
			atlasUse = atlas[1];
		}
		else if (data.chaIcon <= 6013) {
			atlasUse = atlas[2];
		}
		else if (data.chaIcon <= 6024) {
			atlasUse = atlas[3];
		}
		else if (data.chaIcon <= 6038) {
			atlasUse = atlas[4];
		}
		else if (data.chaIcon <= 6046) {
			atlasUse = atlas[5];
		}
		else if (data.chaIcon <= 6056) {
			atlasUse = atlas[6];
		}
		spriteKapai.atlas = atlasUse;  // 显示卡牌
		spriteKapai.spriteName = "qs_" + string.Format("{0:0000}", data.chaIcon);
		bloodSlider.sliderValue = 1.0f;
	}
	// 当卡牌HP发生改变时候调用
	public void ChangeHP (int changeHp){
		if (changeHp > 0) {  // 加血
			int addHpMax = playerData.chaTotalHp - playerData.chaCurrentHp;
			if (changeHp > addHpMax) {
				changeHp = addHpMax;
			}
		}
		else {  // 掉血
			int iCutHpMax = playerData.chaTotalHp;
			if (iCutHpMax + changeHp < 0) changeHp = iCutHpMax;
			changeHp = -Mathf.Abs(changeHp);
		}
		float fStart, fEnd;
		fStart = (float)playerData.chaCurrentHp / (float)playerData.chaTotalHp;  // 掉血前
		playerData.chaCurrentHp += changeHp;
		fEnd = (float)playerData.chaCurrentHp / (float)playerData.chaTotalHp;  // 掉血后
		if (playerData.chaCurrentHp != playerData.chaTotalHp) {
            // 血量用itween动画  改变过程调用onUpdate 结束时候调用onComplete
            iTween.ValueTo(gameObject, iTween.Hash("from",fStart, "to", fEnd, "time", 0.5f, "onupdate", "onUpdate", "oncomplete", "onComplete"));
		}
	}
	
	void onUpdate(float newValue){  // 掉血中
		bloodSlider.sliderValue = newValue;
	}
	
	void onComplete (){  // 掉血完
		if (playerData.chaCurrentHp <= 0) {
			gameObject.SetActive(false);
		}
	}
}
