using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class DebugUI : Singleton<DebugUI>{
	[SerializeField] private defaultTextHolder[] textItemsArray;
	private Dictionary<string,defaultTextHolder> debugTextAssentUseLog = new Dictionary<string,defaultTextHolder>();


	public bool reserveUpdatableTextObject(string reserveName){
		int minDubug = 0;

		while(debugTextAssentUseLog.ContainsValue(textItemsArray[minDubug])){
			minDubug++;
			if(textItemsArray.Length < minDubug){
				return false;
			}
		}

		debugTextAssentUseLog.Add(reserveName, textItemsArray[minDubug]);
		return true;
	}

	public bool useDebugTextObject(string reserveName, out defaultTextHolder textLocation){
		if(!debugTextAssentUseLog.TryGetValue(reserveName, out textLocation)){
			return false;
		}
		return true;
	}

	// Update is called once per frame
	void Start(){

	}
}
