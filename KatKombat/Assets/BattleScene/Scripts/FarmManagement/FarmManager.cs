using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FarmManager : MonoBehaviour {
	[SerializeField]
	private GameObject eggPrefab;
	[SerializeField]
	private GameObject katPrefabVessel;
	private List<GameObject> spawnedKats = new List<GameObject> ();

	private AdventureManager advMngr;
	private int currentlySelectedKat = 0;
	private GameObject[] katPrefabs;
	private List<KatStatsInfo> katsInfo ;

	void Start() {
		initialize ();
	}

	void initialize() {
		katPrefabs = katPrefabVessel.GetComponent<KatPrefabsVesselScript> ().katPrefabs;

		advMngr = GameObject.Find ("AdventureModule").GetComponent<AdventureManager> ();
		katsInfo = advMngr.katsInfo;

		spawnPlayerKats ();
	}


	void Update () {
		if (GameVariables.DEBUG_MODE){
			if (Input.GetKeyDown (KeyCode.F3)) {
				Camera.main.GetComponent<FarmCameraFollowScript>().goToCameraMode(2);
			}

			if (Input.GetKeyDown (KeyCode.F11)) {
				followNextKat ();
			}

			if (Input.GetKeyDown (KeyCode.F12)) {
				Camera.main.GetComponent<FarmCameraFollowScript>().stopFollowingKat();
			}
			
		}
	}
	
	void followNextKat() {
		if (currentlySelectedKat >= spawnedKats.Count - 1)
			currentlySelectedKat = 0;
		else
			currentlySelectedKat ++;

		Camera.main.GetComponent<FarmCameraFollowScript> ().followKat (spawnedKats [currentlySelectedKat], katsInfo[currentlySelectedKat]);
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
		spawnedKat.GetComponent<StatsScript> ().setKatStatsInfo (info);
		spawnedKat.tag = "Player1";
	}
	
	GameObject findKatWithName(string katName){
		for (int i=0; i<katPrefabs.Length; i++){
			if (katPrefabs[i].name.Equals(katName)){
				return katPrefabs[i];
			}
		}
		return katPrefabs [0];
	}
	
	private void disableKatCombatComponents(GameObject kat){
		kat.transform.Find ("CanvasManaBar").gameObject.SetActive (false);
		kat.transform.Find ("CanvasHealthBar").gameObject.SetActive (false);
	}

	
	void spawnPlayerKats(){
		for (int i = 0; i<katsInfo.Count; i++) {
			spawnKatInScene(katsInfo [i]);
		}
	}

	
	public void followThisKat(GameObject kat){
		int katIndex;
		
		for (int i=0; i<spawnedKats.Count; i++) {
			if (kat == spawnedKats[i]){
				KatStatsInfo info = spawnedKats[i].GetComponent<StatsScript>().katStatInfo;
				GameObject.Find ("Main Camera").GetComponent<FarmCameraFollowScript> ().followKat (spawnedKats[i], info);
				break;
			}
		}
	}

}
