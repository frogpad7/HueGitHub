using UnityEngine;
using System.Collections;

public class CreditsCamScript : MonoBehaviour {

	float startTime;
	Vector3 travel;
	public int text;
	
	// Use this for initialization
	void Start () {
		startTime = Time.time;
		travel = new Vector3 (0, 0, -10);
		text = 1;

		if(PlayerPrefs.HasKey("Music"))
		   GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat ("Music");
		GetComponent<AudioSource> ().Play ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0) || Input.GetMouseButtonDown (1))
			Application.LoadLevel ("MainMenu");
		//Debug.Log (Time.time - startTime);
		if (Time.time - startTime > 5) {
			text++;
			startTime = Time.time;
		}
		if (text == 1)
			travel = new Vector3 (0, 0, -10);
		if (text == 2)
			travel = new Vector3 (10, 0, -10);
		if (text == 3)
			travel = new Vector3 (20, 0, -10);
		if (text == 4)
			travel = new Vector3 (30, 0, -10);
		if (text == 5)
			travel = new Vector3 (40, 0, -10);
		if (text == 6)
			travel = new Vector3 (50, 0, -10);
		if (text == 7)
			travel = new Vector3 (60, 0, -10);
		if (text == 9) {
			AutoFade.LoadLevel ("MainMenu", 5, 3, Color.white);
			Screen.showCursor = true;
		}
		float distCovered = (Time.time - startTime); 
		transform.position = Vector3.Lerp(transform.position, travel, distCovered/150);
	}
}
