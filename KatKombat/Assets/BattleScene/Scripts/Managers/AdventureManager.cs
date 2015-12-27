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
	public GameObject battleManagerPrefab;
	public GameObject battleInfoModulePrefab;
	public string battleSceneName;

	private int currentFightingKatIndex = 0;
	private GameObject spawnedBattleInfoModule;
	private string lastLoadedSceneName;
	private GameObject[] katPrefabs;

	private FarmManager farmMngr;

	private PlayerInformation playerDataScript;

	[SerializeField]
	private GameObject eggPrefabVessel;
	[SerializeField]
	private GameObject katPrefabVessel;

	public void initialize(){
		katsInfo = playerDataScript.ownedKats;
		eggs = playerDataScript.ownedEggs;
//		loadPlayerFile ();
	}
	public void attempToLoadFile() {
		loadPlayerFile ();
	}
	public void initiateBattleWithKat(KatStatsInfo playrKat, KatStatsInfo enemy){
		Debug.Log (playrKat.toString ());

		currentFightingKatIndex = katsInfo.IndexOf (playrKat);
//		Debug.Log (currentFightingKatIndex + " fighting");
//		Debug.Log (currentFightingKatIndex);
		//find past battle remnant and destroy
		Destroy (GameObject.Find ("BattleInformationModule"));

		GameObject battleInfo = Instantiate (battleInfoModulePrefab) as GameObject;
		BattleInformation battleInfoScript = battleInfo.GetComponent<BattleInformation> ();
		Debug.Log ("[AdventureManager Player Kat] " + playrKat.toString ());
		Debug.Log ("[AdventureManager Spawned Enemy] " + enemy.toString ());
		battleInfoScript.setKats (playrKat, enemy);
		battleInfoScript.setMap (getCurrentMap ());
		battleInfoScript.setKatPrefabVessel(katPrefabVessel);
		spawnedBattleInfoModule = battleInfo;
		Application.LoadLevel (battleSceneName);

//		farmMngr = GameObject.Find ("FarmManagerModule").GetComponent<FarmManager> ();
	}

	public void initiateBattleWithKat(KatStatsInfo enemy){
		//find past battle remnant and destroy
		Destroy (GameObject.Find ("BattleInformationModule"));

		currentFightingKatIndex = 0;
		GameObject battleInfo = Instantiate (battleInfoModulePrefab) as GameObject;
		BattleInformation battleInfoScript = battleInfo.GetComponent<BattleInformation> ();
		Debug.Log ("[AdventureManager Player Kat] " + katsInfo [currentFightingKatIndex].toString ());
		Debug.Log ("[AdventureManager Spawned Enemy] " + enemy.toString ());
		battleInfoScript.setKats (katsInfo [currentFightingKatIndex], enemy);
		battleInfoScript.setMap (getCurrentMap ());
		battleInfoScript.setKatPrefabVessel(katPrefabVessel);
		spawnedBattleInfoModule = battleInfo;
		Application.LoadLevel (battleSceneName);
		
		farmMngr = GameObject.Find ("FarmManagerModule").GetComponent<FarmManager> ();
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
//		int randomEnemyIndex = UnityEngine.Random.Range (0, katPrefabs.Length);
//		KatStatsInfo enemy = new KatStatsInfo (katPrefabs[randomEnemyIndex].name, katPrefabs, "EnemyName");
		KatStatsInfo enemy = new KatStatsInfo (KatBreed.getRandomBreed(), katPrefabVessel, "EnemyName");
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

//				farmMngr.spawnPlayerEggs();
//				farmMngr.spawnPlayerKats();

//				Debug.Log ("Time Diff: " + DateTime.Now.Subtract(playerDataScript.time_lastActive).ToString());
//				Debug.Log (katsInfo [0].toString ());

//			Debug.Log("Owned Kats : " + playerDataScript.ownedKats.ToString());
			} else {
				handleNewGame ();
			}
		}
	}
	public bool hasSlotForEggOrKat() {
//		Debug.LogError ("Num of kats and eggs " + (eggs.Count + katsInfo.Count));
		if (eggs.Count + katsInfo.Count <= 5)
			return true;
		else
			return false;
	}
	void handleNewGame(){
//		Debug.Log ("Handling New Game");
		playerDataScript = new PlayerInformation ("Kat Tamer");
		playerDataScript.addCurrency (100);
		summonNewEgg ();
//		savePlayerFile ();
		Application.LoadLevel (Application.loadedLevel);
//		playerDataScript.addCurrency (50);
//		String newKatBreed = katPrefabs [UnityEngine.Random.Range (0, katPrefabs.Length)].name;
//		KatStatsInfo newKat = new KatStatsInfo (newKatBreed, katPrefabs, GameVariables.getRandomName());
	//	newKat.initializeStats (newKatName, katPrefabs);
	//	newKat.increaseExp (1000);
//		Debug.Log (newKat.toString ());
//		playerDataScript.addKatToInventory (newKat);
//		katsInfo = playerDataScript.ownedKats;
	}

	void summonNewKat(){
		String newKatName = GameVariables.getRandomName();
//		String newKatBreed = katPrefabs [UnityEngine.Random.Range (0, katPrefabs.Length)].name;
		KatStatsInfo newKat = new KatStatsInfo (KatBreed.getRandomBreed(), katPrefabVessel, newKatName);
		playerDataScript.addKatToInventory (newKat);
		katsInfo = playerDataScript.ownedKats;
	}

	public void summonNewEgg(){
		String newEggName = GameVariables.getRandomName();
//		String newKatBreed = katPrefabs [UnityEngine.Random.Range (0, katPrefabs.Length)].name;
		KatStatsInfo newKat = new KatStatsInfo (KatBreed.getRandomBreed(), katPrefabVessel, newEggName);
		EggInfo newEgg = EggInfo.getNewEgg (newKat, KatBreed.getRandomEggColor());
		playerDataScript.addEggToInventory (newEgg);
		newEgg.getKat ().setKommands (Kommands.getRandomStrKommand (), Kommands.getRandomDexKommand (), Kommands.getRandomIntKommand ());
//		GameObject eggObj = Instantiate (eggPrefabVessel.GetComponent<EggPrefabVessel> ().getEgg (newEgg));
//		Debug.Log (farmMngr.ToString ());
		farmMngr.spawnEgg (newEgg);
		eggs = playerDataScript.ownedEggs;
		savePlayerFile ();
//		Application.LoadLevel (Application.loadedLevel);
	}

	public void sellKat(KatStatsInfo kat) {
		playerDataScript.addCurrency (kat.getLevel () * 15);
	}

	public void addCurrency (int currency) {
		playerDataScript.addCurrency (currency);
	}

	public void savePlayerFile(){
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
		katPrefabs = katPrefabVessel.GetComponent<KatPrefabsVesselScript> ().katPrefabs;
	}
	public int getCurrencyAmount() {
		return playerDataScript.playerCurrency;
	}

	public void reflectPlayerCurrency() {
		string curr = "" + getCurrencyAmount ();
		Camera.main.transform.Find ("MarketplaceButton").transform.Find ("KashText").transform.Find ("Text").GetComponent<Text> ().text = curr;
	}

	void Update () {
		if (GameVariables.DEBUG_MODE == true) {
			if (Input.GetKeyDown (KeyCode.F2)) {
				Debug.Log ("Pressed Save");
				savePlayerFile ();
			}

			if (Input.GetKeyDown (KeyCode.F3)) {
				playerDataScript.addCurrency(100);
			}

			if (Input.GetKeyDown (KeyCode.F4)) {
				Debug.Log ("Handling new game");
				handleNewGame ();

			}

			if (Input.GetKeyDown (KeyCode.F5)) {
				reloadScene();
			}

			if (Input.GetKeyDown (KeyCode.F6)) {
				for (int i=0; i<katsInfo.Count; i++) {
						Debug.Log ("[Kat " + i + "] " + katsInfo [i].toString ());
				}
			}

			if (Input.GetKeyDown (KeyCode.F7)) {
				Debug.Log ("Adding new random Egg to inventory");
				summonNewEgg ();
			}

			if (Input.GetKeyDown (KeyCode.F8)) {
				Debug.Log("Attemping to hatch and spawn first egg in stack");
				if (eggs[0] != null){
					setEggToHatched(eggs[0]);
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


		}

		playerDataScript.UpdateDateTime (DateTime.Now);
	}

	public void setFarmManager(FarmManager farmM) {
//		Debug.Log (farmM.ToString ());
		farmMngr = farmM;
	}

	//To be called externally by Incubator
	public KatStatsInfo setEggToHatched(EggInfo egg){
		KatStatsInfo baby = egg.getKat ();
		eggs.Remove (egg);
//		katsInfo.Add (baby);
//		playerDataScript.addKatToInventory (baby);
//		Debug.Log (katsInfo.Count);
//		farmMngr.spawnKatInScene(baby);
//		Debug.Log ("Egg set to hatch");
		katsInfo.Add (baby);
		savePlayerFile ();
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

	public void reloadScene() {
		Application.LoadLevel (Application.loadedLevel);
	}

	public bool deductCurrency(int curr) {
		if (playerDataScript.deductCurrency(curr)) {
			return true;		
		} else return false;
	}
}
