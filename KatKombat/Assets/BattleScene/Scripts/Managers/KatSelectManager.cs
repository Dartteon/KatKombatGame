using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class KatSelectManager : MonoBehaviour {
	public GameObject[] katPrefabs;
	public GameObject[] commandPrefabs;
	public GameObject[] commandIcons;
	public GameObject katButtonPrefab;
	public GameObject commandButtonPrefab;

	public GameObject buttonCentreKat;
	public GameObject buttonCentreCommand;
	public GameObject mainKatFace;
	public GameObject mainKatType;
	public GameObject command1Button, command2Button, command3Button;
	public GameObject[] katTrails;
	public int battleMode = 0;
	public int numEnemyKats = 0;
	public int MAXCOLSKAT = 6;
	public int MAXCOLSCOMMAND = 6;

	public int currentSelectedCommand = 1;

	public string command1, command2, command3, katName = "";

	private float lastSelectionTime = -1.0f;
	private bool canSelect = true;
	public float selectionCooldown = 1.0f;
	public float katButtonWidthSpacing = 1.0f;
	public float katButtonHeightSpacing = 0.8f;

	public int currentLevel = 0;
	public int highScore = 0;

	public GameObject streakButton;


	void Awake(){
		
//		Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER","yes");
	}
	void Start () {

		loadSaveFile ();
		DontDestroyOnLoad(transform.gameObject);
		spawnKatButtons ();
		spawnCommandButtons ();
		Time.timeScale = 1.0f;
		setCamera ();
	}
	void setCamera(){
		float s_baseOrthographicSize = 5.0f;
		float ratio = ((float)Screen.height / (float)Screen.width);
		float discrepancy = Mathf.Abs ((ratio - (4.0f / 3.0f))) * 3.0f;
//		Debug.Log (discrepancy);
		Camera.main.orthographicSize = s_baseOrthographicSize + discrepancy;
	}

	public void setLastSelectTime(float time){
		lastSelectionTime = time;
		canSelect = false;
	}

	public void increaseLevel(){
		currentLevel++;
	}
	public int getCurrentLevel(){
		return this.currentLevel;
	}
	void Update(){
		if (Time.time - lastSelectionTime >= selectionCooldown && !canSelect) {
			canSelect = true;
		}
	}

	public bool getCanSelect(){
		return canSelect;
	}


	public void setKatName(string name){
		this.katName = name;
	}

	public void loadSaveFile(){
		if (File.Exists(Application.persistentDataPath + "/playerinfo.dat")){
			BinaryFormatter bf = new BinaryFormatter();
			
//			Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER","yes");
			FileStream file = File.Open(Application.persistentDataPath + "/playerinfo.dat", FileMode.Open);
//			Debug.Log(file.Name);
			PlayerData data = (PlayerData)bf.Deserialize(file);
			file.Close();

			
			highScore = data.highScore;
			streakButton.transform.Find("ScoreNumber").GetComponentInChildren<Text>().text = highScore.ToString();
		}
	}

	public bool setCommandName(string name){
		if (checkIfAttackEquipped (name)) {
			switch (currentSelectedCommand) {
			case 1:
				command1 = name;
				command1Button.GetComponentInChildren<Text> ().text = name;
				return true;
			case 2:
				command2 = name;
				command2Button.GetComponentInChildren<Text> ().text = name;
				return true;
			default:
				command3 = name;
				command3Button.GetComponentInChildren<Text> ().text = name;
				return true;
			}
		}

		return false;
	}

	public bool checkIfAttackEquipped(string name){
		if (command1 == name || command2 == name || command3 == name)
			return false;
		else
			return true;
	}

	public void setCommandDisplay(SpriteRenderer spriteR){
		switch (currentSelectedCommand) {
		case 1:
			command1Button.GetComponentInChildren<SpriteRenderer>().sprite = spriteR.sprite;
			break;
		case 2:
			command2Button.GetComponentInChildren<SpriteRenderer>().sprite = spriteR.sprite;
			break;
		default:
			command3Button.GetComponentInChildren<SpriteRenderer>().sprite = spriteR.sprite;
			break;
		}

		currentSelectedCommand++;
		if (currentSelectedCommand > 3) {
			currentSelectedCommand = 1;
		}
	}

	void spawnKatButtons(){
		int currentColPos = 0, currentRowPos = 0;		
		for (int i=0; i<katPrefabs.Length; i++){
			float xCoordOffset = (currentColPos % MAXCOLSKAT) * katButtonWidthSpacing;
			float yCoordOffset = (currentColPos / MAXCOLSKAT) * -katButtonHeightSpacing;
			Vector3 newButtonLocation = new Vector3 (buttonCentreKat.transform.position.x + xCoordOffset, buttonCentreKat.transform.position.y + yCoordOffset, 0);
			GameObject katButton = Instantiate(katButtonPrefab, newButtonLocation, buttonCentreKat.transform.rotation) as GameObject;
			katButton.GetComponent<DisplayKatFace>().attachKat(katPrefabs[i]);
			katButton.GetComponent<DisplayKatFace>().setKatFaceDisplay(mainKatFace);
			katButton.GetComponent<DisplayKatFace>().setKatTypeDisplay(mainKatType);
			katButton.GetComponent<DisplayKatFace>().setManager(this.transform.GetComponent<KatSelectManager>());
			currentColPos ++;
			currentRowPos += i%MAXCOLSKAT;
		}
	}


	void spawnCommandButtons(){
		int currentColPos = 0;		
		for (int i=0; i<commandPrefabs.Length; i++) {			
			float xCoordOffset = (currentColPos % MAXCOLSCOMMAND) * 1.0f;
			float yCoordOffset = (currentColPos / MAXCOLSCOMMAND) * -1.0f;			
			Vector3 newButtonLocation = new Vector3 (buttonCentreCommand.transform.position.x + xCoordOffset, buttonCentreCommand.transform.position.y + yCoordOffset, 0);
			GameObject commandButton = Instantiate(commandButtonPrefab, newButtonLocation, buttonCentreKat.transform.rotation) as GameObject;
			if (commandPrefabs[i] != null){
			commandButton.GetComponent<DisplayCommand>().attachCommand(commandPrefabs[i]);
			SpriteRenderer spriteR = findCorrespondingCommandSprite(commandPrefabs[i].name);
			commandButton.GetComponent<DisplayCommand>().setCommandDisplay(spriteR);
			commandButton.GetComponent<DisplayCommand>().setManagerScript(this.transform.GetComponent<KatSelectManager>());
			}
			currentColPos ++;
		}
	}

	SpriteRenderer findCorrespondingCommandSprite(string name){
		for (int i = 0; i<commandIcons.Length; i++){
			if (commandIcons[i].name.Equals(name)){
				return commandIcons[i].GetComponent<SpriteRenderer>();
			}
		}

		return commandIcons [0].GetComponent<SpriteRenderer> ();
	}

	public void attachTrailToKat(GameObject kat, int katType){
		katTrails[katType].GetComponent<KatSelectTrail>().attachKat(kat);
		katTrails [katType].transform.position = kat.transform.position;
		katTrails[katType].SetActive (true);
	}

	
}
