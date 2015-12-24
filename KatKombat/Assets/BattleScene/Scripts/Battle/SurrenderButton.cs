using UnityEngine;
using System.Collections;

public class SurrenderButton : MonoBehaviour {
	void OnMouseDown() {
		GameObject.Find ("GameStateBattleManager").GetComponent<GamestateBattleManager> ().displayLoss ();
	}

	public void handleTap(Vector2 pos1 ,Vector2 pos2) {
		GameObject.Find ("GameStateBattleManager").GetComponent<GamestateBattleManager> ().displayLoss ();
	}
}
