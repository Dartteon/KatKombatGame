using UnityEngine;
using System.Collections;

public class KatButtonsHolder : MonoBehaviour, Tappable {
	Vector3 startPos;
	Vector3 activatedPos;

	private bool isActivated = false;

	[SerializeField]
	private float activateSpeed = 2.0f;

	void Start() {
		startPos = this.transform.localPosition;
		activatedPos = new Vector3 (startPos.x, 0.5f, startPos.z);
	}

	void OnMouseDown() {
		Debug.Log ("Activating Katbuttons!");
		isActivated = !isActivated;
	}

	public void handleTap(Vector2 pos1, Vector2 pos2) {
		Debug.Log ("Activating Katbuttons!");
		isActivated = !isActivated;

	}

	void Update() {
		if (isActivated) {
			this.transform.localPosition = Vector3.Lerp (this.transform.localPosition, activatedPos, Time.deltaTime*activateSpeed);
		} else {
			this.transform.localPosition = Vector3.Lerp (this.transform.localPosition, startPos, Time.deltaTime*activateSpeed);
		}
	}
}
