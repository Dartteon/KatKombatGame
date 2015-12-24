using UnityEngine;
using System.Collections;

public class CommenceTournamentButton : MonoBehaviour, Tappable {
	private TournamentCreaterScreen tournyScript;
	private bool isClicked = false;
	private float clickTime;

	void Start() {
		tournyScript = this.transform.parent.GetComponent<TournamentCreaterScreen> ();
	}


	void OnMouseDown() {
		isClicked = true;
		clickTime = Time.time;
	}
	
	public void handleTap(Vector2 pos1, Vector2 pos2) {
		isClicked = true;
		clickTime = Time.time;
	}

	void Update() {
		if (isClicked && Time.time - clickTime >= 1.0f) {
			execute();
		}
	}

	void execute(){
		tournyScript.executeTournament ();
	}
}
