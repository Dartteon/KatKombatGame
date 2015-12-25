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
	public string playerUsername { get; private set; }
	public int playerCurrency { get; private set; }
	public int playerGems  { get; private set; }
	public List<KatStatsInfo> ownedKats { get; private set; }
	public List<EggInfo> ownedEggs { get; private set; }
	public List<Item> ownedItems { get; private set; }

	public int arcadeHighscore { get; private set; }
	public int playerExp { get; private set; }
	public int playerPveWins { get; private set; }
	public int playerPveLosses { get; private set; }
	public int playerPvpWins { get; private set; }
	public int playerPvpLosses { get; private set; }

	public DateTime time_lastActive { get; private set; }

	//Constructor
	public PlayerInformation(string username){
//		Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER","yes");
		ownedKats = new List<KatStatsInfo>();
		ownedEggs = new List<EggInfo>();
		ownedItems = new List<Item>();
		playerUsername = username;
		playerCurrency = 0;
		arcadeHighscore = 0;
		playerExp = 0;
		playerPvpWins = 0;
		playerPvpLosses = 0;
		playerPveWins = 0;
		playerPveLosses = 0;
	}

	public void addKatToInventory(KatStatsInfo newKat){
		ownedKats.Add (newKat);
//		Debug.Log ("Added to Kat Inventory: [" + ownedKats.Count + "]");
	}

	public void addEggToInventory(EggInfo newEgg){
		ownedEggs.Add (newEgg);
//		Debug.Log ("Added to Egg Inventory: [" + newEgg.eggToString () + "]");
	}

	public void addCurrency(int amount){
		playerCurrency += amount;
	}
	public bool deductCurrency (int amount) {
		if (playerCurrency >= amount) {
			playerCurrency -= amount;
			return true;
		} else
			return false;
	}

	public void UpdateDateTime(DateTime t) {
//		Debug.Log ("Updating time : " + time_lastActive.ToString ());
		time_lastActive = t;
	}

	public void extractDataFrom(PlayerInformation clone){
		this.playerUsername = clone.playerUsername;
		this.playerCurrency = clone.playerCurrency;
		this.ownedKats = clone.ownedKats;
	}

	public string playerToString() {
		return "[" + playerUsername + "] [" + playerCurrency + " Kash] [" + ownedKats.Count + " Kats] [" + ownedEggs.Count + " Eggs] [Last Login : " + time_lastActive.ToString() + "]";
	}
}
