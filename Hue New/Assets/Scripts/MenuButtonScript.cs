using UnityEngine;
using System.Collections;

public class MenuButtonScript : MonoBehaviour {

	public GameObject pause;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseEnter(){
		if(gameObject.name != "Pause")
		renderer.material.color = Color.green;
	}

	void OnMouseExit(){
		if(gameObject.name != "Pause")
		renderer.material.color = Color.white;
	}

	void OnMouseUp(){
		if (gameObject.name == "New") {
			pause = (GameObject)Instantiate(pause);
			pause.name = "Pause";
			DontDestroyOnLoad (pause);
			Application.LoadLevel (1);
		}
		else if (gameObject.name == "Quit")
			Application.Quit ();
		else if (gameObject.name == "Pause")
			Debug.Log ("I live for a pause");
			//Instantiate ();
	}
}