using UnityEngine;
using System.Collections;
using System;
using EggType = KatBreed.EggType;

[Serializable]
public class EggInfo {
	private KatStatsInfo katInside;
	private EggType eggType;
	private DateTime conceptionTime;

	private EggInfo (KatStatsInfo egg, EggType type) {
		katInside = egg;
		eggType = type;
		conceptionTime = System.DateTime.Now;
	}

	public EggType getEggType() {
		return eggType;
	}

	public string eggToString(){
		string information = "[EGG] ";
		string katInformation = katInside.toString ();
		return information + katInformation;
	}

	public KatStatsInfo getKat(){
		return katInside;
	}

	public static EggInfo getNewEgg (KatStatsInfo egg, EggType type) {
		EggInfo newEgg = new EggInfo(egg, type);
		return newEgg;
	}
}