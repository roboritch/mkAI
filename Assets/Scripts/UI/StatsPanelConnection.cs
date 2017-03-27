using UnityEngine;
using System.Collections;

public class StatsPanelConnection : Singleton<StatsPanelConnection>{
	public GameObject getStatsPanel(){
		return gameObject;
	}
}
