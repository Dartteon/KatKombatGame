using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FarmManager : MonoBehaviour {
	[SerializeField]
	private GameObject eggPrefab;
	[SerializeField]
	private GameObject katPrefabVessel;
//	[SerializeField]
//	private GameObject katClickableEnablerObjectPrefab;

	private List<GameObject> spawnedKats = new List<GameObject> ();

	private AdventureManager advMngr;
	private int currentlySelectedKat = 0;
	private GameObject[] katPrefabs;
	public List<KatStatsInfo> katsInfo { get; private set; }

	public void initialize(AdventureManager mngr) {
		advMngr = mngr;
		katPrefabs = katPrefabVessel.GetComponent<KatPrefabsVesselScript> ().katPrefabs;

		katsInfo = advMngr.katsInfo;

		spawnPlayerKats ();

		setKatButtons ();

		setTournamentScreen ();
	}

	void setKatButtons(){
		string[] katNames = new string[6];
		for (int i=0; i<katsInfo.Count; i++) {
			katNames[i] = katsInfo[i].getName();
		}
		Camera.main.GetComponent<FarmCameraFollowScript> ().enableKatButtons (spawnedKats, this, advMngr, katNames);
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
		if (currentlySelectedKat >= spawnedKats.Count - 1) {
//			Debug.Log("Array over" + currentlySelectedKat);
			currentlySelectedKat = 0;
		}
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
//		GameObject clickEnabler = Instantiate (katClickableEnablerObjectPrefab, spawnedKat.transform.position, spawnedKat.transform.rotation) as GameObject;
//		clickEnabler.transform.parent = spawnedKat.transform;
		spawnedKat.GetComponent<StatsScript> ().setKatStatsInfo (info);
		spawnedKat.tag = "Player1";
//		Debug.Log ("new kat added to scene, total kats : " + katsInfo.Count);
	}
	
	GameObject findKatWithName(string katName){
		katPrefabs = katPrefabVessel.GetComponent<KatPrefabsVesselScript> ().katPrefabs;
		for (int i=0; i<katPrefabs.Length; i++){
			if (katPrefabs[i].name.Equals(katName)){
				return katPrefabs[i];
			}
		}
		Debug.Log (katPrefabs.Length);
		return katPrefabs [0];
	}
	
	private void disableKatCombatComponents(GameObject kat){
		kat.transform.Find ("CanvasManaBar").gameObject.SetActive (false);
		kat.transform.Find ("CanvasHealthBar").gameObject.SetActive (false);
	}

	
	void spawnPlayerKats(){
//		Debug.Log (katsInfo.Count);
		for (int i = 0; i<katsInfo.Count; i++) {
//			Debug.Log("attempting to spawn " + i);
			spawnKatInScene(katsInfo [i]);
		}
	}

	
	public void followThisKat(GameObject kat){
		int katIndex;
		
		for (int i=0; i<spawnedKats.Count; i++) {
			if (kat == spawnedKats[i]){
				KatStatsInfo info = spawnedKats[i].GetComponent<StatsScript>().katStatInfo;
				Camera.main.GetComponent<FarmCameraFollowScript> ().followKat (spawnedKats[i], info);
				break;
			}
		}
	}

	void setTournamentScreen() {
//		Sprite[] faceSprites = new Sprite[spawnedKats.Count];
		GameObject katFaceOptionsBar = Camera.main.transform.Find ("TournamentButton").transform.Find ("KatChooser").transform.Find ("OptionsBar").gameObject;
//		katFaceOptionsBar.GetComponent<OptionsChooser> ().initiate ();
		for (int i = 0; i < spawnedKats.Count; i++) {
//			faceSprites[i] = spawnedKats[i].transform.Find("Sprite").transform.Find("Head").GetComponent<SpriteRenderer>().sprite;
			Sprite sprite = spawnedKats[i].transform.Find("Sprite").transform.Find("Head").GetComponent<SpriteRenderer>().sprite;
			string katFaceObjName = "Kat" + i;
			katFaceOptionsBar.transform.Find(katFaceObjName).transform.Find("FaceSprite").GetComponent<SpriteRenderer>().sprite = sprite;
			katFaceOptionsBar.transform.Find(katFaceObjName).gameObject.SetActive(true);
			katFaceOptionsBar.transform.Find(katFaceObjName).gameObject.name = KatManipulator.getKatShortDescription(katsInfo[i]);
		}
//		katFaceOptionsBar.GetComponent<OptionsChooser>().reflectDescriptionText();

		Camera.main.transform.Find ("TournamentButton").GetComponent<TournamentCreaterScreen> ().initiate (this);
//		this.transform.Find("TournamentButton").GetComponent<TournamentCreaterScreen>().
	}

}
