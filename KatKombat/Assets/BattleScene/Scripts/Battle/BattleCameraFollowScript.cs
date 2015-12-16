using UnityEngine;
using System.Collections;

public class BattleCameraFollowScript : MonoBehaviour {

	[SerializeField]
	private GameObject victorySignPrefab;
	private GameObject levelUpSignPrefab;
	private float zoomSize = 4f;
	private Vector2 targetPos;
	private GameObject target;
	private float panSpeed = 4f;

	private float victoryZoomSize = 3f;
	private bool battleOver = false;

	public void attachKat(GameObject kat){
		target = kat;
		this.GetComponent<Camera> ().orthographicSize = 4f;
	}

	void Update () {
		if (target != null) {
			Vector2 targetPos = target.transform.position;
			Vector2 currPos = transform.position;
			Vector2 newPos = Vector2.Lerp(currPos, targetPos, Time.deltaTime*panSpeed);
			this.transform.position = new Vector3(newPos.x, newPos.y, -10f);
		}
	}

	public void showLevelUp() {
		Vector3 spawnPos = new Vector3 (this.transform.position.x, this.transform.position.y, 0);
		Instantiate (levelUpSignPrefab, spawnPos, this.transform.rotation);
	}

	public void showEnemyVictory(GameObject enemy){
		targetPos = enemy.transform.position;
//		this.transform.position = new Vector3(targetPos.x, targetPos.y, -10f);
	}

	public void showPlayerVictory() {
		Vector3 spawnPos = new Vector3 (this.transform.position.x, this.transform.position.y, 0);
		Instantiate (victorySignPrefab, spawnPos, this.transform.rotation);
	}
}
