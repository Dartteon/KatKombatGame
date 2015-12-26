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
	private Text hpText;
	private List<Kommand> katKommands;

	public GameObject objectBeingFollowed { get; private set; }

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
		hpText = this.transform.Find ("HPText").transform.Find ("Text").GetComponent<Text> ();
		
		//		Debug.Log (katName.ToString () + katBreed.ToString () + katSTR.ToString () + katDEX.ToString () + katINT.ToString ());
	}

	public void AttachKat(KatStatsInfo katInfo, GameObject katObj) {
//		Debug.Log ("Attaching Kat to DataCard " + katInfo.toString ());
		stats = katInfo;
//		InstantiateKatImage (katObj);
		reflectNameAndBreedOfKat ();
		reflectStatsOfKat ();
		reflectHealthAndXP ();
//		Debug.Log ("Reflecting Kommands");
		reflectKommands ();
		objectBeingFollowed = katObj;
		hatchButton.SetActive (false);
	}

	public void AttachEgg (EggInfo eggInfo, GameObject eggObj) {
		katName.text = "Egg";
		katBreed.text = "Ready to hatch!";
		katSTR.text = "??";
		katDEX.text = "??";
		katINT.text = "??";
		hpText.text = "???";
		currHealth.fillAmount = 1.0f;
		hatchButton.SetActive (true);
		objectBeingFollowed = eggObj;
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
		katKommands = stats.getActiveKommands ();

		KatAttacksVessel kommandsScript = katAttackPrefabVessel.GetComponent<KatAttacksVessel> ();
//		stats.setKommands (Kommand.ArcanePulse, Kommand.None, Kommand.Furball);

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
		
		katBreed.text = "Lv." + stats.getLevel() + "   " + stats.getGenderString() + "  " + stats.breed.ToString ();
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
		/*
		stats.setCurrentHP (35);
		float currHP = (float)stats.currentHP;
		float maxHP = (float) stats.getMaxHP ();
		
//		Debug.Log (currHP + " over " + maxHP);
		float hpRatio = currHP / maxHP;
		currHealth.fillAmount = hpRatio;
		*/
		hpText.text = stats.getMaxHP ().ToString();
		currHealth.fillAmount = 1.0f;
	}

	public void enableKommandDescriptionBox (int index) {
		disableAllKommandButtons ();
		switch (index) {
		case 0:
			Debug.Log("Kommandbox 0 activated");
			break;
		case 1:
			Debug.Log("Kommandbox 1 activated");
			break;
		default:
			Debug.Log("Kommandbox 2 activated");
			break;
		}
	}

	public void disableAllKommandDescriptionBoxes () {
		
		Debug.Log("Disabling all Kommandboxes");
	}
}
