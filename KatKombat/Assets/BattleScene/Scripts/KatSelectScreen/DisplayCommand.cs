using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayCommand : MonoBehaviour {
	
	SpriteRenderer commandDisplay;
	GameObject attachedCommand;
	KatSelectManager managerScript;
	private bool isClicked = false;

	private string commandName;

	public void setManagerScript(KatSelectManager script){
		managerScript = script;
	}
	
	public void attachCommand(GameObject cmd){
		this.attachedCommand = cmd;
		if (attachedCommand != null) {
			setButtonCommandName ();
			setButtonCommandText ();
			findDescriptionScriptAndAttach();
		}
	}
	void findDescriptionScriptAndAttach(){
		this.transform.GetComponent<CommandDescription> ().attachCommand (attachedCommand);
	}

	public void setCommandDisplay(SpriteRenderer spriteR){
		transform.GetComponentInChildren<SpriteRenderer> ().sprite
			= spriteR.sprite;
	}

	void setButtonCommandName (){
		commandName = attachedCommand.name;
	}
	void OnMouseDown(){
		isClicked = true;
	}
	
	void OnMouseUp(){
		Vector2 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector2 currentPos = this.transform.position;
		Vector2 distanceVec = (currentPos - mousePos);
		float absDistance = distanceVec.magnitude;
		if (isClicked && absDistance <= 0.4f) {;
			if (managerScript.setCommandName(attachedCommand.name)){
				setMainCommandDisplay();
			}
		}
	}

	void setMainCommandName(){
		managerScript.setCommandName (attachedCommand.name);
	}

	void setMainCommandDisplay(){
		SpriteRenderer spriteR = transform.GetComponentInChildren<SpriteRenderer> ();
		managerScript.setCommandDisplay (spriteR);
	}

	void setButtonCommandText(){
		transform.GetComponentInChildren<Text> ().text = attachedCommand.name;
	}
}
