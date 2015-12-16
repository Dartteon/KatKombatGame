using UnityEngine;
using System.Collections;

public class AttacksScript : MonoBehaviour {

	private KatProjectileLauncherScript atk1Script, atk2Script, atk3Script;
	public float atk1Cooldown, atk2Cooldown, atk3Cooldown;
	public int atk1Cost, atk2Cost, atk3Cost;
	private bool[] isAttackReady;
	private int[] atkCostArray;
	private float[] atkCooldownArray;

	void Start () {
		atk1Script = transform.FindChild ("BasicAttack").gameObject.GetComponent<KatProjectileLauncherScript> ();
		atk2Script = transform.FindChild ("SecondAttack").gameObject.GetComponent<KatProjectileLauncherScript> ();
		atk3Script = transform.FindChild ("ThirdAttack").gameObject.GetComponent<KatProjectileLauncherScript> ();
		atk1Cooldown = atk1Script.cooldown;
		atk2Cooldown = atk2Script.cooldown;
		atk3Cooldown = atk3Script.cooldown;
		atk1Cost = atk1Script.manaCost;
		atk2Cost = atk2Script.manaCost;
		atk3Cost = atk3Script.manaCost;
//		Debug.Log (atk1Cooldown + " " + atk2Cooldown + " " + atk3Cooldown);
	}

	
	//These attack calls shoot projectile in specified direction
	public bool BasicAttack(Vector2 direction){
		float angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (new Vector3(0, 0, angle - 90));
		return atk1Script.Cast (direction);
	}

	public bool SecondAttack(Vector2 direction){
		float angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (new Vector3(0, 0, angle - 90));
		return atk2Script.Cast (direction);
	}

	public bool ThirdAttack(Vector2 direction){
		float angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (new Vector3(0, 0, angle - 90));
		return atk3Script.Cast (direction);
	}



	//These attack calls shoot projectile in direction Kat is currently facing
	public bool basicAttackForward(){
		return atk1Script.Cast (transform.up);
	}
	
	public bool secondAttackForward(){
		return atk2Script.Cast (transform.up);
	}
	
	public bool thirdAttackForward(){
		return atk3Script.Cast (transform.up);
	}


	//These functions get the current cooldown of the Kat's commands
	public float GetAttack1Cooldown(){
		return atk1Script.cooldown;
	}

	public float GetAttack2Cooldown(){
		return atk2Script.cooldown;
	}

	public float GetAttack3Cooldown(){
		return atk3Script.cooldown;
	}

	
	//These functions get the cost of Kat's commands
	public float GetAttack1Cost(){
		return atk1Script.manaCost;
	}
	
	public float GetAttack2Cost(){
		return atk2Script.manaCost;
	}
	
	public float GetAttack3Cost(){
		return atk3Script.manaCost;
	}

}
