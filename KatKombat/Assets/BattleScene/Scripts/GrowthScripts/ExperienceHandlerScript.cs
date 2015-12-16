using UnityEngine;
using System.Collections;

public class ExperienceHandlerScript : MonoBehaviour {
	private readonly float expLevelMultiplier = 2;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public static int calculateExpGain(int level){
		return (int)(level * 2);
	}

	//returns true if Kat levelled up, false otherwise
	public static bool addExperienceToKat(PlayerInformation playerScript, int currentKatIndex, int enemyKatLevel, float multiplier){
		int beforeLevel = playerScript.ownedKats [currentKatIndex].getLevel ();
		int expGain = (int) (calculateExpGain (enemyKatLevel) * multiplier);
		playerScript.ownedKats [currentKatIndex].increaseExp (expGain);
		int afterLevel = playerScript.ownedKats [currentKatIndex].getLevel ();

		if (afterLevel > beforeLevel)
			return true;
		else
			return false;
	}
}
