using UnityEngine;
using System.Collections;

public class MenuOption : MonoBehaviour {
	bool isClicked = false;
	public string levelName;
	private GameObject katSelectManager;
	bool isBattleScene = false;
	public bool isMainRestartButton = true;

	// Use this for initialization
	void Start () {
		katSelectManager = GameObject.Find ("KatSelectManager");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	
	void OnMouseDown(){
		isClicked = true;
	}
	void OnMouseUp(){
		Vector2 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector2 currentPos = this.transform.position;
		Vector2 distanceVec = (currentPos - mousePos);
		float absDistance = distanceVec.magnitude;
		if (isClicked && absDistance <= 1.0f) {
			if (katSelectManager != null){
				Destroy(katSelectManager);
			}
			Application.LoadLevel (levelName);
		}
	}
}
