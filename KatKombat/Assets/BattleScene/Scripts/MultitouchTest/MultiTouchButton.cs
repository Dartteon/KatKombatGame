using UnityEngine;
using System.Collections;

public class MultiTouchButton : MonoBehaviour {
	private bool isBeingTouched = false;
	private Vector2 currPos;
	private Touch toucher;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
//		Debug.Log (toucher.phase.ToString ());
		if (toucher.phase == TouchPhase.Ended) {
			Debug.Log("Touch stopped");
			isBeingTouched = false;
		}
	}

	public void handleTouch (Vector2 position){
		Debug.Log ("Button pressed at " + position.ToString ());
		if (!isBeingTouched) {
			isBeingTouched = true;
		}

		currPos = position;
	}

	public void handleTouchInput(Touch touch){
		Debug.Log ("Button pressed at " + touch.position.ToString ());
		if (!isBeingTouched) {
			toucher = touch;
			isBeingTouched = true;
		}
		
		currPos = touch.position;
	}
}
