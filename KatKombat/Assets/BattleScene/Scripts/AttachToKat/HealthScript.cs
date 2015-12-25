using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour {

	public GameObject dmgPopupPrefab;


	private readonly float EFFECTIVE = 1.3f;
	private readonly float NOT_EFFECTIVE = 0.7f;
	private readonly float NORMAL_EFFECTIVE = 1.0f;

//	private GameObject sprite;
	//private SpriteRenderer spriteImage;
	private int statType;
	private GameObject lastAttackerKat;
//	private GameObject canvasHealthBar;
	private Image[] healthBar; //make sure HealthBar(green) is BELOW RedDelayBar(red) in Kat's components list
	private int health;
	public int maxHealth;
	private float healthRatio;
	private float deathTime;
	private bool isDead;
	private Animator animator;
	private GameObject redDelayBarHolder;
	private float lastDamageTime;
	private float redBarDelay = 1.0f;
	private bool isRecentlyDamaged = false;
	private GameObject[] dmgPopupArray;
	private int currentPopupIndex = 0;
	private bool popupLeft;
	private float dmgMultiplier = 1.0f;
	private float healMultiplier = 1.0f;
	private bool isColliding = false;
	private int collidingDamage;
	private float collideStartTime;
	private float collideTimeLength = 1.0f;
	private readonly int MAX_LEVEL_HP = 0;
//	private float delayHealthBarFill = 1.0f;
	public float delayDefillRate = 1.0f;
//	private float healthBarFill = 1.0f;
	public float damagePopupDistance = 0.6f;


	public bool interrupted = false;

	
	void Start () {

	//	healthBar = transform.FindChild ("CanvasHealthBar").gameObject.transform.FindChild ("HealthBar").gameObject.GetComponentInChildren<Image> ();
		//transform.FindChild ("CanvasHealthBar").gameObject.GetComponentInChildren<Image> ();
//		health = MAX_LEVEL_HP + (int)(transform.GetComponent<StatsScript> ().Str * 0.7f + transform.GetComponent<StatsScript> ().level);
		StatsScript katStats = transform.GetComponent<StatsScript> ();
		health = KatStatsInfo.getMaxHP (katStats.Str, katStats.level);
//		Debug.Log ("Health: " + health);
//		Debug.Log (transform.ToString () + " has " + health + " HP");
		statType = transform.GetComponent<StatsScript> ().statType;
		maxHealth = health;
//		Debug.Log (transform.ToString () + " has " + health + " hp");
//		canvasHealthBar = transform.FindChild ("CanvasHealthBar").gameObject;
//		healthBar = canvasHealthBar.transform.GetComponentsInChildren<Image> ();
		healthBar = transform.FindChild ("CanvasHealthBar").gameObject.transform.GetComponentsInChildren<Image> ();
		//redDelayBarHolder = canvasHealthBar.transform.Find ("RedDelayBar").gameObject;
		//redDelayBar = redDelayBarHolder.Get<Image> ();
/*		healthBar = canvasHealthBar.transform.FindChild("HealthBar").gameObject.GetComponent<Image> ();
		Debug.Log (canvasHealthBar.transform.FindChild("HealthBar").gameObject.ToString ());
		//healthBar = canvasHealthBar.transform.GetComponentInChildren<Image> ();
*/
//		sprite = transform.FindChild ("Sprite").gameObject;
		//spriteImage = sprite.gameObject.transform.GetComponent<SpriteRenderer> ();
		isDead = false;
		animator = transform.FindChild ("Sprite").gameObject.GetComponent<Animator> ();
		SpawnDamagePopups ();
	}

	public void initialize(){
		//	healthBar = transform.FindChild ("CanvasHealthBar").gameObject.transform.FindChild ("HealthBar").gameObject.GetComponentInChildren<Image> ();
		//transform.FindChild ("CanvasHealthBar").gameObject.GetComponentInChildren<Image> ();
//		Debug.Log(
		health = MAX_LEVEL_HP + (int)(transform.GetComponent<StatsScript> ().Str * 0.7f + transform.GetComponent<StatsScript> ().level);
		//		Debug.Log (transform.ToString () + " has " + health + " HP");
		statType = transform.GetComponent<StatsScript> ().statType;
		maxHealth = health;
		//		Debug.Log (transform.ToString () + " has " + health + " hp");
		//		canvasHealthBar = transform.FindChild ("CanvasHealthBar").gameObject;
		//		healthBar = canvasHealthBar.transform.GetComponentsInChildren<Image> ();
		healthBar = transform.FindChild ("CanvasHealthBar").gameObject.transform.GetComponentsInChildren<Image> ();
		//redDelayBarHolder = canvasHealthBar.transform.Find ("RedDelayBar").gameObject;
		//redDelayBar = redDelayBarHolder.Get<Image> ();
		/*		healthBar = canvasHealthBar.transform.FindChild("HealthBar").gameObject.GetComponent<Image> ();
		Debug.Log (canvasHealthBar.transform.FindChild("HealthBar").gameObject.ToString ());
		//healthBar = canvasHealthBar.transform.GetComponentInChildren<Image> ();
*/
		//		sprite = transform.FindChild ("Sprite").gameObject;
		//spriteImage = sprite.gameObject.transform.GetComponent<SpriteRenderer> ();
		isDead = false;
		animator = transform.FindChild ("Sprite").gameObject.GetComponent<Animator> ();
		SpawnDamagePopups ();
//		Debug.Log (transform.GetComponent<StatsScript> ().level);
//		Debug.Log (MAX_LEVEL_HP + (int)(transform.GetComponent<StatsScript> ().Str * 0.7f + transform.GetComponent<StatsScript> ().level)); //its not level 100!??!
//		Debug.Log ("Health: " + health + " MAXHealth : " + maxHealth);

	}
	
	void Update () {
		if (isDead && Time.time - deathTime >= redBarDelay) {
			Destroy(gameObject);
		}

		if (isRecentlyDamaged && Time.time - lastDamageTime >= 0.5f) {
			isRecentlyDamaged = false;
			SetHealthFill ((float)health / (float)maxHealth, 0);
		}


		if (isColliding && Time.time - collideStartTime >= collideTimeLength) {
			isColliding = false;
		}

		/*
		//Defill Delaybar slowly
		if (delayHealthBarFill > healthBarFill) {
			delayHealthBarFill = Mathf.Lerp(healthBarFill, delayHealthBarFill, 0.5f * Time.deltaTime);
		//	Debug.Log ( Mathf.Lerp(healthBarFill, delayHealthBarFill, Time.time) );
			SetHealthFill(delayHealthBarFill, 0);
		}
		*/

	//	Debug.Log (delayHealthBarFill + " vs " + healthBarFill);
	}
	
	void SetHealthVisual(float healthNormalized){
		healthBar[0].transform.localScale = new Vector3( healthNormalized,
		                                                healthBar[0].transform.localScale.y,
		                                                healthBar[0].transform.localScale.z);
		healthBar[1].transform.localScale = new Vector3( healthNormalized,
		                                             healthBar[1].transform.localScale.y,
		                                             healthBar[1].transform.localScale.z);
	}

	public void SetColliding(int collideDmg, float collideTime){
		isColliding = true;
		collideStartTime = Time.time;
		collideTimeLength = collideTime;
		collidingDamage = collideDmg;
//		Debug.Log ("start colliding at " + Time.time);
	}
	public void SetColliding(bool value){
		isColliding = value;
	}
	void OnCollisionEnter2D(Collision2D collidingObj){
		if (isColliding) {
			isColliding = false;
			Damage (collidingDamage, 3);
		//	if (collidingObj.gameObject.tag != null && collidingObj.gameObject.tag != transform.parent.tag)
		//		collidingObj.transform.GetComponent<HealthScript>().Damage(collidingDamage, 3);
//			Debug.Log ("colliding at " + Time.time);
		}
	}

	public bool Damage(int damage, int attackType){
		if (!isDead) {
			damage = (int)(damage*GetEffectivenessMultiplier(attackType)); //multiply by effectiveness
			damage = (int)(damage*dmgMultiplier);
			displayDamage(damage, attackType);
//			Debug.Log("Defending kat stat type is " + statType);
//			Debug.Log("Attacker pj type is " + attackType);
//			Debug.Log(GetEffectivenessMultiplier(attackType) + " multiplied");
//			Debug.Log(transform.ToString() + " took " + damage + " out of " + health + " " + (float)100*damage/health);
			health -= (int)(damage);
			lastDamageTime = Time.time;
			isRecentlyDamaged = true;
//			healthBarFill = (float)((float)health/(float)maxHealth);
			SetHealthFill ((float)health / (float)maxHealth, 1);
			interrupted = true;


			if (health <= 0){
				//Debug.Log(transform.ToString() + " has been killed by " + lastAttackerKat.ToString() + "!");
				DestroySelf();
				return true;
			} else return false;
		}
		else 
			return false;
	}

	public bool Heal(int healAmount){
		if (!isDead) {
			healAmount = (int)(healAmount*healMultiplier);
			displayDamage(healAmount, 3);

			health += (int)(healAmount);
			SetHealthFill ((float)health / (float)maxHealth, 1);
			return true;
		}
		else 
			return false;
	}
	
	void SetHealthFill(float num, int barNum){
		healthBar[barNum].fillAmount = num*.34f;
	}
	
	void DestroySelf(){
		isDead = true;
		transform.GetComponent<StatsScript> ().isBusy = true;
		transform.GetComponent<CircleCollider2D> ().enabled = false;
		this.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
		animator.Play("Death");
		deathTime = Time.time;
	}

	public void SetLastAttacker(GameObject kat){
		lastAttackerKat = kat;
	}

	void SpawnDamagePopups (){
		dmgPopupArray = new GameObject[4];
		for (int i = 0; i < dmgPopupArray.Length; i++) {
			GameObject popup = Instantiate(dmgPopupPrefab) as GameObject;;
			dmgPopupArray[i] = popup;
			dmgPopupArray[i].GetComponent<DamagePopupScript>().setOwnerKat(transform.gameObject);
			dmgPopupArray[i].SetActive(false);
			dmgPopupArray[i].transform.SetParent(this.transform);
			/*
			if (i%2 == 0)
				dmgPopupArray[i].transform.rotation = Quaternion.Euler(0.0f, 0.0f, -20.0f);
			else
				dmgPopupArray[i].transform.rotation = Quaternion.Euler(0.0f, 0.0f, 20.0f);
			*/
			
		}
	}

	void displayDamage(int damage, int damageType){
		dmgPopupArray [currentPopupIndex].SetActive (true);
		dmgPopupArray [currentPopupIndex].GetComponentInChildren<Text> ().text =  damage.ToString();
		dmgPopupArray[currentPopupIndex].GetComponent<DamagePopupScript>().setShadowColor(damageType);

		Vector2 positionDifference = transform.position - lastAttackerKat.transform.position;
		float vectorMultiplier = damagePopupDistance / (positionDifference.magnitude);
		positionDifference *= vectorMultiplier;

		float angle = (Random.value - 0.5f) * 360.0f;
		dmgPopupArray [currentPopupIndex].transform.localRotation = Quaternion.Euler (new Vector3 (0, 0, angle));
		dmgPopupArray [currentPopupIndex].transform.position = new Vector2 (transform.position.x, transform.position.y);


//		Debug.Log (positionDifference.ToString());
		/*
		if (popupLeft) {
			dmgPopupArray [currentPopupIndex].transform.position = new Vector2 (transform.position.x + positionDifference.x, transform.position.y + positionDifference.y);
		}
		else
			dmgPopupArray [currentPopupIndex].transform.position = new Vector2 (transform.position.x + positionDifference.x, transform.position.y + positionDifference.y);

		popupLeft = !popupLeft;
		*/

		currentPopupIndex++;
		if (currentPopupIndex == 4)
			currentPopupIndex = 0;
	}

	float GetEffectivenessMultiplier(int damagerType){
		return 1.0f;
		/*
		if (damagerType == 0) { //incoming damage is STR
			if (statType == 1)
				return EFFECTIVE;
			else if (statType == 2)
				return NOT_EFFECTIVE;
			else
				return NORMAL_EFFECTIVE;
		} else if (damagerType == 1) { //incoming damage is DEX
			if (statType == 2)
				return EFFECTIVE;
			else if (statType == 0)
				return NOT_EFFECTIVE;
			else
				return NORMAL_EFFECTIVE;
		} else if (damagerType == 2) { //incoming damage is INT
			if (statType == 0)
				return EFFECTIVE;
			else if (statType == 1)
				return NOT_EFFECTIVE;
			else
				return NORMAL_EFFECTIVE;
		} else
			return NORMAL_EFFECTIVE;
			*/
	}
}
