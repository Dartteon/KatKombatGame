using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NameRequester : MonoBehaviour, KeyboardRequester {
	[SerializeField]
	private int maxChars;

	private UserStringRequester requester;

	private GameObject errorObj;
	private Text errorText;
	private GameObject textBoxButton;
	private Text textDisplay;

	private string inputString;

	void Start() {
		initialize ();
	}
	/*
	public void initialize(UserStringRequester req) {
		requester = req;
		*/
	public void initialize() {
		textBoxButton = this.transform.Find ("TextBox").gameObject;
		Debug.Log (textBoxButton.ToString ());
		textDisplay = textBoxButton.transform.Find ("TextCanvas").transform.Find ("Text").GetComponent<Text> ();
//		textBoxButton.SetActive (false);

		errorObj = this.transform.Find ("ErrorObject").gameObject;
		errorText = errorObj.transform.Find ("TextCanvas").transform.Find ("Text").GetComponent<Text> ();
	}

	public void handleInput(string input) {
		if (input.Length <= maxChars) {
			inputString = input;
		} else {
			errorText.text = "Max " + maxChars + " characters!";
			errorObj.SetActive(true);
		}
	}

	public void handleEnter() {
		requester.handleEnter (inputString);
		this.gameObject.SetActive (false);
	}

	void OnMouseDown() {
		textBoxButton.GetComponent<KeyboardInput> ().initialize (this);
	}
}
