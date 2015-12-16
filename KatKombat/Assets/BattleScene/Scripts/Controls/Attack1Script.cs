using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Attack1Script : ControllerScript {

	private AttacksScript atkScript;
	private int attackNum;
	private float atkCooldown;
	private bool hasCasted = false;
	private float lastCastTime = -100.0f;
	private Image cooldownCircle;
	private Image manaCircle;
	private int manaCost;
	private ManaScript katMana;

//	private Text manaCostText, cooldownText;

	void Start(){
		centre = transform.position;
		targetRing = transform.FindChild ("TargetRing").gameObject;
		originalRotation = targetRing.transform.localRotation;
		cooldownCircle = transform.Find ("CooldownCircle").gameObject.GetComponentInChildren<Image> ();
		manaCircle = transform.Find ("ManaCircle").gameObject.GetComponentInChildren<Image> ();

		if (gameObject.name == "Attack1Joystick") {
			attackNum = 1;
		} else if (gameObject.name == "Attack2Joystick") {
			attackNum = 2;
		} else {
			attackNum = 3;
		}
	}


	void OnMouseDown(){
		if (attachedKat != null  && !katStats.GetIsBusy()) {
			isClicked = true;
			katAnimator.speed = 1.0f;
			katAnimator.Play ("PrepareAttack");
		}
	}

	void OnMouseUp(){
		if (attachedKat != null && isClicked && !katStats.GetIsBusy()) {
			endPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			direction = endPoint - centre;
			direction.Normalize();
			isClicked = false;
			katAnimator.speed = 1.0f;
			katAnimator.Play ("Idle");
			//attachedKat.SendMessage("BasicAttack", direction);
			targetRing.transform.localRotation = originalRotation;

			switch(attackNum){
			case 1:
				atkCooldown = atkScript.GetAttack1Cooldown();
				if (atkScript.BasicAttack(direction)){
						hasCasted = true;
						lastCastTime = Time.time;
				}

				break;
			case 2:
				atkCooldown = atkScript.GetAttack2Cooldown();
				if (atkScript.SecondAttack(direction)){
					hasCasted = true;
					lastCastTime = Time.time;
				}

				break;
			case 3:
				atkCooldown = atkScript.GetAttack3Cooldown();
				if(atkScript.ThirdAttack(direction)){
					hasCasted = true;
					lastCastTime = Time.time;
				}
				break;
			}
		}
	}

	void AttachKat(GameObject kat){
		this.attachedKat = kat;
		this.atkScript = kat.transform.GetComponent<AttacksScript> ();
		this.katAnimator = kat.transform.FindChild ("Sprite").gameObject.GetComponent<Animator> ();
		this.katStats = kat.GetComponent<StatsScript> ();
		this.katMana = kat.GetComponent<ManaScript> ();

		if (gameObject.name == "Attack1Joystick") {
			attackNum = 1;
		} else if (gameObject.name == "Attack2Joystick") {
			attackNum = 2;
		} else {
			attackNum = 3;
		}
	}

	void Update(){
		if (atkScript != null) {
			switch (attackNum) {
			case 1:
				if (atkScript.atk1Cost > katMana.manaCount) {
					manaCircle.fillAmount = 1.0f;
				} else 
					manaCircle.fillAmount = 0.0f;
				break;
			case 2:
				if (atkScript.atk2Cost > katMana.manaCount) {
					manaCircle.fillAmount = 1.0f;
				} else 
					manaCircle.fillAmount = 0.0f;
				break;
			case 3:
				if (atkScript.atk3Cost > katMana.manaCount) {
					manaCircle.fillAmount = 1.0f;
				} else 
					manaCircle.fillAmount = 0.0f;
				break;
			}

			if (!hasCasted && isClicked && attachedKat != null) {
				endPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				direction = endPoint - centre;
				float angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg;
				attachedKat.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, angle - 90));
				targetRing.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, angle - 90));
			} else if (hasCasted) {
				float timeDifference = Time.time - lastCastTime;
				if (timeDifference >= atkCooldown)
					hasCasted = false;
				else
					cooldownCircle.fillAmount = (atkCooldown - timeDifference) / atkCooldown - 0.02f;
			}
		}
	}
	
/* All these codes have been covered by parent ControllerScript
	 public void AttachKat(GameObject kat){
		this.attachedKat = kat;
	}

	// Use this for initialization
	private Vector2 startPoint, endPoint, direction;
	private bool isClicked = false;
	//public GameObject attachedKat;
	private GameObject attachedKat;
	private Vector2 centre;


	void Start(){
		centre = transform.position;
	}


	void OnMouseDown(){
		//startPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		isClicked = true;
	}
*/
}
