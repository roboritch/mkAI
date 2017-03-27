using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RotationSpeedPanel : FloatUI{
	[SerializeField] Text selectedPartName;
	[SerializeField] Toggle[] toggles;
	private Vector3 rotationForce;
	private Vector3 maxRotationForce;
	//all motors have the +max = -min

	public void setRotationForces(Vector3 rotForce, Vector3 maxRotForce){
		rotationForce = rotForce;
		maxRotationForce = maxRotForce;
		if(maxRotForce.x != 0){
			setXAxisAsMain();
		} else if(maxRotForce.y != 0){
			toggles[1].isOn = true;
		} else if(maxRotForce.z != 0){
			toggles[2].isOn = true;
		}		
		selectedPartName.text = PartsPanelConnection.Instance.getSelectedPart().name;
	}

	public void setXAxisAsMain(){
		setMinAndMax(-maxRotationForce.x, maxRotationForce.x);
		updateValue(rotationForce.x);
	}

	public void setYAxisAsMain(){
		setMinAndMax(-maxRotationForce.y, maxRotationForce.y);
		updateValue(rotationForce.y);
	}

	public void setZAxisAsMain(){
		setMinAndMax(-maxRotationForce.z, maxRotationForce.z);
		updateValue(rotationForce.z);
	}

	public void sendNewValuesToControls(){
		VisualMotorControl controls = PartsPanelConnection.Instance.getSelectedPart().GetComponent<VisualMotorControl>();
	}


}
