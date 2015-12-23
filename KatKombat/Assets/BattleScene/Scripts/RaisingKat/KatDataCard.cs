using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Kommand = Kommands.KommandCode;

public class KatDataCard : MonoBehaviour {
	private Text katName;
	private Text katBreed;
	private Text katSTR,katDEX, katINT;
	private Image currHealth, exp;
	private Text level;
	private GameObject katLocation;
	private KatStatsInfo stats;
	private GameObject hatchButton;

	private SpriteRenderer[] kommandSprites = new SpriteRenderer[3];

	[SerializeField]
	private GameObject katAttackPrefabVessel;

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
		hatchButton = this.transform.Find ("HatchButton").gameObject;
		
		//		Debug.Log (katName.ToString () + katBreed.ToString () + katSTR.ToString () + katDEX.ToString () + katINT.ToString ());
	}

	public void AttachKat(KatStatsInfo katInfo, GameObject katObj) {
		stats = katInfo;
//		InstantiateKatImage (katObj);
		reflectNameAndBreedOfKat ();
		reflectStatsOfKat ();
		reflectHealthAndXP ();
//		Debug.Log ("Reflecting Kommands");
		reflectKommands ();
		hatchButton.SetActive (false);
	}

	public void AttachEgg (EggInfo eggInfo, GameObject eggObj) {
		katName.text = "Egg";
		katBreed.text = "";
		katSTR.text = "???";
		katDEX.text = "???";
		katINT.text = "???";
		currHealth.fillAmount = 1.0f;
		hatchButton.SetActive (true);
		disableAllKommandButtons ();
	}

	void disableAllKommandButtons() {
		for (int i = 0; i < 3; i++) {
			string btnName = "Kommand" + i;
			this.transform.Find(btnName).gameObject.SetActive(false);
		}
	}

	void reflectKommands() {
		//loop through kat kommands, set sprite then enable kommand
		List<Kommand> katKommands = stats.getActiveKommands ();

		KatAttacksVessel kommandsScript = katAttackPrefabVessel.GetComponent<KatAttacksVessel> ();
		stats.setKommands (Kommand.ArcanePulse, Kommand.None, Kommand.Furball);

		for (int i = 0; i < 3; i++) {
			if (katKommands[i] != Kommand.None){
				string btnName = "Kommand" + i;
	//			Debug.Log(this.transform.Find(btnName).Find("Sprite").GetComponent<SpriteRenderer>().ToString());
				kommandSprites[i] = this.transform.Find(btnName).transform.Find("Sprite").GetComponent<SpriteRenderer>();
	//			Debug.Log(kommandSprites[i].ToString());
				kommandSprites[i].sprite = kommandsScript.findIcon(katKommands[i]);
				kommandSprites[i].transform.parent.gameObject.SetActive(true);
				//		kommandSprites[i].sprite
			}
		}
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
		stats.setCurrentHP (35);
		float currHP = (float)stats.currentHP;
		float maxHP = (float) stats.getMaxHP ();
		
//		Debug.Log (currHP + " over " + maxHP);
		float hpRatio = currHP / maxHP;
		currHealth.fillAmount = hpRatio;
	}
}
