using UnityEngine;
using System.Collections;
// 登录成功后的操作
public class DengluProtocol : IProtocol {
	
	#region IProtocol implementation
	public void Process (Message_Body info)
	{
		Data_UserLogin_R data = Globals.ToObject<Data_UserLogin_R>(info.body);
		if(data != null) {
			if (data.result) {
				Globals.It.DestoryDengluView();
				Globals.It.MainPlayer.proMain.SetLogin(data.data);  // 保存到Gloabs去
				if (!data.data.hasRole) {  // 第一次创建角色
					Globals.It.ShowKaiChangGifView();  // 开场白
					Globals.It.HideWaiting();
				}
				else {
					Globals.It.ShowEnterGameView();
				}
			}
			else {
				Globals.It.HideWaiting();
				Globals.It.ShowWarn(Const_ITextID.Msg_Tishi, 14, null);
			}
		}
	}

	public int iCommand {
		get {
			return Const_ICommand.UserLogin;
		}
	}
	#endregion
	
}
