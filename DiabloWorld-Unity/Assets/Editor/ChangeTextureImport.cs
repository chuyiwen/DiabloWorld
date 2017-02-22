using UnityEngine;
using UnityEditor;
using System.Collections;
/*
public class ChangeTextureImport : EditorWindow {
	
	public GUIStyle m_TitleLabelStyle;
	
	private string m_Path;
	
	[MenuItem("Tools/ChangeTextureImport")]
	static void Init (){
		ChangeTextureImport window = EditorWindow.CreateInstance<ChangeTextureImport>();
		window._Init();
		window.Show();
	}
	
	void _Init (){
		m_TitleLabelStyle = new GUIStyle();
		m_TitleLabelStyle.fontSize = 18;
		m_TitleLabelStyle.fontStyle = FontStyle.Bold;
		m_TitleLabelStyle.alignment = TextAnchor.MiddleCenter;
		
		m_Path = "";
	}
		
	void OnGUI (){

		GUILayout.Space(5.0f);
		GUILayout.Label("ChangeTextureImport", m_TitleLabelStyle);
		GUILayout.Space(5.0f);
		
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Open", GUILayout.Width(80))){
			Debug.Log(Application.dataPath);
			m_Path = EditorUtility.OpenFolderPanel("Select Textures Path:", Application.dataPath, "");
		}
		m_Path = GUILayout.TextField(m_Path);
		GUILayout.EndHorizontal();
	}
}
 
*/