using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayKatFace : MonoBehaviour {
	SpriteRenderer katHeadDisplay;
	GameObject attachedKat;
	GameObject mainKatFaceDisplay;
	GameObject mainKatTypeDisplay;
	private bool isClicked = false;
	KatSelectManager managerScript;

	float slowTimeStartTime = 0.0f;
	bool isSlowingTime = false;
	
	SpriteRenderer katTypeCircle;

	public void setManager(KatSelectManager script){
		managerScript = script;
	}

	public void setKatFaceDisplay(GameObject display){
		this.mainKatFaceDisplay = display;
	}

	public void setKatTypeDisplay(GameObject display){
		this.mainKatTypeDisplay = display;
	}

	public void attachKat(GameObject kat){
		this.attachedKat = kat;
		setKatHeadDisplay ();
		setKatTypeCircleDisplay ();
		setButtonKatName ();
	}

	void setKatHeadDisplay(){
		transform.Find ("MovementJoystick").transform.Find ("KatHead").GetComponent<SpriteRenderer> ().sprite
			= attachedKat.transform.FindChild ("Sprite").gameObject.transform.FindChild ("Head").gameObject.GetComponent<SpriteRenderer> ().sprite;
	}

	void setKatTypeCircleDisplay(){
		int katType = attachedKat.GetComponent<StatsScript> ().statType;

		switch (katType) {
		case 0:
			transform.Find ("MovementJoystick").Find ("Red").gameObject.SetActive(true);
			break;
		case 1:
			transform.Find ("MovementJoystick").Find ("Green").gameObject.SetActive(true);
			break;
		case 2:
			transform.Find ("MovementJoystick").Find ("Blue").gameObject.SetActive(true);
			break;
		default:
			transform.Find ("MovementJoystick").Find ("Yellow").gameObject.SetActive(true);
			break;
		}
	}

	void setButtonKatName(){
		//transform.GetComponentInChildren<Text> ().text = attachedKat.name;
	}

	void OnMouseDown(){
		isClicked = true;
	}

	void OnMouseUp(){
		if (isClicked && managerScript.getCanSelect()) {
		//	setMainKatFaceDisplay();
		//	setMainKatTypeDisplay();
			setMainKatIncoming();
			spawnRunningKat();
			//playSelectKatAnimation ();
			managerScript.katName = attachedKat.name;
			managerScript.setLastSelectTime(Time.time);
		}
	}

	void setMainKatIncoming(){
		mainKatFaceDisplay.GetComponent<MainKatDisplay> ().setIncomingKat (
			transform.Find ("MovementJoystick").transform.Find ("KatHead").GetComponent<SpriteRenderer> ()
			, katTypeCircle, attachedKat.GetComponent<StatsScript> ().statType, 
			attachedKat.name, attachedKat);
	}

	void spawnRunningKat(){
		GameObject kat = Instantiate (attachedKat, this.transform.position, this.transform.rotation) as GameObject;
		kat.GetComponent<MovementScript> ().freeLeapInDirection (mainKatFaceDisplay.transform.position - this.transform.position);
		kat.GetComponent<MovementScript> ().instantFaceForward (mainKatFaceDisplay.transform.position - this.transform.position);
		kat.GetComponentInChildren<Animator> ().Play ("Movement");
		managerScript.attachTrailToKat (kat, kat.GetComponent<StatsScript>().statType);
		kat.transform.Find ("TargetRing").gameObject.SetActive (false);
		kat.transform.Find ("UIElements").gameObject.SetActive (false);
//		kat.transform.Find ("CanvasManaBar").gameObject.transform.Find ("EmptyHealthBar").gameObject.SetActive (false);
		kat.transform.Find ("CanvasHealthBar").gameObject.SetActive (false);
	}

	void setMainKatFaceDisplay(){
		mainKatFaceDisplay.transform.transform.Find ("KatHead").GetComponent<SpriteRenderer> ().sprite
			= transform.Find ("MovementJoystick").transform.Find ("KatHead").GetComponent<SpriteRenderer> ().sprite;
	}

	void playSelectKatAnimation(){
		mainKatFaceDisplay.transform.Find ("KatFaceChosenEffect").gameObject.SetActive (false);
		mainKatFaceDisplay.transform.Find ("KatFaceChosenEffect").gameObject.SetActive (true);
	}

	void setMainKatTypeDisplay(){
		int katType = attachedKat.GetComponent<StatsScript> ().statType;

		switch (katType) {
		case 0:
			katTypeCircle = transform.Find ("MovementJoystick").Find ("Red").gameObject.GetComponent<SpriteRenderer>();
			break;
		case 1:
			katTypeCircle = transform.Find ("MovementJoystick").Find ("Green").gameObject.GetComponent<SpriteRenderer>();
			break;
		case 2:
			katTypeCircle = transform.Find ("MovementJoystick").Find ("Blue").gameObject.GetComponent<SpriteRenderer>();
			break;
		default:
			katTypeCircle = transform.Find ("MovementJoystick").Find ("Yellow").gameObject.GetComponent<SpriteRenderer>();
			break;
		}

		mainKatFaceDisplay.transform.transform.Find ("Circle").GetComponent<SpriteRenderer> ().sprite
			= katTypeCircle.GetComponent<SpriteRenderer> ().sprite;
		
	}

	void setOwnTypeCircle(){
			int katType = attachedKat.GetComponent<StatsScript> ().statType;
			
			switch (katType) {
			case 0:
				katTypeCircle = transform.Find ("MovementJoystick").Find ("Red").gameObject.GetComponent<SpriteRenderer>();
				break;
			case 1:
				katTypeCircle = transform.Find ("MovementJoystick").Find ("Green").gameObject.GetComponent<SpriteRenderer>();
				break;
			case 2:
				katTypeCircle = transform.Find ("MovementJoystick").Find ("Blue").gameObject.GetComponent<SpriteRenderer>();
				break;
			default:
				katTypeCircle = transform.Find ("MovementJoystick").Find ("Yellow").gameObject.GetComponent<SpriteRenderer>();
				break;
			}
			
	}

	void Start(){
		setOwnTypeCircle ();
	}

	void slowTime(){
		Time.timeScale = 0.8f;
		isSlowingTime = true;
		slowTimeStartTime = Time.time;
	}

	void Update(){
		if (isSlowingTime && Time.time - slowTimeStartTime >= 0.2f) {
			isSlowingTime = false;
			Time.timeScale = 1.0f;
		}
	}
}
