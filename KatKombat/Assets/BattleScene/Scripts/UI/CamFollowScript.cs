using UnityEngine;
using System.Collections;

public class CamFollowScript : MonoBehaviour {

	public GameObject objectToFollow;

	private float multiplier = 3000f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
//		Vector2 facing = (objectToFollow.transform.up);
//		facing.Normalize ();
//		Debug.Log (facing.x + " " + facing.y);
//		facing *= multiplier;
//		Debug.Log (facing.x + " " + facing.y);
//		Debug.Log (Time.timeScale);

	//	Debug.Log (facing.ToString ());
//		Vector3 pos = new Vector3(objectToFollow.transform.position.x + facing.x, objectToFollow.transform.position.y + facing.y, -100.0f);
		Vector3 pos = new Vector3(objectToFollow.transform.position.x, objectToFollow.transform.position.y, -100.0f);
		transform.position = pos;
	}
}
