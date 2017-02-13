using UnityEngine;
using System.Collections;
// 从服务器获得地图数据后 操作
public class GetMapDataProtocol : IProtocol {
	
	#region IProtocol implementation
	public void Process (Message_Body info)
	{
		Data_MapData_R data = Globals.ToObject<Data_MapData_R>(info.body);
		Globals.It.HideWaiting();
		if(data != null) {
			if (data.result) {
				Globals.It.MainPlayer.proMain.cityList = data.data.citylist;
				Globals.It.MainPlayer.proMain.bNeedRefresh = false;
				Globals.It.MainViewRefresh();
			}
			else {
				Globals.It.ShowWarn(Const_ITextID.Msg_Tishi, data.message, null);
			}
		}
	}

	public int iCommand {
		get {
			return Const_ICommand.GetMapData;
		}
	}
	#endregion
}
