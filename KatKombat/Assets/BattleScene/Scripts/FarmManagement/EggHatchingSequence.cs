using UnityEngine;
using System.Collections;

public class EggHatchingSequence : MonoBehaviour, Tappable {
	/*
	private GameObject hatchScreen;

	void Start() {
		hatchScreen = this.transform.Find ("HatchScreen").gameObject;
	}
*/
	public void handleTap(Vector2 pos1, Vector2 pos2) {
		execute ();
	}

	void OnMouseDown() {
		execute ();
	}

	void execute(){
//		Debug.Log ("Egg clicked");
//		Camera.main.GetComponent<FarmCameraFollowScript> ().followEgg (this.gameObject.GetComponent<EggInfo>() ,this.gameObject);
		GameObject.Find ("FarmManagerModule").GetComponent<FarmManager> ().followThisEgg (this.gameObject);
//		hatchScreen.SetActive (true);
	}
}
