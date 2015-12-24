using UnityEngine;
using System.Collections;

public class DefeatButton : MonoBehaviour {
	bool isClicked = false;
	public string levelName;

	[SerializeField]
	private bool reloadSameScene = false;
	
	void OnMouseDown(){
		isClicked = true;
	}
	void OnMouseUp(){
		if (isClicked) {
			GameObject.Find("AdventureModule").GetComponent<AdventureManager>().savePlayerFile();
			if (reloadSameScene) Application.LoadLevel (Application.loadedLevel);

			else {
				GameObject katSelectManager = GameObject.Find("KatSelectManager");
				Destroy(katSelectManager);
				Application.LoadLevel (levelName);
			}
		}
	}
}
