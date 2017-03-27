using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using UnityEditor;

public class KeyEvents : Singleton<KeyEvents>{
	
	[SerializeField] private string localFileName = "Generic Bindings";
	[SerializeField] private string localFolderName = "Bindings";
	private ActionBindings currentActionBindings;

	void Start(){
		setDefaultBindings();
		if(!Directory.Exists(appendToLocalPath(localFolderName)))
			Directory.CreateDirectory(appendToLocalPath(localFolderName));
		
		if(!File.Exists(appendToLocalPath(localFolderName + "/" + localFileName))){
			SaveAndLoadXML.saveXML<ActionBindings>(appendToLocalPath(localFolderName + "/" + localFileName), currentActionBindings);
		} else{ //disabled while developing new keys
			//tryToLoadBidings();
		}
	}

	public void tryToLoadBidings(){
		ActionBindings tempAB; 
		if(!SaveAndLoadXML.loadXML<ActionBindings>(appendToLocalPath(localFolderName + "/" + localFileName), out tempAB)){
			Debug.LogWarning("XML bindings load failure");
			return;
		}
		currentActionBindings = tempAB;
	}

	private string appendToLocalPath(string filePathAndName){
		return Application.dataPath + "/" + filePathAndName;
	}

	public ActionBindings getActionBindings(){
		return currentActionBindings;
	}

	public void saveNewActionBindings(ActionBindings bindings){
		
	}

	#region setDefaultBindings
	private void setDefaultBindings(){
		currentActionBindings.deselectUI = new KeyCode[1];
		currentActionBindings.deselectUI[0] = KeyCode.Escape;
		////////////////////////////////////////////////////////////
		currentActionBindings.undoPartUIMovment = new KeyCode[2];
		currentActionBindings.undoPartUIMovment[0] = KeyCode.LeftControl;
		currentActionBindings.undoPartUIMovment[1] = KeyCode.Z;
		////////////////////////////////////////////////////////////
	}

	#endregion

	//key events must be manualy placed in the update
	//can be done with delegets be this is easer to debug
	//with all elements visible
	#region button checks
	void Update(){
		deselectUICheck();
	}

	private void deselectUICheck(){
		if(correctKeysPressed(currentActionBindings.deselectUI)){
			if(!PartsPanelConnection.Exists())
				PartsPanelConnection.Instance.deselectAllParts();
		}
		if(correctKeysPressed(currentActionBindings.undoPartUIMovment)){
			if(!PartsPanelConnection.Exists())
				PartsPanelConnection.Instance.undoLastMoveComand();
		}
	}

	/// <summary>
	/// checks to see if all the keys pressed.
	/// </summary>
	/// <returns><c>true</c>, if all keys pressed was corrected, <c>false</c> otherwise.</returns>
	/// <param name="keys">Keys.</param>
	private bool correctKeysPressed(KeyCode[] keys){
		if(keys == null)
			return false;
		bool firstKeyCanBeHeld = false;
		if(keys[0] == default(KeyCode)){ // no key set
			return false;
		} else if(keys[0] == KeyCode.LeftControl || keys[0] == KeyCode.RightControl || keys[0] == KeyCode.LeftAlt || keys[0] == KeyCode.RightAlt || keys[0] == KeyCode.LeftShift || keys[0] == KeyCode.RightShift){
			firstKeyCanBeHeld = true;
		}

		foreach( KeyCode key in keys ){
			if(!Input.GetKeyDown(key)){
				if(!firstKeyCanBeHeld){
					return false;
				} else if(!Input.GetKey(key)){
					return false;
				}
			} else if(key == default(KeyCode)){
				return true;
			}
			firstKeyCanBeHeld = false;
		}
		return true;

	}


	#endregion

}


public struct ActionBindings{
	[XmlArrayItemAttribute("key")]
	[XmlArrayAttribute("deselectUI")]
	public KeyCode[] deselectUI;

	[XmlArrayItemAttribute("key")]
	[XmlArrayAttribute("undoPartUIMovment")]
	public KeyCode[] undoPartUIMovment;
}