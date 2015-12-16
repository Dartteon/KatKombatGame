using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MTControllerCommand : MonoBehaviour, Tappable {
	
	private AttacksScript atkScript;
	private int attackNum;
	private float atkCooldown;
	private bool hasCasted = false;
	private float lastCastTime = -100.0f;
	private Image cooldownCircle;
	private Image manaCircle;
	private int manaCost;
	private ManaScript katMana;

	
	protected Vector2 startPoint, endPoint, direction, centre;
	protected bool isClicked = false;
	protected GameObject attachedKat;
	protected Animator katAnimator;
	protected StatsScript katStats;
	
	void Start(){
		centre = transform.position;
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

			if (hasCasted) {
				float timeDifference = Time.time - lastCastTime;
				if (timeDifference >= atkCooldown)
					hasCasted = false;
				else
					cooldownCircle.fillAmount = (atkCooldown - timeDifference) / atkCooldown - 0.02f;
			}
		}
	}

	public void AttachKat(GameObject kat){
		this.attachedKat = kat;
		this.atkScript = kat.transform.GetComponent<AttacksScript> ();
		this.katAnimator = kat.transform.FindChild ("Sprite").gameObject.GetComponent<Animator> ();
		this.katStats = kat.GetComponent<StatsScript> ();
		this.katMana = kat.GetComponent<ManaScript> ();
		
		if (gameObject.name == "Attack1Joystick") {
			attackNum = 1;
			findIconInProjectileAndAssumeSprite ("BasicAttack");
			/*
			try{
				SpriteRenderer atkSpriteR = kat.transform.Find("BasicAttack").GetComponent<KatProjectileLauncherScript>().projectile.transform.Find("Icon").transform.GetComponent<SpriteRenderer>();
				//SpriteRenderer atkSpriteR = kat.transform.Find("BasicAttack").transform.Find("Icon").transform.GetComponent<SpriteRenderer>();
				Debug.Log(atkSpriteR.ToString());
				Debug.Log(this.transform.Find("Icon").transform.GetComponent<SpriteRenderer>().sprite.ToString());
				this.transform.Find("Icon").transform.GetComponent<SpriteRenderer>().sprite = atkSpriteR.sprite;
			} catch (UnityException e){
				Debug.Log("No Icon attached : " + e);
			}
			*/
		} else if (gameObject.name == "Attack2Joystick") {
			attackNum = 2;
			findIconInProjectileAndAssumeSprite ("SecondAttack");
		} else {
			attackNum = 3;
			findIconInProjectileAndAssumeSprite ("ThirdAttack");
		}
	}

	void findIconInProjectileAndAssumeSprite(string attackNum){
		SpriteRenderer atkSpriteR = attachedKat.transform.Find(attackNum).GetComponent<KatProjectileLauncherScript>().projectile.transform.Find("Icon").transform.GetComponent<SpriteRenderer>();
//		Debug.Log(atkSpriteR.ToString());
//		Debug.Log(this.transform.Find("Icon").transform.GetComponent<SpriteRenderer>().sprite.ToString());
		this.transform.Find("Icon").transform.GetComponent<SpriteRenderer>().sprite = atkSpriteR.sprite;
	}

	public void handleTap(Vector2 clickPosition, Vector2 worldPos){
		if (Time.timeScale != 0 && attachedKat != null && !katStats.GetIsBusy()) {
			endPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			direction = endPoint - centre;
			direction.Normalize();
			isClicked = false;
			katAnimator.speed = 1.0f;
			katAnimator.Play ("Idle");
			
			switch(attackNum){
			case 1:
				atkCooldown = atkScript.GetAttack1Cooldown();
				if (atkScript.basicAttackForward()){
					hasCasted = true;
					lastCastTime = Time.time;
				}
				
				break;
			case 2:
				atkCooldown = atkScript.GetAttack2Cooldown();
				if (atkScript.secondAttackForward()){
					hasCasted = true;
					lastCastTime = Time.time;
				}
				
				break;
			case 3:
				atkCooldown = atkScript.GetAttack3Cooldown();
				if(atkScript.thirdAttackForward()){
					hasCasted = true;
					lastCastTime = Time.time;
				}
				break;
			}
		}
	}
}
