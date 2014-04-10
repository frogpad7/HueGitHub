using UnityEngine;
using System.Collections;

public class MenuButtonScript : MonoBehaviour {

	public GameObject pause;
	Color hover;
	int level;

	// Use this for initialization
	void Start () {
		/*if (PlayerPrefs.HasKey ("Color")) {
			if (PlayerPrefs.GetInt ("Color") == 1)
				hover = Color.red;
		}
		else*/	
			hover = Color.green;
		if(PlayerPrefs.HasKey("Level"))
		   level = PlayerPrefs.GetInt ("Level");
		else
		   level = 1;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.LeftShift))
			hover = Color.red;
	}

	void OnMouseEnter(){
		if(gameObject.name != "Pause")
		renderer.material.color = hover;
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
			pause.GetComponent<PauseScript>().inuse = true;
			Application.LoadLevel (level+((level-1)*2));
			Screen.showCursor = false;
		}
		else if (gameObject.name == "Quit") {
			Application.Quit ();
			PlayerPrefs.SetInt("Color",1);
			PlayerPrefs.SetInt ("Level",level);
		}
		else if (gameObject.name == "Pause")
			Debug.Log ("I live for a pause");
			//Instantiate ();
	}
}