using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZhiyeJsonMgr : ConfigBase {
	
	private Dictionary<int, ZhiyeJson> m_jsons = new Dictionary<int, ZhiyeJson>();
	
	#region implemented abstract members of ConfigBase
	public override string sPath {
		get {
			return "zhiye.json";
		}
	}

	public override void Parse (Object asset)
	{
		if (asset != null && asset is TextAsset) {
			TextAsset textAsset = (TextAsset)asset;		
			var lists = LitJson.JsonMapper.ToObjects<ZhiyeJson>(textAsset.text);
			m_jsons = new Dictionary<int, ZhiyeJson>();
			if (lists != null && lists.Count > 0) {
				lists.ForEach(item=>{
					m_jsons.Add(item.iprofession, item);
				});
			}
			Globals.It.BundleMgr.UnLoadBundleLocal(asset);
		}
	}
	#endregion
	
}