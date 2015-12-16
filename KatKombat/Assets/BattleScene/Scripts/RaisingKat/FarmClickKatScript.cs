using UnityEngine;
using System.Collections;

public class FarmClickKatScript : MonoBehaviour {
	AdventureManager advMngr;
	FarmManager farmMngr;

	// Use this for initialization
	void Start () {
		advMngr = GameObject.Find ("AdventureModule").GetComponent<AdventureManager> ();
		farmMngr = GameObject.Find ("FarmManagerModule").GetComponent<FarmManager> ();
	}

	void handleTap(Vector2 pos1, Vector2 pos2){
		farmMngr.followThisKat (this.gameObject);
	}

	void OnMouseDown(){
		
		farmMngr.followThisKat (this.gameObject);
	}
}
