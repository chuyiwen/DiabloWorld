using UnityEngine;
using System.Collections;
// 登录视图
public class DengluView : MonoBehaviour {
	
	public UIInput inputUserName, inputPassword;
	
	void Start (){
		read();  // 上次登录的角色
	}
	
	void onClickLogin (){  // 点击登录的时候
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
			Globals.It.SendMsg(data, Const_ICommand.UserLogin);  // 发送给服务端
		};
		Globals.It.ShowWaiting();
		if (!Globals.It.Connected) {  // sendMsg是成功连接时候的回调
			Globals.It.Connect(sendMsg);  // 尚未连接 继续连接
		}
		else {
			sendMsg();  // 成功连接后 发送服务端 登录的消息
		}
	}
	
	public void save (){
		PlayerPrefs.SetString("KEY_USERNAME", inputUserName.text);  // 保存
		PlayerPrefs.SetString("KEY_USERNAME2", inputPassword.text);
	}
	
	public void read (){
		string susername = PlayerPrefs.GetString("KEY_USERNAME", "");  // 读取
		string susername2 = PlayerPrefs.GetString("KEY_USERNAME2", "");
		inputUserName.text = susername;
		inputPassword.text = susername2;
	}
}
