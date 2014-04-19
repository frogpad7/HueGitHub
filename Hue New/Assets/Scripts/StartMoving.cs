using UnityEngine;
using System.Collections;

public class StartMoving : MonoBehaviour {

	RightAndLeft eScript1;
	UpAndDown eScript2;
	public GameObject parentblock;

	enum directions {RIGHTANDLEFT = 0, UPANDDOWN = 1};
	public int direction = (int)directions.RIGHTANDLEFT;
	float initSpeed;
		
	// Use this for initialization
	void Start() 
	{
		//Get parent object
		parentblock = this.transform.parent.gameObject;

		//Initialize for RightAndLeft script
		if (direction == (int)directions.RIGHTANDLEFT) {
			eScript1 = parentblock.GetComponent<RightAndLeft> ();
			Debug.Log ("parent:" + parentblock.gameObject.tag);
			initSpeed = eScript1.speed;
			eScript1.speed = 0;
		}
		//Initialize for UpAndDown script
		else if (direction == (int)directions.UPANDDOWN) {
			eScript2 = parentblock.GetComponent<UpAndDown> ();
			Debug.Log ("parent:" + parentblock.gameObject.tag);
			initSpeed = eScript2.speed;
			eScript2.speed = 0;
		}
	}
		
	void OnTriggerEnter2D(Collider2D col)
	{
		//Trigger for RightAndLeft script
		if (direction == (int)directions.RIGHTANDLEFT) {
			if (col.gameObject.tag == "Player") 
				eScript1.speed = initSpeed;
		}
		//Trigger for UpAndDown script
		else if (direction == (int)directions.UPANDDOWN) {
			if (col.gameObject.tag == "Player") 
				eScript2.speed = initSpeed;
		}
	}
}
