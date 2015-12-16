using UnityEngine;
using System.Collections;

public class EggTray : MonoBehaviour {
	private Vector2[] eggLocs = new Vector2[8];

	private GameObject[] eggs = new GameObject[8];

	void Start(){
		initialize ();
	}

	void initialize(){
		for (int i=0; i<eggLocs.Length; i++) {
			string locName = "Loc" + i;
			eggLocs[i] = transform.Find(locName).transform.localPosition;
		}
	}

	int findEmptySlot(){
		for (int i=0; i<eggs.Length; i++){
			if (eggs[i] == null){
				return i;
			}
		}

		return -1;
	}

	public bool addEggToTray(GameObject newEgg){
		int emptySlotIndex = findEmptySlot ();
		if (emptySlotIndex != -1) {
			newEgg.transform.parent = this.transform;
			newEgg.transform.localPosition = eggLocs [emptySlotIndex];
			eggs[emptySlotIndex] = newEgg;
			return true;
		} else {
			return false;
		}
	}

}
