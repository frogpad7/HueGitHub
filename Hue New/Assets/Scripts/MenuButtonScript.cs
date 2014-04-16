using UnityEngine;
using System.Collections;

public class MenuButtonScript : MonoBehaviour {

	MenuCameraScript mcs;
	public GameObject pause;
	Vector3 load = new Vector3 (30, 0, -10);
	Vector3 options = new Vector3 (0, 15, -10);
	Vector3 controls = new Vector3 (-30, 0, -10);
	Vector3 credits = new Vector3 (0, -15, -10);
	Vector3 origin = new Vector3 (0, 0, -10);

	Color hover;
	int level;

	// Use this for initialization
	void Start () {
		/*if (PlayerPrefs.HasKey ("Color")) {
			if (PlayerPrefs.GetInt ("Color") == 1)
				hover = Color.red;
		}
		else
			hover = Color.green;*/

		if(PlayerPrefs.HasKey("Level"))
		   level = PlayerPrefs.GetInt ("Level");
		else
		   level = 1;
		mcs = GameObject.FindWithTag ("MainCamera").GetComponent<MenuCameraScript> ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnMouseEnter(){
		if (gameObject.name != "Pause") 
		{
			if (gameObject.name == "New")			
				renderer.material.color = Color.red;
			if (gameObject.name == "Load")			
				renderer.material.color = Color.gray;
			if (gameObject.name == "Options")			
				renderer.material.color = Color.yellow;
			if (gameObject.name == "Controls")			
				renderer.material.color = Color.green;
			if (gameObject.name == "Credits")			
				renderer.material.color = Color.blue;
			if (gameObject.name == "Quit")			
				renderer.material.color = Color.magenta;
			if (gameObject.name == "Back")			
				renderer.material.color = Color.black;
		}
	}

	void OnMouseExit(){
		if(gameObject.name != "Pause")
		renderer.material.color = Color.white;
	}

	void OnMouseUp(){
		if (gameObject.name == "New") {
			//pause = (GameObject)Instantiate (pause);
			//pause.name = "Pause";
			//DontDestroyOnLoad (pause);
			pause.GetComponent<PauseScript>().inuse = true;
			Application.LoadLevel (1);
			Screen.showCursor = false;
		}
		if (gameObject.name == "Load") {
			//pause = (GameObject)Instantiate (pause);
			//pause.name = "Pause";
			//DontDestroyOnLoad (pause);
			pause.GetComponent<PauseScript> ().inuse = true;
			Application.LoadLevel (level + ((level - 1) * 2));
			Screen.showCursor = false;
		} if (gameObject.name == "Quit") {
			Application.Quit ();
			//PlayerPrefs.SetInt ("Color", 1);
			PlayerPrefs.SetInt ("Level", level);
		} if (gameObject.name == "Options") {
			mcs.changeLoc(options);
		} if (gameObject.name == "Controls") {
			mcs.changeLoc(controls);
		} if (gameObject.name == "Credits") {
			mcs.changeLoc(credits);
		} if (gameObject.name == "Back") {
			mcs.changeLoc(origin);
		}

			//Debug.Log ("I live for a pause");
			//Instantiate ();
	}
}