using UnityEngine;
using System.Collections;

public class Kommands {
	public enum KommandCode {
		None,
		ArcanePulse,
		Charge,
		Earthquake,
		Enrage,
		Furball,
		GlacialTail,
		Lazer,
		LightningHelix,
		Meditate,
		MightyPaw,
		Ponder,
		Rend,
		SplinterBarrage,
		Tornado,
		TwinFang,

	}

	public static KommandCode getRandomStrKommand() {
//		Debug.Log (Random.seed + " RANDOM SEED ------------------------------");
		KommandCode[] strKommands = 
			{KommandCode.Charge, KommandCode.Earthquake, KommandCode.GlacialTail, KommandCode.MightyPaw};

		int index = Random.Range(0, strKommands.Length);
		return strKommands [index];
	}

	public static KommandCode getRandomDexKommand() {
		KommandCode[] dexKommands = 
			{KommandCode.Rend, KommandCode.TwinFang, KommandCode.SplinterBarrage, KommandCode.Tornado};
		
		int index = Random.Range(0, dexKommands.Length);
		return dexKommands [index];
	}

	public static KommandCode getRandomIntKommand() {
		KommandCode[] intKommands = 
			{KommandCode.Lazer, KommandCode.Furball, KommandCode.ArcanePulse, KommandCode.LightningHelix};
		
		int index = Random.Range(0, intKommands.Length);
		return intKommands [index];
	}

	public static string getStringRepOfKommand (KommandCode k) {
		/*
		switch (k) {
		case KommandCode.ArcanePulse:
			return "ArcanePulse";
			break;
		default:
			return "ERROR";
			break;
		}
		*/

		return k.ToString ();
	}
}
