﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour {
	/* This manager is used for battle scenes transitioned from AdventureScene.
	 * It will attempt to find player's Kat (GameObject) and enemy's Kat (GameObject)
	 * 
	 * For battle scenes transitioned from KatSelect, you might want to use
	 * Manager class instead of this one.
	 */

	private List<GameObject> playerKats = new List<GameObject>();
	private List<GameObject> enemyKats = new List<GameObject>();
	private List<GameObject> AIModules = new List<GameObject> ();
	private GameObject[] katPrefabs;

	public GameObject controllerPrefab;
	public GameObject[] mapPrefabs;
	public GameObject gamestateBattleManagerModulePrefab;
	public GameObject katAttackVesselPrefab;

	private int mapIndex;
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
			katPrefabs = battleInfoScript.getKatPrefabs();

			initialize(playerKat, enemykat, mapIndex);
		} else {
			Debug.Log("BattleInformation not found!");
		}
	}

	GameObject searchKat(KatStatsInfo katInfo){
		string katBreed = katInfo.getBreed ();

		for (int i=0; i<katPrefabs.Length; i++){
			if (katBreed.Equals(katPrefabs[i].name)){
				return katPrefabs[i];
			}
		}

		//If kat not found:
		return katPrefabs [0];
	}

	GameObject findKatWithName(string katName){
		for (int i=0; i<katPrefabs.Length; i++){
			if (katPrefabs[i].name.Equals(katName)){
				return katPrefabs[i];
			}
		}
		return katPrefabs [0];
	}
}
