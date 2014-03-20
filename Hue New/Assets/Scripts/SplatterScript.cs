using UnityEngine;
using System.Collections;

public class SplatterScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Splat(Sprite s, Vector3 pos){
		Debug.Log ("SPLAT!!!");
		//Vector3 center = gameObject.transform.position;
		//center.z = 0.0f;
		GameObject splat = new GameObject ();
		splat.transform.parent = this.gameObject.transform;
		splat.transform.position = new Vector3(pos.x,pos.y,0);
		splat.AddComponent<SpriteRenderer> ();
		splat.GetComponent<SpriteRenderer> ().sortingLayerName = "Canvas";
		splat.GetComponent<SpriteRenderer> ().sprite = s;
		if (s.name == "Block_Red")
			splat.name = "Red";
		if (s.name == "Block_Orange")
			splat.name = "Orange";
		if (s.name == "Block_Yellow")
			splat.name = "Yellow";
		if (s.name == "Block_Green")
			splat.name = "Green";
		if (s.name == "Block_Blue")
			splat.name = "Blue";
		if (s.name == "Block_Purple")
			splat.name = "Purple";
		else
			splat.name = "Unknown";
		//Instantiate (splat, pos, Quaternion.Euler (0, 0, 0));
	}
}
