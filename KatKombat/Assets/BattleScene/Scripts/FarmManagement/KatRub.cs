using UnityEngine;
using System.Collections;

public class KatRub : MonoBehaviour, Tappable {
//	private float lastTapTime;
	private Vector2 lastRubPos;
	private Animator anim;

	void Start() {
		anim = transform.parent.GetComponent<Animator> ();
	}

	public void handleTap(Vector2 pos1, Vector2 pos2) {
		Debug.Log ("Tapped");
//		lastTapTime = Time.time;

		if ((pos1 - lastRubPos).magnitude >= 0.001f) {
			Debug.Log("Rubz");
		}
		lastRubPos = pos1;
	}

	void OnMouseDown() {
		anim.Play ("KatRub");
	}
}
