using UnityEngine;
using System.Collections;

public class MeasuringTape : MonoBehaviour {
	GameObject startPt, endPt;
	Vector2 route;
	float distance;
	bool isEndAttached = true;

	// Use this for initialization
	void Start () {
		startPt = transform.Find ("Start").gameObject;
		endPt = transform.Find ("End").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		route = endPt.transform.position - startPt.transform.position;
		distance = route.magnitude;

		startPt.transform.position = startPt.transform.parent.position;

		if (endPt.transform.parent != null)
			endPt.transform.position = endPt.transform.parent.position;

		if (Input.GetKeyDown (KeyCode.Space))
			printDistance ();

		if (isEndAttached) {
			if (endPt.transform.parent == null){
				isEndAttached = false;
				printDistance();
			} else if (!endPt.activeSelf)
			           printDistance ();
		}

	}

	void printDistance(){
		Debug.Log (distance);
	}
}
