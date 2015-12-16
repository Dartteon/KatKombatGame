using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SitInCarGame : MonoBehaviour {
	public List<NonCombatBehavior> playingKats;
	public int maxNumberPlaying;
	public float playDuration = 20.0f;
	public float actionInterval = 2.0f;
	public int numInteractionsPerPlay = 5;
	private bool isSeated = false;

	void Update () {
		
		for (int i=0; i<playingKats.Count; i++) {
			if (playingKats [i].isClicked) {
				playingKats.RemoveAt (i);
			}
			if (Time.time - playingKats[i].startedActivityTime >= 0.2f){
				isSeated = true;
			}
			if (isSeated) {
				playingKats[i].transform.position = this.transform.position;
				playingKats[i].faceDirection(this.transform.up);
			} else {
				playingKats[i].transform.position = Vector2.Lerp(playingKats[i].transform.position, this.transform.position, Time.deltaTime*20f);
				playingKats[i].faceDirection(this.transform.up);
			}

			if (Time.time - playingKats[i].startedActivityTime <= playDuration){

			} else {
				playingKats[i].isBusy = false;
				playingKats[i].jumpInDirection(this.transform.up);
				playingKats.RemoveAt(i);
			}
		}
	}
	
	void OnTriggerEnter2D(Collider2D collidingObj){
//		Debug.Log ("trigger");
		if (playingKats.Count <= maxNumberPlaying 
		    && collidingObj.transform.tag.ToLower().Equals("player1") 
		    && playingKats.Count < maxNumberPlaying) {
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
