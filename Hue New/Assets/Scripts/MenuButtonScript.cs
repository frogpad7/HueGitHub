using UnityEngine;
using System.Collections;

public class MenuButtonScript : MonoBehaviour {

	public MenuCameraScript mcs;
	public GameObject pause;
	Vector3 load = new Vector3 (30, 100, -10);
	Vector3 options = new Vector3 (0, 120, -10);
	Vector3 controls = new Vector3 (-30, 100, -10);
	Vector3 origin = new Vector3 (0, 100, -10);
	
	public GameObject hue;
	public GameObject backdrop;

	int level;

	bool minusM, minusS, plusM, plusS;

	// Use this for initialization
	void Start () {
		//if (gameObject.name == "Continue") {
			renderer.material.color = Color.black;
		//}
		if (this.name == "Level") {
			if (PlayerPrefs.HasKey ("Level1"))
				level = PlayerPrefs.GetInt ("Level1");
			else
				level = 1;
			GetComponent<TextMesh> ().text = "Level " + level;
		}
		minusM = minusS = plusM = plusS = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (minusM) {
			GameObject.FindWithTag("Music").GetComponent<OptionBarScript>().DecSize();
		}
		if (minusS) {
			GameObject.FindWithTag("Sound").GetComponent<OptionBarScript>().DecSize();
		}
		if (plusM) {
			GameObject.FindWithTag("Music").GetComponent<OptionBarScript>().IncSize();
		}
		if (plusS) {
			GameObject.FindWithTag("Sound").GetComponent<OptionBarScript>().IncSize();
		}
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
		if (gameObject.tag == "Volume") {
			minusM = false;
			minusS = false;
			plusM = false;
			plusS = false;
			//play sound
		}
	}

	void OnMouseUp(){
		if (gameObject.name == "New") {
			PlayerPrefs.SetString("Backdrop","");
			PlayerPrefs.SetInt ("Level",1);
			Application.LoadLevel (1);
			GameObject b = (GameObject)Instantiate(backdrop);
			b.GetComponent<SplatterScript>().CreateHue();
			Screen.showCursor = false;
		}
		if (gameObject.name == "Level") {
			PlayerPrefs.SetString ("Backdrop",PlayerPrefs.GetString("Backdrop1"));
			PlayerPrefs.SetInt ("Level",level);
			Application.LoadLevel (1 + ((level - 1) * 2));
			GameObject b = (GameObject)Instantiate(backdrop);
			b.GetComponent<SplatterScript>().CreateHue();
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
			Application.LoadLevel("Credits");
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
				level = PlayerPrefs.GetInt("Level");
				if(level%2 == 0){
					level--;
				}
				PlayerPrefs.SetInt("Level1",1 + ((level - 1) / 2));
			}
			Destroy (GameObject.FindWithTag("Player"));
			Destroy (GameObject.FindWithTag("Backdrop"));
			Application.LoadLevel("MainMenu");
			Time.timeScale = 1;
		}
		if (gameObject.name == "No") {
						pause.GetComponent<PauseScript> ().Confirm ();
						pause.GetComponent<PauseScript> ().saving = false;
		} 
		if(gameObject.tag == "Volume") {
			minusM = false;
			minusS = false;
			plusM = false;
			plusS = false;
			//play sound
		}
	}	

	void OnMouseDown(){
		if (gameObject.name == "MinusM") {
			minusM = true;
		}
		if (gameObject.name == "MinusS") {
			minusS = true;
		}
		if (gameObject.name == "PlusM") {
			plusM = true;
		}
		if (gameObject.name == "PlusS") {
			plusS = true;
		}
	}
}