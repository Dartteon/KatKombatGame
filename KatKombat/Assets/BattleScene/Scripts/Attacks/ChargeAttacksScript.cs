using UnityEngine;
using System.Collections;

public class ChargeAttacksScript : GeneralProjectileScript {
	public float chargeForce= 200.0f;
	public float chargeForceMultiplier = 20.0f;
	private bool isGhost = true;

	void Update () {
		if (Time.time - startTime >= lingerTime && isActive)
			DestroySelf ();
		this.transform.position = ownerKat.transform.position;
		ownerKat.transform.rotation = this.transform.rotation;
	//	ownerKat.transform.position = new Vector2 (transform.position.x, transform.position.y);
	//	transform.rotation = ownerKat.transform.rotation;
	}

	protected override void callExtraOnEnableEffect(){
		this.transform.rotation = ownerKat.transform.rotation;
		Vector2 chargeDirection = getChargeDirection ();
		ownerKat.GetComponent<Rigidbody2D> ().AddForce (chargeDirection, ForceMode2D.Impulse);
		if (isGhost) {
			ownerKat.layer = 10;
//			Debug.Log(ownerKat.layer.ToString());
		}
	}
	protected override void callExtraOnDisableEffect(){
		if (isGhost) {
			ownerKat.layer = 0;
//			Debug.Log(ownerKat.layer.ToString());
		}
	}
	protected override void callExtraOnTriggerEffect(){
		
		//ownerKat.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
	}

	Vector2 getChargeDirection (){
		//Vector2 direction = this.transform.position - ownerKat.transform.position;
		Vector2 direction = transform.up;
		direction.Normalize ();
		direction *= chargeForceMultiplier;
		return direction;
	}

}
