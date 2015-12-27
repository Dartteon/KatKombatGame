using UnityEngine;
using System.Collections;

public class ExperienceHandlerScript : MonoBehaviour {
	private static readonly float expLevelMultiplier = 2;
	public static readonly int BASE_EXP = 5;


	public static int calculateExpGain(int level){
		return (int) (level * 0.8f) + 1;
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
		int expSum = 0;
		
		for (int i=1; i<100; i++) {
			if (currentExp > expSum){
				//Debug.Log("Level " + i + " :" + ((int)expSum));
				expSum += getExpNeededToLevelAt(level);
				level++;
				//expSum = (int)(expSum*1.2f);
			}
		}
		return level;
	}

	public static int getExpNeededToLevelAt (int level) {
		//return (int)((0.8f) * (Mathf.Pow (level, 3)));
		//return (int) Mathf.Log10 (level);
		return (int)((1.5f) * level);
	}


	public static int getExpAtLevel(int lvl) {
		int currLevel = 1;
		int exp = 0;
		while (currLevel < lvl) {
			exp += getExpNeededToLevelAt(currLevel);
			currLevel++;
		}
//		Debug.Log ("Added exp up to level " + currLevel);
		return exp;
	}

	public static void setKatToLevel(KatStatsInfo kat, int lvl) {
		int currLevel = 1;
//		Debug.Log ("settingLevel to " + lvl);
		while (currLevel < lvl) {
//			Debug.Log (currLevel);
			kat.increaseExp(getExpNeededToLevelAt(currLevel));
			currLevel++;
		}
	}

	/*
	public static int getExpNeededToLevelUp(int currExp) {
		int currLvl = getLevel (currExp);
		int nextLvlExp = getExpAtLevel(currLvl + 1);
		return (nextLvlExp - currExp);
	}
	*/
}
