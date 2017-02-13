using UnityEngine;
using System.Collections;
// ’Ω∂∑Ω· ¯ œ‘ æΩ±¿¯
public class MapBattleProtocol : IProtocol {
	
	#region IProtocol implementation
	public void Process (Message_Body info)
	{
		Data_MapBattle_R data = Globals.ToObject<Data_MapBattle_R>(info.body);
		if(data != null) {
			if (data.result) {
				Globals.It.DestoryMainView();
				Globals.It.HideWaiting();
				Globals.It.ShowBattleRecordView(data);
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
			return Const_ICommand.MapBattle;
		}
	}
	#endregion
}
