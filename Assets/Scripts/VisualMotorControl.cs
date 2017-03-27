using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(PartUI))]
public class VisualMotorControl : MonoBehaviour , ISelectable{
	#region mouse click posibilities
	// x = 0 , y = 1 , z = 2 //
	[SerializeField] private int leftMouseVector = -1;
	[SerializeField] private int rightMouseVector = -1;
	[SerializeField] private int middleMouseVector = -1;
	[SerializeField] private int mouseWheelVector = -1;

	[SerializeField] private int moveBothEnabledMouse = -1;
	[SerializeField] private int bothVecHorizontal = 1;
	[SerializeField] private int bothVecVertical = 0;

	[SerializeField] private float mouseWheelSensitivity = 1f;
	[SerializeField] private float mouseSensitivity = 10f;
	[SerializeField] private bool invertHorizontalMouse = false;
	[SerializeField] private bool invertVertucalMouse = false;


	private Vector3 vecConverted(int vecDirection){
		switch(vecDirection){
		case 0:
			return new Vector3(1, 0, 0);
		case 1:
			return new Vector3(0, 1, 0);
		case 2:
			return new Vector3(0, 0, 1);
		default:
			return new Vector3();
		}
	}

	private delegate void MouseClickMove(int axis);

	private MouseClickMove onLeftMouseClick;
	private MouseClickMove onRightMouseClick;
	private MouseClickMove onMiddleMouseClick;

	private void doNothing(int axis){
		
	}

	private void moveHorizontal(int axis){
		float moveAmount = mouseMovmentAmount.x;
		if(moveAmount == 0){
			return;
		}
		rotateUsingFloatAmount(axis, moveAmount);
	}

	private void moveVertical(int axis){
		float moveAmount = mouseMovmentAmount.y;
		if(moveAmount == 0){
			return;
		}
		rotateUsingFloatAmount(axis, moveAmount);
	}

	//due too local eruler angle 360 correction a personal rotation must be
	//used to insure rotation constrantes work properly
	private Vector3 personalRotaiton;

	private void rotateUsingFloatAmount(int axis, float moveAmount){
		if(axis > 2 || axis < 0)
			return;
		moveAmount *= mouseSensitivity;
		if(invertHorizontalMouse){
			moveAmount = -moveAmount;
		}

		Vector3 rotationAmountVec = new Vector3();
		if(minRotation[axis] == 0 && maxRotation[axis] == 360f){ //rotation is unbounded
			rotationAmountVec[axis] = moveAmount;
			personalRotaiton[axis] += moveAmount;
			transform.Rotate(rotationAmountVec);
			return;
		}

		rotationAmountVec[axis] = personalRotaiton[axis];
		if(moveAmount > 0){
			personalRotaiton[axis] = Mathf.MoveTowards(personalRotaiton[axis], maxRotation[axis], moveAmount);
		} else{
			personalRotaiton[axis] = Mathf.MoveTowards(personalRotaiton[axis], minRotation[axis], -moveAmount);
		}
		rotationAmountVec[axis] = personalRotaiton[axis] - rotationAmountVec[axis];

		transform.Rotate(rotationAmountVec);
	}

	private void moveBoth(int horizontalAxis, int verticalAxis){
		moveHorizontal(horizontalAxis);
		moveVertical(verticalAxis);
	}

	private void moveWheel(){
		rotateUsingFloatAmount(mouseWheelVector, Input.GetAxis("Mouse ScrollWheel") * mouseWheelSensitivity);
	}
	#endregion

	#region rotaiton restrictions

	#endregion

	[SerializeField] private int leftClickMoveType = 0;
	[SerializeField] private int rightClickMoveType = 0;
	[SerializeField] private int middleClickMoveType = 0;

	void Awake(){
		PartsPanelConnection.Instance.addPartToList(name, this);
	}

	private VisualMotorControl[] childrenControler;

	void Start(){ 
		#region setup mouse clicks
		switch(leftClickMoveType){
		case 0:
			onLeftMouseClick = moveHorizontal;
			break;
		case 1:
			onLeftMouseClick = moveVertical;
			break;
		default:
			break;
		}

		switch(rightClickMoveType){
		case 0:
			onRightMouseClick = moveHorizontal;
			break;
		case 1:
			onRightMouseClick = moveVertical;
			break;
		default:
			break;
		}

		switch(middleClickMoveType){
		case 0:
			onMiddleMouseClick = moveHorizontal;
			break;
		case 1:
			onMiddleMouseClick = moveVertical;
			break;
		default:
			break;
		}
		#endregion	
		partUI = GetComponent<PartUI>();
		childrenControler = new VisualMotorControl[transform.childCount];
		for(int i = 0; i < transform.childCount; i++){ //assumption all childern have VMC's
			childrenControler[i] = transform.GetChild(i).GetComponent<VisualMotorControl>();
		}
	}


	#region motor info and connections
	//Properties changed here but initaly set by motor
	private Vector3 desiredRotaionForce;

	/// <summary>
	/// Updates the motors conected to this controler.
	/// </summary>
	public void updateMotors(){
		if(motorX != null)
			motorX.updateProperties(0, transform.localEulerAngles.x, desiredRotaionForce.x);
		if(motorY != null)
			motorY.updateProperties(1, transform.localEulerAngles.y, desiredRotaionForce.y);
		if(motorZ != null)
			motorZ.updateProperties(2, transform.localEulerAngles.z, desiredRotaionForce.z);
	}


	public void changeRotationForce(Vector3 allForces){
		desiredRotaionForce = allForces;
	}

	public void changeRotationForce(int axis, float newForce){
		switch(axis){
		case 0:
			desiredRotaionForce.x = newForce;
			break;
		case 1:
			desiredRotaionForce.y = newForce;
			break;
		case 2:
			desiredRotaionForce.z = newForce;
			break;
		default:
			break;
		}
	}

	//Static properties used to restrict the projections movment
	private Vector3 maxRotationForce;
	private Vector3 minRotation;
	private Vector3 maxRotation;

	public Motor motorX;
	public Motor motorY;
	public Motor motorZ;


	public void resetAllPeramsToMatchMotors(){
		setCurrentlyDesiredRotationFromMotors();
		setMinAndMaxRotationFromMotors();
		setDesiredRotationForceFromMotors();
		setMaxRotaionForceFromMotor();
	}

	private Vector3 getCurrentRotation(){
		Vector3 currentRotation = new Vector3();
		if(!(motorX == null)){
			currentRotation.x = motorX.getCurrentRotationAxis(0);
		}
		if(!(motorY == null)){
			currentRotation.y = motorY.getCurrentRotationAxis(1);
		}
		if(!(motorZ == null)){
			currentRotation.z = motorZ.getCurrentRotationAxis(2);
		}
		return currentRotation;
	}

	private void setCurrentlyDesiredRotationFromMotors(){
		Vector3 temp = Vector3.zero;
		if(!(motorZ == null)){
			temp.z = motorZ.getDesiredRotationAxis(2);
		}
		if(!(motorX == null)){
			temp.x = motorX.getDesiredRotationAxis(0);
		}
		if(!(motorY == null)){
			temp.y = motorY.getDesiredRotationAxis(1);
		}
		transform.localEulerAngles = temp;
		personalRotaiton = temp;
	}

	private void setMinAndMaxRotationFromMotors(){
		if(!(motorX == null)){
			minRotation.x = motorX.getMinRotationAngleAxis(0);
		}
		if(!(motorY == null)){
			minRotation.y = motorY.getMinRotationAngleAxis(1);
		}
		if(!(motorZ == null)){
			minRotation.z = motorZ.getMinRotationAngleAxis(2);
		}

		if(!(motorX == null)){
			maxRotation.x = motorX.getMaxRotationAngleAxis(0);
		}
		if(!(motorY == null)){
			maxRotation.y = motorY.getMaxRotationAngleAxis(1);
		}
		if(!(motorZ == null)){
			maxRotation.z = motorZ.getMaxRotationAngleAxis(2);
		}
	}

	private void setDesiredRotationForceFromMotors(){
		if(!(motorX == null)){
			desiredRotaionForce.x = motorX.getDesiredRotationForceAxis(0);
		}
		if(!(motorY == null)){
			desiredRotaionForce.y = motorY.getDesiredRotationForceAxis(1);
		}
		if(!(motorZ == null)){
			desiredRotaionForce.z = motorZ.getDesiredRotationForceAxis(2);
		}
	}

	private void setMaxRotaionForceFromMotor(){
		if(!(motorX == null)){
			maxRotationForce.x = motorX.getMaxRotationForceAxis(0);
		}
		if(!(motorY == null)){
			maxRotationForce.y = motorY.getMaxRotationForceAxis(1);
		}
		if(!(motorZ == null)){
			maxRotationForce.z = motorZ.getMaxRotationForceAxis(2);
		}
	}

	#endregion

	#region onSelection
	private RotationSpeedPanel rotationRatePanel;

	/// <summary>
	/// brodcast by the PartUI when it is selected
	/// </summary>
	public void selected(){
		SelectedExtraInformation.Instance.resetPanal();
		GameObject extraControles = Instantiate(PartsPanelConnection.Instance.getCurrentProjectionBase().getRotationForcePanelPrefab()) as GameObject;
		extraControles.transform.SetParent(SelectedExtraInformation.Instance.transform);
		extraControles.GetComponent<StatsPanelDefaultLocation>().setDefaultStats(); // move to defaultLocation
		rotationRatePanel = extraControles.GetComponent<RotationSpeedPanel>();
		rotationRatePanel.setRotationForces(desiredRotaionForce, maxRotationForce);
	}

	public void deselected(){
		SelectedExtraInformation.Instance.resetPanal();
	}

	#endregion

	#region conection to Other scripts
	private PartUI partUI;


	#endregion

	#region other Movment
	public void rotateEuler(Vector3 rotation){
		transform.Rotate(rotation);
		regeneratMeshCollider();
	}

	#endregion

	#region mouse over this object
	private Vector2 mouseMovmentAmount;

	private bool checkMousePressed(){
		return Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2);
	}

	private bool checkMouseDown(){
		return Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2);
	}

	private bool mouseButtonIsUp(){
		return Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2);
	}

	private bool mouseOverThisFrame = false;
	private bool mouseButtonStillDown = false;

	void OnMouseOver(){
		if(checkMousePressed()){
			mouseOverThisFrame = true; // only change if pressed while on
		}
	}

	private Vector3 moveAmountThisMove = new Vector3();

	void Update(){
		if(mouseOverThisFrame || mouseButtonStillDown){
			mouseButtonStillDown = false;
			mouseMovmentAmount.x = Input.GetAxisRaw("Mouse X");
			mouseMovmentAmount.y = Input.GetAxisRaw("Mouse Y");
			Vector3 vectorMoveStart = transform.localEulerAngles;
			if(Input.GetMouseButton(0)){
				if(moveBothEnabledMouse == 0){
					moveBoth(bothVecHorizontal, bothVecVertical);
				} else{
					onLeftMouseClick(leftMouseVector);
				}
				mouseButtonStillDown = true;
			}
			if(Input.GetMouseButton(1)){
				if(moveBothEnabledMouse == 1){
					moveBoth(bothVecHorizontal, bothVecVertical);
				} else{
					onRightMouseClick(rightMouseVector);
				}
				mouseButtonStillDown = true;
			}
			if(Input.GetMouseButton(2)){
				if(moveBothEnabledMouse == 2){
					moveBoth(bothVecHorizontal, bothVecVertical);
				} else{
					onMiddleMouseClick(middleMouseVector);
				}

				mouseButtonStillDown = true;
			}
			Vector3 endMoveAmoutThisTick = transform.localEulerAngles;

			moveAmountThisMove += endMoveAmoutThisTick - vectorMoveStart;

			//Mouse is up an movent has stoped
			if(!mouseButtonStillDown && moveAmountThisMove != Vector3.zero){
				PartsPanelConnection.Instance.addMovmentToUndoStack(name, moveAmountThisMove);
				moveAmountThisMove = new Vector3();
			
				regeneratMeshCollider();
			}
			mouseOverThisFrame = false;
		}
	}

	public void regeneratMeshCollider(){
		partUI.getCollider().enabled = false;
		partUI.getCollider().enabled = true;
		foreach( VisualMotorControl vmc in childrenControler ){
			vmc.regeneratMeshCollider();
		}
	}
	#endregion

}

