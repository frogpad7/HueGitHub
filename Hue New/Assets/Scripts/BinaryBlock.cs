using UnityEngine;
using System.Collections;

public class BinaryBlock : MonoBehaviour {

	public bool present;

	public Sprite isPresent;
	public Sprite notPresent;

	public GameObject other;

	//Detects player leaving block
	void OnTriggerExit2D(Collider2D col) {
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
}
