using UnityEngine;
using System.Collections;

public class KatSelectTrail : MonoBehaviour {

	GameObject kat;
	GameObject trail1, trail2;

	// Use this for initialization
	void Start () {
		trail1 = transform.Find ("Trail").gameObject;
		trail2 = transform.Find ("Trail1").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (kat != null) {
			this.transform.position = kat.transform.position;

		} else {
			this.gameObject.SetActive(false);
		}
	}

	public void attachKat(GameObject obj){
		kat = null;
		kat = obj;
	}

	void OnDisable(){
		kat = null;
	}
}
