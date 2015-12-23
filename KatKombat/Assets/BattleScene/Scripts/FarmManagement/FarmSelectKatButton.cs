using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FarmSelectKatButton : MonoBehaviour {
	private Text nameText;
	private SpriteRenderer face;
	private AdventureManager advMngr;
	private FarmManager farmMngr;
	private GameObject kat;
	private string katName;

	void Start () {
		advMngr = GameObject.Find ("AdventureModule").GetComponent<AdventureManager> ();
		farmMngr = GameObject.Find ("FarmManagerModule").GetComponent<FarmManager> ();
	}

	public void initialize(AdventureManager adventureManager, FarmManager farmManager, GameObject representedKat, string katName) {
		advMngr = adventureManager;
		farmMngr = farmManager;
		kat = representedKat;
		setButtonSprite ();
//		setKatName (katName);
		this.gameObject.SetActive (true);
	}

	void setButtonSprite() {
		Sprite katFace = kat.transform.Find ("Sprite").transform.Find ("Head").GetComponent<SpriteRenderer> ().sprite;
		Debug.Log (katFace.ToString ());

		if (katFace != null) {
			this.transform.Find ("FaceSprite").GetComponent<SpriteRenderer> ().sprite = katFace;
		}
	//	this.gameObject.name = kat.name;
	}

	void setKatName(string name) {
		katName = name;
		this.transform.Find ("KatName").transform.Find ("Text").GetComponent<Text> ().text = name;
	}

	void OnMouseDown() {
		farmMngr.followThisKat (kat);
	}
}
