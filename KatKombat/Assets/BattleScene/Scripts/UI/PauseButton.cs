using UnityEngine;
using System.Collections;

public class PauseButton : MonoBehaviour {
	bool isClicked = false;
	public bool isMainButton = true;

	void OnMouseDown(){
		isClicked = true;
	}
	void OnEnable(){
		if (!isMainButton) {
			//mainButton is the pause button on top right hand of the screen
		//	transform.parent.Find ("Controllers").gameObject.SetActive (false);
		}
	}
	void OnMouseUp(){
		Vector2 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector2 currentPos = this.transform.position;
		Vector2 distanceVec = (currentPos - mousePos);
		float absDistance = distanceVec.magnitude;

		if (isClicked && Time.timeScale == 1.0f && absDistance <= 1.0f) {
			Time.timeScale = 0.0f;
			if (isMainButton) {
				transform.Find("PauseScreen").gameObject.SetActive(true);
			}
		} else if (isClicked && Time.timeScale == 0.0f && absDistance <= 1.0f) {
			Time.timeScale = 1.0f;
			if (!isMainButton) {
				transform.parent.gameObject.SetActive(false);
			} else {

				GameObject pauseScreen = transform.Find("PauseScreen").gameObject;
				if (pauseScreen != null)
					pauseScreen.SetActive(false);
				else
					this.gameObject.SetActive(false);
			}
		}
	}
}
