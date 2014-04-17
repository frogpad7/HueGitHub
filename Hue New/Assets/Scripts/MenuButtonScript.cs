using UnityEngine;
using System.Collections;

public class MenuButtonScript : MonoBehaviour {

	public MenuCameraScript mcs;
	public GameObject pause;
	Vector3 load = new Vector3 (30, 0, -10);
	Vector3 options = new Vector3 (0, 15, -10);
	Vector3 controls = new Vector3 (-30, 0, -10);
	Vector3 credits = new Vector3 (0, -15, -10);
	Vector3 origin = new Vector3 (0, 0, -10);
	
	public GameObject hue;
	public GameObject backdrop;

	// Use this for initialization
	void Start () {
		//if (gameObject.name == "Continue") {
			renderer.material.color = Color.black;
		//}
		int level;
		if(PlayerPrefs.HasKey("Level1"))
			level = PlayerPrefs.GetInt ("Level1");
		else
			level = 1;
		if (this.name == "Level")
			GetComponent<TextMesh> ().text = "Level " + level;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnMouseEnter(){
		if (gameObject.name != "Pause") 
		{
			if (gameObject.name == "New")			
				renderer.material.color = Color.red;
			else if (gameObject.name == "Load")			
				renderer.material.color = Color.gray;
			else if (gameObject.name == "Options")			
				renderer.material.color = Color.yellow;
			else if (gameObject.name == "Controls")			
				renderer.material.color = Color.green;
			else if (gameObject.name == "Credits")			
				renderer.material.color = Color.blue;
			else if (gameObject.name == "Quit")			
				renderer.material.color = Color.magenta;
			else if (gameObject.name == "Back")			
				renderer.material.color = Color.black;
			else if (gameObject.name == "Level")			
				renderer.material.color = Color.black;
			else if(gameObject.name == "Continue")
				renderer.material.color = Color.cyan;
			else
				renderer.material.color = Color.cyan;
		}
	}

	void OnMouseExit(){
		//if(gameObject.name != "Pause")
			//renderer.material.color = Color.white;
		//if(gameObject.name == "Continue")
			//renderer.material.color = Color.black;
		//else
			renderer.material.color = Color.black;
	}

	void OnMouseUp(){
		if (gameObject.name == "New") {
			PlayerPrefs.SetString("Backdrop","");
			Application.LoadLevel (1);
			Instantiate(hue);
			Instantiate(backdrop);
			Screen.showCursor = false;
		}
		if (gameObject.name == "Level") {
			int level;
			if(PlayerPrefs.HasKey("Level1"))
				level = PlayerPrefs.GetInt ("Level1");
			else
				level = 1;
			PlayerPrefs.SetString ("Backdrop",PlayerPrefs.GetString("Backdrop1"));
			Application.LoadLevel (level + ((level - 1) * 2));
			Instantiate(hue);
			Instantiate(backdrop);
			Screen.showCursor = false;
		} if (gameObject.name == "Quit") {
			Application.Quit ();
		} 
		if (gameObject.name == "Load") {
			mcs.changeLoc(load);
		} if (gameObject.name == "Options") {
			mcs.changeLoc(options);
		} if (gameObject.name == "Controls") {
			mcs.changeLoc(controls);
		} if (gameObject.name == "Credits") {
			mcs.changeLoc(credits);
		} if (gameObject.name == "Back") {
			mcs.changeLoc(origin);
		}
		if (gameObject.name == "Continue") {
			pause.GetComponent<PauseScript>().Unpause();
		}
		if (gameObject.name == "Menu") {
			pause.GetComponent<PauseScript>().Confirm();
		}
		if (gameObject.name == "Save") {
			pause.GetComponent<PauseScript>().Confirm();
			pause.GetComponent<PauseScript>().saving = true;
		}
		if (gameObject.name == "Yes") {
			Debug.Log (pause.GetComponent<PauseScript>().saving);
			if(pause.GetComponent<PauseScript>().saving){
				GameObject.FindWithTag("Backdrop").GetComponent<SplatterScript>().SaveGame();
				PlayerPrefs.SetInt("Level1",PlayerPrefs.GetInt("Level"));
			}
			Destroy (GameObject.FindWithTag("Player"));
			Destroy (GameObject.FindWithTag("Backdrop"));
			Application.LoadLevel("MainMenu");
			Time.timeScale = 1;
		}
		if (gameObject.name == "No") {
			pause.GetComponent<PauseScript>().Confirm();
			pause.GetComponent<PauseScript>().saving = false;
		}
	}	
}