using UnityEngine;
using System.Collections;

public class PauseScript : MonoBehaviour {

	public bool inuse = false;
	public bool findPlayer = false;
	private bool paused = false;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (gameObject);
	}

	// Update is called once per frame
	void Update () {
		if (findPlayer) {
			GameObject p = GameObject.FindWithTag ("Player");
			if (p != null){
				transform.parent = p.transform;
				findPlayer = false;
			}
		}
		if(inuse){
			if(Input.GetMouseButtonDown(0)||Input.GetMouseButtonDown(1))
			{
				if(!paused)
				{
					Pause();
				}
				//Debug.Log ("I live for a pause");
			}
			if(Input.GetMouseButtonDown(1))
				if(paused)
				{
					Unpause();
				}
		}
	}

	void Pause(){
		Time.timeScale = 0;
		GameObject.FindWithTag ("Audio").GetComponent<AudioScript> ().PauseTrack ();
		GameObject.FindWithTag ("Player").GetComponent<PlayerScript> ().maxSpeed = 0;
		Screen.showCursor = true;
		MeshRenderer[] mr = GetComponentsInChildren<MeshRenderer> ();
		BoxCollider2D[] bc = GetComponentsInChildren<BoxCollider2D> ();
		for (int i = 0; i < mr.Length; i++)
			mr [i].enabled = true;
		for (int j = 0; j < bc.Length; j++)
			bc [j].enabled = true;
		paused = !paused;
	}

	public void Unpause(){
		Time.timeScale = 1;
		GameObject.FindWithTag ("Audio").GetComponent<AudioScript> ().ResumeTrack ();
		GameObject.FindWithTag ("Player").GetComponent<PlayerScript> ().maxSpeed = 20;
		Screen.showCursor = false;
		MeshRenderer[] mr = GetComponentsInChildren<MeshRenderer> ();
		BoxCollider2D[] bc = GetComponentsInChildren<BoxCollider2D> ();
		for (int i = 0; i < mr.Length; i++)
			mr [i].enabled = false;
		for (int j = 0; j < bc.Length; j++)
			bc [j].enabled = false;
		paused = !paused;
	}
}
