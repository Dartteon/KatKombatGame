using UnityEngine;
using System.Collections;

public class MovementScript : MonoBehaviour {
	private float force;
	private Animator anim;
//	private float startTime;
//	private float animDuration = 1.0f;
	private Vector2 direction;
	private StatsScript katStats;
//	private ManaScript katMana;
	public int jumpCost = 1;
	private GameObject noMana;
	public float jumpCooldown = 2.0f;
	public bool canMove = true;
	public float lastMoveTime;
	private float moveTimeLength;
	private bool isMoving = false;
	private Rigidbody2D katrb2d;
	private HealthScript hpScript;
	private Quaternion targetRotation;
//	private float rotateSpeed = 1.1f;
	
	private float rotateSpeed = 0.2f;

	private readonly float walkSpeed = 2.0f;

	void Start () {
		anim = GetComponentInChildren<Animator> ();
		katStats = transform.GetComponent<StatsScript> ();
//		katMana = transform.GetComponent<ManaScript> ();
		noMana = transform.Find ("UIElements").gameObject.transform.Find ("NoManaPopup").gameObject;
		katrb2d = GetComponent<Rigidbody2D> ();
		hpScript = transform.GetComponent<HealthScript> ();
		targetRotation = transform.rotation;

	}

	void Update () {

//		if (!targetRotation.Equals((transform.rotation))){
//			Quaternion newRotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.time*rotateSpeed*Time.timeScale);
//			if(newRotation!=null)
//				this.transform.rotation = newRotation;
//		}
//		this.transform.rotation = targetRotation;

		if (!canMove && (Time.time - lastMoveTime >= jumpCooldown)){
			canMove = true;
		}

		if (isMoving) {
			if (Time.time - lastMoveTime >= moveTimeLength){
				isMoving = false;
				//new
				//katStats.isBusy = false;
				//Debug.Log("Stopped Moving");
			}

		}
	}

	void FixedUpdate() {
		Quaternion newRotation = Quaternion.Lerp(transform.rotation, targetRotation, rotateSpeed);
		this.transform.rotation = newRotation;
	}
	/*
	public void walkInDirection(Vector2 direction){
		Vector2 newPosition = this.transform.position;
		newPosition += direction.normalized*0.04f*Time.timeScale;
		this.transform.position = newPosition;
	}
*/
	public void walkInDirection(Vector2 direction){
		Vector2 newPosition = this.transform.position;
		newPosition += direction.normalized*Time.deltaTime*walkSpeed;
		this.transform.position = newPosition;
	}

	public void ShootInDirection(Vector2 direction){
		if (katStats.statusEffect != 'f') {
//			if (canMove && katMana.removeMana (jumpCost)) {
	//		if (canMove) {
				direction.Normalize();
				SetForce ();
				//direction.Normalize (); //Finds the unit vector in correct direction, then multiplies it by variable "force"
				direction.x *= force;
				direction.y *= force;
				katrb2d.AddForce (direction, ForceMode2D.Impulse);
				anim.speed = 1.0f;
				anim.Play ("Movement");
				moveTimeLength = force * 0.15f;
				canMove = false;
				//new
				//katStats.isBusy = true;
				lastMoveTime = Time.time;
				isMoving = true;
				hpScript.SetColliding(false);
				/*
			} else {
//				Debug.Log(canMove + " " + Time.time + " vs " +  lastMoveTime);
				noMana.SetActive (true);
				anim.speed = 1.0f;
				anim.Play ("ShakeHead");
//				Debug.Log ("No mana");
			} 
*/
		} else {
			anim.speed = 1.0f;
			anim.Play ("ShakeHead");
			Debug.Log ("frozen");
		}
		
	} 

	public void jumpForward(){
			ShootInDirection (this.transform.up);

	}
	
	private void OnCollisionEnter2D(Collision2D collidingObj){
		if (isMoving) {
		//	anim.speed = 1.0f;
		//	anim.Play ("ShakeHead");
			isMoving = false;

	//		katrb2d.velocity = new Vector2(0.0f, 0.0f);
		}

	}


	public void FaceForward(Vector2 dir){ //turns to Kat to face correct direction
		float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
		targetRotation = Quaternion.Euler (new Vector3 (0, 0, angle - 90));
//		this.transform.rotation = targetRotation;
	}

	public void instantFaceForward(Vector2 dir){
//		Debug.Log (this.transform.rotation.ToString ());
		float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
		//	transform.rotation = Quaternion.Euler (new Vector3(0, 0, angle - 90));
		targetRotation = Quaternion.Euler (new Vector3(0, 0, angle - 90));
		this.transform.rotation = targetRotation;
//		Debug.Log (this.transform.rotation.ToString ());
	}
	public void instantFaceForward(Quaternion dir){
//		Debug.Log (this.transform.rotation.ToString ());
		targetRotation = dir;
		this.transform.rotation = dir;
//		Debug.Log (this.transform.rotation.ToString ());
	}

	public void FaceForward(Quaternion dir){ //turns to Kat to face correct direction
		targetRotation = dir;
		//Vector3 dir = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;
		//sprite.transform.rotation = Quaternion.Euler (new Vector3(0, 0, angle - 90));
		//sprite.transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler (new Vector3 (0, 0, angle - 90)), Time.deltaTime * turnSpeed);
	}

	public void SetForce(){
		//this.force = 3.0f + katStats.getDex ()/12.0f;
		this.force = 15.0f;
	}

	public void SetJumpCost(int cost){
		jumpCost = cost;
	}

	public void freeLeapInDirection(Vector2 direction){
		FaceForward (direction);
		//direction.Normalize();
		direction.x *= 3.6f;
		direction.y *= 3.6f;
		this.GetComponent<Rigidbody2D>().AddForce (direction, ForceMode2D.Impulse);
	}
}
