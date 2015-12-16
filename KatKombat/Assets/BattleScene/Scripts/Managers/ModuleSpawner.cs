using UnityEngine;
using System.Collections;

public class ModuleSpawner : MonoBehaviour {
	public GameObject adventureModulePrefab;
	private GameObject adventureModule;

	// Use this for initialization
	void Start () {
		adventureModule = GameObject.Find ("AdventureModule");
		if (adventureModule == null) {
			adventureModule = Instantiate(adventureModulePrefab) as GameObject;
			adventureModule.gameObject.name = "AdventureModule";
		}

		adventureModule.GetComponent<AdventureManager> ().initialize ();

		Destroy (GameObject.Find ("BattleInformationModule"));
	}



}
