using UnityEngine;
using System.Collections;

public class MenuButtonScript : MonoBehaviour {

	public MenuCameraScript mcs;
	public GameObject pause;
	Vector3 load = new Vector3 (40, 100, -10);
	Vector3 options = new Vector3 (0, 120, -10);
	Vector3 controls = new Vector3 (-40, 100, -10);
	Vector3 origin = new Vector3 (0, 100, -10);
	
	public GameObject hue;
	public GameObject backdrop;
	public int number;

	int level;

	bool minusM, minusS, plusM, plusS;

	// Use this for initialization
	void Start () {
		if (gameObject.name != "Back" && gameObject.name != "Level") {
			renderer.material.color = Color.black;
		}
		if (this.name == "Level") {
			if (PlayerPrefs.HasKey ("Level1"))
				level = PlayerPrefs.GetInt ("Level1");
			else
				level = 1;
			if(level<number){
				gameObject.renderer.enabled = false;
				gameObject.collider2D.enabled = false;
			}
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
		if (gameObject.tag == "Volume") {
			renderer.material.color = Color.cyan;
			minusM = false;
			minusS = false;
			plusM = false;
			plusS = false;
			//play sound
			return;
		}
		if (gameObject.name != "Pause") 
		{
			if(gameObject.name == "Back")
				return;
			else if (gameObject.name == "Level")			
				return;
			else if(gameObject.name == "Continue")
				return;
			else if(gameObject.name == "Menu")
				return;
			else if(gameObject.name == "Save")
				return;
			else if(gameObject.name == "No")
				return;
			else if(gameObject.name == "Yes")
				return;
			else
				gameObject.renderer.enabled = true;
		}
	}

	void OnMouseExit(){
		//if(gameObject.name != "Pause")
			//renderer.material.color = Color.white;
		//if(gameObject.name == "Continue")
			//renderer.material.color = Color.black;
		//else
		if (gameObject.tag == "Volume") {
			renderer.material.color = Color.black;
			minusM = false;
			minusS = false;
			plusM = false;
			plusS = false;
			//play sound
			return;
		}
		if (gameObject.name != "Pause") 
		{
			if(gameObject.name == "Back")
				return;
			else if (gameObject.name == "Level")			
				return;
			else if(gameObject.name == "Continue")
				return;
			else if(gameObject.name == "Menu")
				return;
			else if(gameObject.name == "Save")
				return;
			else if(gameObject.name == "No")
				return;
			else if(gameObject.name == "Yes")
				return;
			else
				gameObject.renderer.enabled = false;
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
			PlayerPrefs.SetInt ("Level",number);
			Application.LoadLevel (1 + ((number - 1) * 2));
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
			Screen.showCursor = false;
			Application.LoadLevel("Credits");
		} if (gameObject.name == "Back") {
			mcs.changeLoc(origin);
		}
		if (gameObject.name == "Continue") {
			pause.GetComponent<PauseScript>().Unpause();
		}
		if (gameObject.name == "Save") {
			pause.GetComponent<PauseScript>().saving = true;
			pause.GetComponent<PauseScript>().Confirm();
		}
		if (gameObject.name == "Yes") {
			//Debug.Log (pause.GetComponent<PauseScript>().saving);
			if(pause.GetComponent<PauseScript>().saving){
				GameObject.FindWithTag("Backdrop").GetComponent<SplatterScript>().SaveGame();
				level = PlayerPrefs.GetInt("Level");
				if(level%2 == 0){
					level--;
				}
				PlayerPrefs.SetInt("Level1",1 + ((level - 1) / 2));
				pause.GetComponent<PauseScript>().saving = false;
				pause.GetComponent<PauseScript>().Leaving();
			}
			else{
				Destroy (GameObject.FindWithTag("Player"));
				Destroy (GameObject.FindWithTag("Backdrop"));
				Application.LoadLevel("MainMenu");
				Time.timeScale = 1;
			}
		}
		if (gameObject.name == "No") {
			if(pause.GetComponent<PauseScript> ().saving){
				pause.GetComponent<PauseScript>().saving = false;
				pause.GetComponent<PauseScript> ().Leaving ();
			}
			else
				pause.GetComponent<PauseScript> ().Reset();
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