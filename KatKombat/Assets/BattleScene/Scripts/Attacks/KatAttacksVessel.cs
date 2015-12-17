using UnityEngine;
using System.Collections;
using Kommand = Kommands.KommandCode;

public class KatAttacksVessel : MonoBehaviour {
	public GameObject[] katAttackPrefabs;
	public GameObject[] attackIconPrefabs;	

	GameObject[] getAttackPrefabs(){
		return katAttackPrefabs;
	}

	/*
	public GameObject findAttackWithName (string attackName){
		for (int i=0; i<katAttackPrefabs.Length; i++){
			if (attackName.Equals(katAttackPrefabs[i].name)){
				return katAttackPrefabs[i];
			}
		}
		
		return katAttackPrefabs [0];
	}
*/
	public GameObject findKommand (Kommand k) {
		for (int i = 0; i < katAttackPrefabs.Length; i++) {
			Kommand atk = katAttackPrefabs[i].GetComponent<GeneralProjectileScript>().kommand;
			if (k.Equals(atk)){
				return katAttackPrefabs[i];
				break;
			}
		}

		//not found
		Debug.LogError("Kommand not found: " + k.ToString());
		return katAttackPrefabs [0];
	}
}
