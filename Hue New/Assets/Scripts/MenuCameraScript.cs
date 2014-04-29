using UnityEngine;
using System.Collections;

public class MenuCameraScript : MonoBehaviour {

	float startTime;
	Vector3 travel;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
		travel = new Vector3 (0, 100, -10);
	}
	
	// Update is called once per frame
	void Update () {
		float distCovered = (Time.time - startTime); 
		transform.position = Vector3.Lerp(transform.position, travel, distCovered/150);
	}

	public void changeLoc(Vector3 l){
		startTime = Time.time;
		travel = l;
	}
}
