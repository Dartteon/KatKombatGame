using UnityEngine;
using System.Collections;

public class TournamentManager : MonoBehaviour {
	private int difficulty;
	private int numMatches;
	private int currentStage;
	private KatStatsInfo playerKat;
	private KatStatsInfo currentEnemy;
	private int tournamentType;

	[SerializeField]
	private GameObject katPrefabsVessel;
//	[SerializeField]
//	private GameObject battleInstantiaterPrefab;
	private GameObject[] katPrefabs;

	public void commenceTournament() {
		Debug.Log ("Commencing Tournament [" + playerKat.toString () + "] [" + currentEnemy.toString () + "] [Difficulty " + difficulty + "] [No. Matches " + numMatches + "]");
		Debug.Log ("[Player Kat " + playerKat.toString () + "]");
		Debug.Log ("[Enemy Kat " + currentEnemy.toString () + "]");
		startNextStage ();
	}

	public void startNextStage() {
		GameObject.Find ("AdventureModule").GetComponent<AdventureManager> ().initiateBattleWithKat (playerKat, currentEnemy);
	}

	public void initialize(KatStatsInfo plyrKat, int _difficulty, int _tournamentType) {
		DontDestroyOnLoad (this.gameObject);
		katPrefabs = katPrefabsVessel.GetComponent<KatPrefabsVesselScript> ().katPrefabs;
		currentStage = 0;
		tournamentType = _tournamentType;
		playerKat = plyrKat;
		difficulty = _difficulty;
		numMatches = getNumMatches ();
		getEnemy ();
		commenceTournament ();
	}

	public int getNumMatches() {
		if (tournamentType == 0)
			return 10;
		else {
			return 100;
		}
	}

	private KatStatsInfo getEnemy() {
//		string enemyBreed = katPrefabs [Random.Range (0, katPrefabs.Length)].name;
		currentEnemy = new KatStatsInfo (KatBreed.getRandomBreed(), katPrefabsVessel, GameVariables.getRandomName ());
		setStatsOfEnemy (currentEnemy);
		return currentEnemy;
	}

	private void setStatsOfEnemy (KatStatsInfo enemyKat) {
		ExperienceHandlerScript.setKatToLevel (enemyKat, getEnemyLevel ());
	//	enemyKat.setLevel (getEnemyLevel());
		setStatsHandicap (enemyKat);
	}

	private void setStatsHandicap(KatStatsInfo enemyKat) {
		enemyKat.setLevelHandicap ((5 - difficulty) * 4);
	}

	private int getEnemyLevel() {
//		Debug.Log ("Fighting stage " + currentStage);
//		Debug.LogError ("Getting enemy level at type " + tournamentType);
		if (tournamentType == 0) {
//			int bossLevel = (currentStage == 10) ? (int)(difficulty * 1.5) : 0;
//			Debug.Log("Boss level offset " + bossLevel);
//			Debug.Log("Level " + ((difficulty * 10) + currentStage + 1));
			int lvl = Mathf.Clamp(((difficulty) * 10), 0, 200) + (int)(currentStage*1.5) + 1;
//			Debug.Log("Enemy level " + lvl);
//			Debug.LogError(currentStage + " vs " + numMatches);
			if (currentStage == numMatches) {
//				Debug.LogError("Fighting boss stage! " + (int)((difficulty+1) * 3));
				lvl += (int)(difficulty * 1.5);
			}
//			Debug.LogError("Enemy Level : " + lvl);
			return lvl;
		} else {
			return ((difficulty - 1) * 10) + (currentStage * 2);
		}
	}

	public void endRound (bool didPlayerWin) {
		if (didPlayerWin) {
			currentStage++;
			getEnemy();
			startNextStage();

		} else {
			endTournament();
		}
	}

	private void endTournament() {
			GameObject.Find("AdventureModule").GetComponent<AdventureManager>().addCurrency(calculateKashRewards());

	}

	public int calculateKashRewards() {
		if (currentStage == 1)
			return 0;
		if (tournamentType == 0) {
			int reward = (difficulty+1) * currentStage;
			if (currentStage == numMatches){ 
				Debug.LogError("Extra rewards!");
				reward += getTournamentEndBonus();

			}
			Debug.Log("Rewards for tournament : " + reward);
//			Debug.Log ("Kash gain: " + ((difficulty+1) * currentStage) + currentStage);
			return reward;
		} else {
			return currentStage * 3;
		}
	}

	public bool hasTournamentEnded() {
		if (currentStage == numMatches)
			return true;
		else
			return false;
	}

	public int getTournamentEndBonus() {
		return (difficulty+1) * 10;
	}

	public string getStringRepOfStage() {
		string stageNum = (currentStage + 1).ToString ();
		string diff = "[D" + (difficulty + 1) + "]";
		string type = "[";
		if (tournamentType == 0) {
			type += "Limited";
		} else {
			type += "Arcade";
		}
		type += "]";

		if (currentStage == numMatches)
			return diff + " " + type + " " + "LAST BATTLE";
		return diff + " " + type + " " + "Stage " + stageNum;
	}

	public float getEnemyAimDiscrepancy() {
		return 2.0f - (difficulty * 0.3f);
	}
}
