using UnityEngine;
using System.Collections;

public class TournamentManager : MonoBehaviour {
	private int difficulty;
	private int numMatches;
	private int currentStage;
	private KatStatsInfo playerKat;
	private KatStatsInfo currentEnemy;

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

	public void initialize(KatStatsInfo plyrKat, int _difficulty, int _numMatches) {
		DontDestroyOnLoad (this.gameObject);
		katPrefabs = katPrefabsVessel.GetComponent<KatPrefabsVesselScript> ().katPrefabs;
		currentStage = 0;
		playerKat = plyrKat;
		difficulty = _difficulty;
		numMatches = _numMatches;
		getEnemy ();
		commenceTournament ();
	}

	private KatStatsInfo getEnemy() {
		string enemyBreed = katPrefabs [Random.Range (0, katPrefabs.Length)].name;
		currentEnemy = new KatStatsInfo (enemyBreed, katPrefabs, GameVariables.getRandomName ());
		setStatsOfEnemy (currentEnemy);
		return currentEnemy;
	}

	private void setStatsOfEnemy (KatStatsInfo enemyKat) {
		enemyKat.setLevel (5);
		setBirthStats (enemyKat);
		setExtraStats (enemyKat);
	}

	private void setBirthStats (KatStatsInfo enemyKat) {

	}

	private void setExtraStats (KatStatsInfo enemyKat) {

	}

	private int getEnemyLevel() {
		return 5; //stub
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
		if (currentStage == numMatches) {
			//reward extra bonus
		} else {
			//no bonus
		}
	}
}
