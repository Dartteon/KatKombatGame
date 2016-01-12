using UnityEngine;
using System.Collections;
using KommandCode = Kommands.KommandCode;

public class BattleInitiatorScript : MonoBehaviour {
	private string BATTLE_STAGE_NAME = "BattleSceneNew";

//	public string enemyKatBreed;
	public KatBreed.Breed enemyKatBreed;
	public int enemyKatLevel;
//	public string enemyKatCommand1, enemyKatCommand2, enemyKatCommand3;

	public KommandCode kom1, kom2, kom3;
	public GameObject katPrefabsVessel;

	private GameObject[] katPrefabs;

	private AdventureManager advManager;

	void findAdvManagerInScene(){
		if (advManager == null) {
			advManager = GameObject.Find ("AdventureModule").GetComponent<AdventureManager> ();
		}
	}

	void Start(){
		katPrefabs = katPrefabsVessel.GetComponent<KatPrefabsVesselScript> ().katPrefabs;
	}

	void OnMouseDown(){
		findAdvManagerInScene ();
		KatStatsInfo newEnemy = new KatStatsInfo (enemyKatBreed, katPrefabsVessel, "Gary");
//		newEnemy.setCommands (enemyKatCommand1, enemyKatCommand2, enemyKatCommand3);
//		Debug.LogError ("Kommands : " + kom1.ToString () + " " + kom2.ToString () + " " + kom3.ToString ());
		newEnemy.setKommands (kom1, kom2, kom3);
		newEnemy.setLevel (enemyKatLevel);
		Debug.Log ("[BattleInitiator] Created Enemy : " + newEnemy.toString ());
		advManager.initiateBattleWithKat (newEnemy);
	}
}
