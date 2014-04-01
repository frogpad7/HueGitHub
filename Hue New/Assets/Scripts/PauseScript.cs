using UnityEngine;
using System.Collections;

public class PauseScript : MonoBehaviour {

	private bool paused = false;

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)||Input.GetMouseButtonDown(1))
		{
			if(paused)
			{
				Time.timeScale = 1;
				GameObject.FindWithTag ("Audio").GetComponent<AudioScript> ().ResumeTrack ();
				GameObject.FindWithTag ("Player").GetComponent<PlayerScript> ().maxSpeed = 20;
			}
			else
			{
				Time.timeScale = 0;
				GameObject.FindWithTag ("Audio").GetComponent<AudioScript> ().PauseTrack ();
				GameObject.FindWithTag ("Player").GetComponent<PlayerScript> ().maxSpeed = 0;
			}
			paused = !paused;
			//Debug.Log ("I live for a pause");
		}
	}
}
