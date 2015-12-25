using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BattleInformation : MonoBehaviour {
	public KatStatsInfo playerKat, enemyKat;
	public string mapName;
	public GameObject katPrefabVessel { get; private set; }

	public void setKats(KatStatsInfo player_kat, KatStatsInfo enemy_kat){
		playerKat = player_kat;
		enemyKat = enemy_kat;
	}

	public void setKatPrefabVessel(GameObject prefabVessel){
		katPrefabVessel = prefabVessel;
	}

	public void setMap(string map_name){
		mapName = map_name;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Start(){
		this.name = "BattleInformationModule";
		DontDestroyOnLoad(transform.gameObject);
	}
}
