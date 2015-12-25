using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuyEggButton : MonoBehaviour, Tappable {

	void OnMouseDown(){
		execute ();
	}
	public void handleTap(Vector2 pos1, Vector2 pos2){
		execute ();
	}
	void execute() {

		AdventureManager advMngr = GameObject.Find ("AdventureModule").gameObject.GetComponent<AdventureManager> ();
//		advMngr.addCurrency (100);
		if (advMngr.hasSlotForEggOrKat() && advMngr.deductCurrency(100)){
			advMngr.reflectPlayerCurrency();
	//		this.transform.parent.Find("KashBox").transform.Find ("TextCanvas").transform.Find("Text").GetComponent<Text>().text = advMngr.getCurrencyAmount().ToString();
			advMngr.summonNewEgg();

		}
	}
}
