using UnityEditor;
using UnityEngine;

public class DisplayCenterOfMass : EditorWindow{
	#region script vars
	[SerializeField]
	private float sphereScale = 0.1f;

	[SerializeField]
	private GameObject comRepresentation;

	[SerializeField]
	private Material blueMat;
	#endregion

	#region com obj's
	private GameObject parentHolder;
	private GameObject[] coms;
	private GameObject advCOM;

	[MenuItem("Window/Display Center Of Mass")]
	public static void ShowWindow(){
		//Show existing window instance. If one doesn't exist, make one.
		EditorWindow.GetWindow(typeof(DisplayCenterOfMass));
	}
	#endregion

	#region com removal
	/// <summary>
	/// Destroys all COM balls.
	/// WARNING: to be used only if regular methods of destruction don't work
	/// </summary>
	private void destroyAllCOMBalls(){
		GameObject[] go = GameObject.FindGameObjectsWithTag("EditorOnly");
		for(int i = 0; i < go.Length; i++){
			if(go[i] != null)
				DestroyImmediate(go[i]);
		}
	}

	private void removeComBalls(){
		if(coms != null)
			for(int i = 0; i < coms.Length; i++){
				if(coms[i] != null)
					DestroyImmediate(coms[i]);
			}
	}
	#endregion

	/// <summary>
	/// Displays the center of mass for all selected objects.
	/// </summary>
	private void displayAdvCom(){
		removeAdvCom();
		Rigidbody rb;
		Vector3 compoundCOM = new Vector3();
		Object[] objsSelected = Selection.objects;
		for(int i = 0; i < objsSelected.Length; i++){
			rb = ((GameObject)objsSelected[i]).GetComponent<Rigidbody>();
			compoundCOM += rb.transform.TransformPoint(rb.centerOfMass);
		}
		advCOM = Instantiate(comRepresentation);
		advCOM.GetComponent<MeshRenderer>().material = blueMat;
		advCOM.transform.position = compoundCOM;
		if(parentHolder == null){
			parentHolder = Instantiate(new GameObject());
			parentHolder.tag = "EditorOnly";
		}
		advCOM.transform.SetParent(parentHolder.transform);
	}

	private void removeAdvCom(){
		if(advCOM != null)
			DestroyImmediate(advCOM);
	}

	private bool lockCurrentBalls = false;

	#region onGui
	void OnGUI(){
		GUILayout.Toggle(lockCurrentBalls, "lock current coms");

		GUILayout.Space(2f);
		if(GUILayout.Button("Remove all balls")){
			removeComBalls();
		}
		GUILayout.Space(10f);
		if(GUILayout.Button("Display compound com")){
			
		}
		if(GUILayout.Button("Remove compound com")){
			removeAdvCom();
		}
		GUILayout.Space(7f);
		if(GUILayout.Button("Remove all editorTagged objects")){
			destroyAllCOMBalls();
		}
	}
	#endregion

	#region com on selection change
	void OnSelectionChange(){	
		if(lockCurrentBalls){
			return;
		}	
		Vector3 scale = new Vector3(sphereScale, sphereScale, sphereScale);
		Rigidbody rb;
		Object[] objsSelected = Selection.objects;

		removeComBalls();

		coms = new GameObject[objsSelected.Length];
		for(int i = 0; i < objsSelected.Length; i++){
			rb = ((GameObject)objsSelected[i]).GetComponent<Rigidbody>();
			if(rb != null){
				coms[i] = Instantiate(comRepresentation);
				coms[i].transform.position = rb.transform.TransformPoint(rb.centerOfMass);		
				coms[i].transform.localScale = scale;
				if(parentHolder == null){
					parentHolder = Instantiate(new GameObject());
					parentHolder.tag = "EditorOnly";
				}
				coms[i].transform.SetParent(parentHolder.transform);
			}
		}
	}
	#endregion

	#region window loss actions
	void OnDestroy(){
		destroyAllCOMBalls();
	}

	void OnLostFocus(){
		if(lockCurrentBalls){
			return;
		}
		destroyAllCOMBalls();
		destroyComBallHolder();
	}
	#endregion

	private void destroyComBallHolder(){
		DestroyImmediate(parentHolder);
	}

}