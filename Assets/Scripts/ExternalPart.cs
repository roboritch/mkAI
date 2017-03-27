using UnityEngine;
using System.Collections;

/// <summary>
/// External part.
/// Only one external part is alowed per object!
/// </summary>
[RequireComponent(typeof(ConfigurableJoint))]
public class ExternalPart : MonoBehaviour{
	[SerializeField] private MasterControl masterControlForThisPart;

	public MasterControl getMasterControl(){
		return masterControlForThisPart;
	}

	private Motor motorOnThisPart;

	/// <summary>
	/// Tries the get part motor.
	/// </summary>
	/// <returns><c>true</c>, if get part motor was gotten, <c>false</c> otherwise.</returns>
	/// <param name="motor">Motor.</param>
	public bool tryGetPartMotor(out Motor motor){
		motor = motorOnThisPart;
		return !(motorOnThisPart == null);
	}

	void Start(){
		if(masterControlForThisPart == null){
			masterControlForThisPart = transform.root.GetComponent<MasterControl>();
			if(masterControlForThisPart == null){
				masterControlForThisPart = findRootUsingJoints().GetComponent<MasterControl>();
			}
		}
		if(masterControlForThisPart == null){
			Debug.LogError("ERROR: no controler found for '" + name + "' ExternalPart");
		}
		masterControlForThisPart.addPartToDictionary(name, this);

		motorOnThisPart = GetComponent<Motor>(); // can be null
		Collider vs;
	}

	private Transform findRootUsingJoints(){
		Rigidbody body = GetComponent<Rigidbody>();
		if(body == null){
			return null;
		}
		Joint joint;
		do{
			joint = body.GetComponent<Joint>();
			if(joint == null){
				break;
			}
			body = GetComponent<Joint>().connectedBody;
		} while(true);

		return body.transform;
	}

}
