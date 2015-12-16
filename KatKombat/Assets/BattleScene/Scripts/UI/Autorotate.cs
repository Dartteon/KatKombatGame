using UnityEngine;
using System.Collections;

public class Autorotate : MonoBehaviour {
	public float rotateSpeed = 10.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (0, 0, rotateSpeed);
	}
}
