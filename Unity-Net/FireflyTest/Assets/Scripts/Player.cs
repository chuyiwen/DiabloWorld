using UnityEngine;
using System.Collections;
// 玩家类
public class Player : MonoBehaviour {
	// 内聚了一个玩家信息类
	public PlayerPropertyMain proMain { get; private set; }
	
	void Awake (){
		proMain = new PlayerPropertyMain();
	}
}
