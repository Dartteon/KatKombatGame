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
	private List<GameObject> spawnedEggs = new List<GameObject> ();

	private AdventureManager advMngr;
	private int currentlySelectedKat = 0;
	private GameObject[] katPrefabs;
	public List<KatStatsInfo> katsInfo { get; private set; }
	public List<EggInfo> eggsInfo { get; private set; }

	public void initialize(AdventureManager mngr) {
		advMngr = mngr;
		katPrefabs = katPrefabVessel.GetComponent<KatPrefabsVesselScript> ().katPrefabs;

		katsInfo = advMngr.katsInfo;
		eggsInfo = advMngr.eggs;

//		Debug.Log (advMngr.ToString ());
//		Debug.Log (katsInfo.Count + " kats " + eggsInfo.Count + " eggs");
//		spawnPlayerKats ();

//		spawnPlayerEggs ();

//		setKatButtons ();

//		setTournamentScreen ();
	}

	public void setKatButtons(){
		string[] katNames = new string[6];
		for (int i=0; i<katsInfo.Count; i++) {
			katNames[i] = katsInfo[i].getName();
		}
		Camera.main.GetComponent<FarmCameraFollowScript> ().enableKatButtons (spawnedKats, this, advMngr, katNames);

		string[] eggNames = new string[6];
		for (int i = 0; i<eggsInfo.Count; i++) {
			eggNames[i] = "Egg " + i;
		}
//		Debug.Log (spawnedEggs [0].ToString ());
		Camera.main.GetComponent<FarmCameraFollowScript> ().enableEggButtons (spawnedEggs, this, advMngr, eggNames);
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

	public GameObject spawnKatInScene(KatStatsInfo info){
		Vector2 randomPos = new Vector2 (Random.Range(0.0f, 2.0f), Random.Range(0.0f, 2.0f));
		Quaternion randomRot = BaronVec.randomRotation ();
	//	Debug.Log (randomRot.ToString ());
		GameObject spawnedKat = Instantiate (findKatWithName (info.breed), randomPos, randomRot) as GameObject; 
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
		spawnedKat.AddComponent<FarmClickKatScript> ();
		/*
		CircleCollider2D newCollider = new CircleCollider2D ();
		newCollider.isTrigger = true;
		newCollider.radius = 1.0f;
		spawnedKat.AddComponent (newCollider);
		*/
//		Debug.Log ("Spawning kat " + info.toString ());
		return spawnedKat;
//		Debug.Log ("new kat added to scene, total kats : " + katsInfo.Count);
	}
	public GameObject spawnKatInScene(KatStatsInfo info, Vector2 loc){
		GameObject spawnedKat = Instantiate (findKatWithName (info.breed), loc, transform.rotation) as GameObject; 
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
		Debug.Log ("Spawning kat " + info.toString ());
		return spawnedKat;
		//		Debug.Log ("new kat added to scene, total kats : " + katsInfo.Count);
	}
	
	GameObject findKatWithName(KatBreed.Breed breed){
		katPrefabs = katPrefabVessel.GetComponent<KatPrefabsVesselScript> ().katPrefabs;
		for (int i=0; i<katPrefabs.Length; i++){
		//	if (katPrefabs[i].name.Equals(katName)){
		//		return katPrefabs[i];
		//	}
			if (katPrefabs[i].GetComponent<StatsScript>().breed == breed)
				return katPrefabs[i];
		}
//		Debug.Log (katPrefabs.Length);
		return katPrefabs [0];
	}
	
	private void disableKatCombatComponents(GameObject kat){
		kat.transform.Find ("CanvasManaBar").gameObject.SetActive (false);
		kat.transform.Find ("CanvasHealthBar").gameObject.SetActive (false);
	}

	
	public void spawnPlayerKats(){
//		Debug.Log (katsInfo.Count);
		katsInfo = advMngr.katsInfo;
//		Debug.Log (katsInfo.Count);

		for (int i = 0; i<katsInfo.Count; i++) {
//			Debug.Log("attempting to spawn " + i);
			spawnKatInScene(katsInfo [i]);
		}
	}

	public GameObject hatchAndSpawnEgg (GameObject egg) {
		int eggIndex = spawnedEggs.IndexOf (egg);
		KatStatsInfo babyKat = advMngr.setEggToHatched (eggsInfo [eggIndex]);
//		spawnKatInScene (babyKat);
		egg.SetActive (false);
		GameObject babyObj = spawnKatInScene (babyKat, egg.transform.position);
		Destroy (babyObj.GetComponent<NonCombatBehavior> ());
		followThisKat (babyObj);
		return (babyObj);
	}

	public void spawnPlayerEggs() {
		eggsInfo = advMngr.eggs;

		for (int i = 0; i < eggsInfo.Count; i++) {
			spawnEgg(eggsInfo[i]);
			//		spawnedEggs.Add(Instantiate(eggPrefab));
		}
	}
	public void spawnEgg(EggInfo eggInfo) {
		Vector3 randomLoc = new Vector3 (Random.Range (-2f, 2f), Random.Range (-2f, 2f), -.1f);
		GameObject newEgg = Instantiate (eggPrefab, randomLoc, this.transform.rotation) as GameObject;
		newEgg.GetComponent<EggTypeMatcher> ().matchSpriteToEggType (eggInfo.getEggType ());
		KatBreed.EggType eggType = eggInfo.getEggType ();
		newEgg.name = "Egg";
		spawnedEggs.Add (newEgg);
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

	public void followThisEgg(GameObject egg) {
		int eggIndex;

		eggIndex = spawnedEggs.IndexOf (egg);
		EggInfo info = eggsInfo [eggIndex];
		Camera.main.GetComponent<FarmCameraFollowScript> ().followEgg (eggsInfo[eggIndex], egg);
	}

	public void setTournamentScreen() {
//		Sprite[] faceSprites = new Sprite[spawnedKats.Count];
		GameObject katFaceOptionsBar = Camera.main.transform.Find ("TournamentButton").transform.Find ("KatChooser").transform.Find ("OptionsBar").gameObject;
//		katFaceOptionsBar.GetComponent<OptionsChooser> ().initiate ();
		int index = 0;

		for (int i = 0; i < spawnedKats.Count; i++) {
//			faceSprites[i] = spawnedKats[i].transform.Find("Sprite").transform.Find("Head").GetComponent<SpriteRenderer>().sprite;
			Sprite sprite = spawnedKats[i].transform.Find("Sprite").transform.Find("Head").GetComponent<SpriteRenderer>().sprite;
			string katFaceObjName = "Kat" + i;
			katFaceOptionsBar.transform.Find(katFaceObjName).transform.Find("FaceSprite").GetComponent<SpriteRenderer>().sprite = sprite;
			katFaceOptionsBar.transform.Find(katFaceObjName).gameObject.SetActive(true);

			//To set tournament Kat names
			katFaceOptionsBar.transform.Find(katFaceObjName).gameObject.name = KatManipulator.getKatShortDescription(katsInfo[i]); 
			index++;
		}

		//fix this ---------------------------------------------------------------------------------------------------------
		/*
		for (int i = 0; i < eggsInfo.Count + index; i++) {
			Sprite sprite = spawnedEggs[i].transform.Find("EggSprite").GetComponent<SpriteRenderer>().sprite;
			
			Debug.Log (sprite.ToString());
			string katFaceObjName = "Kat" + i;
			katFaceOptionsBar.transform.Find(katFaceObjName).transform.Find("FaceSprite").GetComponent<SpriteRenderer>().sprite = sprite;
			katFaceOptionsBar.transform.Find(katFaceObjName).gameObject.SetActive(true);
//			katFaceOptionsBar.transform.Find(katFaceObjName).gameObject.name = KatManipulator.getKatShortDescription(katsInfo[i]);
			index++;
		}
*/
//		katFaceOptionsBar.GetComponent<OptionsChooser>().reflectDescriptionText();

		Camera.main.transform.Find ("TournamentButton").GetComponent<TournamentCreaterScreen> ().initiate (this);
//		this.transform.Find("TournamentButton").GetComponent<TournamentCreaterScreen>().
	}

	public void followFirstEgg() {
		followThisEgg(spawnedEggs[0]);
	}

}
