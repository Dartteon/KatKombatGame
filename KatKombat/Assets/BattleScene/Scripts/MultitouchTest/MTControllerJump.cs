using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MTControllerJump : MonoBehaviour, Tappable {
	
	protected Vector2 startPoint, endPoint, direction, centre;
	protected bool isClicked = false;
	protected GameObject attachedKat;
	protected Animator katAnimator;
	protected StatsScript katStats;

	public MovementScript moveScript;
	private Image cooldownCircle;
	private float jumpCooldown;
	private float lastJumpTime = -100f;
	private bool hasRecentlyJumped = false;
	
	private Text cdText;
	private SpriteRenderer iconRenderer;
	private Color pureWhite = new Color (1f, 1f, 1f);
	private Color grey = new Color(0.3f, 0.3f, 0.3f);

	// Use this for initialization
	void Start () {
		cooldownCircle = transform.Find ("CooldownCircle").gameObject.GetComponentInChildren<Image> ();
		cdText = transform.Find ("TextCanvas").transform.Find ("Text").GetComponent<Text> ();
		iconRenderer = transform.Find ("Icon").GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (hasRecentlyJumped) {
			float timeDifference = Time.time - lastJumpTime;
			if (timeDifference >= jumpCooldown){
				hasRecentlyJumped = false;
				cdText.text = "";
				iconRenderer.color = pureWhite;
			}
			else {
				float timeLeft = jumpCooldown - timeDifference;
				cooldownCircle.fillAmount = (timeLeft) / jumpCooldown - 0.02f;

				string timeLeftString = timeLeft.ToString();
				if (timeLeftString.Length >= 3) timeLeftString = timeLeftString.Substring(0,3);
				cdText.text = timeLeftString;
				iconRenderer.color = grey;
			}
			
		}
	}

	public void handleTap(Vector2 position, Vector2 worldPos){
		if (Time.timeScale != 0 && Time.time - lastJumpTime >= jumpCooldown) {
			moveScript.jumpForward ();
			hasRecentlyJumped = true;
			lastJumpTime = Time.time;
		}
	}


	void AttachKat(GameObject kat){
		this.attachedKat = kat;
		this.moveScript = kat.transform.GetComponent<MovementScript> ();		
		jumpCooldown = moveScript.jumpCooldown;
		this.katAnimator = kat.transform.FindChild ("Sprite").gameObject.GetComponent<Animator> ();
		this.katStats = kat.GetComponent<StatsScript> ();
	}
}
