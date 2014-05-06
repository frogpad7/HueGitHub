using UnityEngine;
using System.Collections;

public class CrackedBlock : MonoBehaviour {

	//Detects for player collision to enter the delete countdown
	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.tag == "Player") {
			this.gameObject.GetComponent<Animator> ().enabled = true;
		}
	}

	//Destroys block after animation
	void Crumble(){
		this.renderer.enabled = false;
		this.collider2D.enabled = false;
		this.gameObject.GetComponent<Animator> ().enabled = false;
	}
}
