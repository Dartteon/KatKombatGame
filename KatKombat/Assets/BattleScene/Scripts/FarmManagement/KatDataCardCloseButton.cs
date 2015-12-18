using UnityEngine;
using System.Collections;

public class KatDataCardCloseButton : MonoBehaviour {

	void OnMouseDown() {
		Camera.main.GetComponent<FarmCameraFollowScript> ().stopFollowingKat ();
	}
}
