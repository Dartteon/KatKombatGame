using UnityEngine;
using System.Collections;

public class KatProjectileLauncherScript : MonoBehaviour {
	//public
	public GameObject projectile;
	public float force = 1.0f;
	public int maxProjectiles = 3;
	public int manaCost = 9;
	private float originalCooldown;
	public float cooldown = 3.0f;
	public string attackAnim;
	public bool isInterruptable = false;
	private bool canCast = true;
	private float lastCastTime = -100f;

	//protected
	protected Animator anim;
	protected GameObject sprite;
	protected float startTime;
	protected float castTime = 0.6f;
	private float originalCastTime;
	protected bool isCasting;
	protected Vector2 direction;
	protected StatsScript stats;
	protected Color katColor;
	protected int currentprojectile = 0;
	protected GameObject[] projectilesArray = new GameObject[5];	
	protected Vector2 zeroVelocity = new Vector2(0, 0);
	protected ManaScript katMana;
	protected GameObject noMana, noCooldown;
	protected int attackElement; //AKA statType of attack
	private bool spawnCentred;
	private HealthScript katHealth;

	private float atkRange;
	private char atkMode;

	private static readonly float DIVIDER_BASE_DEX = 300.0f;
	private static readonly float DIVIDER_BASE_INT = 300.0f;
//	private Rigidbody2D rb2d;
/*	
	void Start () {
		GetKatInfo ();
		SpawnProjectiles ();
	}
*/
	void OnEnable(){
		GetKatInfo ();
		SpawnProjectiles ();
		startTime = Time.time - cooldown;
		noMana = transform.parent.Find ("UIElements").gameObject.transform.Find ("NoManaPopup").gameObject;
		noCooldown = transform.parent.Find ("UIElements").gameObject.transform.Find ("NoCooldownPopup").gameObject;
		katHealth = transform.parent.GetComponent<HealthScript> ();

		originalCooldown = cooldown;
		originalCastTime = castTime;
		setCooldown ();
		setCastTime ();
//		rb2d = transform.parent.GetComponent<Rigidbody2D> ();
		//getIcon ();
	}

	void setCastTime(){
		if (stats != null) {
			float ratio = (1.0f - (float)(stats.Dex / DIVIDER_BASE_DEX));
			castTime = originalCastTime * ratio;
		}
	}

	void setCooldown(){
		if (stats != null) {
			float ratio = (1.0f - (float)(stats.Int / DIVIDER_BASE_INT));
			cooldown = originalCooldown * ratio;
		}
	}

	void Update(){ //Launches projectile after countdown
		if (isCasting) {
			if (isInterruptable){
				if (katHealth.interrupted){
					isCasting = false;
					stats.SetBusy(false);
					anim.speed = 1.0f;
					anim.Play("ShakeHead");
				}
			}

			if (Time.time - startTime >= castTime)
				Attack ();
		}
		if (!canCast && Time.time - lastCastTime >= cooldown) {
			canCast = true;
		}
	}

	void GetKatInfo(){
		//Get owner Kat's animator
		sprite = transform.parent.FindChild ("Sprite").gameObject;
		anim = sprite.GetComponent<Animator> ();
		//Get owner Kat's color
		stats = transform.parent.GetComponent<StatsScript> ();
		katColor = stats.getColor ();
		katMana = transform.parent.GetComponent<ManaScript> ();
	}

	void SpawnProjectiles(){
		for (int i=0; i<maxProjectiles; i++) {
			GameObject projectileObj = Instantiate (projectile) as GameObject;
			projectilesArray [i] = projectileObj;
			projectilesArray [i].gameObject.SetActive (false);
			projectilesArray [i].GetComponent<GeneralProjectileScript> ().SetOwner (transform.parent.gameObject);
			float mult = transform.parent.transform.localScale.x;
			projectilesArray [i].transform.localScale *= mult;
		}
	}

	public bool getCanCast(){
		return canCast;
	}

	public bool Cast(Vector2 dir){ //Start attack animation and countdown to launch projectile, called by controller
		if (Time.time - startTime >= cooldown) {
				setCooldown ();
				setCastTime();
				this.direction = dir;
				startTime = Time.time;
				isCasting = true;
				anim.speed /= castTime;
				anim.speed /= 1.5f;
				anim.Play (attackAnim);
				stats.SetBusy (true);
				katHealth.interrupted = false;
				canCast = false;
				lastCastTime = Time.time;
				return true;
		} else {
			noCooldown.SetActive (true);
			anim.speed = 1.0f;
			anim.Play ("ShakeHead");
			return false;
		}
	}

	public void SetPrefab(GameObject prefab){
		if (prefab == null)
			this.gameObject.SetActive (false);
		else {
			projectile = prefab;

			GeneralProjectileScript script = prefab.GetComponent<GeneralProjectileScript> ();
			force = script.projectileShootingForce;
			force *= transform.parent.transform.localScale.x;
			maxProjectiles = script.maxProjectiles;
			manaCost = script.manaCost;
			cooldown = script.cooldown;
			attackElement = script.statType;
			castTime = script.castTime;
			attackAnim = script.attackAnim;
			spawnCentred = script.spawnCentred;
			isInterruptable = script.isInterruptable;

			atkMode = script.atkMode;
			atkRange = script.atkRange;
		}
	}

	void Attack(){ //Actual attack command, launches projectile immediately
		if (projectilesArray [currentprojectile] != null) {
			projectilesArray [currentprojectile].gameObject.SetActive (true);
			direction = this.transform.up;
			if (spawnCentred) {
//			Debug.Log("Spawning Centred");
				projectilesArray [currentprojectile].transform.position = this.transform.parent.position;
				projectilesArray [currentprojectile].transform.rotation = this.transform.parent.rotation;
			} else {			
//			Debug.Log("Spawning Outer");
				projectilesArray [currentprojectile].transform.position = this.transform.position;
				projectilesArray [currentprojectile].transform.rotation = this.transform.rotation;
			}
			projectilesArray [currentprojectile].GetComponent<Rigidbody2D> ().velocity = zeroVelocity; //Make sure last life of projectile does not have any speed
			if (force != 0) {
				addForceToProjectile ();
			}
			stats.SetBusy (false);
			loopCurrentProjectile ();
			isCasting = false;
			anim.speed = 1.0f;
		}
	}
	void addForceToProjectile(){
		direction.Normalize ();
		direction.x *= force;
		direction.y *= force;
		projectilesArray [currentprojectile].GetComponent<Rigidbody2D> ().AddForce (direction, ForceMode2D.Impulse);
	}
	void loopCurrentProjectile(){
		currentprojectile++;
		if(currentprojectile==maxProjectiles)
			currentprojectile=0;
	}
	void OnDestroy(){
		for (int i=0; i<projectilesArray.Length; i++)
			Destroy (projectilesArray[i]);
	}

	public float getAttackRange(){
		return atkRange;
	}
	public char getAttackMode(){
		return atkMode;
	}

	public float getCastTime(){
		return castTime;
	}

}
