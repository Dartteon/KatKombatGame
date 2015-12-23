using UnityEngine;
using System.Collections;

public class EggPrefabVessel : MonoBehaviour {
	public GameObject[] eggPrefabs;

	public GameObject getEgg(EggInfo e) {
		return eggPrefabs [0]; //stub
	}

}
