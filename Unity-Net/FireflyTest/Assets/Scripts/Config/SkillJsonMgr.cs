using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillJsonMgr : ConfigBase {
	
	private Dictionary<int, SkillJson> m_jsons = new Dictionary<int, SkillJson>();
	
	#region implemented abstract members of ConfigBase
	public override string sPath {
		get {
			return "skill.json";
		}
	}

	public override void Parse (Object asset)
	{
		if (asset != null && asset is TextAsset) {
			TextAsset textAsset = (TextAsset)asset;		
			string sJson = textAsset.text;
			LitJson.JsonReader jsonR = new LitJson.JsonReader(sJson);
			LitJson.JsonData jsonD = LitJson.JsonMapper.ToObject(jsonR);
			int iCount = jsonD.Count;
			for(int i = 0; i < iCount; i++)  // 将解析到的数据放到dict
            {
				LitJson.JsonData itemData = jsonD[i];
				if (itemData == null) continue;
				SkillJson jsonItem = SkillJson.Parse(itemData);
				m_jsons.Add(jsonItem.skillId, jsonItem);
			}
			Globals.It.BundleMgr.UnLoadBundleLocal(asset);
		}
	}
	#endregion
	
	public SkillJson GetSkill (int skillid) {
		if (m_jsons.ContainsKey(skillid)) {
			return m_jsons[skillid];
		}
		return null;
	}
}
