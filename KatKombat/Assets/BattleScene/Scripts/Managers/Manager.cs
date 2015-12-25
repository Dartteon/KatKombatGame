using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Manager : MonoBehaviour {
	/*	This manager is used for battle scenes
	 * transitioned from KatSelect Screen.
	 * 
	 * It can support 2 player battles by
	 * spawning 2 controllers.
	 * 
	 * For battles transitioned from
	 * adventure scene, use BattleManager.
	 */

	public int numEnemies = 0;
	public int numAIEnemies;
	public GameObject[] kats;
	public GameObject[] spawnLocations;
	public GameObject[] controllers;
	private GameObject[] spawnedKats = new GameObject[1000];
	private GameObject[] enemyAIArray = new GameObject[1000];
	public GameObject[] maps;
	public int mapChoice;
	private int battleMode = 0;

	private GameObject mapLoc;
	public GameObject[] atkPrefabs;
	public string[] kat1Attacks;
	private int atkID;
	public GameObject[] commandIconPrefabs;
	public GameObject[] coloredTypeIcons;
	public string[] katAttackNames;
	public GameObject enemyAIModule;
	private SpriteRenderer movementControllerBackground;
	private GameObject katSelectManager;
	private KatSelectManager katSelectScript;
	public int numPlayerKats = 1;
	public int currentLevel = 2;
	public float timeScale = 1.0f;
	public GameObject victoryButton;
	public GameObject defeatButton;
	private bool gameStarted = false;
	private readonly int LEVEL_MULTIPLIER = 10, BASE_LEVEL = 70;
	public GameObject stageBeginScreen;
	private string[] enemyKatAttackNames = new string[3];
	private GameObject[] enemyKatCommandPrefabs = new GameObject[3];
	public GameObject katSelectMngrPrefab;
	public GameObject targetRingPrefab;
	private int currentKatIndex = 0;
	public GameObject controllerPrefab;
	private bool gameEnded = false;

	SpriteRenderer katSkill1SpriteR, katSkill2SpriteR, katSkill3SpriteR;
	Sprite katFaceSpriteR;
	string katBreed;
	void Awake(){
		Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER","yes");
	}
	void Start(){
		Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
		Time.timeScale = 0f;
		setAttributesFromKatSelectManager ();
		spawnControllers ();
		spawnKats ();
		initializeMap ();
		attachAIComponents ();
		gameStarted = true;
		setStageDisplay (currentLevel);
		setStageBeginScreen ();
		setCamera ();

		loadSaveFile ();
	}
	public void loadSaveFile(){
		Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
		if (File.Exists(Application.persistentDataPath + "/playerinfo.dat")){
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/playerinfo.dat", FileMode.Open);
			PlayerData data = (PlayerData)bf.Deserialize(file);
			file.Close();
//			Debug.Log("Read highscore: " + data.highScore);
			if (data.highScore < currentLevel){
				saveHighScore();
			}
		} else {
			saveHighScore();
		}
	}
	void saveHighScore(){
		Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER","yes");
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/playerinfo.dat");
		PlayerData data = new PlayerData ();
		data.highScore = (currentLevel);
		bf.Serialize (file, data);
		file.Close ();
	}
	void setCamera(){
		float s_baseOrthographicSize = 8.0f;
		float ratio = ((float)Screen.height / (float)Screen.width);
		float discrepancy = Mathf.Abs ((ratio - (4.0f / 3.0f))) * 3.0f;
		Camera.main.orthographicSize = s_baseOrthographicSize + discrepancy;

		//to attachKat
		//GameObject.Find ("Main Camera").GetComponent<CameraScript> ().attachKat (spawnedKats [0]);

	}
	void setStageDisplay(int level){
		Text display = transform.Find("UI Elements").transform.Find("StageDisplay").transform.GetComponentInChildren<Text>();
		display.text = ("Stage " + level);
	}
	void setStageBeginScreen(){
		stageBeginScreen = transform.Find ("StageBeginScreen").gameObject;
		setStageBeginScreenAttributes ();
		stageBeginScreen.SetActive (true);
	}
	void setStageBeginScreenAttributes(){
		stageBeginScreen.transform.Find ("TextCanvas").Find ("Text").GetComponent<Text> ().text = "Stage " + currentLevel;
//		Debug.Log (stageBeginScreen.transform.Find ("MainDisplay").Find ("MovementJoystick").GetComponent<MainKatDisplay> ());
		stageBeginScreen.transform.Find ("MainDisplay").Find ("MovementJoystick").GetComponent<MainKatDisplay> ().setIncomingKatObject (spawnedKats [1]);
		int katType = spawnedKats[1].transform.GetComponent<StatsScript> ().statType;
		stageBeginScreen.transform.Find ("MainDisplay").Find ("MovementJoystick").GetComponent<MainKatDisplay> () .setColorType(
			coloredTypeIcons[katType].GetComponent<SpriteRenderer>());
		stageBeginScreen.transform.Find ("MainDisplay").GetComponent<MainDisplayAttacksScript> ().attachCommandTexts (enemyKatAttackNames [0], enemyKatAttackNames [1], enemyKatAttackNames [2]);
		stageBeginScreen.transform.Find ("MainDisplay").GetComponent<MainDisplayAttacksScript> ().attachCommandSprites (commandIconPrefabs);
		stageBeginScreen.transform.Find ("MainDisplay").GetComponent<MainDisplayAttacksScript> ().setEnemyCommandPrefabs (enemyKatCommandPrefabs);
	}


	void Update(){
		if (gameStarted && !gameEnded) {
			if (spawnedKats [0]==null){
				gameStarted = false;
				GameObject defeatScreen = Instantiate(defeatButton, mapLoc.transform.position, mapLoc.transform.rotation) as GameObject;

				defeatScreen.transform.Find("Face").GetComponent<SpriteRenderer>().sprite = katFaceSpriteR;
				if (katSkill1SpriteR != null)
					defeatScreen.transform.Find("Skill1").GetComponent<SpriteRenderer>().sprite = katSkill1SpriteR.sprite;
				if (katSkill2SpriteR != null)
					defeatScreen.transform.Find("Skill2").GetComponent<SpriteRenderer>().sprite = katSkill2SpriteR.sprite;
				if (katSkill3SpriteR != null)
				defeatScreen.transform.Find("Skill3").GetComponent<SpriteRenderer>().sprite = katSkill3SpriteR.sprite;
				defeatScreen.transform.Find("Billboard").Find("KatNameTextCanvas").GetComponentInChildren<Text>().text = katBreed;
				defeatScreen.transform.Find("Billboard").Find("StageTextCanvas").GetComponentInChildren<Text>().text = "Stage " + currentLevel;
				Time.timeScale = 0.2f;
				gameEnded = true;
			} else if (spawnedKats [1]==null){
				GameObject vicButton = Instantiate(victoryButton, mapLoc.transform.position, mapLoc.transform.rotation) as GameObject;
				Time.timeScale = 0.2f;
				Text display = vicButton.transform.GetComponentInChildren<Text>();
				display.text = ("Stage " + currentLevel + " defeated!");
				gameStarted = false;
			}
		}
	}
	void setAttributesFromKatSelectManager(){
		katSelectManager = GameObject.Find ("KatSelectManager");
		if (katSelectManager != null) {
			katSelectScript = katSelectManager.GetComponent<KatSelectManager> ();
			kats = katSelectScript.katPrefabs;
			setPlayerKatCommands ();
			numEnemies = katSelectScript.numEnemyKats + 1;
			commandIconPrefabs = katSelectScript.commandIcons;
			atkPrefabs = katSelectScript.commandPrefabs;
			currentLevel = katSelectScript.getCurrentLevel ();
			currentLevel++;
			katSelectScript.increaseLevel ();
			battleMode = katSelectScript.battleMode;
		} 

	}
	void spawnControllers(){
		if (battleMode == 0) {
			GameObject controller1 = Instantiate(controllerPrefab) as GameObject;
			controller1.transform.parent = this.transform.Find("Controllers").transform;
			controllers[0] = controller1;
		} else {
			GameObject controller1 = Instantiate(controllerPrefab, new Vector2 (0,0), new Quaternion()) as GameObject;
			controller1.transform.parent = this.transform.Find("Controllers").transform;
			controllers[0] = controller1;
			GameObject controller2 = Instantiate(controllerPrefab, new Vector2 (0,0), new Quaternion()) as GameObject;
			controller2.transform.parent = this.transform.Find("Controllers").transform;
			controller2.transform.Rotate(0,0,180);
			controllers[1] = controller2;

		}
	}
	int getEnemyKatLevel(){
		return (((this.currentLevel-1) * LEVEL_MULTIPLIER) + BASE_LEVEL);
	}

	void setPlayerKatCommands(){
		if (katSelectScript != null) {
			kat1Attacks [0] = katSelectScript.command1;
			kat1Attacks [1] = katSelectScript.command2;
			kat1Attacks [2] = katSelectScript.command3;
		}
	}

	void attachAIComponents(){
		if (battleMode == 0) {
			for (int i=numPlayerKats; i<numEnemies; i++) {
				int k = 0;
				for (int j=0; j<numEnemies; j++) {
					if (i != j) {
						enemyAIArray [i].GetComponent<EnemyKatAI> ().getEnemyArray () [k] = spawnedKats [j];
						k++;
					}
				}
				k = 0;
			}
		}
	}

	void initializeMap(){		
		mapLoc = transform.Find ("MapLocation").gameObject;

		GameObject map = Instantiate(maps[mapChoice], mapLoc.transform.position, mapLoc.transform.rotation) as GameObject;

		try{
			Vector2 loc = map.GetComponent<MapConfig> ().spawnLoc;
			map.transform.position = new Vector3(loc.x, loc.y, 100);

		} catch (NullReferenceException e){
			Debug.Log("You forgot to attach MapConfig to map prefab! .. " + e);
		}

	}

	void spawnPlayerKat(){
		int katBreedIndex = 0;
		katBreed = "";
		if (katSelectManager != null && battleMode == 0) {
			katBreed = katSelectManager.GetComponent<KatSelectManager> ().katName;
		}
		if (katBreed.Equals ("")) {
			katBreed = kats[UnityEngine.Random.Range(0,kats.Length)].name;
		}
		//Find specific kat according to breed string from kat array
		for (int i=0; i<kats.Length; i++){ 
			if(katBreed.Equals(kats[i].name)){
				katBreedIndex = i;
			}
		}
		spawnedKats[currentKatIndex] = Instantiate(kats[katBreedIndex], spawnLocations[currentKatIndex].transform.position, spawnLocations[currentKatIndex].transform.rotation) as GameObject;
		attachTargetRing (currentKatIndex);
		
//		Debug.Log ("attaching kat");
		setKatProjectileLaunchers(currentKatIndex);			
//		setMovementIcon(currentKatIndex);
		tagKat (spawnedKats [currentKatIndex], currentKatIndex);

		katFaceSpriteR = spawnedKats [currentKatIndex].transform.Find ("Sprite").Find ("Head").GetComponent<SpriteRenderer> ().sprite;
		attachKatsToController(currentKatIndex);		
		currentKatIndex++;

	}

	void attachTargetRing(int index){
		GameObject targetRing = Instantiate (targetRingPrefab, spawnedKats [index].transform.position, spawnedKats [index].transform.rotation) as GameObject;
		targetRing.transform.parent = spawnedKats [index].transform;
		targetRing.GetComponent<SpriteRenderer> ().color = spawnedKats [index].GetComponent<StatsScript> ().getColor ();
	}

	void spawnEnemyKats(){
		/*
		for (int i = 1; i < numEnemies; i++) {
			if (i<4){
				spawnedKats[i] = Instantiate(kats[UnityEngine.Random.Range(0, kats.Length)], spawnLocations[i].transform.position, spawnLocations[i].transform.rotation) as GameObject;
			}else{
				int maxDistance = 5;
				spawnedKats[i] = Instantiate(kats[UnityEngine.Random.Range(0, kats.Length)], new Vector2(UnityEngine.Random.Range(-maxDistance/2, maxDistance/2), UnityEngine.Random.Range(-maxDistance, maxDistance)), spawnLocations[UnityEngine.Random.Range(0,3)].transform.rotation) as GameObject;
			}
			setEnemyKatLevel(i);
			initializeEnemyKatAI(i);
			setEnemyProjectiles(i);
			tagKat(spawnedKats[i], i);
		}
*/
		Debug.Log ("[Manager]Spawning a kat");
		spawnedKats[1] = Instantiate(kats[UnityEngine.Random.Range(0, kats.Length)], spawnLocations[1].transform.position, spawnLocations[1].transform.rotation) as GameObject;
		setEnemyKatLevel(1);
		initializeEnemyKatAI(1);
		setEnemyProjectiles(1);
		tagKat(spawnedKats[1], 1);
	}
	
	void tagKat (GameObject kat, int number){
		kat.tag = "Player" + ((number % 8)+1);
	}

	void spawnKats(){
		if (battleMode == 0) {
			spawnPlayerKat ();
			spawnEnemyKats ();
		} else if (battleMode == 1) {
			spawnPlayerKat ();
			spawnPlayerKat ();
		}
	}

	void initializeEnemyKatAI(int i){	
		GameObject enemyAI = Instantiate (enemyAIModule, spawnLocations[i].transform.position, spawnLocations[i].transform.rotation) as GameObject;
		enemyAIArray[i] = enemyAI;
		enemyAIArray[i].transform.parent = spawnedKats[i].transform;
		enemyAIArray[i].SetActive(true);
	}

	void setEnemyKatLevel(int i){
	//	spawnedKats [i].GetComponent<StatsScript> ().level = getEnemyKatLevel ();
		spawnedKats [i].GetComponent<StatsScript> ().setToLevel(getEnemyKatLevel ());
	}

	void setMovementIcon(int i){
		int katType = spawnedKats[i].transform.GetComponent<StatsScript> ().statType;
		movementControllerBackground.sprite = coloredTypeIcons[katType].GetComponent<SpriteRenderer>().sprite;
	}

	void setEnemyProjectiles(int i){
		string[] commandNames = {"stub", "stub", "stub"};
//		string[] commandNames = spawnedKats[i].GetComponent<StatsScript>().defaultCommands.Split(null);
//		Debug.Log (commandNames[0] + commandNames[1] + commandNames[2]);
		int[] atkIndexesArray = new int[3];

		try{
			if (commandNames [0] != null) {
				for (int j = 0; i<atkPrefabs.Length; j++){
					if (commandNames[0].ToLower().Equals(atkPrefabs[j].name.ToLower())){
						atkIndexesArray[0] = j;
						break;
					}
				}
			}
		}catch(IndexOutOfRangeException e){
			Debug.Log(e);
		}

		try{
			if (commandNames [1] != null) {
				for (int j = 0; i<atkPrefabs.Length; j++){
					if (commandNames[1].ToLower().Equals(atkPrefabs[j].name.ToLower())){
						atkIndexesArray[1] = j;
						break;
					}
				}
			}
		}catch(IndexOutOfRangeException e){
			Debug.Log(e);
		}

		try{
			if (commandNames [2] != null) {
				for (int j = 0; i<atkPrefabs.Length; j++){
					if (commandNames[2].ToLower().Equals(atkPrefabs[j].name.ToLower())){
						atkIndexesArray[2] = j;
						break;
					}
				}
			}
		}catch(IndexOutOfRangeException e){
			Debug.Log(e);
		}


		for (int j=0; j<3; j++){
			enemyKatAttackNames[j] = atkPrefabs[atkIndexesArray [j]].name;
			enemyKatCommandPrefabs[j] = atkPrefabs[atkIndexesArray [j]];
		}

		setProjectiles (i, atkPrefabs[atkIndexesArray [0]], atkPrefabs[atkIndexesArray [1]], atkPrefabs[atkIndexesArray [2]]);
		/*
		int[] atkIndexesArray = new int[3];
		atkIndexesArray [0] = UnityEngine.Random.Range (0, atkPrefabs.Length);
		atkIndexesArray [1] = UnityEngine.Random.Range (0, atkPrefabs.Length);
		while (atkIndexesArray [0] == atkIndexesArray [1]) {
			atkIndexesArray [1] = UnityEngine.Random.Range (0, atkPrefabs.Length);
		}
		atkIndexesArray [2] = UnityEngine.Random.Range (0, atkPrefabs.Length);
		while (atkIndexesArray [2] == atkIndexesArray [0] || atkIndexesArray [2] == atkIndexesArray [1]) {
			atkIndexesArray [2] = UnityEngine.Random.Range (0, atkPrefabs.Length);
		}
		
		setProjectiles (i, atkPrefabs[atkIndexesArray [0]], atkPrefabs[atkIndexesArray [1]], atkPrefabs[atkIndexesArray [2]]);
		
		for (int j=0; j<3; j++){
				enemyKatAttackNames[j] = atkPrefabs[atkIndexesArray [j]].name;
				enemyKatCommandPrefabs[j] = atkPrefabs[atkIndexesArray [j]];
		}
		*/
	}

	void setKatProjectileLaunchers(int i){		
		GameObject command0=null, command1=null, command2=null;
		
		for (int k = 0; k < atkPrefabs.Length; k++){ //loop through entire attack prefabs array
			string spellName = atkPrefabs[k].name;
			if (spellName == kat1Attacks[0])
				command0 = atkPrefabs[k];
			else if (spellName == kat1Attacks[1])
				command1 = atkPrefabs[k];
			else if (spellName == kat1Attacks[2])
				command2 = atkPrefabs[k];
		}


		GameObject icon0 = null, icon1 = null, icon2 = null;

		//Loop through command prefabs to see if spells of specified names exist
		for (int k = 0; k < commandIconPrefabs.Length; k++){
			string spellName = commandIconPrefabs[k].name;

			if (spellName == kat1Attacks[0])
				icon0 = commandIconPrefabs[k];
			else if (spellName == kat1Attacks[1])
				icon1 = commandIconPrefabs[k];
			else if (spellName == kat1Attacks[2])
				icon2 = commandIconPrefabs[k];
		}

		// i is katIndex in the spawnedKat array
		setProjectiles (i, command0, command1, command2);

		if (battleMode == 1 || i < 1) {
//justremoved might need
//			setIcons (i, icon0, icon1, icon2);
		}
	}

	void setIcons(int i, GameObject icon0, GameObject icon1, GameObject icon2){
		if (kat1Attacks[0] != ""){
			if (icon0 != null){
				controllers[i].transform.Find("Attack1Joystick").Find("Icon").GetComponent<SpriteRenderer>().sprite = icon0.GetComponent<SpriteRenderer>().sprite;
				katSkill1SpriteR = icon0.GetComponent<SpriteRenderer>();
			}
		}

		if(kat1Attacks[1] != ""){
			if (icon1 != null){
				controllers[i].transform.Find("Attack2Joystick").Find("Icon").GetComponent<SpriteRenderer>().sprite = icon1.GetComponent<SpriteRenderer>().sprite;
				katSkill2SpriteR = icon1.GetComponent<SpriteRenderer>();
			}
		}

		if(kat1Attacks[2] != ""){
			if (icon2 != null){
				controllers[i].transform.Find("Attack3Joystick").Find("Icon").GetComponent<SpriteRenderer>().sprite = icon2.GetComponent<SpriteRenderer>().sprite;
				katSkill3SpriteR = icon2.GetComponent<SpriteRenderer>();
			}
		}
	}

	void setProjectiles(int i, GameObject command0, GameObject command1, GameObject command2){
		if(kat1Attacks[0]=="" && i<1){
			controllers[i].transform.Find("FourCommandButtons").transform.Find("Attack1Joystick").gameObject.SetActive(false);
		}
		else{
			spawnedKats[i].transform.Find("BasicAttack").gameObject.GetComponent<KatProjectileLauncherScript>().SetPrefab(command0);
			spawnedKats[i].transform.Find("BasicAttack").gameObject.SetActive(true);
		}
		if(kat1Attacks[1]=="" && i<1){
			controllers[i].transform.Find("FourCommandButtons").transform.Find("Attack2Joystick").gameObject.SetActive(false);
		}
		else{
			spawnedKats[i].transform.Find("SecondAttack").gameObject.GetComponent<KatProjectileLauncherScript>().SetPrefab(command1);
			spawnedKats[i].transform.Find("SecondAttack").gameObject.SetActive(true);
		}
		if(kat1Attacks[2]=="" && i<1){
			controllers[i].transform.Find("FourCommandButtons").transform.Find("Attack3Joystick").gameObject.SetActive(false);
		}
		else{
			spawnedKats[i].transform.Find("ThirdAttack").gameObject.GetComponent<KatProjectileLauncherScript>().SetPrefab(command2);
			spawnedKats[i].transform.Find("ThirdAttack").gameObject.SetActive(true);
		}
	}

	void attachKatsToController(int i){
			movementControllerBackground = controllers [i].transform.Find ("MovementJoystick").transform.Find ("Circle").GetComponent<SpriteRenderer> ();
			//gotta fix these two below; only the first should be needed
			//MainControllerScript controlScript = controllers [i].GetComponent<MainControllerScript> ();
			//controlScript.attachedKat = spawnedKats [i];
			controllers [i].SendMessage ("AttachKat", spawnedKats [i]);

	}
	
}