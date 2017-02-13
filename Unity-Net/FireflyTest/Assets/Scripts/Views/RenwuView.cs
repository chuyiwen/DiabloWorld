using UnityEngine;
using System.Collections;
// 战斗 界面
public class RenwuView : MonoBehaviour {
	
	public UILabel 	labelMapName, labelMapDesc,
					labelDiren, labelDirenDesc,
					labelExp, labelGold, labelWupin;
	
	private MapJson m_mapJson;
	
	void onReturn (){
		GameObject.DestroyImmediate(gameObject);
	}
	
	void onFire (){  // 开打 发送给服务器
		Globals.It.ShowWaiting();
        Data_MapBattle data = new Data_MapBattle()
        {
            characterId = Globals.It.MainPlayer.proMain.iCharacterId,
            zjid = m_mapJson.id
		};		
		Globals.It.SendMsg(data, Const_ICommand.MapBattle);
		onReturn();
	}
	
	public void show (MapJson mapjson){  // 显示
		m_mapJson = mapjson;
		labelMapName.text = m_mapJson.name;
		labelMapDesc.text = m_mapJson.desc;  // 描述
		labelExp.text = string.Format(Globals.It.LanguageMgr.GetString(9), m_mapJson.exp);
		labelGold.text = string.Format(Globals.It.LanguageMgr.GetString(10), m_mapJson.coin);
		if (string.IsNullOrEmpty(m_mapJson.dropicon) || m_mapJson.dropicon == "\r") {
			labelWupin.text = Globals.It.LanguageMgr.GetString(11);
		}
		else{
			// Set Item.name
			labelWupin.text = "";
		}
		labelDiren.text = Globals.It.LanguageMgr.GetString(12);
	}
}
