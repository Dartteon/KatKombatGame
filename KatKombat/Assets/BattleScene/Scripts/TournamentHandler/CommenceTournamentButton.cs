using UnityEngine;
using System.Collections;

public class CommenceTournamentButton : MonoBehaviour, Tappable {
	private TournamentCreaterScreen tournyScript;

	void Start() {
		tournyScript = this.transform.parent.GetComponent<TournamentCreaterScreen> ();
	}


	void OnMouseDown() {
		execute ();
	}
	
	public void handleTap(Vector2 pos1, Vector2 pos2) {
		execute ();
	}

	void execute(){
		tournyScript.executeTournament ();
	}
}
