using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MovementControllerScript : ControllerScript {
	public MovementScript moveScript;
	private GameObject katIcon;

	
//	private Image cooldownCircle;
	private float jumpCooldown;
	private float lastJumpTime = -100f;
	private bool hasRecentlyJumped = false;
/*	private SpriteRenderer targetRing;
	private Color katColor;
*/
	void Start(){
		centre = transform.position;
		targetRing = transform.FindChild ("TargetRing").gameObject;
		originalRotation = targetRing.transform.localRotation;
//		cooldownCircle = transform.Find ("CooldownCircle").gameObject.GetComponentInChildren<Image> ();
	}

	void Update(){
		if (isClicked && (attachedKat != null)) {
			endPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			direction = endPoint - centre;
			
			float angle = getAngle (direction);
			rotateKatAndRing (angle);
		}

		if (hasRecentlyJumped) {
			float timeDifference = Time.time - lastJumpTime;
			if (timeDifference >= jumpCooldown)
				hasRecentlyJumped = false;
//			else
//				cooldownCircle.fillAmount = (jumpCooldown - timeDifference) / jumpCooldown - 0.02f;
			
		}
	}

	float getAngle(Vector2 direction){
		return (Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg);
	}
	void rotateKatAndRing(float angle){
		attachedKat.transform.rotation = Quaternion.Euler (new Vector3(0, 0, angle - 90));
		targetRing.transform.rotation = Quaternion.Euler (new Vector3(0, 0, angle -90));
	}

	void OnMouseDown(){
		if (attachedKat != null && !katStats.GetIsBusy() && !hasRecentlyJumped) {
			isClicked = true;
			katAnimator.speed = .5f;
			katAnimator.Play ("PrepareForAction");
		}
	}


	void OnMouseUp(){
		if (attachedKat != null && isClicked && !katStats.GetIsBusy ()) {
			endPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			direction = endPoint - centre;
			direction.Normalize ();
			//moveScript.ShootInDirection(direction);
			isClicked = false;
			//Debug.Log ("released " + direction.ToString());
			//attachedKat.SendMessage("ShootInDirection", direction);
			//obj.SendMessage("BasicAttack", direction);
			katAnimator.speed = .5f;
			katAnimator.Play ("Idle");
			moveScript.ShootInDirection (direction);
			moveScript.jumpForward();
			targetRing.transform.localRotation = originalRotation;
			hasRecentlyJumped = true;
			lastJumpTime = Time.time;
		}
	}

	void AttachKat(GameObject kat){
		this.attachedKat = kat;
		this.moveScript = kat.transform.GetComponent<MovementScript> ();		
		jumpCooldown = moveScript.jumpCooldown;
		this.katAnimator = kat.transform.FindChild ("Sprite").gameObject.GetComponent<Animator> ();
		transform.FindChild ("KatHead").gameObject.GetComponent<SpriteRenderer> ().sprite 
			= kat.transform.FindChild ("Sprite").gameObject.transform.FindChild ("Head").gameObject.GetComponent<SpriteRenderer> ().sprite;
		this.katStats = kat.GetComponent<StatsScript> ();
	}
}
