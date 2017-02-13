using UnityEngine;
using System.Collections;

public class DoDestory : MonoBehaviour {

	void Awake (){
		gameObject.isStatic = true;
		DontDestroyOnLoad(gameObject);
	}
}
