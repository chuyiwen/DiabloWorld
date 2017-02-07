using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroJsonMgr : ConfigBase {
	
	private Dictionary<int, HeroJson> m_heros = new Dictionary<int, HeroJson>();
	
	#region implemented abstract members of ConfigBase
	public override string sPath {
		get {
			return "hero.json";
		}
	}

	public override void Parse (Object asset)
	{
		if (asset != null && asset is TextAsset) {
			TextAsset textAsset = (TextAsset)asset;		
			string sJson = textAsset.text;
			LitJson.JsonReader jsonR = new LitJson.JsonReader(sJson);
			LitJson.JsonData jsonD = LitJson.JsonMapper.ToObject(jsonR);
			foreach(string skey in jsonD.Keys) {
				LitJson.JsonData itemData =  jsonD[skey];
				if (itemData == null) continue;
				HeroJson heroItem = HeroJson.Parse(itemData);
				m_heros.Add(heroItem.id, heroItem);
			}		
			
			Globals.It.BundleMgr.UnLoadBundleLocal(asset);
		}
	}
	#endregion
	
}
