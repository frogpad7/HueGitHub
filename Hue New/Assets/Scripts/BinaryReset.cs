using UnityEngine;
using System.Collections;

public class BinaryReset : MonoBehaviour {

	//Resets binary blocks
	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "Player") {
			foreach(Transform child in this.transform) {
				//Reset boolean values
				child.gameObject.GetComponent<BinaryBlock> ().present = true;
				child.gameObject.GetComponent<BinaryBlock> ().other.gameObject.GetComponent<BinaryBlock> ().present = false;

				//Reset box colliders
				child.gameObject.GetComponent<BoxCollider2D> ().enabled = true;
				child.gameObject.GetComponent<BinaryBlock> ().other.gameObject.GetComponent<BoxCollider2D> ().enabled = false;

				//Reset sprites
				child.gameObject.GetComponent<SpriteRenderer> ().sprite = child.gameObject.GetComponent<BinaryBlock> ().isPresent;
				child.gameObject.GetComponent<BinaryBlock> ().other.gameObject.GetComponent<SpriteRenderer> ().sprite = child.gameObject.GetComponent<BinaryBlock> ().other.gameObject.GetComponent<BinaryBlock> ().notPresent;
			}
		}
	}
}
