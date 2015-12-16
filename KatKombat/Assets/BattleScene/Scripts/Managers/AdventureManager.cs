using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using EggType = KatBreed.EggType;

public class AdventureManager : MonoBehaviour {
	public List<KatStatsInfo> katsInfo = new List<KatStatsInfo> ();
	public List<EggInfo> eggs = new List<EggInfo> ();
	public GameObject[] katPrefabs;
	public GameObject battleManagerPrefab;
	public GameObject battleInfoModulePrefab;
	public string battleSceneName;
	public GameObject eggPrefab;
	public GameObject eggTrayPrefab;
	private GameObject eggTray;

	private int currentFightingKatIndex = 0;
	private GameObject spawnedBattleInfoModule;
	private string lastLoadedSceneName;

	private List<GameObject> spawnedKats = new List<GameObject> ();

	private PlayerInformation playerDataScript;

	private bool madeNewGame = false;

	private int currentlySelectedKat = 0;

	public void initialize(){
		eggTray = Camera.main.transform.Find ("EggTray").gameObject;
		loadPlayerFile ();
		spawnPlayerKats ();
	}

	void initiateBattle(){
		if (spawnedBattleInfoModule == null) {
			katsInfo.Add (getNewEnemy ());
			Debug.Log ("Adv.Manager: Created enemy info: " + katsInfo [currentlySelectedKat].toString ());
			GameObject battleInfo = Instantiate (battleInfoModulePrefab) as GameObject;
			BattleInformation battleInfoScript = battleInfo.GetComponent<BattleInformation> ();
			Debug.Log ("[AdventureManager Player Kat] " + katsInfo [currentlySelectedKat].toString ());
			KatStatsInfo enemy = new KatStatsInfo("SkyRai", katPrefabs, "EnemyDude");
			Debug.Log ("[AdventureManager Spawned Enemy] " + enemy.toString ());
			battleInfoScript.setKats (katsInfo [0], enemy);
			battleInfoScript.setMap (getCurrentMap ());
			battleInfoScript.setKatPrefabs(katPrefabs);
			spawnedBattleInfoModule = battleInfo;
			Application.LoadLevel (battleSceneName);
		}
	}

	public void initiateBattleWithKat(KatStatsInfo enemy){
		currentFightingKatIndex = 0;
		GameObject battleInfo = Instantiate (battleInfoModulePrefab) as GameObject;
		BattleInformation battleInfoScript = battleInfo.GetComponent<BattleInformation> ();
		Debug.Log ("[AdventureManager Player Kat] " + katsInfo [currentFightingKatIndex].toString ());
		Debug.Log ("[AdventureManager Spawned Enemy] " + enemy.toString ());
		battleInfoScript.setKats (katsInfo [currentFightingKatIndex], enemy);
		battleInfoScript.setMap (getCurrentMap ());
		battleInfoScript.setKatPrefabs(katPrefabs);
		spawnedBattleInfoModule = battleInfo;
		Application.LoadLevel (battleSceneName);
	}

	public PlayerInformation getPlayerInfoScript(){
		return this.playerDataScript;
	}
	public int getCurrentFightingKatIndex(){
		return this.currentFightingKatIndex;
	}

	string getCurrentMap(){
		//stub
		return "cFarmMap";
	}

	KatStatsInfo getNewEnemy(){
		//stub
		int randomEnemyIndex = UnityEngine.Random.Range (0, katPrefabs.Length);
		KatStatsInfo enemy = new KatStatsInfo (katPrefabs[randomEnemyIndex].name, katPrefabs, "EnemyName");
		return enemy;
	}

	void loadPlayerFile(){
		if (playerDataScript == null) {
			if (File.Exists (Application.persistentDataPath + "/gamesave.dat")) {
				BinaryFormatter bf = new BinaryFormatter ();
				FileStream file = File.Open (Application.persistentDataPath + "/gamesave.dat", FileMode.Open);
				PlayerInformation loadedScript = (PlayerInformation)bf.Deserialize (file);
	//			playerDataScript = (PlayerInformation)bf.Deserialize (file);
				file.Close ();

				playerDataScript = loadedScript;
				
				this.katsInfo = playerDataScript.ownedKats;
				this.eggs = playerDataScript.ownedEggs;
				Debug.Log ("Loaded SaveFile: " + playerDataScript.playerToString ());
				Debug.Log (katsInfo [0].toString ());

//			Debug.Log("Owned Kats : " + playerDataScript.ownedKats.ToString());
			} else {
				handleNewGame ();
			}
		}
	}

	void handleNewGame(){
		playerDataScript = new PlayerInformation ("Baron");
		playerDataScript.addCurrency (50);
		String newKatBreed = katPrefabs [UnityEngine.Random.Range (0, katPrefabs.Length)].name;
		KatStatsInfo newKat = new KatStatsInfo (newKatBreed, katPrefabs, "Jeff");
	//	newKat.initializeStats (newKatName, katPrefabs);
	//	newKat.increaseExp (1000);
		Debug.Log (newKat.toString ());
		playerDataScript.addKatToInventory (newKat);
		katsInfo = playerDataScript.ownedKats;
	}

	void summonNewKat(){
		String newKatName = "Spawn" + katsInfo.Count;
		String newKatBreed = katPrefabs [UnityEngine.Random.Range (0, katPrefabs.Length)].name;
		KatStatsInfo newKat = new KatStatsInfo (newKatBreed, katPrefabs, newKatName);
		playerDataScript.addKatToInventory (newKat);
		katsInfo = playerDataScript.ownedKats;
	}

	void summonNewEgg(){
		String newEggName = "Eggling" + eggs.Count;
		String newKatBreed = katPrefabs [UnityEngine.Random.Range (0, katPrefabs.Length)].name;
		KatStatsInfo newKat = new KatStatsInfo (newKatBreed, katPrefabs, newEggName);
		EggInfo newEgg = EggInfo.getNewEgg (newKat, EggType.BlueYellowStripe);
		playerDataScript.addEggToInventory (newEgg);
		eggs = playerDataScript.ownedEggs;
	}

	void savePlayerFile(){
		if (playerDataScript != null) {
//		Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER","yes");
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Create (Application.persistentDataPath + "/gamesave.dat");
			bf.Serialize (file, playerDataScript);
			file.Close ();
			Debug.Log ("[Save Successful] " + playerDataScript.playerToString());
		} else {
			Debug.LogError("No PlayerData loaded - Save failed");
		}
	}

	// Use this for initialization
	void Start () {
		Time.timeScale = 1;
		DontDestroyOnLoad (this);
	}

	void spawnPlayerKats(){
		for (int i = 0; i<katsInfo.Count; i++) {
			spawnKatInScene(katsInfo [i]);
		}
	}

	public void spawnKatInScene(KatStatsInfo info){
		GameObject spawnedKat = Instantiate (findKatWithName (info.getBreed ()), transform.position, transform.rotation) as GameObject; 
		spawnedKats.Add (spawnedKat);
//		Debug.Log(spawnedKat.ToString());
		StatsScript katStats = spawnedKat.GetComponent<StatsScript> ();
		katStats.setToLevel (info.getLevel ());
		katStats.setStats (info.getTotalStr (), info.getTotalDex (), info.getTotalInt ());
		katStats.setKatCommands (info.getActiveKommands ());
		disableKatCombatComponents (spawnedKat);
		spawnedKat.AddComponent <NonCombatBehavior> ();
		spawnedKat.tag = "Player1";
	}
	
/*
	void spawnEnemyKat(){
		//replace this chunk by extracting enemy data
		int index = 1;
		katsInfo.Add (new KatStatsInfo (enemyKatBreed, katPrefabs));
		katsInfo [index].increaseExp (10000);
//		katsInfo [index].attachKatPrefabsArray (katPrefabs);
		katsInfo [index].initializeStats (enemyKatBreed, katPrefabs);
		
		GameObject spawnedKat = Instantiate(findKatWithName(enemyKatBreed), transform.position, transform.rotation) as GameObject; 
		spawnedKats [1] = spawnedKat;
		StatsScript katStats = spawnedKat.GetComponent<StatsScript> ();
		katStats.setToLevel (katsInfo [index].getLevel());
		katStats.setStats (katsInfo [index].getTotalStr (), katsInfo [index].getTotalDex (), katsInfo [index].getTotalInt ());
		katStats.setKatCommands (katsInfo [index].getCommands ());
	}
*/
	GameObject findKatWithName(string katName){
		for (int i=0; i<katPrefabs.Length; i++){
			if (katPrefabs[i].name.Equals(katName)){
				return katPrefabs[i];
			}
		}
		return katPrefabs [0];
	}

	// Update is called once per frame
	void Update () {
		if (GameVariables.DEBUG_MODE == true) {
			if (Input.GetKeyDown (KeyCode.F2)) {
				Debug.Log ("Pressed Save");
				savePlayerFile ();
			}

			if (Input.GetKey (KeyCode.F3)) {
				Camera.main.GetComponent<FarmCameraFollowScript>().goToCameraMode(2);
			}

			if (Input.GetKey (KeyCode.F4)) {
				if (!madeNewGame) {
					madeNewGame = true;
					Debug.Log ("Handling new game");
					handleNewGame ();
				}
			}

			if (Input.GetKeyDown (KeyCode.F5)) {
				for (int i=0; i<katsInfo.Count; i++) {
					Debug.Log ("[Kat " + i + "] " + katsInfo [i].toString ());
				}
			}

			if (Input.GetKeyDown (KeyCode.F6)) {
				for (int i=0; i<katsInfo.Count; i++) {
					Debug.Log ("[Kat " + i + "] " + katsInfo [i].toString ());
				}
			}

			if (Input.GetKeyDown (KeyCode.F7)) {
				Debug.Log(Kommands.KommandCode.ArcanePulse.ToString());
				Debug.Log ("Adding new random Egg to inventory");
				summonNewEgg ();
			}
			if (Input.GetKeyDown (KeyCode.F8)) {
				Debug.Log("Attemping to hatch and spawn an egg");
				if (eggs[0] != null){
					KatStatsInfo baby = setEggToHatched(eggs[0]);
					spawnKatInScene(baby);
				}
			}

/*
			if (Input.GetKeyDown (KeyCode.F7)) {
				Debug.Log("Attemping to add an egg image to tray");
				GameObject egg = Instantiate(eggPrefab);
				eggTray.GetComponent<EggTray>().addEggToTray(egg);
			}

			if (Input.GetKeyDown (KeyCode.F9)) {
				Debug.Log("Spawning all player Kats");
				spawnPlayerKats ();
			}
			if (Input.GetKeyDown (KeyCode.F8)) {
				Debug.Log ("Adding new random Kat to inventory");
				summonNewKat ();
			}
*/

			if (Input.GetKeyDown (KeyCode.F11)) {
				followNextKat ();
			}
			if (Input.GetKeyDown (KeyCode.F12)) {
				Camera.main.GetComponent<FarmCameraFollowScript>().stopFollowingKat();
			}

		}
	}

	public void followThisKat(GameObject kat){
		int katIndex;

		for (int i=0; i<katsInfo.Count; i++) {
			if (kat == spawnedKats[i]){
				GameObject.Find ("Main Camera").GetComponent<FarmCameraFollowScript> ().followKat (spawnedKats[i], katsInfo[i]);
				break;
			}
		}
	}

	void followNextKat(){
		int numKats = katsInfo.Count;
		if (currentlySelectedKat == numKats-1) {
			currentlySelectedKat = 0;
		} else {
			currentlySelectedKat++;
		}
//		Debug.Log (currentlySelectedKat + " " +  katsInfo.Count);
//		Debug.Log (spawnedKats [currentlySelectedKat].ToString ());
//		Debug.Log (katsInfo [currentlySelectedKat].toString ());
		Camera.main.GetComponent<FarmCameraFollowScript> ().followKat (spawnedKats[currentlySelectedKat], katsInfo[currentlySelectedKat]);
	}


	private void disableKatCombatComponents(GameObject kat){
		kat.transform.Find ("CanvasManaBar").gameObject.SetActive (false);
		kat.transform.Find ("CanvasHealthBar").gameObject.SetActive (false);
	}

	//To be called externally by Incubator
	public KatStatsInfo setEggToHatched(EggInfo egg){
		KatStatsInfo baby = egg.getKat ();
		eggs.Remove (egg);
		katsInfo.Add (baby);
		return baby;
	}

	public bool rewardKatWithExp(int enemyLevel) {
		if (ExperienceHandlerScript.addExperienceToKat (playerDataScript, currentFightingKatIndex, enemyLevel, 1.0f)) {
			return true;
		} else
			return false;
	}

	public void loadLastScene() {
		Application.LoadLevel (lastLoadedSceneName);
	}
}
