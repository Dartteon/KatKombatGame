using UnityEngine;
using System.Collections;

public class ModuleSpawner : MonoBehaviour {
	[SerializeField]
	private GameObject adventureModulePrefab;
	private GameObject adventureModule;
	[SerializeField]
	private GameObject farmManagerModule;

	// Use this for initialization
	void Start () {
		createAdventureManagerModule ();
		createFarmModule ();
		searchAndDestroyBattleInfoModule ();
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
		GameObject farmModule = Instantiate (farmManagerModule) as GameObject;
		farmModule.name = "FarmManagerModule";
	}

	void searchAndDestroyBattleInfoModule() {
		Destroy (GameObject.Find ("BattleInformationModule"));
	}

}
