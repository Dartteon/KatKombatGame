using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kommand = Kommands.KommandCode;

public class GamestateBattleManager : MonoBehaviour {

	public GameObject defeatButtonPrefab;
	public GameObject stageBeginScreenPrefab;
	public GameObject katAttackVesselPrefab;

	public GameObject[] mapPrefabs;
	private GameObject currentMap;
	private int mapChoice;
	//SpawnLocations should be gotten from chosen MAP_PREFAB
	private GameObject[] spawnLocations = new GameObject[2];

	private AdventureManager adventureManager;
	private PlayerInformation playerInfo;
	private bool hasGameStarted = false;
	private GameObject playerKat;
	private GameObject enemyKat;
	private int enemyKatLevel;

	private GameObject playerKatPrefab, enemyKatPrefab;
	private KatStatsInfo playerKatInfo, enemyKatInfo;

	private GameObject controller;

	public Vector2 getSpawnLocation(bool isPlayer){
		if (isPlayer) {
			return spawnLocations[0].transform.position;
		} else {
			return spawnLocations[1].transform.position;
		}
	}

	public void togglePause(){
		if (Time.timeScale == 1) {
			Time.timeScale = 0;
			controller.SetActive (false);
		} else {
			Time.timeScale = 1;
			controller.SetActive (true);
		}
	}

	public void attachKats(GameObject player_kat, GameObject enemy_kat){
		playerKatPrefab = player_kat;
		enemyKatPrefab = enemy_kat;
//		spawnKats ();
	}
	public void attachKatsInfo(KatStatsInfo playerKatStats, KatStatsInfo enemyKatStats){
		playerKatInfo = playerKatStats;
		enemyKatInfo = enemyKatStats;
	}

	void Start(){
		adventureManager = GameObject.Find ("AdventureModule").GetComponent<AdventureManager> ();;
		playerInfo = adventureManager.getPlayerInfoScript ();
	}

	void Update(){
		if (hasGameStarted) {
			if (playerKat == null) {
				displayLoss();
				hasGameStarted = false;
			} else if (enemyKat == null) {
				handleVictoryEffects();
				displayVictory();
				hasGameStarted = false;
			}
		}
		
		if (Input.GetKeyDown (KeyCode.Space)) {
			togglePause();
		}
	}

	void handleVictoryEffects() {
		int currentKatStr = playerKatInfo.getTotalStr ();
		int currentKatDex = playerKatInfo.getTotalDex ();
		int currentKatInt = playerKatInfo.getTotalInt ();
		if (adventureManager.rewardKatWithExp (enemyKatLevel)) {
			//display celebrations and shit
		} else {
			//just display generic win
		}
	}

	public void attachMap(int index){
		mapChoice = index;
	}

	//attach Kats and map first, THEN initialize
	public void initialize(){
		spawnMap ();
		spawnKats ();
	}

	void spawnKats(){
		try{
			if (playerKatPrefab != null) {
				playerKatInfo.setKommands(Kommands.KommandCode.ArcanePulse, Kommands.KommandCode.Charge, Kommands.KommandCode.Furball);
//				playerKatInfo.setCommands("Earthquake", "LightningHelix", "TwinFang");

				if (spawnLocations [0] != null) playerKat = Instantiate(playerKatPrefab, spawnLocations[0].transform.position, spawnLocations[0].transform.rotation) as GameObject;
				else playerKat = Instantiate(playerKatPrefab) as GameObject;

				Debug.Log("[GameStateBattleManager] Instantiated Player Kat ");
				playerKat.GetComponent<StatsScript> ().setStats (playerKatInfo);
				attachAttackToKat(playerKat, playerKatInfo);
				playerKat.tag = "Player1";
				Camera.main.GetComponent<BattleCameraFollowScript>().attachKat(playerKat);
				Camera.main.GetComponent<BattleCameraFollowScript>().initialize();
//				StatsScript katStats = playerKat.GetComponent<StatsScript>();
//				katStats.setStats(playerKat);

			}

			if (enemyKatPrefab != null) {
				if (spawnLocations [1] != null)	enemyKat = Instantiate(enemyKatPrefab, spawnLocations[1].transform.position, spawnLocations[1].transform.rotation) as GameObject;
				else enemyKat = Instantiate(enemyKatPrefab) as GameObject;

				enemyKat.GetComponent<StatsScript> ().setStats (enemyKatInfo);
				attachAttackToKat(enemyKat, enemyKatInfo);
				enemyKat.tag = "Player2";
				enemyKatLevel = enemyKatInfo.getLevel();
				Debug.Log("[GameStateBattleManager] Instantiated Enemy Kat " + enemyKat);
//				Debug.Log(enemyKatInfo.toString());
			}

		} catch (System.NullReferenceException e){
			Debug.Log(e);
			Debug.LogError("Attach spawn lcoation objects as child to map: " + mapChoice);
		}
	}

	GameObject spawnMap(){		
		currentMap = Instantiate(mapPrefabs[mapChoice]) as GameObject;
		try{
			spawnLocations[0] = currentMap.transform.FindChild("PlayerSpawnLocation").gameObject;
			spawnLocations[1] = currentMap.transform.FindChild("EnemySpawnLocation").gameObject;
			//					Debug.Log(spawnLocations[0].transform.position.ToString() + spawnLocations[1].transform.position.ToString());
		} catch (System.NullReferenceException e){
			Debug.Log(e);
			Debug.Log("Attach spawn location objects as child to map at index: " + mapChoice);
		}
		//Map not found:
		return mapPrefabs [mapChoice];
	}

	public void attachController(GameObject controllerPrefab){
//		Debug.Log (playerKat.ToString ());
		controller = Instantiate (controllerPrefab) as GameObject;
		controller.GetComponent<MTMainController> ().AttachKat (playerKat);
	}
	
	
	public void activateGameStart(){
		GameObject stageBeginScreen = Instantiate (stageBeginScreenPrefab) as GameObject;
		hasGameStarted = true;
	}

	public void displayVictory(){
		if (hasGameStarted) {
			hasGameStarted = false;
			controller.SetActive (false);
//		Time.timeScale = 0;
//		Debug.Log ("Won");
			//PlayerInformation currentPlayerInfo = 
			//	int currentKatIndex = adventureManager.transform.GetComponent<AdventureManager> ().getCurrentFightingKatIndex;
			int currentKatIndex = 0;
			Debug.Log (playerInfo.ToString ());
			Debug.Log (currentKatIndex);
			Debug.Log (enemyKatLevel);

//		Instantiate (victoryButtonPrefab);
			BattleCameraFollowScript camScript = Camera.main.GetComponent<BattleCameraFollowScript> ();
			camScript.showPlayerVictory ();

			if (ExperienceHandlerScript.addExperienceToKat (playerInfo, currentKatIndex, enemyKatLevel, 1.0f)) {
				Debug.Log("level up detected");
				camScript.showLevelUp ();
			}
		}
	}
	public void displayLoss(){
		controller.SetActive (false);
//		Time.timeScale = 0;
		Camera.main.GetComponent<BattleCameraFollowScript> ().showEnemyVictory (enemyKat);
//		Debug.Log ("Lost");
		Instantiate (defeatButtonPrefab);
	}

	void attachAttackToKat(GameObject kat, KatStatsInfo katInfo){
		List<Kommand> komList = katInfo.getActiveKommands ();
		//string[] katAttacks = katInfo.getCommands ();

		string[] katAttacks = new string[3];
		for (int i=0; i<3; i++) {
			if (i < komList.Count){
				katAttacks[i] = Kommands.getStringRepOfKommand(komList[i]);
			} else {
				katAttacks[i] = "nil";
			}
		}

		KatAttacksVessel attackPrefabsScript = katAttackVesselPrefab.GetComponent<KatAttacksVessel>();
		//KatProjectileLauncherScript[] projectileLauncherScripts = kat.GetComponentsInChildren<KatProjectileLauncherScript> ();
		KatProjectileLauncherScript[] projectileLauncherScripts = new KatProjectileLauncherScript[3];
		projectileLauncherScripts[0] = kat.transform.Find ("BasicAttack").GetComponent<KatProjectileLauncherScript> ();
		projectileLauncherScripts[1] = kat.transform.Find ("SecondAttack").GetComponent<KatProjectileLauncherScript> ();
		projectileLauncherScripts[2] = kat.transform.Find ("ThirdAttack").GetComponent<KatProjectileLauncherScript> ();

		for (int i=0; i<katAttacks.Length; i++){
			if (!katAttacks.Equals("nil")){
				GameObject attackPrefab = attackPrefabsScript.findAttackWithName(katAttacks[i]);
//				Debug.Log(attackPrefab.ToString());
				projectileLauncherScripts[i].SetPrefab(attackPrefab);
				projectileLauncherScripts[i].transform.gameObject.SetActive(true);
			}
		}
	}
}
