using UnityEngine;
using System.Collections;

public class PvpStartButton : MonoBehaviour {
	bool isClicked = false;
	public string levelName;

	
	
	void OnMouseDown(){
		isClicked = true;
		//		Debug.Log ("Being pressed");
	}
	void OnMouseUp(){
		Vector2 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector2 currentPos = this.transform.position;
		Vector2 distanceVec = (currentPos - mousePos);
		float absDistance = distanceVec.magnitude;
		if (isClicked && absDistance <= 1.0f) {
			GameObject.Find ("KatSelectManager").GetComponent<KatSelectManager>().battleMode = 1;
			Application.LoadLevel (levelName);
		}
	}
}
