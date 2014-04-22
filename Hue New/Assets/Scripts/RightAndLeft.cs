﻿using UnityEngine;
using System.Collections;

public class RightAndLeft : MonoBehaviour {

	public int range = 5;
	public float speed = 0.05f;
	enum directions {RIGHT = 0, LEFT = 1};
	public int direction;
	Vector2 initialPosition;

	// Use this for initialization
	void Start () {
		initialPosition = new Vector2 (transform.position.x, transform.position.y);
		//direction = (int)directions.LEFT;
	}
	
	// Update is called once per frame
	void Update () {
		if (direction == (int)directions.RIGHT) {
			this.transform.position = new Vector2 (transform.position.x + speed * Time.deltaTime, transform.position.y);
			if (transform.position.x >= initialPosition.x + range)
				direction = (int)directions.LEFT;
		} else if (direction == (int)directions.LEFT) {
			this.transform.position = new Vector2 (transform.position.x - speed * Time.deltaTime, transform.position.y);
			if (transform.position.x <= initialPosition.x - range)
				direction = (int)directions.RIGHT;
		}
	}
}
