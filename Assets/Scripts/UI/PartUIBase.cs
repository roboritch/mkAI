using UnityEngine;
using System.Collections;

public class PartUIBase : MonoBehaviour{

	[SerializeField] private Vector3 defaultLocationInPartPanel;
	[SerializeField] private Vector3 defaultScaleInPartPanel;

	[SerializeField] private GameObject rotationForcePanelPrefab;

	public GameObject getRotationForcePanelPrefab(){
		return rotationForcePanelPrefab;
	}

	#region MasterControle refrences
	private MasterControl masterControl;

	public void reciveMasterControl(MasterControl master){
		masterControl = master;
	}

	public MasterControl getMasterControl(){
		return masterControl;
	}
	#endregion

	void Awake(){
		PartsPanelConnection.Instance.setCurrentProjectionBase(this);
	}

	void Start(){
		moveToDefultLocationInPartsPanel();
	}



	public void moveToDefultLocationInPartsPanel(){
		transform.localPosition = defaultLocationInPartPanel;
		transform.localScale = defaultScaleInPartPanel;
	}


}
