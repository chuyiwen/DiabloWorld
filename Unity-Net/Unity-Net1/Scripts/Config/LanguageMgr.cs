using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LanguageMgr : ConfigBase {
	
	private Dictionary<int, TLanguage> m_TLanguages;

	public class TLanguage {
		public int iID { get; set; }
		public string sText { get; set; }
	}
	
	public override string sPath {
		get {
			return "Languages.json";
		}
	}
	
	public override void Parse (Object asset)
	{
		if (asset != null && asset is TextAsset) {
			TextAsset textAsset = (TextAsset)asset;		
			var lists = LitJson.JsonMapper.ToObjects<TLanguage>(textAsset.text);
			m_TLanguages = new Dictionary<int, TLanguage>();
			if (lists != null && lists.Count > 0) {
				lists.ForEach(item=>{
					m_TLanguages.Add(item.iID, item);
				});
			}
			
			Globals.It.BundleMgr.UnLoadBundleLocal(asset);
		}
	}
	
	public string GetString (int id) {
		if (m_TLanguages.ContainsKey(id)) return m_TLanguages[id].sText;
		else return "";
	}
	
}
