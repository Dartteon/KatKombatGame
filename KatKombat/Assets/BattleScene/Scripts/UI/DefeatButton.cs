using UnityEngine;
using System.Collections;

public class DefeatButton : MonoBehaviour {
	bool isClicked = false;
	public string levelName;
	
	void OnMouseDown(){
		isClicked = true;
	}
	void OnMouseUp(){
		if (isClicked) {
			GameObject katSelectManager = GameObject.Find("KatSelectManager");
			Destroy(katSelectManager);
			Application.LoadLevel (levelName);
		}
	}
}
