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

	public Sprite findIcon (Kommand k) {
		GameObject kommand = Instantiate(findKommand (k));
		Sprite sprite = kommand.transform.Find("Icon").GetComponent<SpriteRenderer> ().sprite;
		//	kommand.gameObject.Find ("Icon").GetComponent<SpriteRenderer> ().sprite;
		Destroy (kommand);
		return sprite;
	}

	public string getKommandDescription(Kommand k) {
		return findKommand(k).GetComponent<GeneralProjectileScript>().commandDescription;
	}

	public string getCooldownInString(Kommand k) {
		string cd = findKommand (k).GetComponent<GeneralProjectileScript> ().cooldown.ToString ();
		return cd;
	}

	public string getPowerInString(Kommand k) {
		return findKommand (k).GetComponent<GeneralProjectileScript> ().displayPower.ToString ();
	
	}

public string getKommandName (Kommand k) {
		switch (k) {
		case Kommand.ArcanePulse:
			return "Arcane Pulse";
			break;
		case Kommand.GlacialTail:
			return "Glacial Tail";
			break;
		case Kommand.LightningHelix:
			return "Lightning Helix";
			break;
		case Kommand.SplinterBarrage:
			return "Splinter Barrage";
			break;
		case Kommand.TwinFang:
			return "Twin Fang";
			break;

		default:
			return k.ToString ();
			break;
		}
	}
}
