using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FarmCameraFollowScript : MonoBehaviour {
//	public GameObject targetCirclePrefab;
	private KatDataCard katCard;

	public int startSize = 8;
	public int zoomedInSize = 3;

	private GameObject panner;
	private GameObject katOptions;
	private GameObject eggTray;
	private GameObject tgtCircle;
	private Camera cam;
	private string katName;
	private string information;
	private Text nameText;
	private Text infoText;



	private bool isFollowingKat = false;
	private GameObject katBeingFollowed;

//	public float panSpeed = 1.0f;
	private float currentPanSpeed = 0;
	public float zoomSpeed = 1.0f;
	private float targetZoomSize;

	private Vector3 targetPosition;

	// Use this for initialization
	void Start () {
//		tgtCircle = Instantiate (targetCirclePrefab) as GameObject;
//		tgtCircle.transform.parent = this.transform;
		katCard = this.transform.Find ("KatDataCard").GetComponent<KatDataCard> ();
		katCard.initiate ();

		cam = this.GetComponent<Camera> ();
//		nameText = tgtCircle.transform.Find ("NameCanvas").GetComponentInChildren<Text> ();
//		infoText = tgtCircle.transform.Find ("InfoCanvas").GetComponentInChildren<Text> ();
//		tgtCircle.SetActive (false);
		katCard.gameObject.SetActive (false);
		katOptions = this.transform.Find ("KatOptions").gameObject;
		targetZoomSize = startSize;
		panner = this.transform.Find ("CameraPanner").gameObject;
		eggTray = this.transform.Find ("EggTray").gameObject;
	}

	public bool getIsFollowingKat(){
		return isFollowingKat;
	}

	// Update is called once per frame
	void Update () {
		if (isFollowingKat) {
			Vector2 katPos = katBeingFollowed.transform.position;
			katPos += new Vector2(-1.5f, 0.7f);
			targetPosition = new Vector3 (katPos.x, katPos.y, -10f);
		//	this.transform.position = new Vector3 (katPos.x, katPos.y, -10f);
			currentPanSpeed = Mathf.Lerp(currentPanSpeed, 30.0f, Time.deltaTime);
			this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, Time.deltaTime*currentPanSpeed);
		}

//		Debug.Log (cam.ToString ());
		float currentZoomSize = cam.orthographicSize;
		if (targetZoomSize != currentZoomSize) {
			cam.orthographicSize = Mathf.Lerp(currentZoomSize, targetZoomSize, Time.deltaTime*zoomSpeed);
		}

		
	}
	/*

	public void followKat(GameObject kat){
		isFollowingKat = true;
		katBeingFollowed = kat;
		cam.orthographicSize = 4;
//		tgtCircle.transform.position = kat.transform.position;
//		tgtCircle.transform.parent = kat.transform;

		tgtCircle.SetActive (true);
	}
*/
	public void followKat(GameObject kat, KatStatsInfo katInfo){
		katName = katInfo.getName ();
//		nameText.text = katName;
//		Debug.Log (" FOLLOWING             " + katName);
//		infoText.text = getKatDisplayInfo (katInfo);
		katCard.AttachKat (katInfo, kat);

		isFollowingKat = true;
		katBeingFollowed = kat;
	//	cam.orthographicSize = zoomedInSize;
		targetZoomSize = zoomedInSize;
		//		tgtCircle.transform.position = kat.transform.position;
		//		tgtCircle.transform.parent = kat.transform;
		currentPanSpeed = 2;
		katCard.gameObject.SetActive (true);
		katOptions.SetActive (true);
		panner.SetActive (false);
		goToCameraMode (1);
	}

	string getKatDisplayInfo(KatStatsInfo katInfo){
		string level = "[LV." + katInfo.getLevel () + "] ";
		string breed = katInfo.getBreed ();
		return level + breed;
	}

	public void stopFollowingKat(){
		isFollowingKat = false;
		katBeingFollowed = null;
		//cam.orthographicSize = startSize;
		targetZoomSize = startSize;

		katCard.gameObject.SetActive (false);
		katOptions.SetActive (false);
		panner.SetActive (true);
		goToCameraMode (0);
	}

	
	
	public void goToCameraMode(int mode){
		//0 = basic
		//1 = zoomedIntoKat
		//
		panner.SetActive (false);
		katOptions.SetActive (false);
		eggTray.SetActive(false);
		
		switch (mode) {
		case 0: panner.SetActive(true);
			break;
		case 1: katOptions.SetActive(true);
			break;
		case 2: eggTray.SetActive(true);
			break;
		}
	}

}
