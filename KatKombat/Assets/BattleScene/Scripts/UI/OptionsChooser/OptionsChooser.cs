using UnityEngine;
using System.Collections;

public class OptionsChooser : MonoBehaviour {

	[SerializeField]
	private int numOptions;
	public int currentOption { get; private set; }

	private GameObject optionsBar;
	private Vector2 optionsBarTargetPos;
	private float optionsBarStartingZPos;
	private float optionsBarStartingYPos;
	private Vector2[] optionLocations;
	private OptionsChooserEmployer notifiable;
	
	[SerializeField]
	private int optionBarNumber;
	[SerializeField]
	private float panSpeed = 10f;
	[SerializeField]
	private float optionsWidthApart = 1f;

	public void shiftLeft() {
		if (currentOption > 0 && notifiable.shiftLeft(optionBarNumber)) {
			currentOption--;
//			optionsBarTargetPos = new Vector3(-currentOption*optionsWidthApart, this.transform.parent.position.y, optionsBarStartingZPos);
		}
	}

	public void shiftRight() {
		if (currentOption < numOptions - 1 && notifiable.shiftRight(optionBarNumber)) {
			currentOption++;
//			optionsBarTargetPos = new Vector3(-currentOption*optionsWidthApart, this.transform.parent.position.y, optionsBarStartingZPos);
//			optionsBarTargetPos = new Vector3(-optionLocations[currentOption].x, optionLocations[currentOption].y, optionsBarStartingZPos);
		}
	}

	private void repositionOptionsBar() {
		Vector3 currentPos = optionsBar.transform.localPosition;
		
		optionsBarTargetPos = new Vector3(-currentOption*optionsWidthApart, optionsBarStartingYPos, optionsBarStartingZPos);

		optionsBar.transform.localPosition = Vector3.Lerp (currentPos, optionsBarTargetPos, Time.deltaTime*panSpeed);
	}

	void Update() {
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

		GameObject[] optionObjs = new GameObject[numOptions];
		for (int i = 0; i < numOptions; i++) {
			optionObjs[i] = optionsBar.transform.GetChild (i).gameObject;
		}

		optionLocations = new Vector2[optionObjs.Length];
		for (int i = 0; i<optionLocations.Length; i++) {
			optionLocations[i] = optionObjs[i].transform.position;
		}
	}
}
