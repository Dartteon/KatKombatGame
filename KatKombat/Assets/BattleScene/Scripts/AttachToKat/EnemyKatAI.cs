using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class EnemyKatAI : MonoBehaviour {
	private static readonly int CONST_MAX_PLAYERS = 1000;
	/*
	private float atk1Range, atk2Range, atk3Range;
	private char atk1Mode, atk2Mode, atk3Mode;
	private int atk1type, atk2type, atk3type;
	private int atk1Cost, atk2Cost, atk3Cost;
	private float atk1CastTime, atk2CastTime, atk3CastTime;
	private float atk1Cooldown, atk2Cooldown, atk3Cooldown;
*/

	private KatProjectileLauncherScript atk1Script, atk2Script, atk3Script;
	private StatsScript katStats;
	private ManaScript katMana;
	private MovementScript katMovement;
	public GameObject enemyKat;
	private int ownerKatType;

	private List<GameObject> enemyKatArray = new List<GameObject>();
	private KatProjectileLauncherScript[] atkScriptsArray = new KatProjectileLauncherScript[3];
	private float[] atkRangeArray = new float[3];
	private char[] atkModeArray = new char[3];
	private int[] atkTypeArray = new int[3];
	private int[] atkCostArray = new int[3];
	private float[] atkCastTimeArray = new float[3];
	private float[] atkCooldownArray = new float[3];
	private float[] atkLastCastTimeArray = new float[3];

	private int numAttacksAvailable = 0;

	private float lastDecisionTime;
//	private float lastMoveTime;
	private float decisionDelay;
	private float baseDecisionDelay = 1.5f;
	private bool canMakeDecision = true;
	private bool canMove = true;
	private float lastDecideEnemyTime;

	private float lastRotateTime;

	public float aimDiscrepancy = 0.8f;
//	private float redecisionDelay = 1.0f;

	// Use this for initialization
	void Start () {
		initializeOwnerKat ();
		setAttackArrays ();
		initializeDecisionTimings ();
	}


	public List<GameObject> getEnemyArray(){
		return enemyKatArray;
	}
	void initializeOwnerKat(){
		katStats = transform.parent.gameObject.GetComponent<StatsScript> ();
		katMana = transform.parent.gameObject.GetComponent<ManaScript> ();
		katMovement = transform.parent.gameObject.GetComponent<MovementScript> ();
		ownerKatType = katStats.statType;
	}

	void setAttackArrays(){
		int i = 0;

		if (transform.parent.gameObject.transform.Find ("BasicAttack").GetComponent<KatProjectileLauncherScript> () != null) {
			atkScriptsArray[i] = transform.parent.gameObject.transform.Find ("BasicAttack").GetComponent<KatProjectileLauncherScript> ();
			setAttack(i);
			i++;
		}

		if (transform.parent.gameObject.transform.Find ("SecondAttack").GetComponent<KatProjectileLauncherScript> () != null) {
			atkScriptsArray[i] = transform.parent.gameObject.transform.Find ("SecondAttack").GetComponent<KatProjectileLauncherScript> ();
			setAttack(i);
			i++;
		}

		if (transform.parent.gameObject.transform.Find ("ThirdAttack").GetComponent<KatProjectileLauncherScript> () != null) {
			atkScriptsArray[i] = transform.parent.gameObject.transform.Find ("ThirdAttack").GetComponent<KatProjectileLauncherScript> ();
			setAttack(i);
			i++;
		}

		numAttacksAvailable = i;
	}

	//todo: refine this block
	int decideNextEnemyTarget(){
//		int notDeadKatIndex = - 1;
		bool allDead = true;

		int[] aliveKats = new int[CONST_MAX_PLAYERS];
		int count = 0;

		for (int i=0; i<enemyKatArray.Count; i++) {
			if (enemyKatArray [i] != null) {
				float chanceToAttack = calculateChanceToAttack (i);
				aliveKats[count] = i; //store into alive kats array
				count++;

//				notDeadKatIndex = i;

				allDead = false;
				if (chanceToAttack >= Random.value){
//					Debug.Log("Targeting " + i);
					return i;

				}
			}
		}

		if (allDead) {
			return -1;
		} else {
			int num = aliveKats[Random.Range(0, count)];
			return num;
		}
	}

	float calculateChanceToAttack(int i){
		float distanceFromTarget = (enemyKatArray [i].transform.position - transform.position).magnitude;
		return 1.0f/distanceFromTarget;
	}

	void initializeDecisionTimings(){
		lastDecideEnemyTime = Time.time;
		setDecisionDelay ();
		lastDecisionTime = Time.time;
//		lastMoveTime = Time.time;
		baseDecisionDelay = katMana.getManaGainRate () * 0.8f;
		lastRotateTime = Time.time;
	}

	void setAttack(int num){
		atkRangeArray[num] = atkScriptsArray[num].getAttackRange();
		atkModeArray[num] = atkScriptsArray[num].getAttackMode();
		atkCostArray[num] = atkScriptsArray[num].manaCost;
		atkCastTimeArray[num] = atkScriptsArray[num].getCastTime();
		atkCooldownArray[num] = atkScriptsArray[num].cooldown;
	}
	
	// Update is called once per frame
	void Update () {
		if ((Time.time - lastDecisionTime) >= decisionDelay) {
			canMakeDecision = true;
		}

		if ((Time.time - lastDecisionTime) >= decisionDelay) {
			canMove = true;
		}

		if (Time.time - lastDecisionTime >= lastDecideEnemyTime) {
			setCurrentEnemy ();
		}

		if (enemyKat != null) {
			callDecisionUpdate ();
		} else {
			setCurrentEnemy();
		}
		
		if ((Time.time - lastRotateTime) >= 0.8f) {
			rotateKatToEnemy();
		}
	}


	void callDecisionUpdate(){
		if (!katStats.isBusy) {
			
			Vector2 absDistance = enemyKat.transform.position - this.transform.position;
			
			int decidedAttack = decideAttack(absDistance); //why does this always return -1??????????????????????
			
			float moveStep = decidedAttack;
			
			if(moveStep == -1)
				moveStep = findSmallestRange();
			else 
				moveStep = atkRangeArray[decidedAttack];

//			if (decidedAttack == 2)
//				Debug.Log("2nd attack called");

			if (decidedAttack != -1)
				executeAttack(decidedAttack, absDistance);

//			else if (katMana.manaCount == 3 && (!(absDistance.magnitude <= findSmallestRange())) && canMove) {


			else if (katMovement.canMove && (absDistance.magnitude > moveStep) && canMove) {		
				Vector2 leapDirection = getLeapDirection (this.transform.position, enemyKat.transform.position);
				float angle = Mathf.Atan2 (leapDirection.y, leapDirection.x) * Mathf.Rad2Deg;
				this.transform.parent.rotation = Quaternion.Euler (new Vector3 (0, 0, angle - 90));
				katMovement.ShootInDirection (leapDirection);
				canMakeDecision = false;
//				lastMoveTime = Time.time;
				setDecisionDelay ();
			} else {
				moveKatForward();
			}
			
			//(katMana.manaCount >= katMovement.jumpCost) alternate else if condition
		}
	}

	void moveKatForward(){
		if (enemyKat != null) {
			Vector2 direction = this.transform.up * Time.timeScale;
			Vector2 distance = enemyKat.transform.position - this.transform.position;

			if (distance.magnitude > 2.5f) {
				katMovement.walkInDirection (direction);
			}

		}
	}

	void rotateKatToEnemy(){
		if (enemyKat != null) {
			Vector2 direction = enemyKat.transform.position - this.transform.position;
			lastRotateTime = Time.time;
			katMovement.FaceForward (direction);
		}
	}

	void setCurrentEnemy(){
		int nextTarget = decideNextEnemyTarget ();

		if (nextTarget != -1) {			
			enemyKat = enemyKatArray [nextTarget];
		}
	}

	float findSmallestRange(){
		return Mathf.Min(atkRangeArray [0], atkRangeArray [1], atkRangeArray [2]);
	}

	int decideAttack(Vector2 absDistance){
		float currentTime = Time.time;
		float[] atkList = new float[3];

		for (int i=0; i<numAttacksAvailable; i++){
			if (currentTime - atkLastCastTimeArray[i] >= atkCooldownArray[i]   
			    &&   absDistance.magnitude <= atkRangeArray[i]   
			    &&   katMana.manaCount >= atkCostArray[i]   
			    &&   canMakeDecision
			    &&   atkScriptsArray[i].getCanCast()){ //newlinehere

				float chance = absDistance.magnitude/10.0f;
				atkList[i] = getEffectivenessMultiplier(ownerKatType, enemyKat.GetComponent<StatsScript>().statType) * chance;
				atkList[i] = atkList[i]*getStabMultiplier(i);
//				Debug.Log(chance + " " + i);
				return findLargestIndexInArray(atkList, numAttacksAvailable);
			}
		}

		return -1;
	}

	private float getStabMultiplier (int i){
		if (atkTypeArray [i] == ownerKatType)
			return 5.0f;
		else
			return 1.0f;
	}

	int findLargestIndexInArray(float[] array, int max){
		float largestVal = 0.0f;
		int largestIndex = -1;
		for (int i=0; i<max; i++) {
			if(array[i] > largestVal){
				largestIndex = i;
			}
		}
		return largestIndex;
	}

	void executeAttack(int attackNum, Vector2 absDistance){
		Vector2 castDirection = getCastDirection (this.transform.position, enemyKat.transform.position, aimDiscrepancy);
		float angle = Mathf.Atan2 (castDirection.y, castDirection.x) * Mathf.Rad2Deg;
		this.transform.parent.rotation = Quaternion.Euler (new Vector3 (0, 0, angle - 90));
		katMovement.FaceForward (Quaternion.Euler (new Vector3 (0, 0, angle - 90)));

		atkScriptsArray[attackNum].Cast (castDirection);
		canMakeDecision = false;
		lastDecisionTime = Time.time + atkCastTimeArray[attackNum];
//		lastMoveTime = Time.time;
		setDecisionDelay ();
	}

	float getRandomDecisionDelay(){
		return (baseDecisionDelay + Random.Range (-baseDecisionDelay, baseDecisionDelay));
	}

	void setDecisionDelay(){
		this.decisionDelay = getRandomDecisionDelay();
	}

	Vector2 getCastDirection(Vector2 startPt, Vector2 endPt, float discrepancy){
		float difference1 = Random.Range (-discrepancy, discrepancy);
		float difference2 = Random.Range (-discrepancy, discrepancy);
//		difference *= (startPt - endPt).magnitude / 2.0f;
//		Debug.Log ("Difference = " + difference);
		Vector2 newEndPt = new Vector2 (endPt.x + difference1, endPt.y + difference2);
		return (newEndPt - startPt);
	}

	Vector2 getLeapDirection(Vector2 startPt, Vector2 endPt){
	//	Random.Range(0,2)*2-1;
		float discrepancy = 1.0f;
		Vector2 newEndPt = new Vector2 (endPt.x + ((Random.Range (0, 2) * 2 - 1) * discrepancy), endPt.y + ((Random.Range (0, 2) * 2 - 1) * discrepancy));
		return (newEndPt - startPt);
	}

	float getEffectivenessMultiplier(int myType, int targetType){
		float high = 5.0f, med = 3.0f, low = 1.0f;
		if (myType == 0) { //incoming damage is STR
			if (targetType == 1)
				return high;
			else if (targetType == 2)
				return low;
			else
				return med;
		} 

		else if (myType == 1) {
			//incoming damage is DEX
			if (targetType == 2)
				return high;
			else if (targetType == 0)
				return low;
			else
				return med;
		} 

		else if (myType == 2) {
			//incoming damage is INT
			if (targetType == 0)
				return high;
			else if (targetType == 1)
				return low;
			else
				return med;
		} else
			return med;
	}

	public void addEnemy(GameObject enemy) {
		enemyKatArray.Add (enemy);
		Debug.Log (enemyKatArray.Count);
	}
}
