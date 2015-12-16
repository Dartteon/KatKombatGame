using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicBallGame : MonoBehaviour {
	public List<NonCombatBehavior> playingKats;
	public int maxNumberPlaying;
	public float playDuration = 20.0f;
	public float actionInterval = 2.0f;
	public int numInteractionsPerPlay = 5;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		for (int i=0; i<playingKats.Count; i++){
			if (playingKats[i].isClicked){
				playingKats.RemoveAt(i);
			}

			if (Time.time - playingKats[i].startedActivityTime <= playDuration 
			    && playingKats[i].numCurrentActivityInteractions < numInteractionsPerPlay){

				if (Time.time - playingKats[i].lastActivityActionTime >= actionInterval){
					Vector2 katLocation = playingKats[i].getLocation();
					Vector2 currLocation = this.transform.position;
					Vector2 jumpDirection = currLocation - katLocation;
					playingKats[i].jumpInDirection (jumpDirection);
					playingKats[i].lastActivityActionTime = Time.time + Random.Range(actionInterval/2, actionInterval);
					playingKats[i].numCurrentActivityInteractions++;
//					Debug.Log("Interacted");
				} else {
					Vector2 katLocation = playingKats[i].getLocation();
					Vector2 currLocation = this.transform.position;
					Vector2 jumpDirection = currLocation - katLocation;
					playingKats[i].walkInDirection (jumpDirection);
				}
			} else {
				playingKats[i].isBusy = false;
				playingKats.RemoveAt(i);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collidingObj){
//		Debug.Log (collidingObj.ToString ());
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
