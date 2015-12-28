using UnityEngine;
using System.Collections;

public class MTControllerJoystick : MonoBehaviour, Tappable {
	protected Vector2 direction;
	protected GameObject attachedKat;
//	protected GameObject targetRing;
	protected MovementScript moveScript;
	protected GameObject joystickCentre;
	protected GameObject joystickSource;
	private Vector2 stickSourceStartPos;

	private Vector2 activatedCamPos;
	private Vector2 activatedWorldPos;
	private Vector2 currCamPos;
	private Vector2 currWorldPos;
	private bool isActivated = false;
	private float lastActivatedTime;
	SpriteRenderer stickRenderer;

	private bool isUsingExternalController = false;

	void Start(){
//		targetRing = transform.FindChild ("TargetRing").gameObject;
		joystickCentre = transform.FindChild ("Circle").gameObject;
		stickRenderer = joystickCentre.GetComponent<SpriteRenderer> ();
		joystickSource = transform.FindChild ("StickSource").gameObject;
		stickSourceStartPos = joystickSource.transform.localPosition;
	}
	
	void Update(){
		if (!isUsingExternalController && isActivated && (attachedKat != null)) {
			if (!(Time.time - lastActivatedTime >= 0.1f)){
				direction = currCamPos - activatedCamPos;
				float angle = getAngle(direction);
				rotateKatAndRing(angle);

				Vector2 joyStickSourcePos = joystickSource.transform.position;
				Vector2 difference = currWorldPos - joyStickSourcePos;
				if (difference.magnitude <= 1.2f){
					
					joystickCentre.transform.position = currWorldPos;
				} else {
					Vector2 newJoystickPos = difference.normalized * 1.5f + joyStickSourcePos;
					joystickCentre.transform.position = newJoystickPos;
				}

				
				moveScript.walkInDirection(direction);
		//		Vector2 currSourcePos = joystickSource.transform.position;
			} else {
				joystickSource.transform.localPosition = stickSourceStartPos;
				turnOffActivate();
			}
		}
	}

	void turnOffActivate(){
		isActivated = false;
		joystickCentre.transform.localPosition = joystickSource.transform.localPosition;
//		stickRenderer.sortingOrder = -2;
//		joystickCentre.SetActive (false);
	}
	void turnOnActivate(){
		isActivated = true;
//		lastActivatedTime = Time.time;
//		joystickCentre.SetActive (true);
	}

	void registerTouch(){
		isActivated = true;
		lastActivatedTime = Time.time;
	}
	
	void rotateKatAndRing(float angle){
		moveScript.FaceFoward(Quaternion.Euler (new Vector3(0, 0, angle - 90)));
//		targetRing.transform.rotation = Quaternion.Euler (new Vector3(0, 0, angle -90));
	}
	
	float getAngle(Vector2 direction){
		return (Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg);
	}

	public void handleTap(Vector2 camPos, Vector2 worldPos){
		recordLastTouch (camPos, worldPos);
		registerTouch ();
	}

	void recordLastTouch(Vector2 camPos, Vector2 worldPos){
		currCamPos = camPos;
		currWorldPos = worldPos;
		if (!isActivated) {
			turnOnActivate();
//			Debug.Log ("Activating");
			activatedCamPos = camPos;
			activatedWorldPos = worldPos;

			joystickCentre.transform.position = currWorldPos;
			stickRenderer.sortingOrder = 10;
			joystickSource.transform.localPosition = joystickCentre.transform.localPosition;
		}
	}

	void AttachKat(GameObject kat){
		this.attachedKat = kat;
		this.moveScript = kat.transform.GetComponent<MovementScript> ();
		transform.Find ("Circle").Find ("KatHead").gameObject.GetComponent<SpriteRenderer> ().sprite = kat.transform.Find ("Sprite").Find ("Head").GetComponent<SpriteRenderer> ().sprite;
	}

	public void handleControllerAxisInput(Vector2 displacement){
		isUsingExternalController = true;
		isActivated = true;
		lastActivatedTime = Time.time;

		float angle = getAngle(displacement);
		moveScript.FaceFoward(Quaternion.Euler (new Vector3(0, 0, angle - 90)));
//		Debug.Log (angle);
		
		moveScript.walkInDirection(displacement);
//		startPoint = centre + displacement;
	}
}