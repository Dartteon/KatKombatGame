﻿using UnityEngine;
using System.Collections;

public class ModuleSpawner : MonoBehaviour {
	[SerializeField]
	private GameObject adventureModulePrefab;
	[SerializeField]
	private GameObject farmManagerModulePrefab;

	private GameObject adventureModule;
	private GameObject farmManagerModule;

	// Use this for initialization
	void Start () {
		Screen.orientation = ScreenOrientation.LandscapeLeft;
//		Debug.Log (Screen.orientation.ToString ());
		createAdventureManagerModule ();
		createFarmModule ();
		adventureModule.GetComponent<AdventureManager>().setFarmManager (farmManagerModule.GetComponent<FarmManager> ());
//		Debug.Log ("farM set in advM");
		searchAndDestroyOtherModules ();
	}

	void createAdventureManagerModule() {
		adventureModule = GameObject.Find ("AdventureModule");
		if (adventureModule == null) {
			adventureModule = Instantiate(adventureModulePrefab) as GameObject;
			adventureModule.gameObject.name = "AdventureModule";
		}
		
		adventureModule.GetComponent<AdventureManager> ().initialize ();
	}

	void createFarmModule() {
		farmManagerModule = Instantiate (farmManagerModulePrefab) as GameObject;
		farmManagerModule.GetComponent<FarmManager> ().initialize (adventureModule.GetComponent<AdventureManager>());
		farmManagerModule.name = "FarmManagerModule";
	}

	void searchAndDestroyOtherModules() {
		Destroy (GameObject.Find ("BattleInformationModule"));
//		Destroy (GameObject.Find ("TournamentManagerModule"));
	}

}
