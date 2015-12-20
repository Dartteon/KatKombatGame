using UnityEngine;
using System.Collections;

public class PostBattleNextButton : MonoBehaviour, Tappable {
	private bool isClicked = false;
	public void handleTap(Vector2 pos1, Vector2 pos2) {
		if (!isClicked) execute ();
	}

	void OnMouseDown() {
		if (!isClicked) execute ();
	}

	void execute() {
		GameObject tournamentManager = GameObject.Find ("TournamentManagerModule").gameObject;
		if (tournamentManager != null) {
			tournamentManager.GetComponent<TournamentManager> ().endRound (true);
		} else {
			Debug.LogError("tournament manager not found");
			Application.LoadLevel("HomeScene");
		}
	}
}
