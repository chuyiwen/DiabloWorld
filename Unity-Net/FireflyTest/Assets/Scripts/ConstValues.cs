using UnityEngine;
using System.Collections;

// Commond命令号
public class Const_ICommand {
	public const int UserLogin = 101;  // 登录
	public const int CreateRole = 102;  // 创建角色
	public const int RoleEnterGame = 103;  // 进入房间
	public const int PlayerStat = 105;  // 更新玩家状态
	public const int GetMapData = 4500;  // 获取地图信息
	public const int MapBattle = 4501;  // 获取地图战斗信息
}

public class Const_SPath {
	public const string		Path_Config = "Proto";
}

public class Const_ITextID {
	public const int 	Msg_Tishi = 1;  // 提示
	public const int	Msg_Jinggao = 2;  // 警告
}
