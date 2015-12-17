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
