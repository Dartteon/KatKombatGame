using UnityEngine;
using System.Collections;

public class PVPBattleCameraFollowScript : MonoBehaviour {
	private GameObject kat1, kat2;
	private Vector2 targetPos;
	private float panSpeed = 4f;

	void Update(){
		if (kat1 != null && kat2 != null) {
			Vector2 kat1Pos = kat1.transform.position;
			Vector2 kat2Pos = kat2.transform.position;
			Vector2 centre = kat1Pos + kat2Pos;
			centre = centre / 2;
			targetPos = centre;

		} else if (kat1 != null) {
			targetPos = kat1.transform.position;
		} else if (kat2 != null) {
			targetPos = kat2.transform.position;
		}
		
		Vector2 currPos = transform.position;
		Vector2 newPos = Vector2.Lerp (currPos, targetPos, Time.deltaTime * panSpeed);
		this.transform.position = new Vector3 (newPos.x, newPos.y, -10f);
	}

	public void AttachKats(GameObject _kat1, GameObject _kat2){
		kat1 = _kat1;
		kat2 = _kat2;
	}
}
