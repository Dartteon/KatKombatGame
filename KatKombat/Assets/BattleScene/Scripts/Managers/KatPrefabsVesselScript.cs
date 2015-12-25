using UnityEngine;
using System.Collections;

public class KatPrefabsVesselScript : MonoBehaviour {
	public GameObject[] katPrefabs;

	public GameObject getKatOfBreed (KatBreed.Breed b) {
		for (int i = 0; i<katPrefabs.Length; i++) {
			KatBreed.Breed searchedBreed = katPrefabs[i].GetComponent<StatsScript>().breed;
//			Debug.Log(searchedBreed.ToString());

			if (b.Equals (katPrefabs[i].GetComponent<StatsScript>().breed))
			    return katPrefabs[i];

		}
		Debug.LogError("KatBreed " + b.ToString() + " not found!");
		return katPrefabs[0];

	}
}
