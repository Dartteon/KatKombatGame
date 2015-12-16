using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class KatDataCard : MonoBehaviour {
	private Text katName;
	private Text katBreed;
	private Text katSTR,katDEX, katINT;
	private Image currHealth, exp;
	private Text level;
	private GameObject katLocation;
	private KatStatsInfo stats;

	void Awake () {
		initiate ();
	}

	void initiate() {
		katName = this.transform.Find ("KatName").GetComponentInChildren<Text> ();
		katBreed = this.transform.Find ("KatBreed").GetComponentInChildren<Text> ();
		katSTR = this.transform.Find ("KatStr").GetComponentInChildren<Text> ();
		katDEX = this.transform.Find ("KatDex").GetComponentInChildren<Text> ();
		katINT = this.transform.Find ("KatInt").GetComponentInChildren<Text> ();
		//		level = this.transform.Find ("KatLevel").GetComponentInChildren<Text> ();
		
		currHealth = this.transform.Find ("CurrentHealthCanvas").GetComponentInChildren<Image> ();
		exp = this.transform.Find ("ExpCanvas").GetComponentInChildren<Image> ();
		
		//		Debug.Log (katName.ToString () + katBreed.ToString () + katSTR.ToString () + katDEX.ToString () + katINT.ToString ());
	}


	public void AttachKat(KatStatsInfo katInfo, GameObject katObj) {
		if (katName == null) {
			initiate();
		}
		stats = katInfo;
//		InstantiateKatImage (katObj);
		reflectNameAndBreedOfKat ();
		reflectStatsOfKat ();

	}

	void reflectNameAndBreedOfKat() {
	//	Debug.Log (stats.getName ());
//		Debug.Log (katName.ToString ());
		katName.text = stats.getName ();
		katBreed.text = "Lv." + stats.getLevel() + "   " + stats.getBreed ();
	}

	void reflectStatsOfKat() {
		katSTR.text = stats.getTotalStr ().ToString();
		katDEX.text = stats.getTotalDex ().ToString();
		katINT.text = stats.getTotalInt ().ToString();
//		level.text = stats.getLevel ().ToString();
	}

	void InstantiateKatImage(GameObject kat) {
		GameObject katImage = Instantiate (kat, katLocation.transform.position, katLocation.transform.rotation) as GameObject;
		katImage.transform.parent = this.transform;
		katImage.transform.Find ("CanvasHealthBar").gameObject.SetActive (false);
	}
}
