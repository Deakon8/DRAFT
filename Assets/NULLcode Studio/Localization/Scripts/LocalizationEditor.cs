// NULLcode Studio © 2016
// null-code.ru

#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Localization))]

public class LocalizationEditor : Editor {

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		Localization e = (Localization)target;
		GUILayout.Label("Default Locale:", EditorStyles.boldLabel);
		if(GUILayout.Button("Create / Update"))
		{
			e.SetComponents();
		}
	}
}
#endif
