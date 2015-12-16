using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	private GameObject mainKat;
	public float panSpeed;

	public void attachKat(GameObject kat){
		mainKat = kat;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (mainKat != null) {
			//this.transform.position = Vector2.Lerp(this.transform.position, mainKat.transform.position, panSpeed*Time.deltaTime);
			Vector2 mainKatLoc = mainKat.transform.position;
			mainKatLoc = new Vector2(mainKatLoc.x, mainKatLoc.y -1.0f);
			Vector2 currPos = this.transform.position;
			this.transform.position = Vector2.Lerp(currPos, mainKatLoc, panSpeed*Time.deltaTime*Time.timeScale);
			currPos = this.transform.position;
			this.transform.position = new Vector3(currPos.x, currPos.y, -100f);
			//this.transform.position = mainKatLoc;
		}
	}
}
