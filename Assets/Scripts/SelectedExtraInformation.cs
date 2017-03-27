using UnityEngine;
using System.Collections;

public class SelectedExtraInformation : Singleton<SelectedExtraInformation>{

	void Start(){
		resetPanal();
	}

	public void changePanel(GameObject PreInstateatedGameObject){
		resetPanal();
	}

	public void resetPanal(){
		foreach( Transform child in transform ){
			UnityExtentionMethods.destoryAllChildren(child);
		}
	}


}
