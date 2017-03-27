using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UserControlledObjects : Singleton<UserControlledObjects>{
	private Dictionary<string,GameObject> userControledObjects = new Dictionary<string, GameObject>();

	public void addObject(string name, GameObject obj){
		userControledObjects.Add(name, obj);
	}

	public GameObject getObject(string name){
		GameObject obj;
		bool exists = userControledObjects.TryGetValue(name, out obj);
		if(!exists){
			Debug.LogError("no user controled object with that name!");
		}
		return obj;
	}
}
