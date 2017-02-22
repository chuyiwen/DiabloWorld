using UnityEngine;
using System.Collections;
// 角色状态视图  等级 名字  元宝 金币 体力 矿石
public class PlayerStat : MonoBehaviour {
	
	public UILabel 		labelLevel, labelName, labelYuanbao, labelCoin, labelTili, labelGas;
	public UISlider		expSlider;
	
	public void Refresh (){  // 刷新角色数据
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
