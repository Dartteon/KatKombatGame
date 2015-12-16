using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BattleInformation : MonoBehaviour {
	public KatStatsInfo playerKat, enemyKat;
	public string mapName;
	private GameObject[] katPrefabs;

	public void setKats(KatStatsInfo player_kat, KatStatsInfo enemy_kat){
		playerKat = player_kat;
		enemyKat = enemy_kat;
	}

	public void setKatPrefabs(GameObject[] prefabs){
		katPrefabs = prefabs;
	}

	public GameObject[] getKatPrefabs(){
		return katPrefabs;
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
