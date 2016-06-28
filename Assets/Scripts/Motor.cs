using UnityEngine;
using System.Collections;

public class Motor : MonoBehaviour{

	#region basic motor properties
	[SerializeField] private string motorName;
	[SerializeField] private bool[] rotationAxis = new bool[3];
	[SerializeField] private Vector3 startRotation;
	[SerializeField] private Vector3 minRotation;
	[SerializeField] private Vector3 maxRotation;

	#endregion

	void Start(){
		//Give the motor a name if the current name is not valid
		if(validMoterName(motorName)){
			motorName = gameObject.name + " MOTOR";
		}
	}

	private bool validMoterName(string name){
		return !(name == "" || name == null);
	}

}
