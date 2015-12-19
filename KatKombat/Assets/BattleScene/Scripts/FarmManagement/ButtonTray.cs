using UnityEngine;
using System.Collections;

public class ButtonTray : MonoBehaviour, Tappable {
	Vector3 startPos;
	Vector3 activatedPos;
	private enum ActivationDirection { Horizontal, Vertical };

	[SerializeField]
	private ActivationDirection activationDirection;
	[SerializeField]
	private float activatedCentreOffset = 0.5f;

	
	private bool isActivated = false;
	
	[SerializeField]
	private float activateSpeed = 5.0f;
	
	void Start() {
		startPos = this.transform.localPosition;

		if (activationDirection == ActivationDirection.Vertical) {
			activatedPos = new Vector3 (startPos.x, activatedCentreOffset, startPos.z-0.1f);
		} else {
			activatedPos = new Vector3 (activatedCentreOffset, startPos.y, startPos.z-0.1f);
		}
	}
	
	void OnMouseDown() {
//		Debug.Log ("Activating Katbuttons!");
		isActivated = !isActivated;
	}
	
	public void handleTap(Vector2 pos1, Vector2 pos2) {
//		Debug.Log ("Activating Katbuttons!");
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
