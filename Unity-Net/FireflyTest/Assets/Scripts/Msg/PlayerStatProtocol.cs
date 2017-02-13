using UnityEngine;
using System.Collections;
// ¸üÐÂÍæ¼Ò×´Ì¬
public class PlayerStatProtocol : IProtocol {
	
	#region IProtocol implementation
	public void Process (Message_Body info)
	{
		Data_PlayerStat_R data = Globals.ToObject<Data_PlayerStat_R>(info.body);
		if(data != null) {
			if (data.result) {
				Globals.It.MainPlayer.proMain.UpdateStat(data.data);
				Globals.It.ShowMainView();
			}
			else {
				Globals.It.HideWaiting();
				Globals.It.ShowWarn(Const_ITextID.Msg_Tishi, data.message, null);
			}
		}
		else {
			Globals.It.HideWaiting();
		}
	}

	public int iCommand {
		get {
			return Const_ICommand.PlayerStat;
		}
	}
	#endregion
}
