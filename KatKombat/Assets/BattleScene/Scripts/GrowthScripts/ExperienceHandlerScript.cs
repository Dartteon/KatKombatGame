using UnityEngine;
using System.Collections;

public class ExperienceHandlerScript : MonoBehaviour {
	private static readonly float expLevelMultiplier = 2;
	public static readonly int BASE_EXP = 5;


	public static int calculateExpGain(int level){
		return level * 3;
		//return (int)(level * 2);
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

	public static int getLevel(int currentExp) {

		int level = 1;
		int expSum = BASE_EXP;
		
		for (int i=1; i<100; i++) {
			if (currentExp >= expSum){
				//Debug.Log("Level " + i + " :" + ((int)expSum));
				level++;
				expSum = (int)(expSum*1.2f);
			}
		}
		return level;
	}

	public static int getExpAtLevel(int lvl) {
		int currLevel = 1;
		int exp = 0;
		while (currLevel < lvl) {
			exp++;
			currLevel = getLevel(exp);
		}
		return exp;
	}

	public static int getExpNeededToLevelUp(int currExp) {
		int currLvl = getLevel (currExp);
		int nextLvlExp = getExpAtLevel(currLvl + 1);
		return (nextLvlExp - currExp);
	}
}
