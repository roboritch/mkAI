using UnityEngine;
using System.Collections;

public class FootAngleControl : MonoBehaviour{

	[SerializeField] private Piston PistonL;
	[SerializeField] private Piston PistonR;
	[SerializeField] private Transform PostonEndpointL;
	[SerializeField] private Transform PostonEndpointR;


	[SerializeField] private float lengthLeft, lengthRight, lengthTop, lengthBottem;
	[SerializeField] private float diangonalULBR, diangonalURBL;

	[SerializeField] private float upperAngleL, upperAngleR;
	[SerializeField] private float lowerAngleL, lowerAngleR;

	void Update(){
		//length of all lines
		lengthTop = Vector3.Distance(PistonL.transform.position, PistonR.transform.position);
		lengthRight = Vector3.Distance(PistonR.transform.position, PostonEndpointR.position);
		lengthBottem = Vector3.Distance(PostonEndpointL.position, PostonEndpointR.position);
		lengthLeft = Vector3.Distance(PistonL.transform.position, PostonEndpointL.position);
		diangonalULBR = Vector3.Distance(PistonL.transform.position, PostonEndpointR.position);
		diangonalURBL = Vector3.Distance(PistonR.transform.position, PostonEndpointL.position);

		//get angles
		upperAngleL = triangleAngleCalculation(lengthLeft, lengthTop, diangonalURBL);
		upperAngleR = triangleAngleCalculation(lengthRight, lengthTop, diangonalULBR);
		lowerAngleL = triangleAngleCalculation(lengthLeft, lengthBottem, diangonalULBR);
		lowerAngleR = triangleAngleCalculation(lengthRight, lengthBottem, diangonalURBL);

		//radToDeg
		upperAngleL = radToDeg(upperAngleL);
		upperAngleR = radToDeg(upperAngleR);
		lowerAngleL = radToDeg(lowerAngleL);
		lowerAngleR = radToDeg(lowerAngleR);
	}


	private float triangleAngleCalculation(float adgSide1, float adgSide2, float opSide){
		float a2 = (adgSide1 * adgSide1);
		float b2 = (adgSide2 * adgSide2);
		float c2 = (opSide * opSide);
		float bot = 2 * adgSide1 * adgSide2;

		float inside = a2 + b2 - c2;
		inside /= bot;


		return Mathf.Acos(inside);
	}

	private float radToDeg(float rad){
		return rad * (180f / Mathf.PI);
	}

}
