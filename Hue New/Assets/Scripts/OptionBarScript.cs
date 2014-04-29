using UnityEngine;
using System.Collections;

public class OptionBarScript : MonoBehaviour {

	float mVol;
	float sVol;

	// Use this for initialization
	void Start () {

		if (gameObject.tag == "Music"){
			if (PlayerPrefs.HasKey ("Music"))
				mVol = PlayerPrefs.GetFloat ("Music");
			else
				mVol = 1;
			gameObject.transform.localScale = new Vector3(mVol, 0.75f, 1);
		}	
		if(gameObject.tag == "Sound"){
			if (PlayerPrefs.HasKey ("Sound"))
				sVol = PlayerPrefs.GetFloat ("Sound");
			else
				sVol = 1;
			gameObject.transform.localScale = new Vector3(sVol, 0.75f, 1);
		}
	}
	
	// Update is called once per frame
	void Update () {
		//change music volume
	}

	public void IncSize(){
		if(gameObject.tag == "Music" && mVol < 1){
			mVol += 0.01f;
			gameObject.transform.localScale = new Vector3(mVol, 0.75f, 1);
			PlayerPrefs.SetFloat("Music",mVol);
		}
		if(gameObject.tag == "Sound" && sVol < 1){
			sVol += 0.01f;
			gameObject.transform.localScale = new Vector3(sVol, 0.75f, 1);
			PlayerPrefs.SetFloat("Sound",sVol);
		}
	}

	public void DecSize(){
		if(gameObject.tag == "Music" && mVol > 0){
			mVol -= 0.01f;
			if(mVol<0) mVol = 0;
			gameObject.transform.localScale = new Vector3(mVol, 0.75f, 1);
			PlayerPrefs.SetFloat("Music",mVol);
		}
		if(gameObject.tag == "Sound" && sVol > 0){
			sVol -= 0.01f;
			if(sVol<0) sVol = 0;
			gameObject.transform.localScale = new Vector3(sVol, 0.75f, 1);
			PlayerPrefs.SetFloat("Sound",sVol);
		}
	}

}
