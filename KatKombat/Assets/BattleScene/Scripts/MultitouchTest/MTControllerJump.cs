using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MTControllerJump : MonoBehaviour, Tappable {
	
	protected Vector2 startPoint, endPoint, direction, centre;
	protected bool isClicked = false;
	protected GameObject attachedKat;
	protected Animator katAnimator;
	protected StatsScript katStats;

	public MovementScript moveScript;
	private Image cooldownCircle;
	private float jumpCooldown;
	private float lastJumpTime = -100f;
	private bool hasRecentlyJumped = false;

	// Use this for initialization
	void Start () {
		cooldownCircle = transform.Find ("CooldownCircle").gameObject.GetComponentInChildren<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (hasRecentlyJumped) {
			float timeDifference = Time.time - lastJumpTime;
			if (timeDifference >= jumpCooldown)
				hasRecentlyJumped = false;
			else
				cooldownCircle.fillAmount = (jumpCooldown - timeDifference) / jumpCooldown - 0.02f;
			
		}
	}

	public void handleTap(Vector2 position, Vector2 worldPos){
		if (Time.timeScale != 0 && Time.time - lastJumpTime >= jumpCooldown) {
			moveScript.jumpForward ();
			hasRecentlyJumped = true;
			lastJumpTime = Time.time;
		}
	}


	void AttachKat(GameObject kat){
		this.attachedKat = kat;
		this.moveScript = kat.transform.GetComponent<MovementScript> ();		
		jumpCooldown = moveScript.jumpCooldown;
		this.katAnimator = kat.transform.FindChild ("Sprite").gameObject.GetComponent<Animator> ();
		this.katStats = kat.GetComponent<StatsScript> ();
	}
}
