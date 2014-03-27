using UnityEngine;
using System.Collections;

public class BinaryBlock : MonoBehaviour {
	
	int respawnTotal = 600;
	int respawnTimer = 0;

	Vector3 initialPosition;
	public Vector3 destinationParent = new Vector3(0, 0, 0);
	Vector3 destinationPosition;

	bool destination = false;

	// Use this for initialization
	void Start () { 
		initialPosition = this.transform.position;
		destinationPosition = new Vector3 (destinationParent.x + this.transform.localPosition.x, destinationParent.y + this.transform.localPosition.y, 0);
	}

	//Detects player leaving block
	void OnCollisionExit2D(Collision2D col) {
		//Check if collision with the player
		if (col.gameObject.tag == "Player") {
			//Check if in the initial position or destination position
			if (destination == false) {
				//Move to destination position
				this.transform.position = destinationPosition;
				destination = true;
			} else {
				//Move to initial position
				this.transform.position = initialPosition;
				destination = false;
			}
		}
	}

	// Update is called once per frame
	void Update () {
		//Check if in the destination position
		if (destination == true) {
			//Check if respawn timer has run out
			if (respawnTimer == respawnTotal) {
				//Move back to initial position
				this.transform.position = initialPosition;
				destination = false;
			
				//Reset respawn timer
				respawnTimer = 0;
			} else {
				//Increase respawn timer
				respawnTimer += 1;
			}
		}
	}
}
