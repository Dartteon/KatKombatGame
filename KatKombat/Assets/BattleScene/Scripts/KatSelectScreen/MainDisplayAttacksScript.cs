using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainDisplayAttacksScript : MonoBehaviour {
	private SpriteRenderer command1SpriteR, command2SpriteR, command3SpriteR;
	private Text command1Text, command2Text, command3Text;
	private GameObject[] commandPrefabs = new GameObject[3];

	// Use this for initialization
	void Start () {
		initializeComponents ();
	}
	void initializeComponents(){
		command1SpriteR = transform.Find ("Attack1Joystick").Find ("Icon").GetComponent<SpriteRenderer> ();
		command2SpriteR = transform.Find ("Attack2Joystick").Find ("Icon").GetComponent<SpriteRenderer> ();
		command3SpriteR = transform.Find ("Attack3Joystick").Find ("Icon").GetComponent<SpriteRenderer> ();

		command1Text = transform.Find ("Attack1Joystick").Find ("TextCanvas").Find ("Text").GetComponent<Text> ();
		command2Text = transform.Find ("Attack2Joystick").Find ("TextCanvas").Find ("Text").GetComponent<Text> ();
		command3Text = transform.Find ("Attack3Joystick").Find ("TextCanvas").Find ("Text").GetComponent<Text> ();
		
	}

	public void setEnemyCommandPrefabs(GameObject[] cmdPrefabArray){
		commandPrefabs [0] = cmdPrefabArray[0];
		commandPrefabs [1] = cmdPrefabArray[1];
		commandPrefabs [2] = cmdPrefabArray[2];
		setCommandDescriptions ();
	}
	void setCommandDescriptions(){
		transform.Find ("Attack1Joystick").GetComponent<CommandDescription> ().attachCommand (commandPrefabs [0]);
		transform.Find ("Attack2Joystick").GetComponent<CommandDescription> ().attachCommand (commandPrefabs [1]);
		transform.Find ("Attack3Joystick").GetComponent<CommandDescription> ().attachCommand (commandPrefabs [2]);
	}
	/*
	public void attachCommandSprites(SpriteRenderer cmd1SpriteR, SpriteRenderer cmd2SpriteR, SpriteRenderer cmd3SpriteR){

	}
*/
	public void attachCommandSprites(GameObject[] commandIconsArray){
		for (int i=0; i<commandIconsArray.Length; i++) {
			if (commandIconsArray[i].name == command1Text.text){
				command1SpriteR.sprite = commandIconsArray[i].GetComponent<SpriteRenderer>().sprite;
			}
			else if (commandIconsArray[i].name == command2Text.text){
				command2SpriteR.sprite = commandIconsArray[i].GetComponent<SpriteRenderer>().sprite;
			}
			else if (commandIconsArray[i].name == command3Text.text){
				command3SpriteR.sprite = commandIconsArray[i].GetComponent<SpriteRenderer>().sprite;
			}
		}
	}

	public void attachCommandTexts(string cmd1Text, string cmd2Text, string cmd3Text){
		initializeComponents ();
		command1Text.text = cmd1Text;
		command2Text.text = cmd2Text;
		command3Text.text = cmd3Text;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
