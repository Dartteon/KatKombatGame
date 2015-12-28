using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour {
	/* This manager is used for battle scenes transitioned from AdventureScene.
	 * It will attempt to find player's Kat (GameObject) and enemy's Kat (GameObject)
	 * 
	 * For battle scenes transitioned from KatSelect, you might want to use
	 * Manager class instead of this one.
	 */

//	private List<GameObject> playerKats = new List<GameObject>();
//	private List<GameObject> enemyKats = new List<GameObject>();
//	private List<GameObject> AIModules = new List<GameObject> ();
	private GameObject katPrefabVessel;

	public GameObject controllerPrefab;
	public GameObject[] mapPrefabs;
	public GameObject gamestateBattleManagerModulePrefab;
	public GameObject katAttackVesselPrefab;

	private int mapIndex = 0;
	private Vector2 spawnLoc_player, spawnLoc_enemy;
	private GameObject gamestateBattleManagerModule;

	/* BattleMode legend:
	 * [A]: TBI
	 * [B]:
	 * [C]:
	 */
	private char battleMode = 'A';
	private bool hasGameStarted = false;

	//To attach to player's Kat
	public GameObject targetRingPrefab;

	[SerializeField]
	private GameObject experimentalMultiKatGameStateBattleManagerPrefab;

	/*
	public void initialize(GameObject playerKat, GameObject enemyKat, string nameOfMap){
		playerKats.Add (playerKat);
		enemyKats.Add (enemyKat);
		mapName = nameOfMap;

		gamestateBattleManagerModule = Instantiate (gamestateBattleManagerModulePrefab) as GameObject;
		GamestateBattleManager gamestateScript = gamestateBattleManagerModule.GetComponent<GamestateBattleManager> ();
		gamestateScript.attachKats (playerKat, enemyKat);
		gamestateScript.attachMap (mapName);
		gamestateScript.initialize ();
		gamestateScript.activateGameStart ();
	}
*/

	public void initialize(KatStatsInfo playerKat, KatStatsInfo enemyKat, int _mapIndex){
		//playerKats.Add (playerKat);
		//enemyKats.Add (enemyKat);
		mapIndex = _mapIndex;

		GameObject playerKatPrefab = searchKat (playerKat);
		GameObject enemykatPrefab = searchKat (enemyKat);

//		playerKatPrefab.GetComponent<StatsScript> ().setStats (playerKat);
//		enemykatPrefab.GetComponent<StatsScript> ().setStats (enemyKat);

		gamestateBattleManagerModule = Instantiate (gamestateBattleManagerModulePrefab) as GameObject;
		gamestateBattleManagerModule.name = "GameStateBattleManager";
		GamestateBattleManager gamestateScript = gamestateBattleManagerModule.GetComponent<GamestateBattleManager> ();
		gamestateScript.attachMap (mapIndex);
		gamestateScript.attachKatsInfo (playerKat, enemyKat);
		gamestateScript.attachKats (playerKatPrefab, enemykatPrefab);
		gamestateScript.initialize ();
		gamestateScript.attachController (controllerPrefab);
		gamestateScript.activateGameStart ();
	}


	void Start(){
		GameObject battleInfo = GameObject.Find ("BattleInformationModule");
			
		if (battleInfo != null) {
//			Debug.Log("BattleInfo found. Unpacking!");
			BattleInformation battleInfoScript = battleInfo.GetComponent<BattleInformation>();
			KatStatsInfo playerKat = battleInfoScript.playerKat;
			KatStatsInfo enemykat = battleInfoScript.enemyKat;
			string mapName = battleInfoScript.mapName;
			katPrefabVessel = battleInfoScript.katPrefabVessel;

			initialize(playerKat, enemykat, mapIndex);
		} else {
			Debug.Log("BattleInformation not found!");
			startMultiKatBattle();
		}
	}

	GameObject searchKat(KatStatsInfo katInfo){
//		string katBreed = katInfo.getBreed ();
		KatBreed.Breed breed = katInfo.breed;

		return katPrefabVessel.GetComponent<KatPrefabsVesselScript> ().getKatOfBreed (breed);
		/*
		for (int i=0; i<katPrefabs.Length; i++){
			if (breed.Equals(katPrefabs[i].GetComponent<StatsScript>().breed)){
				return katPrefabs[i];
			}
		}
*/
//		GameObject enemy = katPrefabVessel.GetCo
		//If kat not found:
//		return katPrefabs [0];
	}

	private void startMultiKatBattle() {
		gamestateBattleManagerModule = Instantiate (experimentalMultiKatGameStateBattleManagerPrefab) as GameObject;
		gamestateBattleManagerModule.name = "GameStateBattleManager";
		MultiKatBattleManager gamestateScript = gamestateBattleManagerModule.GetComponent<MultiKatBattleManager> ();
		gamestateScript.attachMap (0);
		gamestateScript.initialize ();
		gamestateScript.activateGameStart ();
	}

	/*
	GameObject findKatWithName(string katName){
		for (int i=0; i<katPrefabs.Length; i++){
			if (katPrefabs[i].name.Equals(katName)){
				return katPrefabs[i];
			}
		}
		return katPrefabs [0];
	}
	*/
}
