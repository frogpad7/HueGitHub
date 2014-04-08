using UnityEngine;
using System.Collections;

public class SplatterScript : MonoBehaviour {

	public Sprite red;
	public Sprite orange;
	public Sprite yellow;
	public Sprite green;
	public Sprite blue;
	public Sprite purple;

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
		if (s.name == "Red_Splatter")
			splat.name = "Red";
		if (s.name == "Orange_Splatter")
			splat.name = "Orange";
		if (s.name == "Yellow_Splatter")
			splat.name = "Yellow";
		if (s.name == "Green_Splatter")
			splat.name = "Green";
		if (s.name == "Blue_Splatter")
			splat.name = "Blue";
		if (s.name == "Purple_Splatter")
			splat.name = "Purple";
		else
			splat.name = "Unknown";
		//Instantiate (splat, pos, Quaternion.Euler (0, 0, 0));
	}
}
