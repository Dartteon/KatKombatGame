using UnityEngine;
using System.Collections;

public class GeneralBuffAttackScript : GeneralProjectileScript {
	public int statToBuff;
	public float buffRatio;

	void Start () {
		sprite = GetComponent<SpriteRenderer> ();
		//rb2d = transform.GetComponent<Rigidbody2D> ();
		//zeroVelocity = new Vector2 (0, 0);
	}
	
	void Update () {
		if (Time.time - startTime >= lingerTime && isActive)
			DestroySelf ();
		transform.position = ownerKat.transform.position;
	}

	void OnEnable(){
		startTime = Time.time;
		isActive = true;

		switch (statToBuff) {
		case 0:
			katStats.Str = (int) (katStats.Str*buffRatio);
			break;
		case 1:
			katStats.Dex = (int) (katStats.Dex*buffRatio);
			break;
		case 2:
			katStats.Int = (int) (katStats.Int*buffRatio);
			break;
		}
		if (!hasColorSet)
			SetColor();
	}

	void OnDisable(){
		switch (statToBuff) {
		case 0:
			katStats.Str = (int) (katStats.Str/buffRatio);
			break;
		case 1:
			katStats.Dex = (int) (katStats.Dex/buffRatio);
			break;
		case 2:
			katStats.Int = (int) (katStats.Int/buffRatio);
			break;
		}
	}
}