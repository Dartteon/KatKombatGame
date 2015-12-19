using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;

[Serializable]
public class PlayerInformation{
	public string playerUsername;
	public int playerCurrency;
	public List<KatStatsInfo> ownedKats = new List<KatStatsInfo>();
	public List<EggInfo> ownedEggs = new List<EggInfo>();

	public int arcadeHighscore;
	public int playerLevel { get; private set; }

	//Constructor
	public PlayerInformation(string username){
//		Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER","yes");
		playerUsername = username;
		playerCurrency = 0;
		ownedKats = new List<KatStatsInfo> ();
		arcadeHighscore = 0;
	}

	public void addKatToInventory(KatStatsInfo newKat){
//		Debug.Log (newKat.toString ());
		//KatStatsInfo clonedKat = newKat.clone ();
		//KatStatsInfo kat = new KatStatsInfo ("AzureGreye");
		//ownedKats.Add (newKat);

		//KatStatsInfo kat = new KatStatsInfo ("AzureGreye");
		ownedKats.Add (newKat);
		Debug.Log ("Added to Kat Inventory: [" + ownedKats.Count + "]");
	}

	public void addEggToInventory(EggInfo newEgg){
		ownedEggs.Add (newEgg);
		Debug.Log ("Added to Egg Inventory: [" + newEgg.eggToString () + "]");
	}

	public void addCurrency(int amount){
		playerCurrency += amount;
	}

	void Awake(){
		Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER","yes");
	}

	public void extractDataFrom(PlayerInformation clone){
		this.playerUsername = clone.playerUsername;
		this.playerCurrency = clone.playerCurrency;
		this.ownedKats = clone.ownedKats;
	}

	public string playerToString() {
		return "[" + playerUsername + "] [" + playerCurrency + " Kash] [" + ownedKats.Count + " Kats] [" + ownedEggs.Count + " Eggs]";
	}
}
