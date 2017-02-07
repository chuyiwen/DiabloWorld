using UnityEngine;
using System.Collections;

public class DengluView : MonoBehaviour {
	
	public UIInput inputUserName, inputPassword;
	
	void Start (){
		read();
	}
	
	void onClickLogin (){
		string sUserName = inputUserName.text.Trim();
		string sPassword = inputPassword.text.Trim();
		if (string.IsNullOrEmpty(sUserName) || sUserName.Length < 2) {
			Globals.It.ShowWarn(Const_ITextID.Msg_Jinggao, 3, null);
			return;
		}
		if (string.IsNullOrEmpty(sPassword) || sPassword.Length < 4) {
			Globals.It.ShowWarn(Const_ITextID.Msg_Jinggao, 4, null);
			return;
		}
		System.Action sendMsg = () => {
			Data_UserLogin data = new Data_UserLogin()
			{
				username = sUserName,
				password = sPassword
			};
			Globals.It.SendMsg(data, Const_ICommand.UserLogin);
		};
		Globals.It.ShowWaiting();
		if (!Globals.It.Connected) {
			Globals.It.Connect(sendMsg);
		}
		else {
			sendMsg();
		}
	}
	
	public void save (){
		PlayerPrefs.SetString("KEY_USERNAME", inputUserName.text);
		PlayerPrefs.SetString("KEY_USERNAME2", inputPassword.text);
	}
	
	public void read (){
		string susername = PlayerPrefs.GetString("KEY_USERNAME", "");
		string susername2 = PlayerPrefs.GetString("KEY_USERNAME2", "");
		inputUserName.text = susername;
		inputPassword.text = susername2;
	}
}
