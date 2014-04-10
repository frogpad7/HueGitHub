using UnityEngine;
using System.Collections;

public class PauseScript : MonoBehaviour {

	public bool inuse = false;

	private bool paused = false;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (gameObject);
	}

	// Update is called once per frame
	void Update () {
		if(inuse){
			if(Input.GetMouseButtonDown(0)||Input.GetMouseButtonDown(1))
			{
				if(paused)
				{
					Time.timeScale = 1;
					GameObject.FindWithTag ("Audio").GetComponent<AudioScript> ().ResumeTrack ();
					GameObject.FindWithTag ("Player").GetComponent<PlayerScript> ().maxSpeed = 20;
					Screen.showCursor = false;
				}
				else
				{
					Time.timeScale = 0;
					GameObject.FindWithTag ("Audio").GetComponent<AudioScript> ().PauseTrack ();
					GameObject.FindWithTag ("Player").GetComponent<PlayerScript> ().maxSpeed = 0;
					Screen.showCursor = true;
				}
				paused = !paused;
				//Debug.Log ("I live for a pause");
			}
		}
	}
}
