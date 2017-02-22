using UnityEngine;
using System.Collections;
// 登录管理类
public class LogicMgr  {
	
	public static void OnLogin (Data_UserLogin_R data){
		Globals.It.HideWaiting();
		if(data != null) {
			if (data.result) {
				Globals.It.MainPlayer.proMain.SetLogin(data.data);
				if (data.data.hasRole) {
					// TODO 已经存在角色
				}
				else {
				    // TODO 第一次创建
				}
			}
			else {
				Globals.It.ShowWarn(Const_ITextID.Msg_Tishi, data.message, null);
			}
		}
	}
	
}
