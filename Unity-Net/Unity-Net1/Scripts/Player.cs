using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public PlayerPropertyMain proMain { get; private set; }
	
	void Awake (){
		proMain = new PlayerPropertyMain();
	}
}
