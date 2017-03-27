using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

/// <summary>
/// To be attached to the root element of any gameObject that is controled by the user
/// </summary>
public class MasterControl : MonoBehaviour{
	[SerializeField] private string masterObjectName;
	[SerializeField] private GameObject mechMotorUI;

	Dictionary<string,ExternalPart> listOfAllObjectsAttachted = new Dictionary<string, ExternalPart>();

	void Start(){
		UserControlledObjects.Instance.addObject("masterObjectName", gameObject);
		displayMechUI();
	}

	#region add script instances to Dictionary
	/// <summary>
	/// Called by all motors to add themselves to the list of active motors
	/// </summary>
	/// <param name="partName">Part name.</param>
	/// <param name="motorInstance">Motor instance.</param>
	public void addPartToDictionary(string partName, ExternalPart partInstance){
		listOfAllObjectsAttachted.Add(partName, partInstance);
	}

	public ExternalPart getExternalPart(string partName){
		ExternalPart part;
		listOfAllObjectsAttachted.TryGetValue(partName, out part);
		return part;
	}

	#endregion

	/// <summary>
	/// Displays the mech UI and
	/// initalizes all conections to the mechs parts
	/// </summary>
	public void displayMechUI(){
		if(listOfAllObjectsAttachted.Count == 0){
			Debug.LogError("no part refrences in master control!");
			return;
		}
		PartsPanelConnection.Instance.changeSelectedRobotUI(name, this, mechMotorUI);
		//send setup calls to motors so they give data to the UI
		Motor curMotor;
		Dictionary<string,ExternalPart>.Enumerator list = listOfAllObjectsAttachted.GetEnumerator();
		while(list.MoveNext()){
			if(list.Current.Value.tryGetPartMotor(out curMotor)){
				curMotor.connectMotorToUI();
			}
		} 

		PartsPanelConnection.Instance.UISetupDone();
	}


}
