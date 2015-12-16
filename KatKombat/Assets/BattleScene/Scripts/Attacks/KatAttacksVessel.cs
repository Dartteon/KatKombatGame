using UnityEngine;
using System.Collections;

public class KatAttacksVessel : MonoBehaviour {
	public GameObject[] katAttackPrefabs;
	public GameObject[] attackIconPrefabs;

	GameObject[] getAttackPrefabs(){
		return katAttackPrefabs;
	}

	public GameObject findAttackWithName (string attackName){
		for (int i=0; i<katAttackPrefabs.Length; i++){
			if (attackName.Equals(katAttackPrefabs[i].name)){
				return katAttackPrefabs[i];
			}
		}
		
		return katAttackPrefabs [0];
	}

	public GameObject findIconWithName(string attackName){
		for (int i=0; i<attackIconPrefabs.Length; i++){
			if (attackName.Equals(attackIconPrefabs[i].name)){
				return attackIconPrefabs[i];
			}
		}

		return attackIconPrefabs [0];
	}
}
