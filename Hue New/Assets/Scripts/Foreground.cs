using UnityEngine;
using System.Collections;

public class Foreground : MonoBehaviour {

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.tag == "Player") {
			//Turn block red
			if (col.gameObject.GetComponent<PlayerScript> ().color == 1)
				this.GetComponent<SpriteRenderer> ().color = new Color(1f, 0f, 0f);

			//Turn block orange
			else if (col.gameObject.GetComponent<PlayerScript> ().color == 2) 
				this.GetComponent<SpriteRenderer> ().color = new Color(1f, 0.5f, 0f);

			//Turn block yellow
			else if (col.gameObject.GetComponent<PlayerScript> ().color == 3)
				this.GetComponent<SpriteRenderer> ().color = new Color(1f, 1f, 0f);

			//Turn block green
			else if (col.gameObject.GetComponent<PlayerScript> ().color == 4)
				this.GetComponent<SpriteRenderer> ().color = new Color(0f, 1f, 0f);

			//Turn block blue
			else if (col.gameObject.GetComponent<PlayerScript> ().color == 5)
				this.GetComponent<SpriteRenderer> ().color = new Color(0f, 0f, 1f);

			//Turn block purple
			else if (col.gameObject.GetComponent<PlayerScript> ().color == 6)
				this.GetComponent<SpriteRenderer> ().color = new Color(1f, 0f, 1f);
		}
	}
}
