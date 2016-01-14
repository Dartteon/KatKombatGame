using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Kommand = Kommands.KommandCode;
using Breed = KatBreed.Breed;

[Serializable]
public class KatStatsInfo {
	//This class is the main information carrier for Kats the player owns.

	//STRICTLY PERMANENT variables. Not changeable once the Kat is born.
	public Breed breed { get; private set;}
	public bool isMaleGender { get; private set; }
	private int variantNumber;
//	private string katBreed;
	private string katName;
	private int breedStr, breedDex, breedInt;
	private readonly int BASE_EXP = ExperienceHandlerScript.BASE_EXP;

	//Birth stats are random stats that Kats gain from birth. (i.e IV in Pokemon)
	private int birthStr = 0;
	private int birthDex = 0;
	private int birthInt = 0;

	//LOOSE variables. To be changed easily.
	public int currentHP { get; private set; }
	private int currentExp = 0;
	private int closeness = 0;
	private int battlesWon = 0;
	private int battlesLost = 0;
	//Extra stats are effort stats that Kats gain from training. (i.e EV in Pokemon)
	private int extraStr = 0;
	private int extraDex = 0;
	private int extraInt = 0;
//	private string[] katCommands = {"nil", "nil", "nil"};

	//Static variables for referencing
	private readonly int EXTRA_STATS_THRESHOLD = 128;
	private readonly int MAX_LEVEL = 60;
	private readonly int STAT_OFFSET = 7;
	private int levelStatOffset = 40;

	private DateTime birthDay;

	private bool hasInitialized = false;

	private bool hasHatched;

	private List<Kommand> knownKommandList = new List<Kommand>();
	private List<Kommand> activeKommandList = new List<Kommand>();

	private List<AncestorPair> ancestors = new List<AncestorPair>();

	public bool isFertile{ get; private set; }
	/*
	//constructor
	public KatStatsInfo(string breed, GameObject[] katPrefabs){
		currentExp = 0;
		randomizeBirthStats ();
		katBreed = breed;
		if (initializeStats (katBreed, katPrefabs)) {
			Debug.Log("[Kat successfully initialized] " + katBreed);
		} else {
		//	Debug.Log(katBreed + " not found. Attach Prefab to katPrefabVessel!");
			Debug.LogError("Breed \"" + katBreed + "\" not found. Attach Prefab to katPrefabVessel!");
		}
	}
	*/
//	public KatStatsInfo(string breed, GameObject[] katPrefabs, string name){
	public KatStatsInfo(KatBreed.Breed kBreed, GameObject katPrefabVessel, string name){
		katName = name;
		currentHP = 1;
		currentExp = 0;
		randomizeBirthStats ();
//		katBreed = breed;
		breed = kBreed;
		hasHatched = true;
		isFertile = true;
		isMaleGender = getRandomGender ();

		activeKommandList.Add (Kommand.None);
		activeKommandList.Add (Kommand.None);
		activeKommandList.Add (Kommand.None);

		if (initializeStats (kBreed, katPrefabVessel)) {
			Debug.Log("[Kat successfully initialized] " + this.toString());
		} else {
			//	Debug.Log(katBreed + " not found. Attach Prefab to katPrefabVessel!");
			Debug.LogError(kBreed.ToString() + " not found. Attach Prefab to katPrefabVessel!");
		}
	}


	public static KatStatsInfo getEgg(KatBreed.Breed kBreed, GameObject katPrefabVessel, string name){
		KatStatsInfo newEgg = new KatStatsInfo (kBreed, katPrefabVessel, name);
		newEgg.setKatAsEgg ();
		return newEgg;
	}

	public void setKatAsEgg(){
		this.hasHatched = false;
	}

	public void hatchEgg(){
		this.hasHatched = true;
	}

	public bool getHasHatched(){
		return hasHatched;
	}

//	public string getBreed(){
//		return katBreed;
//	}

	private void randomizeBirthStats(){
		this.birthStr = UnityEngine.Random.Range (0, 32);
		this.birthDex = UnityEngine.Random.Range (0, 32);
		this.birthInt = UnityEngine.Random.Range (0, 32);
//		Debug.Log (birthStr + " " + birthDex + " " + birthInt);
	}

	public void setBirthDay(DateTime bday){
		birthDay = bday;
	}

	public string getName(){
		return katName;
	}

	//To be refactored in
	public void setKommands(Kommand first, Kommand second, Kommand third) {
//		Debug.LogError ("Adding Kommands " + first.ToString () + " " + second.ToString () + " " + third.ToString ());
		activeKommandList.Clear ();
		activeKommandList.Add (first);
		activeKommandList.Add (second);
		activeKommandList.Add (third);
	}

	public List<Kommand> getActiveKommands(){
		return activeKommandList;
	}

	public bool initializeStats(KatBreed.Breed kBreed, GameObject katPrefabVessel){
		breed = kBreed;
		GameObject kat = katPrefabVessel.GetComponent<KatPrefabsVesselScript> ().getKatOfBreed (kBreed);
		if (kat != null) {
			StatsScript statsScript = kat.GetComponent<StatsScript> ();
			breedStr = statsScript.getStr ();
			breedDex = statsScript.getDex ();
			breedInt = statsScript.getInt ();
			return true;
		} else
			return false;
		/*
		for (int i=0; i<katPrefabs.Length; i++) {
	//		if (katBreed.Equals(katPrefabs[i].name.Split(' ')[0])){
			if (kBreed.Equals(katPrefabs[i].GetComponent<StatsScript>().breed)){
				StatsScript katStatsScript = katPrefabs[i].GetComponent<StatsScript>();
				breedStr = katStatsScript.getStr();
//				Debug.Log("Breed Str: " + breedStr);
				breedDex = katStatsScript.getDex();
				breedInt = katStatsScript.getInt();
				return true;
			}
//			Debug.Log(breedStr + " " + breedDex + " " + breedInt);
		}
		return false;
		*/
	}

	public int getTotalStr(){
		int effectiveLevel = getLevel () + levelStatOffset;
		int ratiodStr = (int)((breedStr + birthStr + extraStr) * (((float)effectiveLevel)/ 60)) + STAT_OFFSET;
	//	int ratiodStr = (int)(((float)getLevel () / MAX_LEVEL) * getScalableStr());
//		Debug.Log ("Getting STR " + getLevel() + " x " + breedStr + " + " + birthStr + " + " + extraStr);
//		Debug.Log (ratiodStr);
		return (ratiodStr);
	}

	public int getTotalDex(){
		int effectiveLevel = getLevel () + levelStatOffset;
		int ratiodDex = (int)((breedDex + birthDex + extraDex) * (((float)effectiveLevel) / 60)) + STAT_OFFSET;
	//	int ratiodDex = (int)(((float)getLevel () / MAX_LEVEL) * getScalableDex());
		return (ratiodDex);
	}

	public int getTotalInt(){
		int effectiveLevel = getLevel () + levelStatOffset;
		int ratiodInt = (int)((breedInt + birthInt + extraInt) * (((float)effectiveLevel) / 60)) + STAT_OFFSET;
	//	int ratiodInt = (int)(((float)getLevel () / MAX_LEVEL) * getScalableInt());
		return (ratiodInt);
	}

	public int getLevel(){
		int level = ExperienceHandlerScript.getLevel (currentExp);

		return level;
	}

	public void setLevel(int level){
		int expSum = BASE_EXP;
		int tempLevel = 1;


		while (tempLevel < level) {
			tempLevel++;
			expSum = (int)(expSum*1.2f);
		}

		increaseExp (expSum);

	}

	public void increaseExp(int exp){
		int oldExp = currentExp;
		currentExp += exp;
//		Debug.Log ("Increasing EXP for " + katBreed + " : " + exp);
//		Debug.Log ("Kat is level " + getLevel ());
//		Debug.Log("EXP needed for next level: " + ExperienceHandlerScript.getExpNeededToLevelUp(currentExp));

	}

	public int getScalableStr(){
		return breedStr + birthStr + extraStr;
	}
	public int getScalableDex(){
		return breedDex + birthDex + extraDex;
	}
	public int getScalableInt(){
		return breedInt + birthInt + extraInt;
	}

	public void setCurrentHP(int hp){
		currentHP = hp;
	}

	public int addExtraStats(int amount, int statType){
		int currentExtraStatsSum = extraStr + extraDex + extraInt;

		if (currentExtraStatsSum + amount > EXTRA_STATS_THRESHOLD) {
			amount = EXTRA_STATS_THRESHOLD - currentExtraStatsSum;
		}

		switch (statType) {
		case 0: 
			birthStr += amount;
			break;
		case 1: 
			birthDex += amount;
			break;
		default: 
			birthInt += amount;
			break;
		}

		//return amount of stat points succesfully added
		return amount;
	}

	public int getMaxHP() {
	//	Debug.Log ("Max HP = " + (getTotalStr () + getLevel () + 50));
		return getMaxHP (getTotalStr (), getLevel ());
	//	return getTotalStr () + getLevel () + 10;
	}
	public static int getMaxHP(int str, int level) {
		return (str * 2) + 70;
	}

	public string toString(){
		int effectiveLevel = getLevel () + levelStatOffset;
	//	Debug.Log (getLevel ());
//		Debug.Log ((int)((breedStr + birthStr + extraStr) * (((float)effectiveLevel)/ MAX_LEVEL)) + STAT_OFFSET);
//		return (katName + " : " + katBreed + " BreedStats: [" + breedStr + "][" + breedDex + "][" + breedInt + "]");

		return (katName + " : " + getGenderString() + breed.ToString() + " [Level: " + getLevel() + "] [STR: " + getTotalStr () + "]" + " [DEX: " + getTotalDex () + "]" + " [INT: " + getTotalInt () + "]" + " [EXP:" + currentExp + "]" );
	}

	public void setHealth (int hp) {
		if (hp <= getMaxHP ()) {
			currentHP = hp;
		} else {
			currentHP = getMaxHP();
		}
	}

	private bool getRandomGender() {
		float randNum = UnityEngine.Random.Range (0.0f, 1.0f);
		if (randNum >= 0.5f)
			return true;
		else
			return false;
	}

	public string getGenderString() {
		return (isMaleGender) ? "♂" : "♀";
	}

	public int getCurrentExp() {
		return currentExp;
	}

	public void setLevelHandicap(int lvlHandicap) {
		levelStatOffset -= lvlHandicap;
//		Debug.LogError ("Stat handicap: " + levelStatOffset);
	}

}
