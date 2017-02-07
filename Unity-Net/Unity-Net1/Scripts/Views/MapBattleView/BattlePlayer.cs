using UnityEngine;
using System.Collections;

public class BattlePlayer : MonoBehaviour {
	
	public UIAtlas[] 	atlas = new UIAtlas[7]; 
	public UISprite		spriteKapai;
	public UISlider		bloodSlider;
	
	[HideInInspector]
	public Data_MapBattle_R.StartData playerData;
	
	public void show (Data_MapBattle_R.StartData data) {
		playerData = data;
		UIAtlas atlasUse = null;
		if (data.chaIcon <= 3 ) {
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
		spriteKapai.atlas = atlasUse;
		spriteKapai.spriteName = "qs_" + string.Format("{0:0000}", data.chaIcon);
		bloodSlider.sliderValue = 1.0f;
	}
	
	public void ChangeHP (int changeHp){
		if (changeHp > 0) {
			int addHpMax = playerData.chaTotalHp - playerData.chaCurrentHp;
			if (changeHp > addHpMax) {
				changeHp = addHpMax;
			}
		}
		else {
			int iCutHpMax = playerData.chaTotalHp;
			if (iCutHpMax + changeHp < 0) changeHp = iCutHpMax;
			changeHp = -Mathf.Abs(changeHp);
		}
		float fStart, fEnd;
		fStart = (float)playerData.chaCurrentHp / (float)playerData.chaTotalHp;
		playerData.chaCurrentHp += changeHp;
		fEnd = (float)playerData.chaCurrentHp / (float)playerData.chaTotalHp;
		if (playerData.chaCurrentHp != playerData.chaTotalHp) {
			iTween.ValueTo(gameObject, iTween.Hash("from",fStart, "to", fEnd, "time", 0.5f, "onupdate", "onUpdate", "oncomplete", "onComplete"));
		}
	}
	
	void onUpdate(float newValue){
		bloodSlider.sliderValue = newValue;
	}
	
	void onComplete (){
		if (playerData.chaCurrentHp <= 0) {
			gameObject.SetActive(false);
		}
	}
}
