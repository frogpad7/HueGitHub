using UnityEngine;
using System.Collections;

public class CrackedBlock : MonoBehaviour {

	float deleteTotal = 1;
	float deleteTimer = 0;
	float respawnTotal = 10;
	float respawnTimer = 0;

	bool deleteCountdown = false;
	bool respawnCountdown = false;

	//Detects for player collision to enter the delete countdown
	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.tag == "Player") {
			deleteCountdown = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		//Detects if the delete timer has run out
		if (deleteTimer >= deleteTotal) {
			//Deletes cracked block
			this.renderer.enabled = false;
			this.collider2D.enabled = false;

			//Reset delete timer
			deleteTimer = 0;

			//Enter respawn countdown
			respawnCountdown = true;
			deleteCountdown = false;
		}
		//Detects if the respawn timer has run out
		else if (respawnTimer >= respawnTotal) {
			//Respawns block
			this.renderer.enabled = true;
			this.collider2D.enabled = true;

			//Reset respawn timer
			respawnTimer = 0;

			//Exit respawn countdown
			respawnCountdown = false;

		}

		//Increase delete timer
		if (deleteCountdown == true)
			deleteTimer += 1 * Time.deltaTime;
		//Increase respawn timer
		else if (respawnCountdown == true)
			respawnTimer += 1 * Time.deltaTime;
	}

	void Crumble(){

	}
}
