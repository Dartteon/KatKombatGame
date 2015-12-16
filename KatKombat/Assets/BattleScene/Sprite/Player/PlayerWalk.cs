using UnityEngine;
using System.Collections;

public class PlayerWalk : MonoBehaviour {
	float speed = 5 ;
	private Quaternion targetRotation;
	private float rotateSpeed = 0.02f;
	Animator charAnimator;
	
	Vector2 positionShift = new Vector2 (0, 0);
	// Use this for initialization
	void Start () {
		charAnimator = this.transform.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector2 currPos = this.transform.position;
		faceDirection (mousePos - currPos);

		if (targetRotation != null && !targetRotation.Equals((transform.rotation))){
			Quaternion newRotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.time*rotateSpeed*Time.timeScale);
			if(newRotation!=null)
				this.transform.rotation = newRotation;
		}

		bool isWalking = false;

		positionShift = new Vector2 (0, 0);

		if (Input.GetKey(KeyCode.W)){
			positionShift += (new Vector2 (0, 1)).normalized;
			isWalking = true;
		}
		
		if (Input.GetKey(KeyCode.A)){
			positionShift += (new Vector2 (-1, 0)).normalized;
			isWalking = true;
		}

		if (Input.GetKey(KeyCode.S)){
			positionShift += (new Vector2 (0, -1)).normalized;
			isWalking = true;
		}

		if (Input.GetKey(KeyCode.D)){
			positionShift += (new Vector2 (1, 0)).normalized;
			isWalking = true;
		}

		if (isWalking) {
			positionShift = positionShift.normalized;
			this.transform.position = currPos + positionShift * .05f;
			charAnimator.Play("PlayerWalk");
		} else {
			charAnimator.Play("PlayerIdle");
		}
	}

	void faceDirection (Vector2 dir){
		float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
		targetRotation = Quaternion.Euler (new Vector3(0, 0, angle - 90));
	}
}
