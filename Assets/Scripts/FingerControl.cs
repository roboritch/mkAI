using UnityEngine;
using System.Collections;

public class FingerControl : MonoBehaviour{
	[SerializeField]
	private FingerScript[] thumb;
	[SerializeField]
	private FingerScript[] firstFinger;
	[SerializeField]
	private FingerScript[] secondFinger;
	[SerializeField]
	private FingerScript[] thirdFinger;
	[SerializeField]
	private FingerScript[] forthFinger;


	void Start(){
		if(thumb.Length != 0){
			thumb[0].GetComponent<ConfigurableJoint>().connectedBody = GetComponent<Rigidbody>();
			for(int i = 1; i < thumb.Length; i++){
				thumb[i].GetComponent<ConfigurableJoint>().connectedBody = thumb[i - 1].GetComponent<Rigidbody>();
			}
		}

		if(firstFinger.Length != 0){
			firstFinger[0].GetComponent<ConfigurableJoint>().connectedBody = GetComponent<Rigidbody>();
			for(int i = 1; i < thumb.Length; i++){
				firstFinger[i].GetComponent<ConfigurableJoint>().connectedBody = firstFinger[i - 1].GetComponent<Rigidbody>();
			}
		}

		if(secondFinger.Length != 0){
			secondFinger[0].GetComponent<ConfigurableJoint>().connectedBody = GetComponent<Rigidbody>();
			for(int i = 1; i < thumb.Length; i++){
				secondFinger[i].GetComponent<ConfigurableJoint>().connectedBody = secondFinger[i - 1].GetComponent<Rigidbody>();
			}
		}

		if(thirdFinger.Length != 0){
			thirdFinger[0].GetComponent<ConfigurableJoint>().connectedBody = GetComponent<Rigidbody>();
			for(int i = 1; i < thumb.Length; i++){
				thirdFinger[i].GetComponent<ConfigurableJoint>().connectedBody = thirdFinger[i - 1].GetComponent<Rigidbody>();
			}
		}
			
		if(forthFinger.Length != 0){
			forthFinger[0].GetComponent<ConfigurableJoint>().connectedBody = GetComponent<Rigidbody>();
			for(int i = 1; i < thumb.Length; i++){
				forthFinger[i].GetComponent<ConfigurableJoint>().connectedBody = forthFinger[i - 1].GetComponent<Rigidbody>();
			}
		}
	}



}
