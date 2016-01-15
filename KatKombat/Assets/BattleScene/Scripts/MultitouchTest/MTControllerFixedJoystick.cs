using UnityEngine;
using System.Collections;

public class MTControllerFixedJoystick : MonoBehaviour, Tappable {
	protected Vector2 direction;
	protected GameObject attachedKat;
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
		joystickCentre = transform.FindChild ("Circle").gameObject;
		stickRenderer = joystickCentre.GetComponent<SpriteRenderer> ();
		joystickSource = transform.FindChild ("StickSource").gameObject;
		stickSourceStartPos = joystickSource.transform.localPosition;
	}
	
	void Update(){
		//		Debug.Log("test" + isUsingExternalController);
		if (isActivated && (attachedKat != null)) {
			if (!(Time.time - lastActivatedTime >= 0.1f)){
				Vector2 currCentre = joystickCentre.transform.position;
				direction = currWorldPos - currCentre;
//				direction = currCamPos - activatedCamPos;
				float angle = getAngle(direction);
				rotateKatAndRing(angle);
				moveScript.walkInDirection(direction);
			}
		}
	}

	
	void registerTouch(){
		isActivated = true;
		lastActivatedTime = Time.time;
	}
	
	void rotateKatAndRing(float angle){
		moveScript.instantFaceForward(Quaternion.Euler (new Vector3(0, 0, angle - 90)));
		//		targetRing.transform.rotation = Quaternion.Euler (new Vector3(0, 0, angle -90));
	}
	
	float getAngle(Vector2 direction){
		return (Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg);
	}
	
	public void handleTap(Vector2 camPos, Vector2 worldPos){
		recordLastTouch (camPos, worldPos);
		registerTouch ();
//		Debug.Log ("Tapped");
	}
	
	void recordLastTouch(Vector2 camPos, Vector2 worldPos){
		currCamPos = camPos;
		currWorldPos = worldPos;
		if (!isActivated) {
			isActivated = true;
			activatedCamPos = camPos;
			activatedWorldPos = worldPos;
			
	//		joystickCentre.transform.position = currWorldPos;
			stickRenderer.sortingOrder = 10;
	//		joystickSource.transform.localPosition = joystickCentre.transform.localPosition;
		}
	}
	
	void AttachKat(GameObject kat){
		this.attachedKat = kat;
		this.moveScript = kat.transform.GetComponent<MovementScript> ();
		transform.Find ("Circle").Find ("KatHead").gameObject.GetComponent<SpriteRenderer> ().sprite = kat.transform.Find ("Sprite").Find ("Head").GetComponent<SpriteRenderer> ().sprite;
	}
	
	public void handleControllerAxisInput(Vector2 displacement){
		//notused
	}
}