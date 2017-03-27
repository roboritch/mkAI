using UnityEngine;
using System.Collections;

public class PrefabHolder : MonoBehaviour{
	void Start(){
		transform.DetachChildren();
		Destroy(gameObject);
	}
}
