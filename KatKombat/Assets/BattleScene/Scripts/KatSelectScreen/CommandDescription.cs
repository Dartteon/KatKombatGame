using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class CommandDescription : MonoBehaviour {
	GameObject attachedCommand;
	GameObject commandDescription;
	private bool isClicked = false;
	public bool isKatSelect = true;
	
	public void attachCommand(GameObject cmd){
		this.attachedCommand = cmd;
		if (attachedCommand != null) {
			setCommandDescription ();
		}
	}

	void setCommandDescription (){
		commandDescription = this.transform.Find ("CommandDescription").gameObject;
		commandDescription.transform.Find ("DescriptionCanvas").Find ("Text").GetComponent<Text> ().text = attachedCommand.GetComponent<GeneralProjectileScript> ().commandDescription;
		if (isKatSelect) {
			Vector2 centrePos = new Vector2 (0, 0);
			Vector2 currPos = this.transform.position;
			Vector2 midPoint = (currPos - centrePos) / 2;
			commandDescription.transform.position = midPoint;
		}
	}

	void OnMouseDown(){
		isClicked = true;
		commandDescription.SetActive (true);
	}
	
	void OnMouseUp(){
		commandDescription.SetActive (false);
	}
}