using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ExternalPart))]
/*[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ConfigurableJoint))]*/
public class Motor : MonoBehaviour{

	#region basic motor properties
	[SerializeField] private string[] motorUIControlNames;

	[SerializeField] private Vector3 startRotation = new Vector3();
	[SerializeField] private Vector3 minRotation = new Vector3();
	[SerializeField] private Vector3 maxRotation = new Vector3();


	private Vector3 desiredRotation = new Vector3();
	private Vector3 desiredRotationForce = new Vector3();
	private float secondsOverPhysicsFramesRatio;
	[SerializeField] private Vector3 maxRotationForce = new Vector3(0, 0, 0);
	// 0 means an axis is fixed

	#endregion

	#region getters for basic properties

	public Vector3 getDesiredRotation(){
		return desiredRotation;
	}

	public float getDesiredRotationAxis(int axis){
		return desiredRotation[axis];
	}

	//////////////////////////////////////////////////////////

	/// <summary>
	/// Gets the current rotation.
	/// </summary>
	/// <returns>The current rotation.</returns>
	public Vector3 getCurrentRotation(){
		return transform.localEulerAngles;
	}

	/// <summary>
	/// 0 = x
	/// </summary>
	public float getCurrentRotationAxis(int axis){
		return transform.localEulerAngles[axis];
	}

	/////////////////////////////////////////////////////	
	public Vector3 getMinRotationAngle(){
		return minRotation;
	}

	/// <summary>
	/// 0 = x
	/// </summary>
	public float getMinRotationAngleAxis(int axis){
		return minRotation[axis];
	}

	/////////////////////////////////////////////////////	
	public Vector3 getMaxRotationAngle(){
		return maxRotation;
	}

	/// <summary>
	/// 0 = x
	/// </summary>
	public float getMaxRotationAngleAxis(int axis){
		return maxRotation[axis];
	}

	/////////////////////////////////////////////////////	
	public Vector3 getMaxRotationForce(){
		return maxRotationForce;
	}

	/// <summary>
	/// 0 = x
	/// </summary>
	public float getMaxRotationForceAxis(int axis){
		return maxRotationForce[axis];
	}

	/////////////////////////////////////////////////////	
	public Vector3 getDesiredRotaionForce(){
		return desiredRotationForce;
	}

	/// <summary>
	/// 0 = x
	/// </summary>
	public float getDesiredRotationForceAxis(int axis){
		return desiredRotationForce[axis];
	}

	#endregion

	#region update wanted rotation and force

	public void updateProperties(Vector3 wantedRot, Vector3 wantedForce){
		desiredRotation = wantedRot;
		desiredRotationForce = wantedForce;
	}

	public void updateProperties(int axis, float wantedRot, float wantedForce){
		switch(axis){
		case 0:
			desiredRotation.x = wantedRot;
			desiredRotationForce.x = wantedForce;
			break;
		case 1:
			desiredRotation.y = wantedRot;
			desiredRotationForce.y = wantedForce;
			break;
		case 2:
			desiredRotation.z = wantedRot;
			desiredRotationForce.z = wantedForce;
			break;
		default:
			break;
		}
	}

	#endregion

	#region initalization

	private ExternalPart conectedObject;
	private VisualMotorControl controler;

	/// <summary>
	/// Rotations constraint error avodance prevents the 0 equal to 360 convertion
	/// from causing a constrant error by shifting all roations + or - min float accurat numer.
	/// </summary>
	private void rotationConstraintErrorAvodance(){
		for(int axis = 0; axis < 3; axis++){
			if(maxRotationForce[axis] != 0){
				if(minRotation[axis] == 0 && maxRotation[axis] == 360f){
					return; // the rotation for this axis is free
				}
				if(startRotation[axis] == minRotation[axis]){
					startRotation[axis] += 0.02f;
				} else if(startRotation[axis] == maxRotation[axis]){
					startRotation[axis] -= 0.02f;
				}
			}
		}
	}

	void Awake(){
		rotationConstraintErrorAvodance();
		desiredRotationForce = maxRotationForce;
	}

	void Start(){
		conectedObject = GetComponent<ExternalPart>();
		rBody = GetComponent<Rigidbody>();
		joint = GetComponent<ConfigurableJoint>();

		secondsOverPhysicsFramesRatio = Time.fixedDeltaTime;
		//give the motor acsess to it's UI controler
		//this property can be null
		controler = PartsPanelConnection.Instance.getVisualMotorControl(name);
		transform.localEulerAngles = startRotation;
		setupJoint();
	}

	private bool validMoterName(string name){
		return !(name == "" || name == null);
	}

	[SerializeField] private Vector3 mainAxis;

	private void setupJoint(){
		if(!(mainAxis[0] == 1f ^ mainAxis[1] == 1f ^ mainAxis[2] == 1f)){
			Debug.LogError("joint setup failed, no valid main axis.");
			return;
		}

		joint.axis = mainAxis;
		// values represent the world axis of the joints local axis
		// ie the vec3's used can be maped to these axis
		// example the joint axis is 1 then for axis0
		// the all vectors set using the world cordatint X will be maped to y
		int axis0, axis1, axis2; 
		axis0 = 0; 
		axis1 = 1;
		axis2 = 2;

		if(mainAxis[0] == 1f){
			joint.secondaryAxis = new Vector3(0, 1f, 0);
			axis0 = 0; // look at x for all joint x values
			axis1 = 1;
			axis2 = 2;
		} else if(mainAxis[1] == 1f || mainAxis[2] == 1f){
			if(mainAxis[1] == 1f){
				axis0 = 1; // look at y for all joint x values
				axis1 = 0;
				axis2 = 2;
			} else{
				axis0 = 2; // look at z for all joint x values
				axis1 = 0;
				axis2 = 1;
			}
			joint.secondaryAxis = new Vector3(1f, 0, 0);
		}	

		SoftJointLimit limitValues = default(SoftJointLimit);
		limitValues.contactDistance = 3f; // run the solver more often (less janky for fast limit hits)
		// set main axis range of motion
		if(minRotation[axis0] == 0 && maxRotation[axis0] == 360f){
			joint.angularXMotion = ConfigurableJointMotion.Free;
		} else{
			joint.angularXMotion = ConfigurableJointMotion.Limited;
			limitValues.limit = minRotation[axis0];
			joint.lowAngularXLimit = limitValues;
			limitValues.limit = maxRotation[axis0];
			joint.highAngularXLimit = limitValues;
		}
			
		if(minRotation[axis1] == 0 && maxRotation[axis1] == 360f){
			joint.angularYMotion = ConfigurableJointMotion.Free;
		} else if(maxRotationForce[axis1] == 0){
			joint.angularYMotion = ConfigurableJointMotion.Locked;
		} else{
			joint.angularYMotion = ConfigurableJointMotion.Limited;
			limitValues.limit = maxRotation[axis1];
			joint.angularYLimit = limitValues;
		}
			
		if(minRotation[axis2] == 0 && maxRotation[axis2] == 360f){
			joint.angularZMotion = ConfigurableJointMotion.Free;
		} else if(maxRotationForce[axis2] == 0){
			joint.angularZMotion = ConfigurableJointMotion.Locked;
		} else{
			joint.angularZMotion = ConfigurableJointMotion.Limited;
			limitValues.limit = maxRotation[axis2];
			joint.angularZLimit = limitValues;
		}


	}

	#endregion

	#region phisics work
	private Rigidbody rBody;
	private ConfigurableJoint joint;

	void FixedUpdate(){
		
	}
		
	#endregion

	#region getDataFrom VisualMotorControl
	void Update(){
		VisualMotorControl UIControler;
		UIControler = PartsPanelConnection.Instance.getVisualMotorControl(name);
	}
	#endregion

	#region motor to ui connections
	public void connectMotorToUI(){ 
		foreach( string motorName in motorUIControlNames ){
			char axis = motorName[motorName.Length - 2];
			string motorNameNoAxis = motorName.Remove(motorName.Length - 3);
			switch(axis){
			case 'X':
				PartsPanelConnection.Instance.getVisualMotorControl(motorNameNoAxis).motorX = this;
				break;
			case 'Y':
				PartsPanelConnection.Instance.getVisualMotorControl(motorNameNoAxis).motorY = this;
				break;
			case 'Z':
				PartsPanelConnection.Instance.getVisualMotorControl(motorNameNoAxis).motorZ = this;
				break;
			default:
				Debug.LogError("not a valid axis for motor connection");
				break;
			}
		}
	}
	#endregion

	#region unused code

	/// <summary>
	/// rotates the part, not physics based 
	/// </summary>
	private void incrementMovment(){
		Vector3 rotationIncrement = Vector3.zero;
		rotationIncrement.x = Mathf.MoveTowards(transform.localEulerAngles.x, desiredRotation.x, Mathf.Min(desiredRotationForce.x, maxRotation.x) * secondsOverPhysicsFramesRatio) - transform.localEulerAngles.x;
		rotationIncrement.y = Mathf.MoveTowards(transform.localEulerAngles.y, desiredRotation.y, Mathf.Min(desiredRotationForce.y, maxRotation.y) * secondsOverPhysicsFramesRatio) - transform.localEulerAngles.y;
		rotationIncrement.z = Mathf.MoveTowards(transform.localEulerAngles.z, desiredRotation.z, Mathf.Min(desiredRotationForce.z, maxRotation.z) * secondsOverPhysicsFramesRatio) - transform.localEulerAngles.z;
		transform.Rotate(rotationIncrement);
	}
	#endregion

}


/*
	/// <summary>
/// Sets the desired rotation.
/// </summary>
/// <returns><c>true</c>, if desired rotation was set, <c>false</c> otherwise.</returns>
public bool setDesiredRotation(char axisWanted, float degOfRotation, float degPerSecond){
	if(axisWanted != 'x' && axisWanted != 'y' && axisWanted != 'z'){
		Debug.LogError("no valid axis selected");
		return false;
	}

	if(axisWanted == 'x'){
		desieredRotation.x = degOfRotation;
		desieredRotationForce.x = degPerSecond;
	} else if(axisWanted == 'y'){
		desieredRotation.y = degOfRotation;
		desieredRotationForce.y = degPerSecond;
	} else if(axisWanted == 'z'){
		desieredRotation.z = degOfRotation;
		desieredRotationForce.z = degPerSecond;
	}


	return true;
}
	*/