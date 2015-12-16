using UnityEngine;
using System.Collections;

public class DelayedProjectileScript : GeneralProjectileScript {
	float explosionDelay = 1f;
	float spellCastStartTime = -100f;
//	float katChargeForceMultiplier = 0;
	GameObject colliderChild;
	private bool hasLocatedCollider = false;
	public float explosionLingerTime = 0.5f;

	protected override void callUpdateEffects(){
		float currentTime = Time.time;
		this.transform.rotation = ownerKat.transform.rotation;
		this.transform.position = ownerKat.transform.position;
		
		if (currentTime - startTime >= lingerTime && isActive) {
			DestroySelf ();
		}
		if (currentTime - spellCastStartTime >= explosionDelay) {
			colliderChild.SetActive(true);
		}
		
	}
	protected override void callExtraOnDisableEffect(){
		
		colliderChild.SetActive (false);
	}
	protected override void callExtraOnEnableEffect (){
		if (!hasLocatedCollider) {
			locateCollider ();
		}
		spellCastStartTime = Time.time;
	}

	private void locateCollider(){
		this.colliderChild = transform.Find ("Collider").gameObject;
		this.lingerTime = explosionDelay + explosionLingerTime + 0.1f;
	}
}
