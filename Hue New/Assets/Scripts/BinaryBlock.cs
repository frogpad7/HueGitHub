using UnityEngine;
using System.Collections;

public class BinaryBlock : MonoBehaviour {
	
	//float respawnTotal = 10;
	//float respawnTimer = 0;

	public bool present;

	public Sprite isPresent;
	public Sprite notPresent;

	public GameObject other;

	// Use this for initialization
	void Start () { 
	
	}

	//Detects player leaving block
	void OnCollisionExit2D(Collision2D col) {
		//Check if collision with the player
		if (col.gameObject.tag == "Player") {
			//Check if the block is present
			if (present == true) {
				//Switch this block to not present
				present = false;
				this.gameObject.GetComponent<SpriteRenderer>().sprite = notPresent;
				this.gameObject.GetComponent<BoxCollider2D>().enabled = false;

				//Switch other block to present
				other.gameObject.GetComponent<BinaryBlock>().present = true;
				other.gameObject.GetComponent<SpriteRenderer>().sprite = other.gameObject.GetComponent<BinaryBlock>().isPresent;
				other.gameObject.GetComponent<BoxCollider2D>().enabled = true;
			}
		}
	}

	// Update is called once per frame
	void Update () {
//		//Check if in the destination position
//		if (destination == true) {
//			//Check if respawn timer has run out
//			if (respawnTimer >= respawnTotal) {
//				//Move back to initial position
//				this.transform.position = initialPosition;
//				destination = false;
//			
//				//Reset respawn timer
//				respawnTimer = 0;
//			} else {
//				//Increase respawn timer
//				respawnTimer += 1 * Time.deltaTime;
//			}
//		} 
	}
}
