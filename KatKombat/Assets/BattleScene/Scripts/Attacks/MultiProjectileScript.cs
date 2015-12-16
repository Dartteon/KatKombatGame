using UnityEngine;
using System.Collections;

//This class is a projectile that spawns multiple projectiles, each independant
//of each other, but dependant on the main projectiles (this script) that spawned them

public class MultiProjectileScript : GeneralProjectileScript {
	public GameObject[] miniProjectilePrefab;
	public int maxMiniProjectiles;
	public float miniProjectilesShootingForce = 0.0f;
	public float spreadAngle = 10.0f;
	private int currentMiniProjectileIndex = 0;
	private bool hasSpawnedMiniProjectiles;
	private GameObject[] spawnedMiniProjectilesArray;
	private float lastMiniProjectileLaunchTime = -100f;
	public float miniProjectilesInterval = 0.4f;
	private bool isEjecting = false;

	protected override void callUpdateEffects(){
		this.transform.rotation = ownerKat.transform.rotation;
		this.transform.position = ownerKat.transform.position;
		if (Time.time - startTime >= lingerTime && isActive) {
			DestroySelf ();
		}
		if (isEjecting && (currentMiniProjectileIndex < spawnedMiniProjectilesArray.Length) && (Time.time - lastMiniProjectileLaunchTime >= miniProjectilesInterval)) {
			lastMiniProjectileLaunchTime = Time.time;
			spawnedMiniProjectilesArray [currentMiniProjectileIndex].transform.position = this.transform.position;
			spawnedMiniProjectilesArray [currentMiniProjectileIndex].transform.rotation = this.transform.rotation;
			//	spawnedMiniProjectilesArray [currentMiniProjectileIndex].transform.rotation = this.transform.rotation;
			spawnedMiniProjectilesArray [currentMiniProjectileIndex].gameObject.SetActive (true);
			spawnedMiniProjectilesArray [currentMiniProjectileIndex].GetComponent<Rigidbody2D> ().velocity = zeroVelocity; //Make sure last life of projectile does not have any speed
			if (miniProjectilesShootingForce != 0) {
				//Vector2 direction = this.transform.up;
				Vector3 direction = getRandomizedRotation()*(this.transform.up);
				direction.Normalize ();
				direction.x *= miniProjectilesShootingForce;
				direction.y *= miniProjectilesShootingForce;
				spawnedMiniProjectilesArray [currentMiniProjectileIndex].GetComponent<Rigidbody2D> ().AddForce (direction, ForceMode2D.Impulse);
			}
			currentMiniProjectileIndex++;
			if (currentMiniProjectileIndex >= maxMiniProjectiles){
				currentMiniProjectileIndex = 0;
				isEjecting = false;
			}
		}
	}
	Quaternion getRandomizedRotation(){
//		Quaternion rotation = this.transform.rotation;
		float rotationDifference = Random.Range (-spreadAngle/2, spreadAngle/2);
		Quaternion newRotation = Quaternion.Euler(0, 0, rotationDifference);
		//Quaternion newRotation = Quaternion.Euler(Vector3(0, 0, rotation.z + rotationDifference));
		return newRotation;
	}
	protected override void callOnEnableEffects(){
		startTime = Time.time;
		isActive = true;
		if (!hasSpawnedMiniProjectiles) {
			spawnProjectiles();
			hasSpawnedMiniProjectiles = true;
		}

		if (hasSpawnedMiniProjectiles) {
			currentMiniProjectileIndex = 0;
			isEjecting = true;
			/*
			for (int i=0; i<maxMiniProjectiles; i++) {
				spawnedMiniProjectilesArray [i].transform.position = this.transform.position;
				spawnedMiniProjectilesArray [i].transform.rotation = this.transform.rotation;
				spawnedMiniProjectilesArray [i].gameObject.SetActive (true);
				spawnedMiniProjectilesArray [i].GetComponent<Rigidbody2D> ().velocity = zeroVelocity; //Make sure last life of projectile does not have any speed
				if (miniProjectilesShootingForce != 0) {
					Vector2 direction = this.transform.up;
					direction.Normalize ();
					direction.x *= miniProjectilesShootingForce;
					direction.y *= miniProjectilesShootingForce;
					spawnedMiniProjectilesArray [i].GetComponent<Rigidbody2D> ().AddForce (direction, ForceMode2D.Impulse);
				}
			}
			*/
		}
	//	this.gameObject.SetActive (false);
	}

	void spawnProjectiles(){
//		bool isFlipped = false;
		this.lingerTime = maxMiniProjectiles * miniProjectilesInterval + 0.1f;
	//	this.miniProjectilesShootingForce = miniProjectilePrefab.GetComponent<GeneralProjectileScript> ().projectileShootingForce;
		this.spawnedMiniProjectilesArray = new GameObject[maxMiniProjectiles];

		int sizeOfPrefabsArray = miniProjectilePrefab.Length;
		int currentPrefabIndex = 0;

		for (int i=0; i<maxMiniProjectiles; i++) {
			GameObject projectileObj = Instantiate (miniProjectilePrefab[currentPrefabIndex++]) as GameObject;
			spawnedMiniProjectilesArray [i] = projectileObj;
			spawnedMiniProjectilesArray [i].gameObject.SetActive (false);
//			Debug.Log(spawnedMiniProjectilesArray[i].ToString());
			spawnedMiniProjectilesArray [i].GetComponent<GeneralProjectileScript> ().SetOwner (this.ownerKat);
			float mult = this.ownerKat.transform.localScale.x;
			spawnedMiniProjectilesArray [i].transform.localScale *= mult;

			if (currentPrefabIndex >= sizeOfPrefabsArray){
				currentPrefabIndex = 0;
			}
		}
	}









	protected override void callTriggerEffects(Collider2D collidingObj){
		//Do nothing. Let the projectiles call their own trigger effects.
	}
}
