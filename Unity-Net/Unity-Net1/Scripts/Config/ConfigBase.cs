using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZhiyeJson {
	public int iprofession { get; set; }
	public string sMapBig { get; set; }
	public string sMapSmall { get; set; }
	public string sDesc { get; set; }
}

public class MapJson {
	public string dropicon { get; set; }
	public int yid  { get; set; }
	public int coin  { get; set; }
	public List<int> mconfig { get; set; }
	public string name { get; set; }
	public int levelrequired  { get; set; }
	public int resourceid  { get; set; }
	public int scene  { get; set; }
	public int dropid  { get; set; }
	public int priority  { get; set; }
	public int exp  { get; set; }
	public int icon  { get; set; }
	public int quality  { get; set; }
	public int id  { get; set; }
	public string desc { get; set; }
	
	public static MapJson Parse (LitJson.JsonData data){
		MapJson json = new MapJson();
		try {
			json.dropicon = (string)data["dropicon"];
			json.yid = (int)data["yid"];
			json.coin = (int)data["coin"];
			
			json.name = (string)data["name"];
			json.levelrequired = (int)data["levelrequired"];
			json.resourceid = (int)data["resourceid"];
			json.scene = (int)data["scene"];
			json.dropid = (int)data["dropid"];
			json.priority = (int)data["priority"];
			json.exp = (int)data["exp"];
			json.icon = (int)data["icon"];
			json.quality = (int)data["quality"];
			json.id = (int)data["id"];
			json.desc = (string)data["desc"];
			
			json.mconfig = new List<int>();
			LitJson.JsonData arr = data["mconfig"];
			int iarrCount = arr.Count;
			LitJson.JsonData arrItem;
			for(int i = 0; i < iarrCount; i++) {
				arrItem = arr[i];
				json.mconfig.Add((int)arrItem);
			}			
			/*
			mapJson.dropicon = (string)data["dropicon"];
			mapJson.yid = (int)data["yid"];
			mapJson.coin = (int)data["coin"];
			mapJson.mconfig = new List<int>();
			LitJson.JsonData arr = data["mconfig"];
			int iarrCount = arr.Count;
			for(int i = 0; i < iarrCount; i++) {
				mapJson.mconfig.Add((int)arr[i]);
			}
			//mapJson.mconfig = (List<int>)data["mconfig"];
			mapJson.name = (string)data["name"];
			mapJson.levelrequired = (int)data["levelrequired"];
			mapJson.resourceid = (int)data["resourceid"];
			mapJson.scene = (int)data["scene"];
			mapJson.dropid = (int)data["dropid"];
			mapJson.priority = (int)data["priority"];
			mapJson.exp = (int)data["exp"];
			mapJson.icon = (int)data["icon"];
			mapJson.quality = (int)data["quality"];
			mapJson.id = (int)data["id"];
			mapJson.desc = (string)data["desc"];
			*/
		}
		catch(System.Exception ex) {
			Debug.Log("Parse MapJson Error :" + ex.Message);
		}
		return json;
	}
}

public class HeroJson {
	/*
	 * 
	 {
            "attrType": 3, 
            "energy": 1, 
            "xy": -1, 
            "baseQuality": 1, 
            "skill": 100020, 
            "id": 15001, 
            "resourceid": 6000, 
            "type": 220000, 
            "maxenergy": 5, 
            "soulcount": 5, 
            "growpet": 15002, 
            "VitGrowth": 26, 
            "coin": 600, 
            "nickname": "阿卡拉", 
            "icon": 6000, 
            "ordSkill": 610101, 
            "level": 1, 
            "descript": "实力强大的女战士", 
            "soulrequired": 41001001, 
            "DexGrowth": 28, 
            "StrGrowth": 16, 
            "WisGrowth": 1
      } 
	 * */
	
	public int attrType { get; set; }
	public int energy { get; set; }
	public int xy { get; set; }
	public int baseQuality { get; set; }
	public int skill { get; set; }
	public int id { get; set; }
	public int resourceid { get; set; }
	
	public int type { get; set; }
	public int maxenergy { get; set; }
	public int soulcount { get; set; }
	public int growpet { get; set; }
	public int VitGrowth { get; set; }
	public int coin { get; set; }
	
	public string nickname { get; set; }
	public int icon { get; set; }
	public int ordSkill { get; set; }
	public int level { get; set; }
	
	public string descript { get; set; }
	public int soulrequired { get; set; }
	public int DexGrowth { get; set; }
	public int StrGrowth { get; set; }
	public int WisGrowth { get; set; }
	
	public static HeroJson Parse (LitJson.JsonData data) {
		HeroJson json = new HeroJson();
		try {
			json.attrType = (int)data["attrType"];
			json.energy = (int)data["energy"];
			json.xy = (int)data["xy"];
			json.baseQuality = (int)data["baseQuality"];
			json.skill = (int)data["skill"];
			
			json.id = (int)data["id"];
			json.resourceid = (int)data["resourceid"];
			json.type = (int)data["type"];
			json.maxenergy = (int)data["maxenergy"];
			json.soulcount = (int)data["soulcount"];
			
			json.growpet = (int)data["growpet"];
			json.VitGrowth = (int)data["VitGrowth"];
			json.coin = (int)data["coin"];
			json.nickname = (string)data["nickname"];
			json.icon = (int)data["icon"];
			
			json.ordSkill = (int)data["ordSkill"];
			json.level = (int)data["level"];
			json.descript = (string)data["descript"];
			json.soulrequired = (int)data["soulrequired"];
			json.DexGrowth = (int)data["DexGrowth"];
			
			json.StrGrowth = (int)data["StrGrowth"];
			json.WisGrowth = (int)data["WisGrowth"];
		}
		catch{
		}		
		return json;
	}
}

public class SkillJson 
{
	/*
	  {
        "weaponRequied": 0, 
        "skillName": "破甲", 
        "rangeType": 1, 
        "releaseType": 1, 
        "levelUpMoney": 0, 
        "attributeType": 1, 
        "releaseEffect": 61, 
        "skillId": 100008, 
        "throwEffectId": 52, 
        "distanceType": 1, 
        "releaseCD": 0, 
        "expendPower": 60, 
        "itemRequired": 0, 
        "levelRequired": 0, 
        "profession": 6, 
        "bearEffect": 8, 
        "effect": 100008, 
        "expendHp": 0, 
        "skillGroup": 0, 
        "icon": 22, 
        "targetType": 2, 
        "itemCountRequired": 0, 
        "skillDescript": "", 
        "level": 1, 
        "type": 0, 
        "skillType": 0, 
        "aoeEffectId": 0
    }	 
	 */
	
	public int weaponRequied  { get; set; }
	public string skillName { get; set; }
	public int rangeType  { get; set; }
	public int releaseType  { get; set; }
	public int levelUpMoney  { get; set; }
	public int attributeType  { get; set; }
	public int releaseEffect  { get; set; }
	public int skillId  { get; set; }
	public int throwEffectId  { get; set; }
	public int distanceType  { get; set; }
	public int releaseCD  { get; set; }
	public int expendPower  { get; set; }
	public int itemRequired  { get; set; }
	public int levelRequired  { get; set; }
	public int profession  { get; set; }
	public int bearEffect  { get; set; }
	public int effect  { get; set; }
	public int expendHp  { get; set; }
	public int skillGroup  { get; set; }
	public int icon  { get; set; }
	public int targetType  { get; set; }
	public int itemCountRequired  { get; set; }
	public string skillDescript { get; set; }
	public int level  { get; set; }
	public int type  { get; set; }
	public int skillType  { get; set; }
	public int aoeEffectId  { get; set; }
	
	public static SkillJson Parse (LitJson.JsonData data){
		SkillJson json = new SkillJson();
		try {
			json.weaponRequied = (int)data["weaponRequied"];
			json.skillName = (string)data["skillName"];
			json.rangeType = (int)data["rangeType"];
			json.releaseType = (int)data["releaseType"];
			json.levelUpMoney = (int)data["levelUpMoney"];
			json.attributeType = (int)data["attributeType"];
			json.releaseEffect = (int)data["releaseEffect"];
			json.skillId = (int)data["skillId"];
			json.throwEffectId = (int)data["throwEffectId"];
			json.distanceType = (int)data["distanceType"];
			json.releaseCD = (int)data["releaseCD"];
			json.expendPower = (int)data["expendPower"];
			json.itemRequired = (int)data["itemRequired"];
			json.levelRequired = (int)data["levelRequired"];
			json.profession = (int)data["profession"];
			json.bearEffect = (int)data["bearEffect"];
			json.effect = (int)data["effect"];
			json.expendHp = (int)data["expendHp"];
			json.skillGroup = (int)data["skillGroup"];
			json.icon = (int)data["icon"];
			json.targetType = (int)data["targetType"];
			json.itemCountRequired = (int)data["itemCountRequired"];
			json.skillDescript = (string)data["skillDescript"];
			json.level = (int)data["level"];
			json.type = (int)data["type"];
			json.skillType = (int)data["skillType"];
			json.aoeEffectId = (int)data["aoeEffectId"];
		}
		catch{
		}		
		return json;
	}
}

public class MonsterJson 
{
	public int MagicAttack  { get; set; }
	public int dropoutid  { get; set; }
	public string skill { get; set; }
	public int id  { get; set; }
	public int speak  { get; set; }
	public int Dodge  { get; set; }
	public int Hit  { get; set; }
	public int expbound  { get; set; }
	public int resourceid  { get; set; }
	public int MagicDefense  { get; set; }
	public int PhysicalAttack  { get; set; }
	public int type  { get; set; }
	public int viptype  { get; set; }
	public int hp  { get; set; }
	public int difficulty  { get; set; }
	public int moveable  { get; set; }
	public string nickname { get; set; }
	public int PhysicalDefense  { get; set; }
	public int icon  { get; set; }
	public int ordSkill  { get; set; }
	public int Force  { get; set; }
	public int level  { get; set; }
	public string descript { get; set; }
	public int mp  { get; set; }
	public int Speed  { get; set; }
	
	public static MonsterJson Parse (LitJson.JsonData data) {
		MonsterJson json = new MonsterJson();
		try {
			json.MagicAttack = (int)data["MagicAttack"];
			json.dropoutid = (int)data["dropoutid"];
			json.skill = (string)data["skill"];
			json.id = (int)data["id"];
			json.speak = (int)data["speak"];
			json.Dodge = (int)data["Dodge"];
			json.Hit = (int)data["Hit"];
			json.expbound = (int)data["expbound"];
			json.resourceid = (int)data["resourceid"];
			json.MagicDefense = (int)data["MagicDefense"];
			json.PhysicalAttack = (int)data["PhysicalAttack"];
			json.type = (int)data["type"];
			json.viptype = (int)data["viptype"];
			json.hp = (int)data["hp"];
			json.difficulty = (int)data["difficulty"];
			json.moveable = (int)data["moveable"];
			json.nickname = (string)data["nickname"];
			json.PhysicalDefense = (int)data["PhysicalDefense"];
			json.icon = (int)data["icon"];
			json.ordSkill = (int)data["ordSkill"];
			json.Force = (int)data["Force"];
			json.level = (int)data["level"];
			json.descript = (string)data["descript"];
			json.mp = (int)data["mp"];
			json.Speed = (int)data["Speed"];
		}
		catch {}
		return json;
	}
}

public abstract class ConfigBase : MonoBehaviour {
	
	public abstract string sPath {
		get;
	}
	
	public abstract void Parse (Object asset);
		
	void Start () {
		gameObject.isStatic = true;
		enabled = false;
		
		_LoadConfig();
	}
	
	void _LoadConfig (){
		StartCoroutine(Globals.It.BundleMgr.CreateObject(kResource.Config, Const_SPath.Path_Config, sPath, Parse));
	}
}
