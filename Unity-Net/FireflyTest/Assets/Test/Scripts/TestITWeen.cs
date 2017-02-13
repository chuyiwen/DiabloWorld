using UnityEngine;
using System.Collections;

public class TestITWeen : MonoBehaviour {
	
	public Animation activeAnim;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI (){
		if (GUI.Button(new Rect(10,10, 50, 40), "MoveAdd")) {
			Vector3 v3 = gameObject.transform.localPosition;
			float fT = Time.deltaTime * 20;
			Hashtable args = new Hashtable();
			args.Add("easeType", iTween.EaseType.easeInExpo);
			args.Add("looptype", "none");
			args.Add("speed", 50f);
			args.Add("time", fT);
			args.Add("x", v3.x);
			args.Add("y", v3.y + 50);
			args.Add("z", v3.z);
			args.Add("islocal", true);
			iTween.MoveTo(gameObject, args);
		}
		if (GUI.Button(new Rect(10,60, 50, 40), "Animation")) {
			 ActiveAnimation.Play(activeAnim, "attack", AnimationOrTween.Direction.Forward);
		}
	}
}
