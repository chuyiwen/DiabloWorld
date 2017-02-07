using UnityEngine;
using System.Collections;

public class PlayerStat : MonoBehaviour {
	
	public UILabel 		labelLevel, labelName, labelYuanbao, labelCoin, labelTili, labelGas;
	public UISlider		expSlider;
	
	public void Refresh (){
		PlayerPropertyMain proMain = Globals.It.MainPlayer.proMain;
		labelLevel.text = proMain.iLevel.ToString();
		labelName.text = proMain.sRoleName;
		labelYuanbao.text = proMain.iGold.ToString();
		labelCoin.text = proMain.iCoin.ToString();
		labelTili.text = proMain.iTili.ToString();
		labelGas.text = proMain.iHuoli.ToString();
		expSlider.sliderValue = proMain.fExp / proMain.fMaxExp;
	}
	
	public void onClick (){
		Debug.Log("onClick");
	}
}
