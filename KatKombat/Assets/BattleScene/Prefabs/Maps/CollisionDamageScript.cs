using UnityEngine;
using System.Collections;

public class CollisionDamageScript : MonoBehaviour {
	[SerializeField]
	private bool isPercentageDamage;
	[SerializeField]
	private float damage;
	
	void OnTriggerEnter2D(Collider2D collidingObj){
//		Debug.Log (collidingObj.ToString ());
		if (collidingObj.GetComponent<HealthScript> () != null) {
			HealthScript hpScript = collidingObj.GetComponent<HealthScript> ();
			if (hpScript != null) {
				if (isPercentageDamage)
					hpScript.DealPercentageDamage (damage);
			} else {
				hpScript.Damage ((int)damage, 3);
			}
		}
	}

}
