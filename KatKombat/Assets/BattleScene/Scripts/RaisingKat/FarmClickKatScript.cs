using UnityEngine;
using System.Collections;

public class FarmClickKatScript : MonoBehaviour {
	AdventureManager advMngr;

	// Use this for initialization
	void Start () {
		advMngr = GameObject.Find ("AdventureModule").GetComponent<AdventureManager> ();
	}

	void handleTap(Vector2 pos1, Vector2 pos2){
		advMngr.followThisKat (this.gameObject);
	}

	void OnMouseDown(){
		
		advMngr.followThisKat (this.gameObject);
	}
}
