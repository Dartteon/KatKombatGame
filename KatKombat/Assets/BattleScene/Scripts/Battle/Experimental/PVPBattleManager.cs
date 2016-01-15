using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Kommand = Kommands.KommandCode;

public class PVPBattleManager : MonoBehaviour {
	
	public GameObject katPrefabVessel;
	public GameObject katAttackVesselPrefab;
	
	public GameObject controllerPrefab;
	public GameObject[] mapPrefabs;
//	public GameObject gamestateBattleManagerModulePrefab;
	
	private int mapIndex = 0;
	private GameObject spawnLoc_player1, spawnLoc_player2;

	private GameObject kat1, kat2;

//	private GameObject gamestateBattleManagerModule;

	// Use this for initialization
	void Start () {
		initialize ();
	}

	void initialize() {
		spawnMap ();
		createKats ();
		AttachKatsToController ();
	}

	void spawnMap() {
		GameObject map = Instantiate (mapPrefabs [0]) as GameObject;
		spawnLoc_player2 = map.transform.Find ("PlayerSpawnLocation").gameObject;
		spawnLoc_player1 = map.transform.Find ("EnemySpawnLocation").gameObject;
	//	Debug.Log (spawnLoc_player1.ToString () + spawnLoc_player2.ToString ());
	}

	void createKats() {
		Debug.Log (katPrefabVessel.ToString ());
		KatStatsInfo katStats1 = new KatStatsInfo (KatBreed.getRandomBreed (), katPrefabVessel, "Player1Kat");
		KatStatsInfo katStats2 = new KatStatsInfo (KatBreed.getRandomBreed (), katPrefabVessel, "Player2Kat");
		katStats1.setKommands (Kommands.KommandCode.FunBasketFurball, Kommands.KommandCode.None, Kommands.KommandCode.None);
		katStats2.setKommands (Kommands.KommandCode.FunBasketFurball, Kommands.KommandCode.None, Kommands.KommandCode.None);

		kat1 = Instantiate (katPrefabVessel.GetComponent<KatPrefabsVesselScript> ().getKatOfBreed (katStats1.breed), spawnLoc_player1.transform.position, spawnLoc_player1.transform.rotation) as GameObject;
		kat2 = Instantiate (katPrefabVessel.GetComponent<KatPrefabsVesselScript> ().getKatOfBreed (katStats2.breed), spawnLoc_player2.transform.position, spawnLoc_player2.transform.rotation) as GameObject;
		kat1.tag = "Player1";
		kat2.tag = "Player2";
//		kat1.GetComponent<StatsScript> ().setStats (katStats1);
//		kat2.GetComponent<StatsScript> ().setStats (katStats2);
		kat1.GetComponent<StatsScript> ().setFunStats ();
		kat2.GetComponent<StatsScript> ().setFunStats ();
		attachAttackToKat (kat1, katStats1);
		attachAttackToKat (kat2, katStats2);
		Camera.main.GetComponent<PVPBattleCameraFollowScript> ().AttachKats (kat1, kat2);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void AttachKatsToController() {
		
		GameObject controller = Instantiate (controllerPrefab) as GameObject;
//		controller.transform.parent = Camera.main.transform;
		controller.transform.Find ("Controller1").GetComponent<MTMainController> ().AttachKat (kat1);
		controller.transform.Find ("Controller2").GetComponent<MTMainController> ().AttachKat (kat2);
	}

	
	void attachAttackToKat(GameObject kat, KatStatsInfo katInfo){
		List<Kommand> komList = katInfo.getActiveKommands ();
		Debug.LogError("Attaching Koms to Kat : " + komList[0].ToString() + " " + komList[1].ToString() + " " + komList[2].ToString() + " ");
		//string[] katAttacks = katInfo.getCommands ();
		
		KatAttacksVessel attackPrefabsScript = katAttackVesselPrefab.GetComponent<KatAttacksVessel>();
		KatProjectileLauncherScript[] projectileLauncherScripts = new KatProjectileLauncherScript[3];
		projectileLauncherScripts[0] = kat.transform.Find ("BasicAttack").GetComponent<KatProjectileLauncherScript> ();
		projectileLauncherScripts[1] = kat.transform.Find ("SecondAttack").GetComponent<KatProjectileLauncherScript> ();
		projectileLauncherScripts[2] = kat.transform.Find ("ThirdAttack").GetComponent<KatProjectileLauncherScript> ();
		
		for (int i=0; i<3; i++) {
			if (!komList[i].Equals(Kommand.None)) {
				GameObject atkPrefab = attackPrefabsScript.findKommand(komList[i]);
				projectileLauncherScripts[i].SetPrefab(atkPrefab);
				//				Debug.Log("Launcher set: " + atkPrefab.ToString());
				projectileLauncherScripts[i].gameObject.SetActive(true);
				//				Debug.Log("Kommand found: " + komList[i].ToString());
			} else {
				Debug.LogError("Kommand not found! " + komList[i]);
				projectileLauncherScripts[i].SetInactive();
			}
		} 
	}
}
