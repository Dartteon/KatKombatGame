using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HatchEggButton : MonoBehaviour, Tappable {
	GameObject egg;
	private bool isHatching = false;
	private float startHatchTime;
	private float hatchDuration = 3.0f;

//	private float hatchedTime;
//	private bool hasHatched;

	FarmCameraFollowScript camScript;


	public void handleTap (Vector2 pos1, Vector2 pos2) {
		execute ();
	}

	void OnMouseDown() {

//		Debug.Log ("hatch clicked");
		execute ();
	}

	void execute() {
		if (!isHatching) {
			egg = this.transform.parent.GetComponent<KatDataCard> ().objectBeingFollowed;
			egg.GetComponent<Animator> ().Play ("Hatching");
			isHatching = true;
			startHatchTime = Time.time;
			this.transform.Find("HatchText").transform.Find("Text").GetComponent<Text>().text = "Hatching!";
		}
	}

	void Update() {
		if (isHatching && (Time.time - startHatchTime >= hatchDuration)) {
			isHatching = false;
//			egg.SetActive(false);
			GameObject.Find("FarmManagerModule").GetComponent<FarmManager>().hatchAndSpawnEgg(egg);
//			hasHatched = true;
//			hatchedTime = Time.time;
//			Debug.Log("hatch success");
			this.transform.parent.Find("ReloadSceneButton").gameObject.SetActive(true);
			this.transform.parent.Find("CloseButton").gameObject.SetActive(false);
		}

		/*
		Debug.Log (hasHatched.ToString() + " " + (Time.time - hatchedTime));
		if (hasHatched && Time.time - hatchedTime >= 1f) {
			Debug.Log("Attempt to loadlevel");
			hasHatched = false;
			GameObject.Find("AdventureModule").GetComponent<AdventureManager>().savePlayerFile();
			Application.LoadLevel(Application.loadedLevel);
		}
		*/
	}

}
