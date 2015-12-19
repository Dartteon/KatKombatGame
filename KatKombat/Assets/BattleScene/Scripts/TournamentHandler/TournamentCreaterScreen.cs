using UnityEngine;
using System.Collections;

public class TournamentCreaterScreen : MonoBehaviour, OptionsChooserEmployer {
	private int tournamentDifficulty = 0;
	private int tournamentType = 0;
	private OptionsChooser difficultyChooser;
	private OptionsChooser tournamentTypeChooser;
	private OptionsChooser katChooser;
	private KatStatsInfo[] playerKats;

	[SerializeField]
	private int maxDifficulty;
	
	[SerializeField]
	private int numTournamentTypes;

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
			return false;
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
			return false;
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

	public void executeTournament() {
		Debug.Log ("Executing tournament [Difficulty " + tournamentDifficulty + "] [Type " + tournamentType + "]");
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.Space))
			executeTournament ();
	}

}
