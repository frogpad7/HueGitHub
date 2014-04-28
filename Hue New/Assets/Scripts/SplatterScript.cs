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

	public GameObject hue;

	// Use this for initialization
	void Start () {
		//PlayerPrefs.SetString ("Backdrop", "");
		DontDestroyOnLoad (gameObject);
		LoadGame ();
	}
	
	// Update is called once per frame
	void Update () {
		Objects = GameObject.FindGameObjectsWithTag("Savable");
	}

	public void Splat(int c, Vector3 pos, Quaternion rot){
		//Vector3 center = gameObject.transform.position;
		//center.z = 0.0f;
		GameObject splat;
		if (c == 1) {
			splat = (GameObject)Instantiate (red);	
			splat.name = "Red";
			splat.transform.parent = this.gameObject.transform;
			splat.transform.position = new Vector3(pos.x,pos.y,0);
			splat.transform.rotation = rot;
		}
		if (c == 2) {
			splat = (GameObject)Instantiate (orange);	
			splat.name = "Orange";
			splat.transform.parent = this.gameObject.transform;
			splat.transform.position = new Vector3(pos.x,pos.y,0);
			splat.transform.rotation = rot;
		}
		if (c == 3) {
			splat = (GameObject)Instantiate (yellow);	
			splat.name = "Yellow";
			splat.transform.parent = this.gameObject.transform;
			splat.transform.position = new Vector3(pos.x,pos.y,0);
			splat.transform.rotation = rot;
		}
		if (c == 4) {
			splat = (GameObject)Instantiate (green);	
			splat.name = "Green";
			splat.transform.parent = this.gameObject.transform;
			splat.transform.position = new Vector3(pos.x,pos.y,0);
			splat.transform.rotation = rot;
		}
		if (c == 5) {
			splat = (GameObject)Instantiate (blue);	
			splat.name = "Blue";
			splat.transform.parent = this.gameObject.transform;
			splat.transform.position = new Vector3(pos.x,pos.y,0);
			splat.transform.rotation = rot;
		}
		if (c == 6) {
			splat = (GameObject)Instantiate (purple);	
			splat.name = "Purple";
			splat.transform.parent = this.gameObject.transform;
			splat.transform.position = new Vector3(pos.x,pos.y,0);
			splat.transform.rotation = rot;
		}
	}

	public void SaveGame(){
		SaveString = "";
		for(int i = 0; i < Objects.Length; i++)
		{
			SaveString += 
				Objects[i].name 
					+ ","+ 
					Objects[i].transform.position.x + "|" + Objects[i].transform.position.y + "|"
					+ ","+ 
					Objects[i].transform.rotation.eulerAngles.z
					+ ";";
			Debug.Log (SaveString);
		}
		PlayerPrefs.SetString ("Backdrop1", SaveString);
	}
	
	void LoadGame(){	
		if(PlayerPrefs.HasKey("Backdrop")&&PlayerPrefs.GetString("Backdrop").Length>0){
			LoadString = PlayerPrefs.GetString ("Backdrop1");
			string[] ObjectsLoaded = LoadString.Split(';');
			
			foreach(string record in ObjectsLoaded)
			{
				if(record.Length>0){
				string[] recordSelected = record.Split(',');
				
				string naz, poz1, rot1;
				string[] poz, rot;
				
				//Vector3 p;
				//Quaternion r;
				
				naz = recordSelected[0].ToString();
				Debug.Log("Loaded: "+naz);
				
				poz1 = recordSelected[1].ToString();
				Debug.Log("Loaded: "+poz1);
				
				rot1 = recordSelected[2].ToString();
				Debug.Log("Loaded: "+rot1);

				poz = poz1.Split('|');
				//rot = rot1.Split('|');
				
				//p.x = Convert.ToSingle(poz[0]);
				//p.y = Convert.ToSingle(poz[1]);
				//p.z = 0;

				//r.x = 0;
				//r.y = 0;
				//r.z = Convert.ToSingle(rot1);
				//r.w = 1;

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
				splat.transform.rotation = Quaternion.Euler (new Vector3(0,0,Convert.ToSingle(rot1)));
				}
			}
		}
	}

	public void CreateHue(){
		Instantiate (hue);
	}
}
