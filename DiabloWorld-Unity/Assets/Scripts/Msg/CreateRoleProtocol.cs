using UnityEngine;
using System.Collections;
// 创建角色成功后的操作
public class CreateRoleProtocol : IProtocol {
	
	#region IProtocol implementation
	public void Process (Message_Body info)
	{
		Data_CreateRole_R data = Globals.ToObject<Data_CreateRole_R>(info.body);
		Globals.It.HideWaiting();
		if(data != null) {
			if (data.result) {
				Globals.It.HideInput();
				Globals.It.MainPlayer.proMain.SetCreateRole(data.data);  // 保存到Global去
				Globals.It.DestoryCreateRoleView ();
				Globals.It.ShowEnterGameView();
			}
			else {
				Globals.It.ShowWarn(Const_ITextID.Msg_Tishi, Globals.It.ErrorHintMgr.GetErrorString(data.message), null);
			}
		}
	}

	public int iCommand {
		get {
			return Const_ICommand.CreateRole;
		}
	}
	#endregion
}
