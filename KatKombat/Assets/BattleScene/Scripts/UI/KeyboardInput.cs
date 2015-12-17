using UnityEngine;
using System.Collections;

public class KeyboardInput : MonoBehaviour {
	
	TouchScreenKeyboard keyboard;
	KeyboardRequester requester;
	bool isActivated = false;

	public void initialize(KeyboardRequester req) {
		Debug.Log ("Keyboard init");
//		keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false, false, "Enter Name");
//		Debug.Log (keyboard.ToString ());
		requester = req;
		isActivated = true;
	}

	/*
	void Update() {
		if (isActivated) {
			Debug.Log (keyboard.active);
		}
	}*/

	void Update() {
		if (isActivated) {
			keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false, false, "Enter Name");
			requester.handleInput (keyboard.text);
			Debug.Log(keyboard.active);

			if(keyboard.done){
//				isActivated = false;
//				requester.handleEnter();
			
			}
		}
	}

}
