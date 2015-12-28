using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Kommand = Kommands.KommandCode;

public class MultiKatBattleManager : MonoBehaviour {
	
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
	private List<GameObject> playerKats = new List<GameObject>();
	private List<GameObject> enemyKats = new List<GameObject>();
//	private int enemyKatLevel;
	
//	private GameObject playerKatPrefab, enemyKatPrefab;
//	private KatStatsInfo playerKatInfo, enemyKatInfo;
	
//	private GameObject controller;
	
	[SerializeField]
	private GameObject enemyAIModule;
	[SerializeField]
	private GameObject defeatButtonPrefab;
	[SerializeField]
	private GameObject tournamentEndSignPrefab;
	[SerializeField]
	private GameObject katPrefabVessel;
	[SerializeField]
	private int numKatsPerSide = 1;
	
	public Vector2 getSpawnLocation(bool isPlayer){
		if (isPlayer) {
			return spawnLocations[0].transform.position;
		} else {
			return spawnLocations[1].transform.position;
		}
	}

	void Start(){
		Time.timeScale = 0.5f;
//		adventureManager = GameObject.Find ("AdventureModule").GetComponent<AdventureManager> ();;
//		playerInfo = adventureManager.getPlayerInfoScript ();
	}
	
	void Update(){
		if (hasGameStarted) {
			if (playerKats.Count == 0) {
				displayLoss();
				hasGameStarted = false;
			} else if (enemyKats.Count == 0) {
				handleVictoryEffects();
				displayVictory();
				hasGameStarted = false;
			}
		}
	}
	
	void handleVictoryEffects() {
	}
	
	public void attachMap(int index){
		mapChoice = index;
	}
	
	//attach Kats and map first, THEN initialize
	public void initialize(){
		spawnMap ();
		spawnKats ();
		attachEnemyAI ();
	}
	
	void spawnKats(){
		for (int i = 0; i < numKatsPerSide; i++) {
			KatStatsInfo playerKat = getRandomKat();
			playerKat.setKommands(Kommands.getRandomStrKommand(), Kommands.getRandomDexKommand(), Kommands.getRandomIntKommand());
			GameObject playerKatObjPrefab = katPrefabVessel.GetComponent<KatPrefabsVesselScript>().getKatOfBreed(playerKat.breed);
			GameObject playerKatObj = Instantiate(playerKatObjPrefab, spawnLocations[0].transform.position, spawnLocations[0].transform.rotation) as GameObject;
			playerKatObj.GetComponent<StatsScript>().setStats(playerKat);
			attachAttackToKat(playerKatObj, playerKat);
			playerKatObj.tag = "Player1";
			playerKats.Add(playerKatObj);
		}

		for (int i=0; i < numKatsPerSide; i++) {
			KatStatsInfo enemyKat = getRandomKat();
			enemyKat.setKommands(Kommands.getRandomStrKommand(), Kommands.getRandomDexKommand(), Kommands.getRandomIntKommand());
			GameObject enemyKatObjPrefab = katPrefabVessel.GetComponent<KatPrefabsVesselScript>().getKatOfBreed(enemyKat.breed);
			GameObject enemyKatObj = Instantiate(enemyKatObjPrefab, spawnLocations[1].transform.position, spawnLocations[1].transform.rotation) as GameObject;
			enemyKatObj.GetComponent<StatsScript>().setStats(enemyKat);
			attachAttackToKat(enemyKatObj, enemyKat);
			enemyKatObj.tag = "Player2";
			enemyKats.Add(enemyKatObj);
		}

	}

	private KatStatsInfo getRandomKat() {
		KatBreed.getRandomBreed ();
		KatBreed.getRandomBreed ();
		KatStatsInfo kat = new KatStatsInfo (KatBreed.getRandomBreed (), katPrefabVessel, "Kat");
		kat.setLevel (60);
		return kat;
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
	
	
	public void activateGameStart(){
//		GameObject stageBeginScreen = Instantiate (stageBeginScreenPrefab) as GameObject;
		hasGameStarted = true;
	}
	
	public void displayVictory(){
	}
	public void displayLoss(){
	}

	
	void attachAttackToKat(GameObject kat, KatStatsInfo katInfo){
		List<Kommand> komList = katInfo.getActiveKommands ();
		//string[] katAttacks = katInfo.getCommands ();
		
		KatAttacksVessel attackPrefabsScript = katAttackVesselPrefab.GetComponent<KatAttacksVessel>();
		KatProjectileLauncherScript[] projectileLauncherScripts = new KatProjectileLauncherScript[3];
		projectileLauncherScripts[0] = kat.transform.Find ("BasicAttack").GetComponent<KatProjectileLauncherScript> ();
		projectileLauncherScripts[1] = kat.transform.Find ("SecondAttack").GetComponent<KatProjectileLauncherScript> ();
		projectileLauncherScripts[2] = kat.transform.Find ("ThirdAttack").GetComponent<KatProjectileLauncherScript> ();
		
		for (int i=0; i<3; i++) {
			if (!komList[i].Equals(Kommand.None)) {
				GameObject atkPrefab = attackPrefabsScript.findKommand(komList[i]);
				projectileLauncherScripts[i].SetPrefab(atkPrefab);
				//				Debug.Log("Launcher set: " + atkPrefab.ToString());
				projectileLauncherScripts[i].gameObject.SetActive(true);
				//				Debug.Log("Kommand found: " + komList[i].ToString());
			} else {
				Debug.LogError("Kommand not found! " + komList[i]);
				projectileLauncherScripts[i].SetInactive();
			}
		} 
	}
	
	void attachEnemyAI() {
		Debug.Log ("attaching enemy AI");
		for (int i = 0; i < playerKats.Count; i++) {
			Debug.Log ("attaching for player " + i);
			GameObject enemyAI = Instantiate (enemyAIModule, playerKats[i].transform.position, playerKats[i].transform.rotation) as GameObject;
			enemyAI.transform.parent = playerKats[i].transform;
			//		Debug.Log (enemyAI.ToString ());
			EnemyKatAI aiScript = enemyAI.GetComponent<EnemyKatAI> ();

			for (int j = 0; j < enemyKats.Count; j++) {
				aiScript.addEnemy(enemyKats[j]);
			}
			
			enemyAI.SetActive (true);
		}

		for (int i = 0; i < enemyKats.Count; i++) {
			Debug.Log ("attaching for enemy " + i);
			GameObject enemyAI = Instantiate (enemyAIModule, enemyKats[i].transform.position, enemyKats[i].transform.rotation) as GameObject;
			enemyAI.transform.parent = enemyKats[i].transform;
			//		Debug.Log (enemyAI.ToString ());
			EnemyKatAI aiScript = enemyAI.GetComponent<EnemyKatAI> ();
			
			for (int j = 0; j < playerKats.Count; j++) {
				aiScript.addEnemy(playerKats[j]);
			}
			
			enemyAI.SetActive (true);
		}
	}
}