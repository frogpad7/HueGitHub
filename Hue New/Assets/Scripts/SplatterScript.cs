using UnityEngine;
using System.Collections;
using System;

public class SplatterScript : MonoBehaviour {

	/// <summary>
	/// The objects.
	/// </summary>
	public GameObject[] Objects;
	public string SaveString;
	
	/// <summary>
	/// The objects loaded.
	/// </summary>
	public string LoadString;

	public GameObject red;
	public GameObject orange;
	public GameObject yellow;
	public GameObject green;
	public GameObject blue;
	public GameObject purple;

	// Use this for initialization
	void Start () {
		//PlayerPrefs.SetString ("Backdrop", "");
		LoadGame ();
	}
	
	// Update is called once per frame
	void Update () {
		//if(Input.GetKeyDown(KeyCode.F5))
		//{
			Objects = GameObject.FindGameObjectsWithTag("Savable");
			//SaveGame();
		//}
	}

	public void Splat(int c, Vector3 pos){
		//Vector3 center = gameObject.transform.position;
		//center.z = 0.0f;
		GameObject splat = new GameObject ();
		if (c == 1) {
			splat = (GameObject)Instantiate (red);	
			splat.name = "Red";
		}
		if (c == 2) {
			splat = (GameObject)Instantiate (orange);	
			splat.name = "Orange";
		}
		if (c == 3) {
			splat = (GameObject)Instantiate (yellow);	
			splat.name = "Yellow";
		}
		if (c == 4) {
			splat = (GameObject)Instantiate (green);	
			splat.name = "Green";
		}
		if (c == 5) {
			splat = (GameObject)Instantiate (blue);	
			splat.name = "Blue";
		}
		if (c == 6) {
			splat = (GameObject)Instantiate (purple);	
			splat.name = "Purple";
		}
		splat.transform.parent = this.gameObject.transform;
		splat.transform.position = new Vector3(pos.x,pos.y,0);
		//splat.AddComponent<SpriteRenderer> ();
		//splat.GetComponent<SpriteRenderer> ().sortingLayerName = "Canvas";
		//splat.GetComponent<SpriteRenderer> ().sprite = s;
		//splat.tag = "Savable";

		//Instantiate (splat, pos, Quaternion.Euler (0, 0, 0));
	}

	public void SaveGame(){
		SaveString = "";
		for(int i = 0; i < Objects.Length; i++)
		{
			SaveString += 
				Objects[i].name 
					+ ","+ 
					Objects[i].transform.position.x + "|" + Objects[i].transform.position.y
					+ ";";
		}
		PlayerPrefs.SetString ("Backdrop", SaveString);
	}
	
	void LoadGame(){	
		if(PlayerPrefs.HasKey("Backdrop")&&PlayerPrefs.GetString("Backdrop").Length>0){
			LoadString = PlayerPrefs.GetString ("Backdrop");
			string[] ObjectsLoaded = LoadString.Split(';');
			
			foreach(string record in ObjectsLoaded)
			{
				if(record.Length>0){
				string[] recordSelected = record.Split(',');
				
				string naz, poz1, rot1;
				string[] poz, rot;
				
				Vector3 pozycja;
				//Quaternion rotacja;
				
				naz = recordSelected[0].ToString();
				Debug.Log("Loaded: "+naz);
				
				poz1 = recordSelected[1].ToString();
				Debug.Log("Loaded: "+poz1);
				
				//rot1 = recordSelected[2].ToString();
				//Debug.Log("Loaded: "+rot1);
				
				poz = poz1.Split('|');
				
				//rot = rot1.Split('|');
				
				pozycja.x = Convert.ToSingle(poz[0]);
				pozycja.y = Convert.ToSingle(poz[1]);
				pozycja.z = 0;
				
				//rotacja.x = 0;
				//rotacja.y = 0;
				//rotacja.z = 0;
				//rotacja.w = 1;

				GameObject splat = new GameObject ();
				if(naz == "Red")
				{
					splat = (GameObject)Instantiate (red);	
					splat.name = "Red";
				}
				if(naz == "Orange")
				{
					splat = (GameObject)Instantiate (orange);	
					splat.name = "Orange";
				}
				if(naz == "Yellow")
				{
					splat = (GameObject)Instantiate (yellow);	
					splat.name = "Yellow";
				}
				if(naz == "Green")
				{
					splat = (GameObject)Instantiate (green);	
					splat.name = "Green";
				}
				if(naz == "Blue")
				{
					splat = (GameObject)Instantiate (blue);	
					splat.name = "Blue";
				}
				if(naz == "Purple")
				{
					splat = (GameObject)Instantiate (purple);	
					splat.name = "Purple";
				}
				splat.transform.parent = this.gameObject.transform;
				splat.transform.position = new Vector3(Convert.ToSingle(poz[0]),Convert.ToSingle(poz[1]),0);
				}
			}
		}
	}
}
