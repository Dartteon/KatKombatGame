using UnityEngine;
using System.Collections;

public class MainControllerScript : MonoBehaviour {
	public GameObject attachedKat;
	private Color katColor;
	private GameObject movementStick, attack1Stick;

	void Start () {

	}
	
	void Update () {
	
	}

	public void AttachKat(GameObject kat){
		attachedKat = kat;

		movementStick = transform.FindChild ("MovementJoystick").gameObject;
		movementStick.SendMessage ("AttachKat", attachedKat);
		katColor = attachedKat.GetComponent<StatsScript> ().getColor ();
		movementStick.transform.FindChild ("TargetRing").gameObject.GetComponent<SpriteRenderer> ().color
			= new Color (katColor.r, katColor.g, katColor.b, 0.4f);
//		Debug.Log (movementStick.transform.FindChild ("TargetRing").gameObject.GetComponent<SpriteRenderer> ().color.ToString ());
		
//		attack1Stick = transform.FindChild ("Attack1Joystick").gameObject;
//		attack1Stick.SendMessage("AttachKat", attachedKat);
		transform.FindChild ("Attack1Joystick").gameObject.SendMessage("AttachKat", attachedKat);
		transform.FindChild ("Attack2Joystick").gameObject.SendMessage("AttachKat", attachedKat);
		transform.FindChild ("Attack3Joystick").gameObject.SendMessage("AttachKat", attachedKat);
	}
}
