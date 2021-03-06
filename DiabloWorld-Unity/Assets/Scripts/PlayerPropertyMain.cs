using UnityEngine;
using System.Collections;
// 玩家信息类
public class PlayerPropertyMain  {
    // 玩家属性
	public int 		iUserID { get; set; }
	public int 		iCharacterId { get; set; }
	public bool		bHasRole { get; set; }
	public string 	sRoleName { get; set; }
	public int 		iLevel { get; set; }
	public float 	fExp { get; set; }
	public float 	fMaxExp { get; set; }
	public int 		iGold { get; set; }
	public int 		iCoin { get; set; }
	public int 		iTili { get; set; }
	public int 		iTiliMax { get; set; }
	public int 		iHuoli { get; set; }
	public int 		iVipLevel { get; set; }
	public int 		iProfession { get; set; }
	
	public int[]	cityList { get; set; }
	public int		iCurrentMapIndex { get; set; }
	public bool		bNeedRefresh { get; set; }		// Refresh Player stat是否需要刷新数据
	
    // 设置登录成功后的数据
	public void SetLogin(Data_UserLogin_R.Data data){
		iUserID = data.userId;
		iCharacterId = data.characterId;
		bHasRole = data.hasRole;
	}
	// 设置创建角色成功后的数据
	public void SetCreateRole (Data_CreateRole_R.Data data){
		iCharacterId = data.newCharacterId;
		iUserID = data.userId;
	}
	// 设置角色进入场景后的数据
	public void SetRoleEnterGame (Data_RoleEnterGame_R.Data data){
		iCharacterId = data.cid;
		sRoleName = data.name;
		iLevel = data.level;
		fExp = data.exp;
		fMaxExp = data.maxexp;
		iGold = data.yuanbao;
		iCoin = data.coin;
		iTili = data.power;
		iHuoli = data.gas;
		iVipLevel = data.viplevel;
		iProfession = data.profession;
		bNeedRefresh = true;  // 需要刷新数据
	}
	// 设置角色更新状态后的数据
	public void UpdateStat (Data_PlayerStat_R.Data data){
		iCharacterId = data.characterId;
		iCoin = data.coin;
		iGold = data.gold;
		iProfession = data.profession;
		sRoleName = data.rolename;
		iLevel = data.level;
		fMaxExp = data.maxexp;
		fExp = data.exp;
		iHuoli = data.huoli;
		iTili = data.tili;
		iTiliMax = data.tilimax;
		bNeedRefresh = false;
	}
	
    // 设置角色战斗完毕后的数据
	public void UpdateFromBattle (Data_MapBattle_R.Data data){
		iCoin += data.setData.coin;
		iTili += data.setData.huoli;
		fExp += data.setData.exp;
		if (fExp > fMaxExp) bNeedRefresh = true;  // 升级了，这里需要刷新数据
		if (data.battleResult == 2) { return; }  // 现在只有三个地图
        // 下面是更新地图显示  已经通关的点星星 没通关的没有
		if (cityList == null || cityList.Length <= 0) {
			cityList = new int[1];
			cityList[0] = data.setData.star;
		}
		else if (cityList.Length >= iCurrentMapIndex + 1) {
			int iTempStar = cityList[iCurrentMapIndex];
			if (iTempStar < data.setData.star) {
				cityList[iCurrentMapIndex] = data.setData.star;
			}
		}
		else {
			int[] newCityList = new int[cityList.Length + 1];
			for(int i = 0; i < cityList.Length; i++) {
				newCityList[i] = cityList[i];
			}
			newCityList[cityList.Length] = data.setData.star;
			this.cityList = newCityList;
		}
	}
}
