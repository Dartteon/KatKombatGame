using UnityEngine;
using System.Collections;

public class SpriteConfigurator : MonoBehaviour {

	private readonly int LAYER_HEAD = 11;

	// Use this for initialization
	void Start () {
		GameObject sprite = this.transform.Find ("Sprite").gameObject;

		GameObject head = sprite.transform.Find ("Head").gameObject;
//		Debug.Log (head);
		head.transform.GetComponent<SpriteRenderer> ().sortingOrder = LAYER_HEAD;
		head.transform.Find ("HeadShadow").GetComponent<SpriteRenderer> ().sortingOrder = LAYER_HEAD-2;
		
		GameObject body = sprite.transform.Find ("Body").gameObject;
//		Debug.Log (body);
		body.transform.GetComponent<SpriteRenderer> ().sortingOrder = LAYER_HEAD-1;
		body.transform.Find ("BodyShadow").GetComponentInChildren<SpriteRenderer> ().sortingOrder = LAYER_HEAD-2;

		//Debug.Log("Head : " + head.transform.GetComponent<SpriteRenderer> ().sortingOrder);
		//Debug.Log ("Body : " + body.transform.GetComponent<SpriteRenderer> ().sortingOrder);

		GameObject healthCanvas = this.transform.Find ("CanvasHealthBar").gameObject;
		healthCanvas.GetComponent<Canvas> ().sortingOrder = LAYER_HEAD - 5;
		healthCanvas.transform.Find ("EmptyHealthBar").GetComponent<SpriteRenderer> ().sortingOrder = LAYER_HEAD - 4;
		healthCanvas.transform.Find ("HollowHealthBar").GetComponent<SpriteRenderer> ().sortingOrder = LAYER_HEAD - 5;


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
