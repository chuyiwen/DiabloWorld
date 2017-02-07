using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterJsonMgr : ConfigBase {
	
	private Dictionary<int, MonsterJson> m_jsons = new Dictionary<int, MonsterJson>();
	
	#region implemented abstract members of ConfigBase
	public override string sPath {
		get {
			return "monster.json";
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
			for(int i = 0; i < iCount; i++) {
				LitJson.JsonData itemData = jsonD[i];
				if (itemData == null) continue;
				MonsterJson jsonItem = MonsterJson.Parse(itemData);
				m_jsons.Add(jsonItem.id, jsonItem);
			}
			Globals.It.BundleMgr.UnLoadBundleLocal(asset);
		}
	}
	#endregion
}
