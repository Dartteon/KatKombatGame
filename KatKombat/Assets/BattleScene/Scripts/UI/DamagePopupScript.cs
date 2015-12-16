using UnityEngine;
using System.Collections;

public class DamagePopupScript : MonoBehaviour {
	private float enableTime;
	public float lingerTime = 0.35f;
	private GameObject ownerKat;	
	private SpriteRenderer damageShadow;
	void OnEnable(){
		enableTime = Time.time;
		damageShadow = transform.GetComponentInChildren<SpriteRenderer> ();
	}

	void Update(){
		if (Time.time - enableTime >= lingerTime)
			gameObject.SetActive (false);
//		if (ownerKat != null)
//			transform.position = ownerKat.transform.position;
	}

	public void setOwnerKat(GameObject kat){
		ownerKat = kat;
	}

	public void setShadowColor(int attackType){
		switch (attackType) {
		case 0:
			damageShadow.color = new Color32(255, 0, 0, 150);
			break;
		case 1:
			damageShadow.color = new Color32(0, 214, 0, 150);
			break;
		case 2:
			damageShadow.color = new Color32(0, 0, 255, 150);
			break;
		default:
			damageShadow.color = new Color32(214, 214, 0, 150);
			break;
		}
	}
}
