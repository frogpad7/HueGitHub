using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour 
{
	public float speed = 0;
	private float lifetime = 0;

	Vector3 initialPosition;
	public bool follow = false;
	public bool walker = false;
	//public bool jumper = false;
	public int moveRange = 10;//0-stationary else-directional
	enum directions {LEFT=0, RIGHT=1, UP=2, DOWN=3};
	int direction;
	private SpriteRenderer myRenderer;
	public Sprite frozenPlatform;

	// Use this for initialization
	void Start () 
	{
		initialPosition = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
		direction = (int)directions.LEFT;
		myRenderer = gameObject.GetComponent<SpriteRenderer>();
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Projectile" || col.gameObject.tag == "Red" ||col.gameObject.tag == "Grenade"||
			col.gameObject.tag == "Blue" || col.gameObject.tag == "Purple") 
		{
			//if grenade, explode
			//if glove, destroy enemy
			//if paint, stun
			//if freeze, make block
			//if bubble, stun
			//Collider2D boom = Physics2D.OverlapCircle(gameObject.transform.position,10f);
			if (col.gameObject.tag != "Grenade") Destroy (col.gameObject);
			Destroy (gameObject);
		} 
		else if (col.gameObject.tag == "Orange") 
		{
            this.rigidbody2D.isKinematic = true;
            speed = 0;
            lifetime = Time.time + 5;
			myRenderer.sprite = frozenPlatform;
			Destroy(col.gameObject);
		}
		else if (col.gameObject.tag == "Green")
		{
			//lifetime = Time.time + 5;
			//this.rigidbody2D.isKinematic = true;
			//speed = 0;
		}
		else if (col.gameObject.tag == "Yellow")
		{
			lifetime = Time.time + 5;
			this.rigidbody2D.isKinematic = true;
			speed = 0;
		}
		else if (col.gameObject.tag == "Stage") {
			changeDirection();
		}
	}

	//for dash and bubble shield
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Projectile") 
		{
			Debug.Log ("dash/bubble");
			Destroy(col.gameObject);
			Destroy(gameObject);
		}
	}

	// Update is called once per frame
	void Update () 
	{
		if (lifetime <= Time.time && this.rigidbody2D.isKinematic == true) Destroy(gameObject);
		if (walker) 
		{
			rigidbody2D.gravityScale=1;		
		}
		else
			rigidbody2D.gravityScale=0;
		if (follow) 
		{
			transform.position += Vector3.Normalize(GameObject.FindWithTag("Player").transform.position-this.transform.position)*.1f;
		}
		else if (direction == (int)directions.LEFT) 
		{
			this.transform.position = new Vector3(transform.position.x+speed * Time.deltaTime, transform.position.y, transform.position.z);
			if (transform.position.x >= initialPosition.z + moveRange) direction = (int)directions.RIGHT;
		}
		else if (direction == (int)directions.RIGHT) 
		{
			this.transform.position = new Vector3(transform.position.x-speed * Time.deltaTime, transform.position.y, transform.position.z);
			if (transform.position.x <= initialPosition.z - moveRange) direction = (int)directions.LEFT;
		}
		else if (direction == (int)directions.UP) 
		{
			this.transform.position = new Vector3(transform.position.x, transform.position.y+ speed * Time.deltaTime, transform.position.z);
			if (transform.position.y >= initialPosition.y + moveRange) direction = (int)directions.DOWN;
		}
		else if (direction == (int)directions.DOWN)
		{
			this.transform.position = new Vector3(transform.position.x, transform.position.y - speed * Time.deltaTime, transform.position.z);
			if (transform.position.y <= initialPosition.y - moveRange) direction = (int)directions.UP;
		}

	}

	void changeDirection(){
		if (direction == (int)directions.DOWN)
			direction = (int)directions.UP;
		else if (direction == (int)directions.UP)
			direction = (int)directions.DOWN;
		else if (direction == (int)directions.LEFT)
			direction = (int)directions.RIGHT;
		else if (direction == (int)directions.RIGHT)
			direction = (int)directions.LEFT;
	}
}
