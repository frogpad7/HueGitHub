using UnityEngine;
using System.Collections;

public class CrackedBlock : MonoBehaviour {

	Sprite initSprite;

	//Initialize initial sprite for reset
	void Start () {
		initSprite = this.gameObject.GetComponent<SpriteRenderer> ().sprite;
	}

	//Detects for player collision to enter the delete countdown
	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.tag == "Player") {
			this.gameObject.GetComponent<Animator> ().enabled = true;
		}
	}

	//Reset blocks
	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "Player") {
			this.renderer.enabled = true;
			this.collider2D.enabled = true;
			this.gameObject.GetComponent<Animator> ().enabled = false;
			this.gameObject.GetComponent<Animator> ().Play("Cracking", 0, 0f);
			this.gameObject.GetComponent<SpriteRenderer> ().sprite = initSprite;
		}
	}

	//Destroys block after animation
	void Crumble(){
		this.renderer.enabled = false;
		this.collider2D.enabled = false;
		this.gameObject.GetComponent<Animator> ().enabled = false;
	}
}
