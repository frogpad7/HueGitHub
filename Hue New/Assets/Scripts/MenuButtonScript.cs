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
	float sVol = 1;
	float mVol = 1;

	// Use this for initialization
	void Start () {
		//if (gameObject.name == "Continue") {
			renderer.material.color = Color.black;
		//}
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
		if (gameObject.name == "MinusM") {
			if(mVol > 0)
				mVol -= 0.01f;
			GameObject.FindWithTag("Music").transform.localScale = new Vector3(mVol, 0.75f, 1);
			//change menu music
		}
		if (gameObject.name == "MinusS") {
			if(sVol > 0)
				sVol -= 0.01f;
			GameObject.FindWithTag("Sound").transform.localScale = new Vector3(sVol, 0.75f, 1);
			//play ping sound
		}
		if (gameObject.name == "PlusM") {
			if(mVol < 1)
				mVol += 0.01f;
			GameObject.FindWithTag("Music").transform.localScale = new Vector3(mVol, 0.75f, 1);
			//change menu music
		}
		if (gameObject.name == "PlusS") {
			if(sVol < 1)
				sVol += 0.01f;
			GameObject.FindWithTag("Sound").transform.localScale = new Vector3(sVol, 0.75f, 1);
			//play ping sound
		}
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
			pause.GetComponent<PauseScript>().Confirm();
			pause.GetComponent<PauseScript>().saving = false;
		}
	}	

	//void OnGUI(){
	//	sVol = GUI.HorizontalSlider(new Rect(25, 25, 100, 30), sVol, 0.0F, 1.0F);
	//	mVol = GUI.HorizontalSlider(new Rect(25, 50, 100, 30), mVol, 0.0F, 10.0F);
	//}
}