using UnityEngine;
using System.Collections;

public class ErrorHintMgr : ConfigBase {
	
	private LitJson.JsonData	m_data;
		
	#region implemented abstract members of ConfigBase
	public override string sPath {
		get {
			return "error.json";
		}
	}

	public override void Parse (Object asset)
	{
		if (asset != null && asset is TextAsset) {
			TextAsset textAsset = (TextAsset)asset;		
			string sJson = textAsset.text;
			LitJson.JsonReader jsonR = new LitJson.JsonReader(sJson);
			m_data = LitJson.JsonMapper.ToObject(jsonR);
			Globals.It.BundleMgr.UnLoadBundleLocal(asset);
		}
	}
	#endregion
	
	public string GetErrorString (string strErrorCode){
		return m_data[strErrorCode].ToString();
	}
}
