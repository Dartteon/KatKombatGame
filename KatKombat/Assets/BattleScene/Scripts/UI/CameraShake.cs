using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {
	
	public Camera theCamera;
	public float magnitude = 5.0f;
	public float duration = 2.0f;
	public Vector3 direction;
	
	void Update(){
		if (Input.GetKey(KeyCode.Space)){
			Shake ();
		}
	}

	void Start () {
		Time.timeScale = 1;
		if(theCamera == null) theCamera = GetComponent<Camera>();
		
		if(theCamera == null){ // STILL == NULL?
			Debug.LogWarning("CAN'T FIND THE CAMERA FOR CAMERA SHAKING");
			return;
		}
	}
	
	public void Shake()
	{
		//Debug.Log("START CAMERA SHAKE");
		if(theCamera.orthographic){
			StartCoroutine(ShakeOrtho());
		}else{
			StartCoroutine(ShakePerspective());
		}
	}
	
	
	IEnumerator ShakePerspective()
	{
		//Debug.Log("PERSPECTIVE SHAKE");
		float elapsed = 0.0f;
		
		Vector3 originalCamPos = theCamera.transform.position;
		
		while (elapsed < duration) {
			
			elapsed += Time.deltaTime;         
			float percentComplete = elapsed / duration;         
			float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);
			
			// map noise to [-1, 1]
			float x = Random.value * 2.0f - 1.0f;
			float y = Random.value * 2.0f - 1.0f;
			float z = Random.value * 2.0f - 1.0f;
			x *= magnitude * damper * direction.x;
			y *= magnitude * damper * direction.y;
			z *= magnitude * damper * direction.z;
			
			theCamera.transform.position = (new Vector3(x, y, z) + originalCamPos);
			
			yield return null;
		}
		
		theCamera.transform.position = originalCamPos;
		yield return null;
	}
	
	IEnumerator ShakeOrtho()
	{
		float elapsed = 0.0f;
		
		float originalCamOrtho = theCamera.orthographicSize;
		Debug.Log (originalCamOrtho);
		
		while (elapsed < duration) { 
			elapsed += Time.deltaTime;   
			float percentComplete = elapsed / duration;         
			float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);
			
			// ONLY EFFECT Z
			float z = Random.value * 2.0f - 1.0f;
			z *= magnitude * damper * direction.z;
			theCamera.orthographicSize = originalCamOrtho - Mathf.Abs(z);
			
			yield return null;
		}
		
		theCamera.orthographicSize = originalCamOrtho;
		Debug.Log (originalCamOrtho);
		yield return null;
	}
}