using UnityEngine;
using System.Collections;

public class NonBattleMoveTowards : MonoBehaviour {
	public GameObject movementTarget;
	bool hasStarted = false;
	MovementScript katMovementScript;
	bool hasJumped = false;
	private float lastJumpTime;
	private float jumpCooldown;

	private readonly float minWalkDistance = 1.4f;
	private readonly float minJumpDistance = 4f;

	public void attachMoveTowardsObject(GameObject obj){
		movementTarget = obj;
		hasStarted = true;
	}

	void Start(){
		katMovementScript = this.GetComponent<MovementScript> ();
		movementTarget = GameObject.Find ("MouseTracker");
		hasStarted = true;
		lastJumpTime = Time.time;
		jumpCooldown = Random.Range(0.5f, 1.5f);
	}

	public void attachKat(GameObject kat){
		katMovementScript = kat.GetComponent<MovementScript> ();
	}

	// Update is called once per frame
	void Update () {
		if (hasStarted && movementTarget != null) {
			Vector2 targetPos = movementTarget.transform.position;
			Vector2 currentPos = this.transform.position;

			if ((Time.time - lastJumpTime >= jumpCooldown) && (targetPos-currentPos).magnitude >= minJumpDistance){
				katMovementScript.FaceForward(targetPos-currentPos);
				katMovementScript.jumpForward();
				lastJumpTime = Time.time;
				jumpCooldown = Random.Range(0.5f, 1.5f);
			} else if ((targetPos-currentPos).magnitude >= minWalkDistance){
				katMovementScript.walkInDirection(targetPos-currentPos);
				katMovementScript.FaceForward(targetPos-currentPos);
			} else if ((targetPos-currentPos).magnitude <= minWalkDistance){
				//katMovementScript.FaceFoward(targetPos-currentPos);
				
				katMovementScript.FaceForward(movementTarget.transform.up);
				lastJumpTime = Time.time;

			}
		}
	}
}
