﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuyEggButton : MonoBehaviour, Tappable {
	private bool isClicked = false;

	void OnMouseDown(){
		execute ();
	}
	public void handleTap(Vector2 pos1, Vector2 pos2){
		execute ();
	}
	void execute() {
		AdventureManager advMngr = GameObject.Find ("AdventureModule").gameObject.GetComponent<AdventureManager> ();
		if (isClicked) {
//		advMngr.addCurrency (100);
			if (advMngr.deductCurrency (100)) {
				advMngr.reflectPlayerCurrency ();
				//		this.transform.parent.Find("KashBox").transform.Find ("TextCanvas").transform.Find("Text").GetComponent<Text>().text = advMngr.getCurrencyAmount().ToString();
				advMngr.summonNewEgg ();
				advMngr.reloadScene();
			}
		
		} else {
			if (advMngr.hasSlotForEggOrKat () && advMngr.getCurrencyAmount() >= 100) {
				isClicked = true;
				this.transform.Find("DescriptionText").transform.Find("Text").GetComponent<Text>().text = "Confirm";
			} else if (advMngr.getCurrencyAmount() < 100){
				this.transform.Find("DescriptionText").transform.Find("Text").GetComponent<Text>().text = "No Kash..";
			} else {
//				Debug.LogError("2 many eggs");
				this.transform.Find("DescriptionText").transform.Find("Text").GetComponent<Text>().text = "Too many \n kats!";
			}
		}
	}
}
