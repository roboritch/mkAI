using UnityEngine;
using System.Collections;

public class StatsPanelDefaultLocation : MonoBehaviour{
	[SerializeField] Vector3 startLocation;
	[SerializeField] Vector3 startScale;

	public void setDefaultStats(){
		transform.localPosition = startLocation;
		transform.localScale = startScale;
	}

}
