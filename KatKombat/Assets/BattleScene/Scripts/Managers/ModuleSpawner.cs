using UnityEngine;
using UnityEngine.UI;
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
//		adventureModule.initialize ();
//		Debug.Log ("farM set in advM");
		searchAndDestroyOtherModules ();
		adventureModule.GetComponent<AdventureManager> ().attempToLoadFile ();

		farmManagerModule.GetComponent<FarmManager> ().spawnPlayerEggs ();
		farmManagerModule.GetComponent<FarmManager> ().spawnPlayerKats ();
		farmManagerModule.GetComponent<FarmManager> ().setKatButtons ();
		farmManagerModule.GetComponent<FarmManager> ().setTournamentScreen ();

		adventureModule.GetComponent<AdventureManager> ().reflectPlayerCurrency ();
		adventureModule.GetComponent<AdventureManager> ().initialize ();
	//	Camera.main.transform.Find("MarketplaceButton").transform.Find("KashBox").transform.Find("Text").GetComponent<Text>().text = 
	}

	void createAdventureManagerModule() {
		adventureModule = GameObject.Find ("AdventureModule");
		if (adventureModule == null) {
//			Debug.Log("First time opening");
			adventureModule = Instantiate(adventureModulePrefab) as GameObject;
			adventureModule.gameObject.name = "AdventureModule";
		}
//		Camera.main.transform.Find ("MarketplaceButton").transform.Find ("KashBox").transform.Find ("TextCanvas").transform.Find ("Text").GetComponent<Text> ().text = 
//			adventureModule.GetComponent<AdventureManager> ().getCurrencyAmount ().ToString();
	}

	void createFarmModule() {
		farmManagerModule = Instantiate (farmManagerModulePrefab) as GameObject;
		farmManagerModule.GetComponent<FarmManager> ().initialize (adventureModule.GetComponent<AdventureManager>());
		farmManagerModule.name = "FarmManagerModule";
	}

	void searchAndDestroyOtherModules() {
		Destroy (GameObject.Find ("BattleInformationModule"));
		Destroy (GameObject.Find ("TournamentManagerModule"));
	}

}
