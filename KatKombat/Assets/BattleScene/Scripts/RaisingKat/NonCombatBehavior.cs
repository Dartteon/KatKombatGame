using UnityEngine;
using System.Collections;

public class NonCombatBehavior : MonoBehaviour {
	public bool isBusy = false;
	public GameObject attachedObject;
	public float startedActivityTime;
	public float lastActivityActionTime;
	public bool isClicked = false;
	public int numCurrentActivityInteractions = 0;

	bool hasStarted = false;
	MovementScript katMovementScript;
	bool hasJumped = false;
	private float lastJumpTime;
	private float jumpCooldown;
	private float lastCollidedTime;
	private float lastTurnedTime;


	private bool isMoving = true;

	private readonly float minJumpCooldown = 8.0f, maxJumpCooldown = 10.0f;

	void Start(){
		katMovementScript = this.GetComponent<MovementScript> ();
		//movementTarget = GameObject.Find ("MouseTracker");
		hasStarted = true;
		lastJumpTime = Time.time;
		lastCollidedTime = Time.time;
		lastTurnedTime = Time.time;
		jumpCooldown = Random.Range(minJumpCooldown, maxJumpCooldown);
	}

	void FixedUpdate () {
		if (attachedObject == null && !isBusy) {
			Vector2 facingDirection = this.transform.up;

			if (Time.time - lastJumpTime >= jumpCooldown && isMoving) {
				katMovementScript.jumpForward ();
				lastJumpTime = Time.time;
				jumpCooldown = Random.Range (minJumpCooldown, maxJumpCooldown);
			} else if (Time.time - lastCollidedTime >= .001f && isMoving) {
				katMovementScript.walkInDirection (facingDirection);
//				Debug.Log(energy);
			}
		} else if (attachedObject != null && !checkIfObjectCloseEnough(1.0f, attachedObject.transform.position - this.transform.position)) {
			walkInDirection(attachedObject.transform.position - this.transform.position);
		}

	}

	bool checkIfObjectCloseEnough (float minDistance, Vector2 differenceVector){
		if (differenceVector.magnitude <= minDistance) {
			return true;
		} else
			return false;
	}

	/*
	void OnMouseDown(){
		isClicked = true;
		isBusy = true;
	}
*/
	
	public void faceDirection(Vector2 direction){
		katMovementScript.FaceForward(direction);

	}

	public void walkInDirection(Vector2 direction){
		katMovementScript.FaceForward(direction);
		katMovementScript.walkInDirection(direction);
	}

	public void jumpInDirection(Vector2 direction){
		katMovementScript.FaceForward(direction);
		katMovementScript.jumpForward();
	}

	public Vector2 getLocation (){
		return this.transform.position;
	}

	public void attachKatToObject(GameObject obj){
		this.attachedObject = obj;
	}

	public void detachKatFromObject(){
		attachedObject = null;
	}

	void OnCollisionEnter2D(Collision2D collidingObj){
//		Debug.Log ("Collided");
		string collidingTag = collidingObj.transform.tag.ToLower ();
		if (collidingTag.Equals("terrain")) {
			Vector2 currentFacingDirection = this.transform.up;
			Vector2 reflectedDirection = (new Vector2(0,0)) - currentFacingDirection;
			Vector2 newDirection = new Vector2 (reflectedDirection.x + Random.Range(-0.5f,0.5f), reflectedDirection.y + Random.Range(-0.5f,0.5f));
			katMovementScript.FaceForward(newDirection);

		}
		else if (!collidingTag.Equals ("interactableradius") 
		    && Time.time - lastTurnedTime >= 0.5f
		    && !isBusy) {
			lastCollidedTime = Time.time;
			Vector2 currentFacingDirection = this.transform.up;
			Vector2 reflectedDirection = (new Vector2(0,0)) - currentFacingDirection;
			Vector2 newDirection = new Vector2 (reflectedDirection.x + Random.Range(-0.5f,0.5f), reflectedDirection.y + Random.Range(-0.5f,0.5f));
			katMovementScript.FaceForward(newDirection);
			lastTurnedTime = Time.time;
		}
	}

}
