using UnityEngine;
using System.Collections;

public class BaronVec {
	public static Vector2 zeroVector = new Vector2(0,0);

	public static Vector3 vector2ToVector3 (Vector2 vec, float zPos) {
		return new Vector3 (vec.x, vec.y, zPos);
	}

	public static Quaternion randomRotation(){
		//		Quaternion rotation = this.transform.rotation;
		float rotationDifference = Random.Range (0.0f, 360.0f);
		Quaternion newRotation = Quaternion.Euler(0, 0, rotationDifference);
		//Quaternion newRotation = Quaternion.Euler(Vector3(0, 0, rotation.z + rotationDifference));
		return newRotation;
	}
}
