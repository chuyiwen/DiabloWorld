using UnityEngine;
using System.Collections;
// 进入主场景
public class RoleEnterGameProtocol : IProtocol {

	#region IProtocol implementation
	public void Process (Message_Body info)
	{
		Data_RoleEnterGame_R data = Globals.ToObject<Data_RoleEnterGame_R>(info.body);
		if(data != null) {
			if (data.result) {
				Globals.It.MainPlayer.proMain.SetRoleEnterGame(data.data);
				Globals.It.ShowMainView();
			}
			else {
				Globals.It.HideWaiting();
				Globals.It.ShowWarn(Const_ITextID.Msg_Tishi, data.message, null);
			}
		}
	}

	public int iCommand {
		get {
			return Const_ICommand.RoleEnterGame;
		}
	}
	#endregion
}
