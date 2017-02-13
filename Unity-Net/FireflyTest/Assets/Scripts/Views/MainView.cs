using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// 主场景
public class MainView : MonoBehaviour {
	
	public UIGrid 		gridMapItemParent;
	public GameObject 	gridChildItem;
	public PlayerStat	playerStat;
	public UIPanel		mapPanel;

	private List<MapItem> m_MapItems = new List<MapItem>();
	
	void Start (){
		RefreshPlayerStat ();
	}
    // 一下是按钮事件
	void onShouye (){// 首页
		Globals.It.ShowWarn(1, 13, null);
	}
	
	void onJingji (){// 竞技
		Globals.It.ShowWarn(1, 13, null);
	}
	
	void onDuanxin (){// 短信
		Globals.It.ShowWarn(1, 13, null);
	}
	
	void onChongzhi (){//充值
		Globals.It.ShowWarn(1, 13, null);
	}
	
	void onGuYong (){//雇佣
		Globals.It.ShowWarn(1, 13, null);
	}
	
	void onHeCheng (){// 合成
		Globals.It.ShowWarn(1, 13, null);
	}

	public void RefreshPlayerStat (){
		playerStat.Refresh();
	}
	
	public void RefreshMapStat (){  // 刷新主场景地图状态
		int[] cityList = Globals.It.MainPlayer.proMain.cityList;
		if (cityList != null && cityList.Length > 0) {
			int iCount = cityList.Length;
			for(int i = 0; i < iCount; i++) {
				m_MapItems[i].Refresh(cityList[i]);  // 设置星星 评级
			}
			if (Globals.It.MapJsonMgr.iCount > iCount) {
				m_MapItems[iCount].Refresh(0);  // 还没有打过
			}
		}		
		else {
			m_MapItems[0].Refresh(0);
		}
	}
	
	public void GetMapData (){  //向服务器获取主场景地图信息
		Data_MapData mode = new Data_MapData();
		mode.characterId = Globals.It.MainPlayer.proMain.iCharacterId;
		mode.index = 0;
		Globals.It.SendMsg(mode, Const_ICommand.GetMapData);
	}
	
	public void InitMap (){  // 初始化地图（主场景）
		
		List<MapJson> mapjsons = Globals.It.MapJsonMgr.MapJsons;
		int iMapCount = mapjsons.Count;
		int iObjectCount = 1 + (iMapCount - 2) / 3;  // 2
		bool bMore = false;
		if ((iMapCount-2) % 3 > 0) {  // 1个？ > 0
			bMore = true;
		}
		int iPos = 0, iOffset = 0;
		for (; iPos < iObjectCount; iPos++) {
			GameObject gridItem = (GameObject)GameObject.Instantiate(gridChildItem);
			MapItemParent itemParent = gridItem.GetComponent<MapItemParent>();
			if (iPos == 0) {				
				m_MapItems.AddRange(itemParent.Init(0, 1));  // 0 1号位置
				iOffset = 2;
			}
			else{ // 1
				m_MapItems.AddRange(itemParent.Init(iOffset, iOffset + 2));  // 2 4 ？？  糊涂了不懂这里
				iOffset += 3;
			}
			NGUIUtility.SetParent(gridMapItemParent.transform, gridItem.transform);
		}
		if (bMore) {
			GameObject gridItem = (GameObject)GameObject.Instantiate(gridChildItem);
			MapItemParent itemParent = gridItem.GetComponent<MapItemParent>();
			m_MapItems.AddRange(itemParent.Init(iOffset, iMapCount-1));
			NGUIUtility.SetParent(gridMapItemParent.transform, gridItem.transform);
		}
		gridMapItemParent.Reposition();

        //dragPanel.MoveRelative(new Vector3(0, -1200, 1));
    }
}
