using UnityEngine;
using System.Collections;

public class UpAndDown : MonoBehaviour {
	
	public int range = 5;
	public float speed = 0.05f;
	enum directions {UP = 0, DOWN = 1};
	public int direction;
	Vector2 initialPosition;
	
	// Use this for initialization
	void Start () {
		initialPosition = new Vector2 (transform.position.x, transform.position.y);
		//direction = (int)directions.UP;
	}
	
	// Update is called once per frame
	void Update () {
		if (direction == (int)directions.UP) {
			this.transform.position = new Vector2 (transform.position.x, transform.position.y + speed * Time.deltaTime);
			if (transform.position.y >= initialPosition.y + range)
				direction = (int)directions.DOWN;
		} else if (direction == (int)directions.DOWN) {
			this.transform.position = new Vector2 (transform.position.x, transform.position.y - speed * Time.deltaTime);
			if (transform.position.y <= initialPosition.y - range)
				direction = (int)directions.UP;
		}
	}
}