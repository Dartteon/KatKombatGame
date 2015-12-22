using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsChooser : MonoBehaviour {

	[SerializeField]
	private int numOptions;
	public int currentOption { get; private set; }

	private GameObject optionsBar;
	private Vector2 optionsBarTargetPos;
	private float optionsBarStartingZPos;
	private float optionsBarStartingYPos;
	private GameObject[] optionObjs;
	private Vector2[] optionLocations;
	private OptionsChooserEmployer notifiable;
	private Text descriptionText;
	
	[SerializeField]
	private int optionBarNumber;
	[SerializeField]
	private float panSpeed = 10f;
	[SerializeField]
	private float optionsWidthApart = 1f;

	public void shiftLeft() {
		if (currentOption > 0 && notifiable.shiftLeft(optionBarNumber)) {
			currentOption--;
			reflectDescriptionText();
//			optionsBarTargetPos = new Vector3(-currentOption*optionsWidthApart, this.transform.parent.position.y, optionsBarStartingZPos);
		}
	}

	public void shiftRight() {
//		Debug.Log (currentOption + " " + numOptions);
		if (currentOption < numOptions - 1 && notifiable.shiftRight(optionBarNumber)) {
			currentOption++;
			reflectDescriptionText();
//			optionsBarTargetPos = new Vector3(-currentOption*optionsWidthApart, this.transform.parent.position.y, optionsBarStartingZPos);
//			optionsBarTargetPos = new Vector3(-optionLocations[currentOption].x, optionLocations[currentOption].y, optionsBarStartingZPos);
		}
	}

	public void reflectDescriptionText() {
		string name = optionObjs [currentOption].name;
		descriptionText.text = name;
	}

	private void repositionOptionsBar() {
		Vector3 currentPos = optionsBar.transform.localPosition;
		
		optionsBarTargetPos = new Vector3(-currentOption*optionsWidthApart, optionsBarStartingYPos, optionsBarStartingZPos);

		optionsBar.transform.localPosition = Vector3.Lerp (currentPos, optionsBarTargetPos, Time.deltaTime*panSpeed);
	}

	void Update() {
		//temp fix (unoptimized) -----------------------------------------------------
		reflectDescriptionText();
		//temp fix (unoptimized) -----------------------------------------------------

		repositionOptionsBar ();
		if (Input.GetKeyDown (KeyCode.LeftArrow))
			shiftLeft ();
		if (Input.GetKeyDown (KeyCode.RightArrow))
			shiftRight ();
	}

	void Start() {
		currentOption = 0;
		optionsBar = this.transform.Find ("OptionsBar").gameObject;
		optionsBarStartingZPos = optionsBar.transform.localPosition.z;
		optionsBarStartingYPos = optionsBar.transform.localPosition.y;
		notifiable = this.transform.parent.GetComponent<OptionsChooserEmployer> ();
		descriptionText = this.transform.Find ("Description").Find ("Text").GetComponent<Text> ();

		optionObjs = new GameObject[numOptions];
		for (int i = 0; i < numOptions; i++) {
			try{
				optionObjs[i] = optionsBar.transform.GetChild (i).gameObject;
			} catch (UnityException e) {
				Debug.LogError("Num options may be more than num of objects in options bar");
			}
		}

		optionLocations = new Vector2[optionObjs.Length];
		for (int i = 0; i<optionLocations.Length; i++) {
			optionLocations[i] = optionObjs[i].transform.position;
		}

		
		reflectDescriptionText();
	}
}
