using UnityEngine;
using System.Collections;

public class SelectedKatButtonOptionsScript : MonoBehaviour {
	private KatStatsInfo attachedKat;



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void attachKat(KatStatsInfo kat){
		this.attachedKat = kat;
	}
}
