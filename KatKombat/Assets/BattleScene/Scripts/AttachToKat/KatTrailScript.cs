using UnityEngine;
using System.Collections;

public class KatTrailScript : MonoBehaviour {

	void Start () {
		Color trailCol = transform.parent.GetComponent<StatsScript> ().col;

		TrailRenderer trailRenderer = this.GetComponent<TrailRenderer> ();
		trailRenderer.material.color = trailCol;
	}
}
