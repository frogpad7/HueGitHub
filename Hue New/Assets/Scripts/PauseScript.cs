using UnityEngine;
using System.Collections;

public class PauseScript : MonoBehaviour {
	
	private bool paused = false;
	public bool saving = false;
	public GameObject menu;

	// Use this for initialization
	void Start () {
		//DontDestroyOnLoad (gameObject);
		//GameObject p = GameObject.FindWithTag ("MainCamera");
		//if (p != null){
		//	transform.parent = p.transform;
		//}
	}
	// Update is called once per frame
	void Update () {
			if(Input.GetMouseButtonDown(0)||Input.GetMouseButtonDown(1))
			{
				if(!paused)
				{
					Pause();
				}
				//Debug.Log ("I live for a pause");
			}
	}

	void Pause(){
		//Debug.Log ("p a u s e");
		Time.timeScale = 0;
		//GameObject.FindWithTag ("Audio").GetComponent<AudioSource> ().Pause ();
		GameObject[] pause = GameObject.FindGameObjectsWithTag ("Audio");
		foreach (GameObject go in pause) {
			AudioSource[] play = go.GetComponents<AudioSource> ();
			foreach(AudioSource p in play)
				p.Pause();
		}
		Screen.showCursor = true;
		MeshRenderer[] mr = menu.GetComponentsInChildren<MeshRenderer> ();
		BoxCollider2D[] bc = menu.GetComponentsInChildren<BoxCollider2D> ();
		for (int i = 0; i < mr.Length; i++)
			mr [i].enabled = true;
		for (int j = 0; j < bc.Length; j++)
			bc [j].enabled = true;
		paused = !paused;
	}

	public void Unpause(){
		Time.timeScale = 1;
		//GameObject.FindWithTag ("Audio").GetComponent<AudioScript> ().ResumeTrack ();
		GameObject[] resume = GameObject.FindGameObjectsWithTag ("Audio");
		foreach (GameObject go in resume) {
			AudioSource[] play = go.GetComponents<AudioSource> ();
			foreach(AudioSource p in play)
				p.Play();
		}
		Screen.showCursor = false;
		MeshRenderer[] mr = menu.GetComponentsInChildren<MeshRenderer> ();
		BoxCollider2D[] bc = menu.GetComponentsInChildren<BoxCollider2D> ();
		for (int i = 0; i < mr.Length; i++)
			mr [i].enabled = false;
		for (int j = 0; j < bc.Length; j++)
			bc [j].enabled = false;
		paused = !paused;
	}	

	public void Confirm(){
		MeshRenderer[] mr = GetComponentsInChildren<MeshRenderer> ();
		BoxCollider2D[] bc = GetComponentsInChildren<BoxCollider2D> ();
		for (int i = 0; i < mr.Length; i++)
			mr [i].enabled = !mr[i].enabled;
		for (int j = 0; j < bc.Length; j++)
			bc [j].enabled = !bc [j].enabled;
	}
}
