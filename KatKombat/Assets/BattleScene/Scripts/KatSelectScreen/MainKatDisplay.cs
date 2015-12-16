using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainKatDisplay : MonoBehaviour {
	SpriteRenderer katFace;
	SpriteRenderer typeDisplay;

	SpriteRenderer incomingKatFace;
	SpriteRenderer incomingTypeDisplay;
	string incomingKatName;
	int incomingKatType;
	Text katDisplayName;
	Text katSTR;
	Text katDEX;
	Text katINT;
	Image katStrBar;
	Image katDexBar;
	Image katIntBar;

	GameObject incomingKat;

	GameObject selectAnimation;
	GameObject selectAnimationRed;
	GameObject selectAnimationGreen;
	GameObject selectAnimationBlue;

	public float maxStatBarWidth = 120.0f;

	// Use this for initialization
	void Start () {
	}
	void getComponentsInSelf(){
		katFace = transform.Find ("KatHead").gameObject.GetComponent<SpriteRenderer> ();
		typeDisplay = transform.Find ("Circle").gameObject.GetComponent<SpriteRenderer> ();
		selectAnimation = transform.Find ("KatFaceChosenEffect").gameObject;
		selectAnimationRed = transform.Find ("KatFaceChosenEffectRed").gameObject;
		selectAnimationGreen = transform.Find ("KatFaceChosenEffectGreen").gameObject;
		selectAnimationBlue = transform.Find ("KatFaceChosenEffectBlue").gameObject;
		katDisplayName = transform.Find("KatNameCanvas").transform.GetComponentInChildren<Text> ();
		
		katSTR = transform.Find("STRCanvas").transform.GetComponentInChildren<Text> ();
		katDEX = transform.Find("DEXCanvas").transform.GetComponentInChildren<Text> ();
		katINT = transform.Find("INTCanvas").transform.GetComponentInChildren<Text> ();
		
		katStrBar = transform.Find("CanvasSTRBar").transform.GetComponentInChildren<Image> ();
		katDexBar = transform.Find("CanvasDEXBar").transform.GetComponentInChildren<Image> ();
		katIntBar = transform.Find("CanvasINTBar").transform.GetComponentInChildren<Image> ();

	}
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D obj){
		getComponentsInSelf ();
		Destroy (obj.gameObject);
		katFace.sprite = incomingKatFace.sprite;
		typeDisplay.sprite = incomingTypeDisplay.sprite;
		triggerSelectAnimation ();
		katDisplayName.text = incomingKatName;
		int katStr = incomingKat.GetComponent<StatsScript> ().getStr ();
		int katDex = incomingKat.GetComponent<StatsScript> ().getDex ();
		int katInt = incomingKat.GetComponent<StatsScript> ().getInt ();

		katSTR.text = katStr.ToString();
		katDEX.text = katDex.ToString();
		katINT.text = katInt.ToString();

		katStrBar.fillAmount = katStr / maxStatBarWidth;
		katDexBar.fillAmount = katDex / maxStatBarWidth;
		katIntBar.fillAmount = katInt / maxStatBarWidth;
	}


	public void setIncomingKatObject(GameObject kat){
		incomingKat = kat;
		transform.Find ("KatHead").gameObject.GetComponent<SpriteRenderer> ().sprite = kat.transform.Find ("Sprite").Find ("Head").GetComponent<SpriteRenderer> ().sprite;
		string incomingName = incomingKat.name;
		//Debug.Log (incomingName);
		incomingName.Remove(incomingName.IndexOf('('));
		//Debug.Log(incomingName.Remove(incomingName.IndexOf('(')));
		transform.Find ("KatNameCanvas").Find ("KatName").GetComponent<Text> ().text = 
			incomingName.Remove(incomingName.IndexOf('('));
		int katStr = incomingKat.GetComponent<StatsScript> ().getStr ();
		int katDex = incomingKat.GetComponent<StatsScript> ().getDex ();
		int katInt = incomingKat.GetComponent<StatsScript> ().getInt ();
		
//		Debug.Log ("Setting mainDisplay stats now");
		transform.Find("STRCanvas").Find("STRText").transform.GetComponent<Text> ().text = katStr.ToString();
		transform.Find("DEXCanvas").Find("DEXText").transform.GetComponent<Text> ().text = katDex.ToString();
		transform.Find("INTCanvas").Find("INTText").transform.GetComponent<Text> ().text = katInt.ToString();
		
		transform.Find("CanvasSTRBar").Find("RedDelayBar").transform.GetComponent<Image> ().fillAmount = katStr / maxStatBarWidth;
		transform.Find("CanvasDEXBar").Find("RedDelayBar").transform.GetComponent<Image> ().fillAmount = katDex / maxStatBarWidth;
		transform.Find("CanvasINTBar").Find("RedDelayBar").transform.GetComponent<Image> ().fillAmount = katInt / maxStatBarWidth;
	}


	public void setColorType(SpriteRenderer katTypeCirc){
		transform.Find ("Circle").gameObject.GetComponent<SpriteRenderer> ().sprite = 
			katTypeCirc.sprite;
	}











	public void setIncomingKat(SpriteRenderer face, SpriteRenderer typeCircle, int typeNum, string name, GameObject kat){
		incomingKatFace = face;
		incomingTypeDisplay = typeCircle;
		incomingKatType = typeNum;
		incomingKatName = name;
		incomingKat = kat;
	}

	void triggerSelectAnimation (){
		switch (incomingKatType) {
		case 0:			
			selectAnimationRed.SetActive (false);
			selectAnimationRed.SetActive (true);
			selectAnimationGreen.SetActive (false);
			selectAnimationBlue.SetActive (false);
			break;
		case 1:			
			selectAnimationGreen.SetActive (false);
			selectAnimationGreen.SetActive (true);
			selectAnimationRed.SetActive (false);
			selectAnimationBlue.SetActive (false);
			break;
		case 2:			
			selectAnimationBlue.SetActive (false);
			selectAnimationBlue.SetActive (true);
			selectAnimationGreen.SetActive (false);
			selectAnimationRed.SetActive (false);
			break;

		}
	}
}
