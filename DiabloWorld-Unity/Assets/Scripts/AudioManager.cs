using UnityEngine;
using System.Collections;
// 音效管理类
public class AudioManager : MonoBehaviour {
	
	public static AudioManager shareAudioManager{
		get
		{
			if(_shareAudioManager == null)
				Init();
			return _shareAudioManager;
		}
	}
	
	private static AudioManager _shareAudioManager;
	private AudioObject[] _audioObj;
	private static readonly int _audioObjLength = 20;
	
	private GameObject _thiObj;
	private Transform _thisT;
	private Camera cam;
	
	public static void Init(){
		if(_shareAudioManager == null){
			GameObject obj = new GameObject();
			obj.name = "audiomanager";
			_shareAudioManager = obj.AddComponent<AudioManager>();
		}
	}
	
	public static void PlaySound(AudioClip clip){
				
		AudioObject audioObj = shareAudioManager.GetUnUseAudioObject();
		audioObj.isUser = true;
		audioObj.source.clip = clip;
		audioObj.source.Play();
		
		shareAudioManager.StartCoroutine(shareAudioManager.ClearUserFlag(audioObj));
	}
	
	public static void PlayLoopSound(AudioClip clip){
		GameObject obj = new GameObject("loopObj");
		obj.transform.parent = shareAudioManager.cam.transform;
		AudioSource source = obj.AddComponent<AudioSource>();
		source.playOnAwake = true;
		source.loop = true;
		source.clip = clip;
		source.Play();		
	}
	
	private AudioObject GetUnUseAudioObject(){
		foreach(AudioObject obj in _audioObj ){
			if(!obj.isUser)return obj;
		}
		return _audioObj[0];
	}
	
	IEnumerator ClearUserFlag(AudioObject audioObj){
		yield return new WaitForSeconds(audioObj.source.clip.length);
		audioObj.isUser = false;
	}
	
	void Awake(){
		
		_thiObj = this.gameObject;
		_thisT = _thiObj.transform;
		cam = Camera.main.GetComponent<Camera>();
		
		_audioObj = new AudioObject[_audioObjLength];
		for(int i = 0; i < _audioObjLength; i++ ){
			GameObject obj = new GameObject();
			obj.name = "audioobj";
			obj.transform.parent = _thisT;
			
			AudioSource source = obj.AddComponent<AudioSource>();
			source.playOnAwake = false;
			source.loop = false;
			
			_audioObj[i] = new AudioObject(){ source = source, transform = obj.transform, isUser = false, ID = i };
		}
	}
	
}

public class AudioObject{
	public int ID;
	public AudioSource source;
	public Transform transform;
	public bool isUser = false;
}
