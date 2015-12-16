using UnityEngine;
using System.Collections;

public class OnClickReaction : MonoBehaviour {
	float xScale, yScale;
	float xPushedScale, yPushedScale;
	bool isClicked = false;
	
	bool hasSprite = false;
	Color[] originalColor;
	Color clickedColor = new Color (.7f,.7f,.7f);
	SpriteRenderer[] sprite;

	// Use this for initialization
	void Start () {
		xScale = this.transform.localScale.x;
		yScale = this.transform.localScale.y;
		xPushedScale = 0.8f * xScale;
		yPushedScale = 0.8f * yScale;
		
		SpriteRenderer[] sprites = transform.GetComponentsInChildren<SpriteRenderer> ();
		if (sprites.Length != 0) {
			sprite = sprites;
			hasSprite = true;
			originalColor = new Color[sprite.Length];
			for (int i=0; i<sprite.Length; i++){
				originalColor[i] = sprites [i].color;
			}
		} else {
			sprite[0] = transform.GetComponent<SpriteRenderer>();
			if (sprite[0] != null){
				hasSprite = true;
				originalColor[0] = sprite[0].color;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (isClicked) {		
			Vector2 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			Vector2 currentPos = this.transform.position;
			Vector2 distanceVec = (currentPos - mousePos);
			float absDistance = distanceVec.magnitude;
			if (absDistance >= 1.0f){
				isClicked = false;
				this.transform.localScale = new Vector2 (xScale, yScale);
				if (hasSprite) {
					for (int i=0; i<sprite.Length; i++){
						sprite[i].color = originalColor[i];
					}
				}
			}
		}
	}

	void OnMouseDown(){
		this.transform.localScale = new Vector2 (xPushedScale, yPushedScale);
		isClicked = true;
		if (hasSprite) {
			for (int i=0; i<sprite.Length; i++){
				sprite[i].color = clickedColor;
			}
		}
	}

	void OnMouseUp(){
		this.transform.localScale = new Vector2 (xScale, yScale);
		isClicked = false;
		this.transform.localScale = new Vector2 (xScale, yScale);
		if (hasSprite) {
			for (int i=0; i<sprite.Length; i++){
				sprite[i].color = originalColor[i];
			}
		}
	}
}
