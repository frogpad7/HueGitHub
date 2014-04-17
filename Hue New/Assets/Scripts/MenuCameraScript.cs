using UnityEngine;
using System.Collections;

public class MenuCameraScript : MonoBehaviour {

	float startTime;
	Vector3 travel;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
		travel = new Vector3 (0, 0, -10);
	}
	
	// Update is called once per frame
	void Update () {
		//if (Input.GetKeyDown (KeyCode.LeftShift))
		//hover = Color.red;
		float distCovered = (Time.time - startTime); 
		//transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);
		//mc.camera.transform.position = Vector3.MoveTowards(mc.transform.position, travel, 100); 
		transform.position = Vector3.Lerp(transform.position, travel, distCovered/250);
	}

	public void changeLoc(Vector3 l){
		//Debug.Log ("Lerp-a-derp");
		startTime = Time.time;
		travel = l;
	}
}
