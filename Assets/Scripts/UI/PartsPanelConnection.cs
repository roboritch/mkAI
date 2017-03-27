using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartsPanelConnection : Singleton<PartsPanelConnection>{
	[SerializeField] private float MouseMoveInterval = 0.02f;
	[SerializeField] private float minTimeDiffSecondsForDoubleClick = 0.5f;
	[SerializeField] private GameObject currentDisplayBase;
	private PartUI currentlySelectedPartProjection;
	#region visual representation base
	private PartUIBase currentProjectionBase;

	public void setCurrentProjectionBase(PartUIBase uiBase){
		currentProjectionBase = uiBase;
	}

	public PartUIBase getCurrentProjectionBase(){
		return currentProjectionBase;
	}
	#endregion
	private GameObject SelectedRobot;

	private void clearAllChildren(){
		for(int i = 0; i < transform.childCount; i++){
			UnityExtentionMethods.destoryAllChildren(transform.GetChild(i));
		}
	}

	void Awake(){
		clearAllChildren();
	}

	//a list of all parts used by the selected mech
	//currently only the vishial motor controles are refrenced here
	#region part list
	private Dictionary<string,VisualMotorControl> partUIDictionary = new Dictionary<string,VisualMotorControl>();

	/// <summary>
	/// Gets the visual motor control or returns null if
	/// non exists for this part.
	/// </summary>
	/// <returns>The visual motor control or null</returns>
	/// <param name="motorUIName">Motor user interface name.</param>
	public VisualMotorControl getVisualMotorControl(string motorUIName){
		VisualMotorControl control;
		if(partUIDictionary.TryGetValue(motorUIName, out control)){
			return control;
		}

		return null;
	}

	public void addPartToList(string partName, VisualMotorControl partUIScript){
		if(!partUIDictionary.ContainsKey(partName)){
			partUIDictionary.Add(partName, partUIScript);
		}
	}

	public void removePartFromList(string partName){
		if(partUIDictionary.ContainsKey(partName)){
			partUIDictionary.Remove(partName);
		}
	}

	public void removeAllPartsFromList(){
		partUIDictionary.Clear();
	}
	#endregion

	#region part movment undo
	private Stack<PartAndMoveAmount> partMoveUndoStack = new Stack<PartAndMoveAmount>();

	public void addMovmentToUndoStack(string partUIName, Vector3 rotationAmount){
		PartAndMoveAmount undoSlice;
		undoSlice.partName = partUIName;
		undoSlice.rotationAmount = rotationAmount;
		partMoveUndoStack.Push(undoSlice);
	}

	public void undoLastMoveComand(){
		if(partMoveUndoStack.Count == 0)
			return;
		PartAndMoveAmount undoSlice = partMoveUndoStack.Pop();
		VisualMotorControl moterScript;
		partUIDictionary.TryGetValue(undoSlice.partName, out moterScript);
		moterScript.rotateEuler(-undoSlice.rotationAmount);
	}
	#endregion

	#region part selection

	/// <summary>
	/// Selects the part user interface element.
	/// </summary>
	/// <param name="selectedElement">Selected element. Set to null to deselect</param>
	public void selectPartUIElement(PartUI selectedElement){
		//send mesage to part that it is deselected
		if(currentlySelectedPartProjection != null)
			currentlySelectedPartProjection.deselectThisUIElement();
		currentlySelectedPartProjection = selectedElement;

		//send mesage to part that it is selected
		if(selectedElement != null)
			selectedElement.selectThisUIElement();
	}

	public PartUI getSelectedPart(){
		return currentlySelectedPartProjection;
	}

	public void deselectAllParts(){
		selectPartUIElement(null);
	}

	#endregion

	/// <summary>
	///  Changes the selected robot UI.
	/// All stored data from that UI must be removed
	/// </summary>
	/// <param name="partBaseName">Part base name.</param>
	/// <param name="controler">Controler.</param>
	/// <param name="PartUIPrefab">Part user interface prefab.</param>
	public void changeSelectedRobotUI(string partBaseName, MasterControl controler, GameObject PartUIPrefab){
		removeAllPartsFromList();
		selectPartUIElement(null);
		if(currentDisplayBase != null){
			UnityExtentionMethods.destoryAllChildren(currentDisplayBase.transform);
		}
		GameObject temp = Instantiate(PartUIPrefab) as GameObject;
		temp.name = partBaseName;
		temp.transform.SetParent(transform);
		temp.GetComponent<PartUIBase>().reciveMasterControl(controler);
	}
		
	#region singlton values used by parented object

	public float getMouseMoveInterval(){
		return MouseMoveInterval;
	}

	public float getDoubleClickTime(){
		return minTimeDiffSecondsForDoubleClick;
	}

	public GameObject getPartsPanel(){
		return gameObject;
	}

	#endregion

	public void UISetupDone(){
		foreach( VisualMotorControl vmc in GetComponentsInChildren<VisualMotorControl>() ){
			vmc.resetAllPeramsToMatchMotors();
		}

	}
}


public struct PartAndMoveAmount{
	public string partName;

	public Vector3 rotationAmount;
}