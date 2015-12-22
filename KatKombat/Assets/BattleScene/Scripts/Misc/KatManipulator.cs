using UnityEngine;
using System.Collections;

public class KatManipulator : MonoBehaviour {

	public static string getKatShortDescription (KatStatsInfo kat) {
		string katName = kat.getName ();
		int katLevel = kat.getLevel ();
//		string katBreed = kat.getBreed ();
		return "[Lv." + katLevel + "] " + katName;
	}
}
