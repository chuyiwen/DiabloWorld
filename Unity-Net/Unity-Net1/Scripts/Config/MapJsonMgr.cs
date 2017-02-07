using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapJsonMgr : ConfigBase {
	
	private Dictionary<int, MapJson> m_jsons = new Dictionary<int, MapJson>();
	private List<MapJson>	m_jsonList = new List<MapJson>();
	
	#region implemented abstract members of ConfigBase
	public override string sPath {
		get {
			return "map.json";
		}
	}

	public override void Parse (Object asset)
	{
		if (asset != null && asset is TextAsset) {
			TextAsset textAsset = (TextAsset)asset;		
			string sJson = textAsset.text;
			m_jsonList = LitJson.JsonMapper.ToObject<List<MapJson>>(sJson);
			if (m_jsonList != null) {
				foreach(MapJson jsonItem in m_jsonList) {
					m_jsons.Add(jsonItem.id, jsonItem);
				}
			}
			/*
			LitJson.JsonReader jsonR = new LitJson.JsonReader(sJson);
			LitJson.JsonData jsonD = LitJson.JsonMapper.ToObject(jsonR);
			jsonD = jsonD["data"];
			int iCount = jsonD.Count;
			for(int i = 0; i < iCount; i++) {
				LitJson.JsonData itemData = jsonD[i];
				if (itemData == null) continue;
				MapJson jsonItem = MapJson.Parse(itemData);
				m_jsons.Add(jsonItem.id, jsonItem);
				m_jsonList.Add(jsonItem);
			}
			*/
			Globals.It.BundleMgr.UnLoadBundleLocal(asset);
		}
	}
	#endregion
	
	public int iCount {
		get { return m_jsons.Count; }
	}
	
	public List<MapJson> MapJsons {
		get { return m_jsonList; }
	}
	
	public MapJson GetMapJsonByID (int iMapID){
		if (m_jsons.ContainsKey(iMapID)) return m_jsons[iMapID];
		return null;
	}
}
