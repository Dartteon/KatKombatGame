using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ModeSelectButton : MonoBehaviour {
	bool isClicked = false;
	public string levelName;
	int difficultyLevel = 1;
	public int maxDifficultyLevel = 1;
	Text levelText;
	GameObject katSelectManager;
	SpriteRenderer bannerSpriteR;

	void Start(){
		levelText = transform.Find("TextCanvas").GetComponentInChildren<Text> ();
		katSelectManager = GameObject.Find ("KatSelectManager");
		bannerSpriteR = transform.Find ("Banner").GetComponentInChildren<SpriteRenderer> ();
		setHighestDifficulty ();
	}


	void setHighestDifficulty (){
		getHighestDifficulty ();
		difficultyLevel = maxDifficultyLevel;
		setDifficultyColor();
		setDifficultyLevelText ();
		setManagerLevel ();
	}

	void getHighestDifficulty(){
		if (katSelectManager != null){
			katSelectManager.GetComponent<KatSelectManager> ().loadSaveFile();
			int highestStreak = katSelectManager.GetComponent<KatSelectManager> ().highScore;
			int newMaxLevel = (highestStreak/5) + 1;
			if (newMaxLevel > maxDifficultyLevel){
				maxDifficultyLevel = newMaxLevel;
			}
		}
	}

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
			incrementDifficulty();
			setDifficultyColor();
		}
	}

	void incrementDifficulty(){
		getHighestDifficulty ();
		difficultyLevel++;
		if (difficultyLevel > maxDifficultyLevel) {
			difficultyLevel = 1;
		}

		setDifficultyLevelText ();
		setManagerLevel ();
	}

	void setManagerLevel(){
		katSelectManager.GetComponent<KatSelectManager> ().currentLevel = ((difficultyLevel-1)*5);
	}

	void setDifficultyLevelText(){
		switch (difficultyLevel) {
		case 1: levelText.text = "Easy"; break;
		case 2: levelText.text = "Normal"; break;
		case 3: levelText.text = "Hard"; break;
		case 4: levelText.text = "Insane"; break;
		}
	}

	void setDifficultyColor(){
		switch (difficultyLevel) {
		case 1: bannerSpriteR.color = new Color(0.4f, 0.8f, 0.4f); levelText.color = new Color(0f, 0f, 0f); break;
		case 2: bannerSpriteR.color = new Color(0.8f, 0.8f, 0.2f); levelText.color = new Color(0f, 0f, 0f); break;
		case 3: bannerSpriteR.color = new Color(1f, 0.45f, 0.45f); levelText.color = new Color(0f, 0f, 0f); break;
		case 4: bannerSpriteR.color = new Color(0.1f, 0.1f, 0.1f); levelText.color = new Color(1f, 1f, 1f); break;
		}
	}

}
