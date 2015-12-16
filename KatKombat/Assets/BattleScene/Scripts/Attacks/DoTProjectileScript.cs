using UnityEngine;
using System.Collections;

public class DoTProjectileScript : GeneralProjectileScript {
	private float lastTickTime = -100f;
	public float tickInterval = 0.1f;
	public int maxEnemiesHitPerTick;
	//public float damageRadius;
	public int maxNumTicks = 5;
	private bool hasLocatedCollider = false;
	private int currentTickCount = 0;
	private GameObject colliderChild;
	private bool tickIsActive = false;

	protected override void callExtraOnEnableEffect (){
		currentTickCount = 0;
		if (!hasLocatedCollider) {
			locateCollider ();
			setLingerTime();
		}
	}
	public void OnDisable(){
		colliderChild.SetActive (false);
	}
	protected override void callUpdateEffects(){
		float currentTime = Time.time;
//		this.transform.rotation = ownerKat.transform.rotation;
	//	this.transform.position = ownerKat.transform.position;

		if (currentTime - startTime >= lingerTime && isActive) {
			DestroySelf ();
		}

		if ((currentTime - lastTickTime >= (tickInterval/2)) && (currentTickCount < (maxNumTicks*2))) {
			//tickEffect();
			alternateTickEffect();
			lastTickTime = currentTime;
			currentTickCount++;
		}

		if (currentTickCount == maxNumTicks * 2) {
			this.gameObject.SetActive(false);
		}

	}
	private void setLingerTime(){
		this.lingerTime = (maxNumTicks * tickInterval) + 0.1f;
		this.fadeTime = 0;
//		Debug.Log (lingerTime);
	}
	private void locateCollider(){
		this.colliderChild = transform.Find ("Collider").gameObject;
	}


	private void alternateTickEffect(){
		if (tickIsActive) {
			colliderChild.SetActive (true);
	//		Debug.Log("ticked times : " + currentTickCount);
		} else {
			colliderChild.SetActive (false);
		}
		tickIsActive = !tickIsActive;
	}



















	/*
	private void tickEffect(){
		Collider2D[] hitObjects = new Collider2D[maxEnemiesHit];
		Physics2D.OverlapCircleNonAlloc(this.transform.position, damageRadius, hitObjects, 11);
		
		for (int i=0; i<hitObjects.Length; i++){
			Debug.Log("Calling Tick Effect ");
			if (hitObjects[i] != null && !hitObjects[i].tag.Equals(ownerKat.tag)){
				Debug.Log(hitObjects[i]);
				HealthScript hpScript = hitObjects[i].transform.GetComponent<HealthScript> ();
				if (ownerKat != null && hpScript != null){
					hpScript.SetLastAttacker (ownerKat);
				}
				
				callExtraOnTriggerEffect();
				
				if (!hasHit){
					setHasHit();
				};
				
				statToUse = getStatToUse(statType);
				damage = (int)(statToUse * statRatio);
				repelCollidingObj(hitObjects[i]);
				damage = (int)(Random.Range(0.85f, 1.0f) * damage);
				if (damage > 0){
					hpScript.Damage (damage, statType);
				}
			}
		}
	}*/
}
