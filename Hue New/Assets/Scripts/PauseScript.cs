using UnityEngine;
using System.Collections;

public class PauseScript : MonoBehaviour {
	
	private bool paused = false;
	public bool saving = false;
	public GameObject menu;
	public GameObject save;
	public GameObject quit;

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
		SpriteRenderer[] mr = menu.GetComponentsInChildren<SpriteRenderer> ();
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
		SpriteRenderer[] sr = menu.GetComponentsInChildren<SpriteRenderer> ();
		BoxCollider2D[] bc = menu.GetComponentsInChildren<BoxCollider2D> ();
		for (int i = 0; i < sr.Length; i++)
			sr [i].enabled = false;
		for (int j = 0; j < bc.Length; j++)
			bc [j].enabled = false;
		paused = !paused;
	}	

	public void Confirm(){
		SpriteRenderer[] sr = GetComponentsInChildren<SpriteRenderer> ();
		BoxCollider2D[] bc = GetComponentsInChildren<BoxCollider2D> ();
		foreach (SpriteRenderer tx in sr) {
			if(tx.transform.parent.name == "Menu")
				tx.enabled = false;
			else
				tx.enabled = true;
		}
		foreach (BoxCollider2D bx in bc) {
			if(bx.transform.parent.name == "Menu")
				bx.enabled = false;
			else
				bx.enabled = true;
		}
		quit.renderer.enabled = false;
	}

	public void Leaving(){
		save.GetComponent<SpriteRenderer> ().enabled = false;
		quit.GetComponent<SpriteRenderer> ().enabled = true;
	}

	public void Reset(){
		SpriteRenderer[] sr = GetComponentsInChildren<SpriteRenderer> ();
		BoxCollider2D[] bc = GetComponentsInChildren<BoxCollider2D> ();
		foreach (SpriteRenderer tx in sr) {
			if(tx.transform.parent.name == "Menu")
				tx.enabled = true;
			else
				tx.enabled = false;
		}
		foreach (BoxCollider2D bx in bc) {
			if(bx.transform.parent.name == "Menu")
				bx.enabled = true;
			else
				bx.enabled = false;
		}
	}
}
