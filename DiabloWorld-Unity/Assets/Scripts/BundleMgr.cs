using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
// AssetBundle 管理类
public class BundleMgr : MonoBehaviour {
	
	private Dictionary<string, AssetBundle> m_Bundlers;  // 通过 网络 加载后的资源 保存到Dict
	private Dictionary<string, Object> 		m_Objects;
	private kLoadResult			m_kLoadResult;
	
	public kLoadResult LoadResult {
		get {
			return m_kLoadResult;
		}
	}
	
	void Awake (){
		m_Bundlers = new Dictionary<string, AssetBundle>();
		m_Objects = new Dictionary<string, Object>();
		m_kLoadResult = kLoadResult.None;
	}
	// 通过网络 Load 资源
	public IEnumerator DoLoadOneBundle (string bundleName){
		m_kLoadResult = kLoadResult.Load;
		
		string savePath = Application.dataPath + "/DownLoad/";
		string bundleVersion = Globals.BUNDLEVERSION;
		string fileName = bundleName + "." + bundleVersion + ".assetbundle";
		string path = savePath + fileName;
		string url = "";	
		if (System.IO.File.Exists(path)) {
			url = "file:///" + path;  // 本地存在
		}
		else{
			url = Globals.It.urlResource + "/" + fileName;
		}
		
		WWW www = new WWW(url);  // 通过 www 获取资源
		while(!www.isDone) {
			yield return new WaitForSeconds(0.1f);
		}
		if (string.IsNullOrEmpty(www.error)) {  // 没发生错误
			m_Bundlers.Add(bundleName, www.assetBundle);
			m_kLoadResult = kLoadResult.SUCC;
			
			if (!Directory.Exists(savePath))  // 创建个文件夹存放
			{
				Directory.CreateDirectory(savePath);
			}
			if (File.Exists(savePath + fileName)==false)  // 资源不存在则写入
			{
				FileStream fs = File.Create(savePath + fileName);
				fs.Write(www.bytes, 0, www.bytes.Length);
				fs.Flush();
				fs.Close();
			}
		}
		else{
			Debug.LogError(www.error);
			m_kLoadResult = kLoadResult.Fail;
		}
		yield return null;
	}
	
    // 通过路径加载 资源
	public void LoadResource (kResource kres, string bundle, string res, out Object obj){
		if (Globals.It.bUseLocalResources) {  // 通过 本地 加载
			string sPath = "";
			switch(kres) {
			case kResource.Common:
			{
				sPath = "Common/" + res;
				break;
			}
			case kResource.Config:
			{
				sPath = Globals.It.sBundlePath + "/Config/" + res;
				break;
			}
			case kResource.View:
			{
				sPath = Globals.It.sBundlePath + "/Views/" + res;
				break;
			}
			case kResource.Effect:
			{
				sPath = Globals.It.sBundlePath + "/Effect/" + res;
				break;
			}
			}
			obj = Resources.Load(sPath); 
			//m_Objects.Add(bundle, obj);
		}
		else {  // 通过 网络 加载
			if (m_Bundlers.ContainsKey(bundle)) {  // 已经加载过
				AssetBundle ab = m_Bundlers[bundle];
				if (string.IsNullOrEmpty(res)) {
					obj = ab.mainAsset;
				}
				else{
					obj = ab.LoadAsset(res);
				}
			}
			else{  // 第一次加载，需要开启协程去下载
				obj = null;
				StartCoroutine(DoLoadOneBundle(bundle));
			}
		}
	}
	
	public bool CheckBundleExist (string bundle){
		return m_Bundlers.ContainsKey(bundle);
	}
	
	public void UnLoadBundle (string bundle) {  // 移除资源
		if (CheckBundleExist(bundle)) {
			if (Globals.It.bUseLocalResources) {
				Object obj = m_Objects[bundle];
				m_Objects.Remove(bundle);  // 查看引用后发现这里只有remove没有add过   FIXME
				Resources.UnloadAsset(obj);
			}
			else{
				AssetBundle obj = m_Bundlers[bundle];
				m_Bundlers.Remove(bundle);
				obj.Unload(true);
			}
		}
	}
	
	public void UnLoadBundleLocal (Object assect){  // 移除本地资源
		if (Globals.It.bUseLocalResources) {
			Resources.UnloadAsset(assect);
		}
	}
	
	public IEnumerator CreateObject (kResource kres, string bundle, string res, System.Action<Object> initHandler){
		Object assect = null;
		LoadResource(kres, bundle, res, out assect);
		if (assect == null) {
			while (true) {
				// BUG: IF THE RESOURCE NOT FOUND THIS WILL BE LOOP FOREVER
				if (CheckBundleExist(bundle)) {
					LoadResource(kres, bundle, res, out assect);
					break;
				}
				yield return new WaitForSeconds(0.1f);
			}
		}
        // initHandler 是事件回调
		if (assect != null) {
			if (initHandler != null) {
				initHandler(assect);
			}
			//Globals.It.BundleMgr.UnLoadBundle(bundle);
		}
		else {
			if (initHandler != null) {
				initHandler(null);
			}
		}
	}
	
	public void DoInit (){
		if (!Globals.It.bUseLocalResources) {
			Object asset = null;
			LoadResource(kResource.Config, Const_SPath.Path_Config, "", out asset);
			LoadResource(kResource.Font, "font26", "", out asset);
            // TODO 加载后没使用
		}
	}
}
