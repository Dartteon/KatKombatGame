using UnityEngine;
using System.Collections;

public class MTMainController : MonoBehaviour {
	public GameObject attachedKat;
	private Color katColor;
	private GameObject movementStick, attack1Stick;
	MTControllerJoystick angleController;
	MTControllerCommand[] commandControllersArray = new MTControllerCommand[3];
	MTControllerJump jumpController;
	Manager manager;
	Vector2 zeroVector = new Vector2(0,0);

	void Start(){
		alignControlsToScreen ();
	}

	void alignControlsToScreen(){
		float screenWidth = Screen.width;
		this.transform.parent = Camera.main.transform;
	}

	public void setManager(Manager theManager){
		manager = theManager;
	}

	public void AttachKat(GameObject kat){
		Debug.Log ("Attaching Kat " + kat.ToString ());
		attachedKat = kat;
		GameObject fourCmdButtons = transform.Find ("FourCommandButtons").gameObject;

		katColor = attachedKat.GetComponent<StatsScript> ().getColor ();
		transform.FindChild ("MovementJoystick").gameObject.SendMessage("AttachKat", attachedKat);
		angleController = transform.FindChild ("MovementJoystick").GetComponent<MTControllerJoystick> ();

		fourCmdButtons.transform.FindChild ("Attack1Joystick").gameObject.SendMessage("AttachKat", attachedKat);
//		fourCmdButtons.transform.FindChild ("Attack1Joystick").GetComponent<MTControllerCommand> ().AttachKat (attachedKat);
		commandControllersArray [0] = fourCmdButtons.transform.FindChild ("Attack1Joystick").GetComponent<MTControllerCommand> ();

		fourCmdButtons.transform.FindChild ("Attack2Joystick").gameObject.SendMessage("AttachKat", attachedKat);
		commandControllersArray [1] = fourCmdButtons.transform.FindChild ("Attack2Joystick").GetComponent<MTControllerCommand> ();

		fourCmdButtons.transform.FindChild ("Attack3Joystick").gameObject.SendMessage("AttachKat", attachedKat);
		commandControllersArray [2] = fourCmdButtons.transform.FindChild ("Attack3Joystick").GetComponent<MTControllerCommand> ();

		fourCmdButtons.transform.FindChild ("JumpButton").gameObject.SendMessage("AttachKat", attachedKat);
		jumpController = fourCmdButtons.transform.FindChild ("JumpButton").GetComponent<MTControllerJump> ();

	//	Debug.Log (angleController.ToString () + " " + commandControllersArray [0].ToString ());
	}

	void Update(){
		if (Input.GetKey(KeyCode.JoystickButton0)){
			commandControllersArray[1].handleTap(zeroVector, zeroVector);
		}
		else if (Input.GetKey(KeyCode.JoystickButton1)){
			commandControllersArray[0].handleTap(zeroVector, zeroVector);
		}
		else if (Input.GetKey(KeyCode.JoystickButton2)){
			commandControllersArray[2].handleTap(zeroVector, zeroVector);
		}
		else if (Input.GetKey(KeyCode.JoystickButton3)){
			jumpController.handleTap(zeroVector, zeroVector);
		}

		float horzAxis = Input.GetAxis ("Horizontal");
		float vertAxis = Input.GetAxis ("Vertical");
//		Debug.Log (horzAxis + " " + vertAxis);
		if (horzAxis != 0 || vertAxis != 0) {
			float xCoord = (horzAxis != 0)? horzAxis : 0f;
			float yCoord = (vertAxis != 0)? vertAxis : 0f;
			Vector2 finalPosition = new Vector2(xCoord, yCoord);
			angleController.handleControllerAxisInput(finalPosition);

		}
	}
}
