using UnityEngine;
using UnityEditor;
using System.Collections;

class MyWindow : EditorWindow{
	private string tagName = "";
	private string componentName = "";
	private Component wantedComponent;

	// Add menu item named "My Window" to the Window menu
	[MenuItem("Window/Get Tagged Objects")]
	public static void ShowWindow(){
		//Show existing window instance. If one doesn't exist, make one.
		EditorWindow.GetWindow(typeof(MyWindow));
	}

	void OnGUI(){

		GUILayout.Label("Tag", EditorStyles.boldLabel);
		tagName = EditorGUILayout.TextField("TagName", tagName);
		if(GUILayout.Button("get taged objects")){
			selectObjectsWithTag(tagName);
		}
		if(GUILayout.Button("deselect taged objects")){
			deselectObjectsWithTag(tagName);
		}
		GUILayout.Space(5f);

		if(GUILayout.Button("select objects with the same tag")){
			selectObjectsWithTag(((GameObject)Selection.objects[0]).tag);
		}

		GUILayout.Space(5f);

		/*	
		GUILayout.Space(5f);
		System.Type wantedComponentType;


		wantedComponentType = EditorGUIUtility.systemCopyBuffer;

		componentName = EditorGUILayout.TextField("ComponentName", componentName);

		GameObject[] selectedObjects;
		if(GUILayout.Button("Add component to selected objects")){
			selectedObjects = (GameObject[])Selection.objects;
			if(selectedObjects.Length != 0)
				foreach( GameObject obj in selectedObjects ){
					obj.AddComponent();
				}
		}*/

	}

	public static void deselectObjectsWithTag(string selectedTag){
		UnityEngine.Object[] objects = Selection.objects;
		UnityEngine.Object[] newSelection = new GameObject[objects.Length];
		int selectionIndex = 0;
		foreach( var item in objects ){
			if(((GameObject)item).tag != selectedTag){
				newSelection[selectionIndex++] = item;
			}
		}
		objects = new GameObject[selectionIndex];
		for(int i = 0; i < selectionIndex; i++){
			objects[i] = newSelection[i];
		}
		Selection.objects = objects;

	}

	public static void selectObjectsWithTag(string selectedTag){
		Object[] objects = GameObject.FindGameObjectsWithTag(selectedTag);
		Object[] curSelection = Selection.objects;
		Object[] newSelection = new GameObject[objects.Length + curSelection.Length];
		for(int i = 0; i < curSelection.Length; i++){
			newSelection[i] = curSelection[i];
		}
		for(int i = curSelection.Length; i < newSelection.Length; i++){
			newSelection[i] = objects[i - curSelection.Length];
		}

		Selection.objects = newSelection;
	}
}	