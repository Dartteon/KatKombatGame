using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ManaScript : MonoBehaviour {
	private float manaGainRate;
	public int manaCount;
	private GameObject canvasManaBar;
	private Image manaBar;
	private float startTime;
	private float maxMana = 3;
	private int katInt;

	void Start () {
		katInt = transform.GetComponent<StatsScript> ().getInt ();
		manaGainRate = (1.0f/(float)(katInt*0.01f) + 1.5f);
		manaCount = 0;
		startTime = Time.time;
		canvasManaBar = transform.FindChild ("CanvasManaBar").gameObject;
		//this.manaBar = canvasManaBar.transform.GetComponentInChildren<Image> ();
		//UpdateMana ();
	}
	/*
	void Update () {

		if (manaCount == maxMana)
			startTime = Time.time;
		else if (Time.time - startTime >= manaGainRate) {
			manaCount++;
		//	Debug.Log(Time.time + " time");
			startTime = Time.time;
			UpdateMana();
		}
	}
	*/
	public void UpdateMana(){
		manaBar.fillAmount = (manaCount / (float)maxMana)*1;
	}
	
	public bool removeMana(int mana){
		if (manaCount >= mana) {
			manaCount -= mana;			
			UpdateMana();
			return true;
		} else
			return false;
	}

	public float getManaGainRate(){
		return manaGainRate;
	}
}
