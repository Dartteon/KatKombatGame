using UnityEngine;
using System.Collections;

public class KommandDescriptionButton : MonoBehaviour {
	private bool isClicked;
	private KatDataCard dataCard;
	[SerializeField]
	private int kommandIndex;

	void Start() {
		dataCard = this.transform.parent.GetComponent<KatDataCard> ();
	}

	void OnMouseDown() {
		isClicked = !isClicked;

		if (isClicked) {
			dataCard.enableKommandDescriptionBox (kommandIndex);
		} else {
			dataCard.disableAllKommandDescriptionBoxes();
		}
	}
}
