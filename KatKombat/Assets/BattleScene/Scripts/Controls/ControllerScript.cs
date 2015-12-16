using UnityEngine;
using System.Collections;

public class ControllerScript : MonoBehaviour {
	protected Vector2 startPoint, endPoint, direction, centre;
	protected bool isClicked = false;
	protected GameObject attachedKat;
	protected Animator katAnimator;
	protected StatsScript katStats;

	protected GameObject targetRing;
	protected Quaternion originalRotation;

	void Start(){
		centre = transform.position;
		targetRing = transform.FindChild ("TargetRing").gameObject;
		originalRotation = targetRing.transform.localRotation;
	}

	void Update(){
		if (isClicked && isKatAttached()) {
			endPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			direction = endPoint - centre;

			float angle = getAngle(direction);
			rotateKatAndRing(angle);
		}
	}

	bool isKatAttached(){
		if (attachedKat != null)
			return true;
		else
			return false;
	}

	void rotateKatAndRing(float angle){
		attachedKat.transform.rotation = Quaternion.Euler (new Vector3(0, 0, angle - 90));
		targetRing.transform.rotation = Quaternion.Euler (new Vector3(0, 0, angle -90));
	}

	float getAngle(Vector2 direction){
		return (Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg);
	}


/* Functions below should be present in CHILD
	void OnMouseDown(){
		isClicked = true;
		katAnimator.Play ("PrepareForAction");
	}

	void OnMouseUp(){
		if (isClicked) {
			endPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			direction = endPoint - centre;
			direction.Normalize();
			//moveScript.ShootInDirection(direction);
			isClicked = false;
			//Debug.Log ("released " + direction.ToString());
			//attachedKat.SendMessage("ShootInDirection", direction);
			//obj.SendMessage("BasicAttack", direction);
			moveScript.ShootInDirection(direction);
		}
	}

	void AttachKat(GameObject kat){
		this.attachedKat = kat;
		this.moveScript = kat.transform.GetComponent<MovementScript> ();
		this.katAnimator = kat.transform.FindChild ("Sprite").gameObject.GetComponent<Animator> ();
	}
*/
	
}
