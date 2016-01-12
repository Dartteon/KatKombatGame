using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattleCameraFollowScript : MonoBehaviour {

	[SerializeField]
	private GameObject victorySignPrefab;
	[SerializeField]
	private GameObject levelUpSignPrefab;

	private float targetZoomSize = 3f;
	private float currentZoomSize = 2f;
	private Vector2 targetPos;
	private GameObject target;
	private float panSpeed = 4f;

//	private float victoryZoomSize = 3f;
	private bool battleOver = false;
	private Camera cam;

	public void initialize() {
		cam = this.GetComponent<Camera> ();

		GameObject tmObj = GameObject.Find ("TournamentManagerModule");
		if (tmObj != null) {
			TournamentManager tm = tmObj.GetComponent<TournamentManager> ();
			this.transform.Find ("StageText").transform.Find ("Text").GetComponent<Text> ().text = tm.getStringRepOfStage ();

		}
	}

	public void attachKat(GameObject kat){
		target = kat;
//		this.GetComponent<Camera> ().orthographicSize = 4f;
	}

	void Update () {
		if (target != null) {
			Vector2 targetPos = target.transform.position;
			Vector2 currPos = transform.position;
			Vector2 newPos = Vector2.Lerp(currPos, targetPos, Time.deltaTime*panSpeed);
			this.transform.position = new Vector3(newPos.x, newPos.y, -10f);
			
			float currentZoomSize = cam.orthographicSize;
			if (targetZoomSize != currentZoomSize) {
				cam.orthographicSize = Mathf.Lerp(currentZoomSize, targetZoomSize, Time.deltaTime);
			}
		}
	}

	public void showLevelUp() {
		Vector3 spawnPos = new Vector3 (this.transform.position.x, this.transform.position.y, 0);
		GameObject lvlUp = Instantiate (levelUpSignPrefab, spawnPos, this.transform.rotation) as GameObject;
		lvlUp.transform.parent = this.transform;
	}

	public void showEnemyVictory(GameObject enemy){
		target = enemy;
		targetPos = enemy.transform.position;
		finishGame ();
//		this.transform.position = new Vector3(targetPos.x, targetPos.y, -10f);
	}

	public void showPlayerVictory() {
		Vector3 spawnPos = new Vector3 (this.transform.position.x, this.transform.position.y, 0);
		GameObject vic = Instantiate (victorySignPrefab, spawnPos, this.transform.rotation) as GameObject;
		vic.transform.parent = this.transform;
		finishGame ();
	}

	public void disableSceneButtons () {
		transform.Find ("SurrenderButton").gameObject.SetActive (false);
		transform.Find ("StageText").gameObject.SetActive (false);
	}

	public void finishGame() {
		targetZoomSize = 4f;
		disableSceneButtons ();
	}
}
