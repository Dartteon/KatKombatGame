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

	public void initiate() {
		katName = this.transform.Find ("KatName").transform.Find("Text").GetComponent<Text> ();
//		Debug.Log ("init: " + this.transform.Find ("KatName").ToString ());
		katBreed = this.transform.Find ("KatBreed").transform.Find("Text").GetComponent<Text> ();
		katSTR = this.transform.Find ("KatStr").transform.Find("Text").GetComponent<Text> ();
		katDEX = this.transform.Find ("KatDex").transform.Find("Text").GetComponent<Text> ();
		katINT = this.transform.Find ("KatInt").transform.Find("Text").GetComponent<Text> ();
		//		level = this.transform.Find ("KatLevel").GetComponentInChildren<Text> ();
		
		currHealth = this.transform.Find ("CurrentHealthCanvas").transform.Find("Bar").GetComponent<Image> ();
		exp = this.transform.Find ("ExpCanvas").transform.Find("Bar").GetComponent<Image> ();
		
		//		Debug.Log (katName.ToString () + katBreed.ToString () + katSTR.ToString () + katDEX.ToString () + katINT.ToString ());
	}


	public void AttachKat(KatStatsInfo katInfo, GameObject katObj) {
		stats = katInfo;
//		InstantiateKatImage (katObj);
		reflectNameAndBreedOfKat ();
		reflectStatsOfKat ();
		reflectHealthAndXP ();
	}

	void reflectNameAndBreedOfKat() {
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

	void reflectHealthAndXP() {
		float currHP = (float)stats.currentHP;
		float maxHP = (float) stats.getMaxHP ();
		
//		Debug.Log (currHP + " over " + maxHP);
		float hpRatio = currHP / maxHP;
		currHealth.fillAmount = hpRatio;
	}
}
