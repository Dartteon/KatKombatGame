using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MerryGoRoundGame : MonoBehaviour {
	public List<NonCombatBehavior> playingKats;
	public int maxNumberPlaying;
	public float playDuration = 20.0f;
	public float actionInterval = 2.0f;
	public int numInteractionsPerPlay = 5;

	public GameObject seat1;

	// Use this for initialization
	void Start () {
		seat1 = transform.Find ("Seat1").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
		for (int i=0; i<playingKats.Count; i++) {
			if (playingKats [i].isClicked) {
				playingKats.RemoveAt (i);
			}
			
			playingKats[i].transform.position = seat1.transform.position;
			playingKats[i].faceDirection(seat1.transform.up);

			if (Time.time - playingKats[i].startedActivityTime <= playDuration){
			} else {
				playingKats[i].isBusy = false;
				playingKats.RemoveAt(i);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collidingObj){
		Debug.Log ("trigger");
		if (playingKats.Count <= maxNumberPlaying && collidingObj.transform.tag.ToLower().Equals("player1")) {
			//remember to tag kats as Player1
			NonCombatBehavior katBehavior = collidingObj.transform.GetComponent<NonCombatBehavior>();
			//Debug.Log(katBehavior.isBusy);
			if (katBehavior.isBusy == false && !katBehavior.isClicked){
				playingKats.Add(katBehavior);
				//				Debug.Log("attached kat" + collidingObj.ToString());
				katBehavior.isBusy = true;
				katBehavior.startedActivityTime = Time.time;
				katBehavior.lastActivityActionTime = Time.time;
				katBehavior.numCurrentActivityInteractions = 0;
			}
		}
	}
	
	
	void OnDisable(){
		for (int i=0; i<playingKats.Count; i++) {
			playingKats[i].isBusy = false;
			
		}
	}
}
