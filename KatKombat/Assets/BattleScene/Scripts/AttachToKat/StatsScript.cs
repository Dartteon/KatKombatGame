using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Kommand = Kommands.KommandCode;

public class StatsScript : MonoBehaviour {
	public int katID;
	public int Str, Dex, Int;
	public Color col;
	public bool isBusy = false;
	public int statType;
	public int level = 100;
//	public string defaultCommands = "Furball MightyPaw TwinFang";
	private List<Kommand> kommands;
	public KatStatsInfo katStatInfo { get; private set; }


	public char statusEffect = 'n';
	private readonly float MAXLEVEL = 100;

	[SerializeField]
	public KatBreed.Breed breed;

	//n(nil), f(freeze), b(burn), s(stun), x(bleed), p(poison)

	// Use this for initialization
//	void Start () {
//		isBusy = false;
//		transform.FindChild ("TargetRing").gameObject.GetComponent<SpriteRenderer> ().color = col;
//		defaultCommands = "Furball MightyPaw TwinFang";
	//	setStatsToLevel (level);
//	}

	public void setKatStatsInfo(KatStatsInfo info){
		katStatInfo = info;
	}

	public void setKatCommands(List<Kommand> koms){
		kommands = koms;
	}

	public void setStatsToLevel(int setLevel){		
		Debug.Log ("Setting stats to level now!! <THIS SHOULD BE SET BY AN EXTERNAL CALLER, THOUGH>");
		Str = (int)((float)Str * (float)level / MAXLEVEL);
		Dex = (int)((float)Dex * (float)level / MAXLEVEL);
		Int = (int)((float)Int * (float)level / MAXLEVEL);
	}

	public void setToLevel(int setLevel){
		this.level = setLevel;
		//setStatsToLevel (setLevel);
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void setStats(int newStr, int newDex, int newInt){
//		Debug.Log ("Set stats:   [ " + this.name.Replace("(Clone)", "") + " ]   STR[" + newStr + "]  DEX[" + newDex + "]   INT[" + newInt + "]");
		Str = newStr;
		Dex = newDex;
		Int = newInt;
	}

	public int getLevel(){
		return level;
	}

	public int getStr(){
		return Str;
	}

	public int getDex(){
		return Dex;
	}

	public int getInt(){
		return Int;
	}

	public Color getColor(){
		return col;
	}

	public int getID(){
		return katID;
	}

	public bool GetIsBusy(){
		return isBusy;
	}

	public void SetBusy(bool busyness){
		isBusy = busyness;
	}

	public void setStats(KatStatsInfo katStatsInfo){
		Str = katStatsInfo.getTotalStr ();
		Dex = katStatsInfo.getTotalDex ();
		Int = katStatsInfo.getTotalInt ();
		level = katStatsInfo.getLevel ();
//		Debug.Log ("Initializing HP Script for " + this.gameObject.name);
		transform.GetComponent<HealthScript> ().initialize ();
	}

	public void setFunStats() {
		Str = 0;
		Dex = 0;
		Int = 0;
		level = 1;
		transform.GetComponent<HealthScript> ().initialize ();
		transform.GetComponent<HealthScript> ().setMaxHealth (140);
		transform.GetComponent<MovementScript> ().SetForce (5);
	}
}
