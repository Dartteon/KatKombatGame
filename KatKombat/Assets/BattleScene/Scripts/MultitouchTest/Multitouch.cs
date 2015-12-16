using UnityEngine;
using System.Collections;

public class Multitouch : MonoBehaviour {
	int numTouches;
//	public GameObject testMouse;
	Camera camera;

	// Use this for initialization
	void Start () {
		camera = GetComponent<Camera> ();
	}

	void Update(){
		numTouches = Input.touchCount;

		for (int i = 0; i<numTouches; i++) {
			TouchPhase touchPhase = Input.GetTouch (i).phase;
			if (touchPhase == TouchPhase.Began || touchPhase == TouchPhase.Moved || touchPhase == TouchPhase.Ended || touchPhase == TouchPhase.Stationary) {
//				Debug.Log ("Touch number " + i);
				RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint ((Input.GetTouch (i).position)), Vector2.zero);

				if (hit.collider != null) {
					Vector2 camPos = Input.GetTouch(i).position;
					Vector2 worldPos = camera.ScreenToWorldPoint(Input.GetTouch (i).position);
					handleTap (hit.collider.gameObject, camPos, worldPos);
//					Debug.Log(hit.collider.ToString());
				} else {
//					Debug.Log("Nothing hit");
				}
			}

		}


	}

	void handleTap (GameObject obj, Vector2 camPos, Vector2 worldPos){
		if (obj.tag == "GameController") {
			Tappable tapScript = obj.GetComponent<Tappable> ();
//			Debug.Log(tapScript.ToString());
			if (tapScript != null){
				tapScript.handleTap (camPos, worldPos);
			}	
		}
	}
}
