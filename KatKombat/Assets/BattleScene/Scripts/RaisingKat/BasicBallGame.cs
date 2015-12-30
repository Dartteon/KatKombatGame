using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicBallGame : MonoBehaviour, Tappable {
	public List<NonCombatBehavior> playingKats;
	public int maxNumberPlaying;
	public float playDuration = 20.0f;
	public float actionInterval = 2.0f;
	public int numInteractionsPerPlay = 5;

	private Animator ballAnim;

	// Use this for initialization
	void Start () {
		ballAnim = this.GetComponent<Animator> ();
	}

	void OnMouseDown() {
//		Debug.Log ("Clicked");
		Vector2 randomDir = new Vector2 (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
//		Debug.Log (randomDir.ToString ());
		this.GetComponent<Rigidbody2D> ().velocity = randomDir * 10f;
		ballAnim.Play ("BallClicked");
	}

	public void handleTap (Vector2 pos1, Vector2 pos2) {
//		Vector2 randomDir = new Vector2 (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
		Vector2 currentPos = this.transform.position;
		Vector2 dir = currentPos - pos1;
		dir.Normalize ();
//		Debug.Log (randomDir.ToString ());
		this.GetComponent<Rigidbody2D> ().AddForce (dir * 5f, ForceMode2D.Impulse);
		ballAnim.Play ("BallClicked");
	}

	// Update is called once per frame
	void FixedUpdate () {
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
			if (katBehavior != null &&  katBehavior.isBusy == false && !katBehavior.isClicked){
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
