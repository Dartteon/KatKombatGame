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

	public void initialize(KatStatsInfo plyrKat, int _difficulty, int tournamentType) {
		DontDestroyOnLoad (this.gameObject);
		katPrefabs = katPrefabsVessel.GetComponent<KatPrefabsVesselScript> ().katPrefabs;
		currentStage = 0;
		playerKat = plyrKat;
		difficulty = _difficulty;
		numMatches = getNumMatches ();
		getEnemy ();
		commenceTournament ();
	}

	public int getNumMatches() {
		if (tournamentType == 0)
			return 10;
		else
			return 100;

	}

	private KatStatsInfo getEnemy() {
		string enemyBreed = katPrefabs [Random.Range (0, katPrefabs.Length)].name;
		currentEnemy = new KatStatsInfo (enemyBreed, katPrefabs, GameVariables.getRandomName ());
		setStatsOfEnemy (currentEnemy);
		return currentEnemy;
	}

	private void setStatsOfEnemy (KatStatsInfo enemyKat) {
		enemyKat.setLevel (getEnemyLevel());
		setBirthStats (enemyKat);
		setExtraStats (enemyKat);
	}

	private void setBirthStats (KatStatsInfo enemyKat) {

	}

	private void setExtraStats (KatStatsInfo enemyKat) {

	}

	private int getEnemyLevel() {
		if (tournamentType == 0) {
			Debug.Log("Level " + difficulty + " * " + (currentStage+1));
			return difficulty * currentStage + 1;
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
			Debug.Log (difficulty + " " + currentStage);
			return difficulty * currentStage;
		} else {
			return currentStage * 3;
		}
	}
}
