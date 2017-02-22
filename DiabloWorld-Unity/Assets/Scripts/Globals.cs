#define USER_LOCAL_RESOURCES
//#define USER_WEB_RESOURCES
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 全局管理类
public class Globals : MonoBehaviour {

	public static readonly string 	VERSION = "V1.1";
	public static readonly int 		SERVERVERSION = 0;
	public static readonly string 	BUNDLEVERSION = "2017020701";  // 年月日 版本号
	
	public static Globals It;  // 单例
	
	public bool				bTestView;
	public GameObject		waitingView;
	public Transform		waitingParentT;  // Anchor
	public kLanguage		klanguage;
	public string			urlResource;
	public AudioClip		bgAudio;
    
	private Transform		m_CacheTran;  // this

    private string			m_sBundlePath;
    public string sBundlePath { get { return m_sBundlePath; } }

    private bool			m_bUseLocalResources;
    public bool bUseLocalResources { get { return m_bUseLocalResources; } }

    #region 各类视图
    private GameObject 		m_WaitingView;
	private DengluView		m_DengluView;
	private MessageBoxView	m_MessageBoxView;
	private InoutBoxView	m_InoutBoxView;
	private SelectRoleView	m_SelectRoleView;
	private MainView		m_MainView;
	private BattleRecordView m_BattleRecordView;
    #endregion

    #region Manager 类
    private NetMgr 			m_NetMgr;  // 最主要的网络通讯 管理类
	private ProtocolMgr		m_ProtocolMgr;
	private Player			m_MainPlayer;
	private LanguageMgr		m_LanguageMgr;
	private BundleMgr		m_BundleMgr;
	private HeroJsonMgr		m_HeroJsonMgr;
	private MapJsonMgr		m_MapJsonMgr;
	private ErrorHintMgr	m_ErrorHintMgr;
	private MonsterJsonMgr	m_MonsterJsonMgr;
	private SkillJsonMgr	m_SkillJsonMgr;
	
	public NetMgr 			NetManager 	{ get { return m_NetMgr; } }
	public ProtocolMgr 		ProtocolMgr { get { return m_ProtocolMgr; } }
	public Player			MainPlayer 	{ get { return m_MainPlayer; } }
	public LanguageMgr		LanguageMgr { get { return m_LanguageMgr; } }
	public BundleMgr		BundleMgr 	{ get { return m_BundleMgr;} }
	public HeroJsonMgr		HeroJsonMgr	{ get { return m_HeroJsonMgr;} }
	public MapJsonMgr		MapJsonMgr 	{ get { return m_MapJsonMgr; } }
	public ErrorHintMgr		ErrorHintMgr{ get { return m_ErrorHintMgr; } }
	public MonsterJsonMgr	MonsterJsonMgr { get { return m_MonsterJsonMgr; } }
	public SkillJsonMgr		SkillJsonMgr{ get { return m_SkillJsonMgr; } }
    #endregion

    public bool Connected {
		get {
			return NetManager.Connected;
		}
	}
	
	#region sys
	
	void Awake (){
		It = this;
		m_CacheTran = transform;
        // 资源可以选择从 Loacl 或者 Web 加载
#if USER_LOCAL_RESOURCES
		m_bUseLocalResources = true;	
#endif
#if USER_WEB_RESOURCES
		m_bUseLocalResources = false;	
#endif
	}
	
	void Start (){
		switch(klanguage) {
		case kLanguage.Chinese:  // 这里可以本地化
		{
			m_sBundlePath = "Chinese";
			break;
		}
		}
		
		StartCoroutine("_DoStart");
	}
	
	void OnDestroy (){
		NetManager.Close();
	}
	
	/*
	void OnApplicationFocus(bool focusStatus) {
        if (focusStatus) {
			if (!NetManager.Connected) {
				NetManager.ReInit();
				GameObject.DestroyImmediate(waitingParentT);
				GameObject newAnchorObject = new GameObject("Anchor");
				GameObject uiroot = GameObject.Find("UI Root (2D)");
				newAnchorObject.transform.parent = uiroot.transform;
				newAnchorObject.transform.localScale = Vector3.one;
				UIAnchor anchor = newAnchorObject.AddComponent<UIAnchor>();
				anchor.side = UIAnchor.Side.Center;
				waitingParentT = newAnchorObject.transform;
				StartCoroutine("_CreateLoginView");
			}
		}
    }
	*/
	
	void _LoadLinkBeforeLoad (){  // 加载 主要管理类
		m_NetMgr 		= _CreateLink<NetMgr>();
		m_ProtocolMgr 	= _CreateLink<ProtocolMgr>();
		m_BundleMgr 	= _CreateLink<BundleMgr>();
		m_MainPlayer 	= _CreateLink<Player>();
	}
	
	void _LoadLinkAfterLoad (){  // 加载 其他管理类
		m_LanguageMgr 	= _CreateLink<LanguageMgr>();
		m_HeroJsonMgr = _CreateLink<HeroJsonMgr>();
		m_MapJsonMgr = _CreateLink<MapJsonMgr>();
		m_ErrorHintMgr = _CreateLink<ErrorHintMgr>();
		m_MonsterJsonMgr = _CreateLink<MonsterJsonMgr>();
		m_SkillJsonMgr = _CreateLink<SkillJsonMgr>();
	}
	
	T _CreateLink<T> () where T: Component {  // 加载管理类到Global的下面，作为子组件
		T link = this.GetComponentInChildren<T>();
		if (link == null) {
			GameObject linkObject = new GameObject(typeof(T).Name);
			link = linkObject.AddComponent<T>();
			linkObject.transform.parent = m_CacheTran;
		}
		return link;
	}
	
    // Start结束后 开启协程
	IEnumerator _DoStart (){
		yield return 0;
		_LoadLinkBeforeLoad();
		
		BundleMgr.DoInit();  // 加载资源
		
		_LoadLinkAfterLoad();
		
		if (!bTestView) {  // 不存在就创建
			StartCoroutine("_CreateLoginView");
		}
	}
	
    // 开启协程 创建登录视图
	IEnumerator _CreateLoginView (){
		yield return null;
        // 加载完之后需要执行这个回调 handler
		System.Action<Object> handler = (asset) => {
			if(asset != null) {
				GameObject dengluObject = (GameObject)GameObject.Instantiate(asset);  // 登录视图
				m_DengluView = dengluObject.GetComponent<DengluView>();
				NGUIUtility.SetParent(waitingParentT, dengluObject.transform);  // waitingParentT => Anchor
				
				AudioManager.PlayLoopSound(bgAudio);  // 播放音乐
			}			
			HideWaiting();
		};
		ShowWaiting();
		StartCoroutine(BundleMgr.CreateObject(kResource.View, "DengluView", "DengluView", handler));
	}
	
	#endregion
	
	#region ui
	// 等待界面 网络连接 账号认证 等情况
	public void ShowWaiting (){
		StartCoroutine("_ShowWaiting");
	}
	
	public void ShowWaitingImmediate (){
		StartCoroutine("_ShowWaitingImmediate");
	}
	
	public void HideWaiting (){
		StopCoroutine("_ShowWaiting");
		StopCoroutine("_ShowWaitingImmediate");
		if (m_WaitingView != null) {
			GameObject.DestroyObject(m_WaitingView);  // 一次性？？ 频繁销毁不好 以后可以考虑用UI栈实现
        }
	}
	
	private IEnumerator _ShowWaitingImmediate (){
		//yield return new WaitForSeconds(1.0f);
		yield return 0;
		{
			if (m_WaitingView == null && waitingView != null) {
				
				System.Action<Object> handler = (asset) => {
					if(asset != null) {
						m_WaitingView = (GameObject)GameObject.Instantiate(asset);  // 创建等待界面 显示
						NGUIUtility.SetParent(waitingParentT, m_WaitingView.transform);
					}			
				};
				
				StartCoroutine(BundleMgr.CreateObject(kResource.Common, "WaitingView", "WaitingView", handler));
			}
		}
	}
	
	private IEnumerator _ShowWaiting (){
		yield return new WaitForSeconds(1.0f);
		{
			if (m_WaitingView == null && waitingView != null) {
				
				System.Action<Object> handler = (asset) => {
					if(asset != null) {
						m_WaitingView = (GameObject)GameObject.Instantiate(asset);
						NGUIUtility.SetParent(waitingParentT, m_WaitingView.transform);
					}			
				};
				
				StartCoroutine(BundleMgr.CreateObject(kResource.Common, "WaitingView", "WaitingView", handler));
			}
		}
	}
	
    // 警告弹窗 没连接上服务器 账号错误 等情况
	public void ShowWarn (int iTitle, int iMsg, System.Action callback){
		string strMsg = LanguageMgr.GetString(iMsg);
		ShowWarn(iTitle, strMsg, callback);
	}
	
	public void ShowWarn (int iTitle, string strMsg, System.Action callback){
		string sTitle = LanguageMgr.GetString(iTitle);
		Debug.LogError(strMsg);
		if (m_MessageBoxView == null) {

			System.Action<Object> handler = (asset) => {
				if(asset != null) {
					GameObject objMsgBox = (GameObject)GameObject.Instantiate(asset);  // 创建弹窗 显示
					m_MessageBoxView = objMsgBox.GetComponent<MessageBoxView>();
					NGUIUtility.SetParent(waitingParentT, objMsgBox.transform);
					m_MessageBoxView.show(sTitle, strMsg, callback);
				}			
			};
			
			StartCoroutine(BundleMgr.CreateObject(kResource.View, "MessageBoxView", "MessageBoxView", handler));
		
		}
	}
	
	public void HideWarn (){
		if (m_MessageBoxView != null) {
			GameObject.DestroyObject(m_MessageBoxView.gameObject);
			m_MessageBoxView = null;
		}
	}
	
    // 显示输入   输入角色名字 等情况
	public void ShowInput (System.Action<string> callback){
		if (m_InoutBoxView == null) {
			System.Action<Object> handler = (asset) => {
				if(asset != null) {
					GameObject objInputBox = (GameObject)GameObject.Instantiate(asset);  // 创建输入界面  显示
					m_InoutBoxView = objInputBox.GetComponent<InoutBoxView>();
					NGUIUtility.SetParent(waitingParentT, objInputBox.transform);
					m_InoutBoxView.onShow(callback);
				}			
			};
			
			StartCoroutine(BundleMgr.CreateObject(kResource.View, "InputBoxView", "InputBoxView", handler));
		}
	}
	
	public void HideInput (){
		if (m_InoutBoxView != null) {
			GameObject.DestroyObject(m_InoutBoxView.gameObject);
			m_InoutBoxView = null;
		}
	}
	
	#endregion
	
	#region net
	// 连接
	public void Connect (System.Action callback){
		NetManager.Connect(callback);	
	}
	// 处理数据  根据 协议号 分发给其他 （handler） 处理
	public void ProcessMsg (MessageData info){
		if (info == null) return;
		ProtocolMgr.Process(info.body);
	}
	// 发送数据  参数 数据 + 命令号
	public void SendMsg<T>(T data, int iCommand) where T : Data_Base {
		string sJson = LitJson.JsonMapper.ToJson(data);
		Debug.LogWarning(string.Format("::OnSend:{0} ,{1}", iCommand, sJson));
		byte[] sendBytes = MessageParse.Parse(SERVERVERSION, iCommand, sJson);
		Globals.It.NetManager.Send(sendBytes);  // 转交给 网络管理类 处理
	}
	
	#endregion
	
	#region view
	
	public void DestoryDengluView (){
		if (m_DengluView != null) {
			m_DengluView.save();  // 保存登录时的用户名密码
			GameObject.DestroyImmediate(m_DengluView.gameObject, true);
			m_DengluView = null;
		}
	}
	// 开场动画
	public void ShowKaiChangGifView (){
		System.Action<Object> handler = (asset) => {
			if(asset != null) {
				GameObject gifView = (GameObject)GameObject.Instantiate(asset);
				NGUIUtility.SetParent(waitingParentT, gifView.transform);
			}			
		};
		
		StartCoroutine(BundleMgr.CreateObject(kResource.View, "KaiChangView", "KaiChangView", handler));
	}
	// 角色创建面板
	public void ShowCreateRoleView (){
		if (m_SelectRoleView == null) {
			System.Action<Object> handler = (asset) => {
				if(asset != null) {
					GameObject selectRoleView = (GameObject)GameObject.Instantiate(asset);
					m_SelectRoleView = selectRoleView.GetComponent<SelectRoleView>();
					NGUIUtility.SetParent(waitingParentT, selectRoleView.transform);
				}			
			};
			StartCoroutine(BundleMgr.CreateObject(kResource.View, "SelectRoleView", "SelectRoleView", handler));
		}
	}
	
	public void DestoryCreateRoleView (){
		if (m_SelectRoleView != null) {
			GameObject.DestroyImmediate(m_SelectRoleView.gameObject, true);
			m_SelectRoleView = null;
		}
	}
	
	public void ShowEnterGameView (){
		ShowWaiting();
        // 参数 characterId
		Data_RoleEnterGame mode = new Data_RoleEnterGame(){ characterId = MainPlayer.proMain.iCharacterId };
        // 发送角色id 给 服务器
        Globals.It.SendMsg(mode, Const_ICommand.RoleEnterGame);
	}
	// 刷新主面板
	public void MainViewRefresh (){
		if (m_MainView != null) {
			m_MainView.RefreshMapStat();
		}
	}
	// 结束战斗回到主面板
	public void EndBattleToMainView (){
		ShowWaiting();
		DestoryBattleRecordView();  // 销毁战斗结果面板  同理这里应该采用UI栈
		ShowMainView ();
	}
	
	public void ShowMainView (){
		if (m_MainView == null) {  // 创建
			System.Action<Object> handler = (asset) => {
				if(asset != null) {
					GameObject mainView = (GameObject)GameObject.Instantiate(asset);
					m_MainView = mainView.GetComponent<MainView>();
					NGUIUtility.SetParent(waitingParentT, mainView.transform);
					m_MainView.InitMap();
					m_MainView.GetMapData();
				}	
			};
			StartCoroutine(BundleMgr.CreateObject(kResource.View, "MainView", "MainView", handler));
		}
		else {  // 是否需要刷新 玩家id  发送到服务端
			if (MainPlayer.proMain.bNeedRefresh) {
				ShowWaiting();  // 这里等待是为了下面数据的发送吗？？？
				Data_PlayerStat data = new Data_PlayerStat()
				{
					characterId = MainPlayer.proMain.iCharacterId
				};
				Globals.It.SendMsg(data, Const_ICommand.PlayerStat);
			}
			else {  // 刷新玩家 数据
				m_MainView.gameObject.SetActive(true);
				m_MainView.RefreshMapStat();
				m_MainView.RefreshPlayerStat();
				Globals.It.HideWaiting();
			}
		}
	}
	
	public void DestoryMainView (){
		if (m_MainView != null) {
			m_MainView.gameObject.SetActive(false);
			/*
			GameObject.DestroyImmediate(m_MainView.gameObject, true);
			m_MainView = null;
			*/
		}
	}
	// 任务
	public void ShowRenwuView (MapJson mapjson){
		System.Action<Object> handler = (asset) => {
			if(asset != null) {
				GameObject renwuViewObject = (GameObject)GameObject.Instantiate(asset);
				RenwuView renwuView = renwuViewObject.GetComponent<RenwuView>();
				renwuView.show(mapjson);
				NGUIUtility.SetParent(waitingParentT, renwuViewObject.transform);
			}			
		};
		StartCoroutine(BundleMgr.CreateObject(kResource.View, "RenwuView", "RenwuView", handler));
	}
	// 战斗记录
	public void ShowBattleRecordView (Data_MapBattle_R data){
		if (m_BattleRecordView == null) {
			System.Action<Object> handler = (asset) => {
				if(asset != null) {
					GameObject battleRecordViewObject = (GameObject)GameObject.Instantiate(asset);
					m_BattleRecordView = battleRecordViewObject.GetComponent<BattleRecordView>();
					m_BattleRecordView.show(data);
					NGUIUtility.SetParent(waitingParentT, battleRecordViewObject.transform);
				}			
			};
			StartCoroutine(BundleMgr.CreateObject(kResource.View, "BattleRecordView", "BattleRecordView", handler));
		}
	}
	
	public void DestoryBattleRecordView (){
		if (m_BattleRecordView != null) {
			GameObject.DestroyImmediate(m_BattleRecordView.gameObject, true);
			m_BattleRecordView = null;
		}
	}
	// 战斗结果
	public void ShowBattleRecordResultView (bool bresult, Data_MapBattle_R.SetData data){
		System.Action<Object> handler = (asset) => {
			if(asset != null) {
				GameObject resultViewObject = (GameObject)GameObject.Instantiate(asset);
				JiesuanView resultView = resultViewObject.GetComponent<JiesuanView>();
				NGUIUtility.SetParent(waitingParentT, resultViewObject.transform);
				resultView.show(bresult, data);
			}			
		};
		StartCoroutine(BundleMgr.CreateObject(kResource.View, "JiesuanView", "JiesuanView", handler));
	}
	
	#endregion
	// 将 json数据 转换成 对象
	public static T ToObject<T>(byte[] buffer) where T : Data_Base {
		if (buffer != null && buffer.Length > 0) {
			string sJson = System.Text.Encoding.UTF8.GetString(buffer);
			T data = LitJson.JsonMapper.ToObject<T>(sJson);
			return data;
		}
		return default(T);
	}
	
}

public enum kLanguage {
	Chinese = 0,
	English,
}

public enum kResource {
	Config,
	View,
	Common,
	Font,
	Effect,
}

public enum kLoadResult {
	None = 0,
	SUCC,
	Load,
	Fail,
}
