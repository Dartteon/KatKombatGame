using UnityEngine;
using System.Collections;

public class GameVariables {
	public static bool DEBUG_MODE = true;

	public static string[] possibleKatNamesPart1 = {
		"Kro",
		"Var",
		"Rek",
		"Kra",
		"Cho",
		"Vel",
		"Shy",
		"Shar",
		"Mao",
		"Mo",


	};

	
	
	
	public static string[] possibleKatNamesPart2 = {
		"shushu",
		"ruga",
		"dane",
		"meth",
		"riel",
		"gas",
		"rall",
		"lia",
		"mao",
	};

	public static string getRandomName() {
		int part1Index = Random.Range (0, possibleKatNamesPart1.Length-1);
		int part2Index = Random.Range (0, possibleKatNamesPart2.Length-1);
		return possibleKatNamesPart1 [part1Index] + possibleKatNamesPart2 [part2Index];
	}
}
