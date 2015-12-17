using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kommand = Kommands.KommandCode;

public class GeneralProjectileScript : MonoBehaviour {
	public Kommand kommand;

	public string commandDescription = "Default Text";
	public float lingerTime = 1.0f;
	public int statType; //str(0), dex(1), int(2)
	public float statRatio;
	public float cooldown = 3.0f;
	public bool isMelee;
	public float projectileShootingForce = 4.0f;
	public int maxProjectiles = 1;
	public float repelForceMultiplier = 200.0f;
	public float transparency = 1.0f;
	public float castTime = 0.3f;
	public string attackAnim = "Frolick";
	public bool spawnCentred = false;
	public bool isInterruptable = false;
	public float critChance = 0.0f;
	public bool collideDamager = false;
	public float collideDamageMultiplier = 1.0f;
	public float fadeTime = 0.4f; //Note that Unity will add a minimum of 0.2 seconds thereabout to fade somehow
	public bool isPiercing = false;
	public bool hitOnceOnly = false;
	public bool mimicOwnerColor = true;
	public int maxEnemiesHit = 1;
	public bool hasExplosionEffect = false;

	private List<Collider2D> hitKats = new List<Collider2D>();
	private int baseDamage = 0;

	//for AI's info
	public float atkRange;
	public char atkMode;

	protected float firstHitTime;
//	protected bool stillHitting;
	protected Vector2 forceDirection;
	protected int damage;
	protected float critMultiplier = 1.5f;
	protected bool isActive = false;
	protected SpriteRenderer sprite;
	protected float startTime;
	protected GameObject ownerKat;
	protected StatsScript katStats;
	protected bool hasHit = false;
	protected Rigidbody2D rb2d;
	protected Vector2 zeroVelocity = new Vector2 (0, 0);
	protected int statToUse;
//	protected Animator anim;
	protected bool hasColorSet = false;
	protected Vector2 repelForce;
	protected Vector2 startPoint;
	protected readonly float COLLIDE_TIME_MULTIPLIER = 0.0027f;
	protected readonly float MULTIPLIER_CRIT_DEX = 0.0015f;

	protected bool explosionSet = false;
	protected GameObject explosionEffectObj;
	public int manaCost = 0; //TOdo: Remove all mana cost components from objects

	private readonly float strRepelRatio = 0.02f;

	void Start () {
		rb2d = transform.GetComponent<Rigidbody2D> ();
	}
	protected virtual void callUpdateEffects(){
		if (Time.time - startTime >= lingerTime && isActive) {
			DestroySelf ();
		}
	}
	void Update () {
		if (this.gameObject.activeSelf) {
			callUpdateEffects ();
		}
	}

	public void SetOwner(GameObject kat){
		this.ownerKat = kat;
		katStats = kat.GetComponent<StatsScript> ();
	}
	
	protected void DestroySelf(){
		isActive = false;
		this.gameObject.SetActive (false);
	}

	protected void SetColor(){
		if (mimicOwnerColor) {
			Color katColor = katStats.col;
			Color newCol = new Color (katColor.r, katColor.g, katColor.b, transparency);
		
			if (transform.gameObject.transform.Find ("Sprite").gameObject != null) {
				SpriteRenderer[] childSprites = transform.Find ("Sprite").gameObject.GetComponentsInChildren<SpriteRenderer> ();
				//				Debug.Log (childSprites.Length);
			
				if (childSprites.Length == 0)
					transform.Find ("Sprite").GetComponent<SpriteRenderer> ().color = newCol;
			
				for (int j=0; j<childSprites.Length; j++)
					childSprites [j].color = katColor;
			}
			hasColorSet = true;
		} else {
			hasColorSet = true;
		}
	}
	
	protected virtual void callOnEnableEffects(){
		hitKats.Clear ();
		startTime = Time.time;
		isActive = true;
		hasHit = false;
		//		stillHitting = true;
		if (!hasColorSet)
			SetColor();
		hitKats.Clear ();
		//EXPLOSION
		if (hasExplosionEffect) {
			if(!explosionSet){
				explosionEffectObj = transform.Find ("Explosion").gameObject;
				explosionSet = true;
			}
			explosionEffectObj.SetActive (false);
		}
		callExtraOnEnableEffect ();
		//below for finding range of atk
		//	startPoint = ownerKat.transform.position;
	}
	void OnEnable(){
		callOnEnableEffects ();
	}
	void OnDisable(){
		callExtraOnDisableEffect ();
	}
	protected virtual void callExtraOnDisableEffect(){
	}
	protected virtual void callExtraOnEnableEffect(){
	}

	
	protected virtual void callExtraOnTriggerEffect(){
	}

	protected int getStatToUse(int statType){
		switch (statType) {
		case 0:
			return katStats.getStr ();
		case 1:
			return katStats.getDex ();
		default:
			return katStats.getInt ();
		}
	}
	protected virtual void callTriggerEffects(Collider2D collidingObj){
		if ((collidingObj != null) 
		         && !(collidingObj.tag.Equals(ownerKat.tag)) 
		         && !(collidingObj.tag.Equals("Terrain")) 
		         && (hitOnceOnly && !hasHit)||(!hitOnceOnly)
		         && (!ownerKat.tag.Equals(collidingObj.tag))  ) {
			
			bool shouldHit = false;
			if(hitOnceOnly){
				shouldHit = checkIfEnemyHitBefore(collidingObj);
			}
			if(!shouldHit){
				addKatToHitKats(collidingObj);
				//				stillHitting = false;
				setHasHit();
				HealthScript hpScript = collidingObj.transform.GetComponent<HealthScript> ();
				if (ownerKat != null && hpScript != null){
					hpScript.SetLastAttacker (ownerKat);
				}
				
				callExtraOnTriggerEffect();
				
				if (!hasHit){
					setHasHit();
				};
				
				statToUse = getStatToUse(statType);
				damage = (int)(statToUse * statRatio) + baseDamage;
				repelCollidingObj(collidingObj);
				damage = (int)(Random.Range(0.85f, 1.0f) * damage);
				if ((damage > 0) && (hpScript != null)){
					hpScript.Damage (damage, statType);
				}
				if (!isPiercing){
					rb2d.velocity = zeroVelocity;
				}
				
				if (collideDamager){
					setColliding(hpScript, damage);
				}
			}
		}
	}
	void OnTriggerEnter2D(Collider2D collidingObj){
		callTriggerEffects (collidingObj);
	}
	protected void setHasHit(){	
		firstHitTime = Time.time;	
		hasHit = true;
		startTime = Time.time - fadeTime + lingerTime;
		
		if (hasExplosionEffect)
			explosionEffectObj.SetActive (true);		
	}
	protected int parseIfCritical(int damage){
		if (IsCrit())
			damage = (int)((float)(damage) * (critMultiplier));
		return damage;
	}
	protected void repelCollidingObj(Collider2D collidingObj){
		if (isMelee){
			repelMelee(collidingObj);
		}
		else{
			repelNormal(collidingObj);
		}
	}
	protected void setColliding(HealthScript hpScript, int dmg){
		float collideTimeLength = repelForce.magnitude*COLLIDE_TIME_MULTIPLIER;
		int collideDmg = (int)((float)dmg * collideDamageMultiplier);
		hpScript.SetColliding(collideDmg, collideTimeLength);
	}
	protected void repelNormal(Collider2D collidingObj){
		forceDirection = collidingObj.transform.position - transform.position; //Repel from projectile
		
		if(repelForceMultiplier != 0){
			repelForce = (forceDirection.normalized)*repelForceMultiplier;
			collidingObj.GetComponent<Rigidbody2D>().AddForce(repelForce);
		}
	}
	protected void repelMelee(Collider2D collidingObj){
		forceDirection = collidingObj.transform.position - transform.position; //Repel from projectile
		repelForce = (forceDirection.normalized)*repelForceMultiplier*katStats.Str*strRepelRatio;
		collidingObj.GetComponent<Rigidbody2D>().AddForce(repelForce);
	}
	protected bool IsCrit(){
		if (Random.Range (0.0f, 1.0f) <= (critChance + (katStats.Dex * MULTIPLIER_CRIT_DEX))) {
			return true;
		}
		else
			return false;
	}
	protected bool checkIfEnemyHitBefore(Collider2D kat){
		bool hasHitBefore = hitKats.Contains(kat);
		if (hasHitBefore) {
			return true;
		} else {
			return false;
		}
	}
	protected void addKatToHitKats(Collider2D kat){
		hitKats.Add (kat);
	}
}
