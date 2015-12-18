using UnityEngine;
using System.Collections;

public class BaronVec {
	public static Vector2 zeroVector = new Vector2(0,0);

	public static Vector3 vector2ToVector3 (Vector2 vec, float zPos) {
		return new Vector3 (vec.x, vec.y, zPos);
	}
}
