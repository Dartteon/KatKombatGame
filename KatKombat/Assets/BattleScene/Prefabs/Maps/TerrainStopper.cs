using UnityEngine;
using System.Collections;

public class TerrainStopper : MonoBehaviour {
	Vector2 zeroVector = new Vector2(0,0);

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void OnCollisionEnter2D(Collision2D collidingObj){

		Rigidbody2D katBody = collidingObj.transform.GetComponent<Rigidbody2D> ();
		if (katBody != null)
			katBody.velocity = zeroVector;

	}
		
	
}
