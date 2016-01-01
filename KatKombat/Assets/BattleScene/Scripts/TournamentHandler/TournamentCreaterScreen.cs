using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TournamentCreaterScreen : MonoBehaviour, OptionsChooserEmployer {
	private int tournamentDifficulty = 0;
	private int tournamentType = 0;
	private OptionsChooser difficultyChooser;
	private OptionsChooser tournamentTypeChooser;
	private OptionsChooser katChooser;
	private List<KatStatsInfo> playerKats;
	private int numPlayerKats;
	private int currentKat = 0;

	[SerializeField]
	private int maxDifficulty;
	
	[SerializeField]
	private int numTournamentTypes;
	
	[SerializeField]
	private GameObject tournamentManagerPrefab;


	public void initiate(FarmManager farmMngr) {
		playerKats = farmMngr.katsInfo;
		numPlayerKats = playerKats.Count;
	}

	public bool shiftLeft(int optionsBarNumber) {
		switch (optionsBarNumber) {
		case 0:
			if (shiftTournamentDifficultyLeft()) return true;
			else return false;
			break;
		case 1:
			if (shiftTournamentTypeLeft()) return true;
			else return false;
			break;
		default:
			if (shiftKatLeft()) return true;
			else return false;
			break;
		}
	}

	public bool shiftRight(int optionsBarNumber) {
		switch (optionsBarNumber) {
		case 0:
			if (shiftTournamentDifficultyRight()) return true;
			else return false;
			break;
		case 1:
			if (shiftTournamentTypeRight()) return true;
			else return false;
			break;
		default:
			if (shiftKatRight()) return true;
			else return false;
			break;
		}
	}

	bool shiftTournamentDifficultyRight(){
		if (tournamentDifficulty + 1 < maxDifficulty) {
			tournamentDifficulty++;
			return true;
		} else
			return false;
	}
	bool shiftTournamentDifficultyLeft(){
		if (tournamentDifficulty - 1 >=  0) {
			tournamentDifficulty--;
			return true;
		} else
			return false;
	}

	bool shiftTournamentTypeRight() {
		if (tournamentType + 1 < numTournamentTypes) {
			tournamentType++;
			return true;
		} else
			return false;
	}
	bool shiftTournamentTypeLeft() {
		if (tournamentType - 1 >= 0) {
			tournamentType--;
			return true;
		} else
			return false;
	}

	bool shiftKatRight() {
		if (currentKat + 1 < numPlayerKats) {
			currentKat++;
			return true;
		} else
			return false;
	}
	bool shiftKatLeft() {
		if (currentKat - 1 >= 0) {
			currentKat--;
			return true;
		} else
			return false;
	}

	public void executeTournament() {
		searchAndDestroyErrorModules ();
		Debug.Log ("Executing tournament [Difficulty " + tournamentDifficulty + "] [Type " + tournamentType + "] [" + playerKats[currentKat].toString() + "]");
		GameObject tournament = Instantiate (tournamentManagerPrefab);
		tournament.name = "TournamentManagerModule";
		tournament.GetComponent<TournamentManager> ().initialize (playerKats [currentKat], tournamentDifficulty, tournamentType);
	}

	void searchAndDestroyErrorModules() {
		Destroy (GameObject.Find ("TournamentManagerModule"));
		Destroy (GameObject.Find ("BattleInformationModule"));
		Destroy (GameObject.Find ("GameStateBattleManager"));
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.Space))
			executeTournament ();
	}

}
