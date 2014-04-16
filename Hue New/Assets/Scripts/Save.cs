using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;

public class Save : MonoBehaviour {
	/// <summary>
	/// The objects.
	/// </summary>
	public GameObject[] Objects;
	public string SaveString;
	
	/// <summary>
	/// The objects loaded.
	/// </summary>
	public string LoadString;
	
	/// <summary>
	/// GameObjects
	/// </summary>
	public GameObject Rock2;
	public GameObject Rock4;
	public GameObject Player;
	public GameObject Fence;
	
	void Start(){
		LoadGame();
	}
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.F5))
		{
			Objects = GameObject.FindGameObjectsWithTag("Savable");
			SaveGame();
		}
	} 
	
	void SaveGame(){
		SaveString = "";
		for(int i = 0; i < Objects.Length; i++)
		{
			SaveString += 
				Objects[i].name 
					+ ","+ 
					Objects[i].transform.position.x + "|" + Objects[i].transform.position.y + "|" +Objects[i].transform.position.z + "|"
					+ ";";
		}

		FileOperations.WriteToFile("Saves/Save.isf", SaveString);
	}
	
	void LoadGame(){	
		LoadString = FileOperations.ReadFile("Saves/Save.isf");
		string[] ObjectsLoaded = LoadString.Split(';');
		
		foreach(string record in ObjectsLoaded)
		{
			string[] recordSelected = record.Split(',');
			
			string naz, poz1, rot1;
			string[] poz, rot;
			
			Vector3 pozycja;
			Quaternion rotacja;
			
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
			pozycja.z = Convert.ToSingle(poz[2]);
			
			rotacja.x = 0;
			rotacja.y = 0;
			rotacja.z = 0;
			rotacja.w = 1;
			
			if(naz == "Player" || naz == "Player(Clone)")
			{
				Instantiate(Player, pozycja, rotacja);
			}
			
			if(naz == "Rock2" || naz == "Rock2(Clone)")
			{
				Instantiate(Rock2, pozycja, rotacja);
			}
			
			if(naz == "Rock4" || naz == "Rock4(Clone)")
			{
				Instantiate(Rock4, pozycja, rotacja);
			}
			if(naz == "Fence" || naz == "Fence(Clone)")
			{
				Instantiate(Fence, pozycja, rotacja);
			}
		}
	}
}
//By Erdroy (c) HLTC
//