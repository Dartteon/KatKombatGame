using UnityEngine;
using System.Collections;

public class AncestorPair {
	public KatStatsInfo male { get; private set;}
	public KatStatsInfo female { get; private set;}

	public void setPair (KatStatsInfo maleKat, KatStatsInfo femaleKat) {
		male = maleKat;
		female = femaleKat;
	}

}
