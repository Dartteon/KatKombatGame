using UnityEngine;
using System.Collections;

public class CameraDragPannerScript : MonoBehaviour, Tappable {
	private Vector2 lastPos;

	public float mult = .01f;

	private GameObject cam;
	private FarmCameraFollowScript camScript;

	private float lastTapTime;
	private bool isPanning = false;

	// Use this for initialization
	void Start () {
		cam = Camera.main.gameObject;
		camScript = cam.GetComponent<FarmCameraFollowScript> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - lastTapTime >= .1f) {
			isPanning = false;
		}
	}

	public void handleTap(Vector2 pos2, Vector2 pos1){
		if (lastPos != null && isPanning) {
			camScript.goToCameraMode(1);
//			Debug.Log("notnull");
			Vector2 diff = lastPos - pos2;
			diff = new Vector2(diff.x*mult, diff.y*mult);
		//	diff *= mult;
//			Debug.Log(diff.x*mult);
	//		diff *= mult;
			Vector2 currCamPos = cam.transform.position;
			Vector2 newCamPos = currCamPos + diff;

			cam.transform.position = new Vector3 (newCamPos.x, newCamPos.y, -100f);
			lastPos = pos2;

			lastTapTime = Time.time;
			isPanning = true;
		} else {
//			Debug.Log("null");
			lastPos = pos2;
			lastTapTime = Time.time;
			isPanning = true;
		}
	}
}
