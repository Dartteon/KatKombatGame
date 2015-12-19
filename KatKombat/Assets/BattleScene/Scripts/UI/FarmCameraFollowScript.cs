using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
	private GameObject katButtons;

	private Camera cam;
	private string katName;
	private string information;
	private Text nameText;
	private Text infoText;


	private bool isFollowingKat = false;
	private GameObject katBeingFollowed;

//	public float panSpeed = 1.0f;
	private bool isPanning = false;
	private float currentPanSpeed = 0;
	public float zoomSpeed = 1.0f;
	private float targetZoomSize;

	private bool hasInit = false;
	private Vector3 targetPosition;
	
	private FarmSelectKatButton[] katButtonScripts = new FarmSelectKatButton[6];

	private enum CameraMode { Panning, ZoomedIntoKat, EggTray, }

	// Use this for initialization
	void Start () {
		Debug.Log ("Locating KatButtons");
		locateKatButtons ();
		initiate ();
	}

	void initiate() {
		//		tgtCircle = Instantiate (targetCirclePrefab) as GameObject;
		//		tgtCircle.transform.parent = this.transform;
		isPanning = true;
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
		katButtons = this.transform.Find ("KatButtons").gameObject;


		hasInit = true;
	}

	void locateKatButtons() {
		for (int i=0; i<6; i++) {
			string btnName = "Button" + i;
			Debug.Log (btnName);
			katButtonScripts[i] = this.transform.Find("KatButtons").transform.Find(btnName).GetComponent<FarmSelectKatButton>();
			Debug.Log (katButtonScripts[i].ToString());
		}
	}

	public void enableKatButtons(List<GameObject> kats, FarmManager fManager, AdventureManager aManager, string[] katNames) {
		for (int i=0; i<kats.Count; i++) {
			Debug.Log (katButtonScripts[i].ToString());
			katButtonScripts[i].initialize(aManager, fManager, kats[i], katNames[i]);
		}
	}

	public bool getIsFollowingKat(){
		return isFollowingKat;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (hasInit) {
			if (isFollowingKat) {
				repositionCameraToKat();
			} else if (!isPanning) {
				repositionToCentre();
			}
			handleZoom();
		}
	}

	void repositionToCentre() {
		currentPanSpeed = Mathf.Lerp (currentPanSpeed, 30.0f, Time.deltaTime);
		Vector3 newPos = BaronVec.vector2ToVector3 (BaronVec.zeroVector, -10f);
		this.transform.position = Vector3.Lerp (this.transform.position, newPos, Time.deltaTime * currentPanSpeed);
	}

	void repositionCameraToKat() {
		Vector2 katFacing = katBeingFollowed.transform.up.normalized * 0.2f;
		Vector2 katPos = katBeingFollowed.transform.position;
		katPos += new Vector2 (-1.2f, 0.7f) + katFacing;

		targetPosition = new Vector3 (katPos.x, katPos.y, -10f);
		//	this.transform.position = new Vector3 (katPos.x, katPos.y, -10f);
		currentPanSpeed = Mathf.Lerp (currentPanSpeed, 30.0f, Time.deltaTime);
		this.transform.position = Vector3.Lerp (this.transform.position, targetPosition, Time.deltaTime * currentPanSpeed);
	}

	void handleZoom() {
		float currentZoomSize = cam.orthographicSize;
		if (targetZoomSize != currentZoomSize) {
			cam.orthographicSize = Mathf.Lerp (currentZoomSize, targetZoomSize, Time.deltaTime * zoomSpeed);
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
		currentPanSpeed = 2;
		isPanning = false;
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
		//1 = basic + panning
		//1 = zoomedIntoKat
		//2 = egghatching
		panner.SetActive (false);
		katOptions.SetActive (false);
		eggTray.SetActive(false);
		katButtons.SetActive (false);
		
		switch (mode) {
		case 0:
			panner.SetActive(true);
			katButtons.SetActive(true);
			break;
		case 1:
			panner.SetActive(true);
			katButtons.SetActive(true);
			isPanning = true;
			break;
		case 2:
			katOptions.SetActive(true);
			isPanning = false;
			break;
		case 3:
			eggTray.SetActive(true);
			isPanning = false;
			break;
		}
	}

}
