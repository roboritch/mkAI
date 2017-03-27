using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

[RequireComponent(typeof(MeshCollider))]
public class PartUI : MonoBehaviour{
	private bool currentlySelected = false;
	[SerializeField] private Material deselectedPartMaterial;
	[SerializeField] private Material selectedPartMaterial;

	private MeshRenderer MR;

	void Start(){
		MR = GetComponent<MeshRenderer>();
		connectedPartObject = PartsPanelConnection.Instance.getCurrentProjectionBase().getMasterControl().getExternalPart(name);
		collider = GetComponent<MeshCollider>();
	}

	#region part connection
	private ExternalPart connectedPartObject;

	public ExternalPart getConnectedPart(){
		return connectedPartObject;
	}

	private MeshCollider collider;

	public MeshCollider getCollider(){
		return collider;
	}

	#endregion


	#region selection
	public void selectThisUIElement(){
		currentlySelected = true;
		MR.material = selectedPartMaterial;
		ExecuteEvents.Execute<ISelectable>(gameObject, null, (handler, data) => handler.selected());
	}

	public void deselectThisUIElement(){
		currentlySelected = false;
		MR.material = deselectedPartMaterial;
		ExecuteEvents.Execute<ISelectable>(gameObject, null, (handler, data) => handler.deselected());
	}
	#endregion

	void OnMouseDown(){
		if(doubleClickCheck()){
			//select this part in the UI
			PartsPanelConnection.Instance.selectPartUIElement(this); 
		}
	}

	private float lastClickTime = -1f;

	private bool doubleClickCheck(){
		float clickTimeCurrent = Time.time;
		if(clickTimeCurrent - lastClickTime <= PartsPanelConnection.Instance.getDoubleClickTime()){
			return true;
		}
		lastClickTime = clickTimeCurrent; // here to prevent consective triple click detection
		return false;
	}

}
