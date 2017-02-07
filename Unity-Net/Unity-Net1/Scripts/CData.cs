using UnityEngine;
using System.Collections;

public class Data_Base {}

public class Data_UserRegister : Data_Base
{
	public string username { get; set; }
	public string password { get; set; }
}

public class Data_UserRegister_R : Data_Base
{
	public bool result { get; set; }
	public string message { get; set; }
}

public class Data_UserLogin : Data_Base
{
	public string username { get; set; }
	public string password { get; set; }
}

public class Data_UserLogin_R : Data_Base
{
	public class Data {
		public int userId { get; set; }
		public bool hasRole { get; set; }
		public int characterId { get; set; }
	}
	public bool result { get; set; }
	public string message { get; set; }
	public Data data { get; set; }
}

public class Data_CreateRole : Data_Base 
{
	public string rolename { get; set; }
	public int profession { get; set; }
}

public class Data_CreateRole_R : Data_Base 
{
	public class Data {
		public int newCharacterId { get; set; }
		public int userId { get; set; }
	}
	
	public bool result { get; set; }
	public string message { get; set; }
	public Data data { get; set; }
}

public class Data_RoleEnterGame : Data_Base
{
	public int characterId { get; set; }
}

public class Data_RoleEnterGame_R : Data_Base
{
	public class Data {
		public string name { get; set; }
		public int power { get; set; } 
		public int cid { get; set; }
		public int level { get; set; }
		public int exp { get; set; }
		public int maxexp { get; set; }
		public int yuanbao { get; set; }
		public int coin { get; set; }
		public int viplevel { get; set; }
		public int profession { get; set; }
		
		public int gas { get; set; }
	}
	
	public bool result { get; set; }
	public string message { get; set; }
	public Data data { get; set; }
}

public class Data_PlayerStat : Data_Base
{
	public int characterId { get; set; }
}

public class Data_PlayerStat_R : Data_Base
{
	public class Data {
		public int gold  { get; set; }
		public int profession  { get; set; }
		public int maxhuoli  { get; set; }
		public string rolename { get; set; }
		public int coin  { get; set; }
		public int characterId  { get; set; }
		public int level  { get; set; }
		public int maxexp  { get; set; }
		public int huoli  { get; set; }
		public int tili  { get; set; }
		public int exp  { get; set; }
		public int tilimax  { get; set; }
	}
	
	public bool result { get; set; }
	public string message { get; set; }
	public Data data { get; set; }
}

public class Data_MapData : Data_Base
{
	public int characterId { get; set; }
	public int index { get; set; }
}

public class Data_MapData_R : Data_Base
{
	public class Data {
		public int cityid { get; set; }
		public int[] citylist { get; set; }
	}
	
	public bool result { get; set; }
	public string message { get; set; }
	public Data data { get; set; }
}

public class Data_MapBattle : Data_Base {
	public int characterId { get; set; }
	public int zjid { get; set; }
}

public class Data_MapBattle_R : Data_Base {
	public class Data {
		public int battleResult { get; set; } // 1 succ ,2 fail
		public SetData setData { get; set; }
		public StartData[] startData { get; set; }
		public StepData[] stepData { get; set; }
	}
	
	public class SetData {
		public int star { get; set; }
		public int huoli { get; set; }
		public int exp { get; set; }
		public int coin { get; set; }
		public int[] item { get; set; }	// TEMPID
	}
	
	public class StartData {
		public int charId { get; set; }
		public int chaBattleId { get; set; }
		public string chaName { get; set; }
		public int chaLevel { get; set; }
		public int chaDirection { get; set; } // 1 DOWN, 2 UP
		public int chaCurrentHp { get; set; }
		public int chaTotalHp { get; set; }
		public int chaPos { get; set; }
		public int chaIcon { get; set; }
		public int difficulty { get; set; }
		public int chaCurrentPower { get; set; }
	}
	
	public class StepData {
		public int chaBattleId { get; set; }
		public int chaExpendHp { get; set; }
		public int chaId { get; set; }
		public int actionId { get; set; }
		public int chaCurrendHp { get; set; }
		public int chaTotalHp { get; set; }
		public int skill { get; set; }
		public int chaCurrentPower { get; set; }
		public int[] chaBuffArr { get; set; }
		public StepData_Enemy[] enemyChaArr { get; set; }
	}
	
	public class StepData_Enemy {
		public int enemyBattleId { get; set; }
		public int enemyChaId { get; set; }
		public int enemyActionId { get; set; }
		public int enemyChangeHp { get; set; }
		public int enemyCurrentHp { get; set; }
		public int enemyTotalHp { get; set; }
		public int enemyTxtEffectId { get; set; }
		public int enemyCurrentPower { get; set; }
		public int[] enemyBuffArr { get; set; }
	}
	
	public bool result { get; set; }
	public string message { get; set; }
	public Data data { get; set; }
}