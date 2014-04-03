using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour 
{

	enum directions {LEFT=0, RIGHT=1, UP=2, DOWN=3};
	
	//status and attributes
	public Vector3 initialPosition;
	public int moveRange = 10; //0-stationary else-directional
	private int direction;
	private float cooldown = 0;
	public float speed = 1;
	private float lifetime = 0;

	bool facingRight = false;
	
	//enemy type flags
	public bool follow = false;
	public bool walker = false;
	public bool shooter = false;
	public bool frozen = false;
	//public bool jumper = false;
	
	//freeze & groundPound related
	public Rigidbody2D freeze;
	public Sprite frozenPlatform;
	private SpriteRenderer myRenderer;

	Animator anim;

	void Start () 
	{
		if(gameObject.tag == "Enemy") anim = GetComponent<Animator> ();
		initialPosition = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
		direction = (int)directions.LEFT;
		myRenderer = gameObject.GetComponent<SpriteRenderer>();
		
		if (walker) 	rigidbody2D.gravityScale=1; 
		else			rigidbody2D.gravityScale=0;
	}
	// Update is called once per frame
	void Update () 
	{
		if(gameObject.tag == "Enemy")
			anim.SetFloat ("Speed", speed);
		if (!frozen) enemy_AI();
		if (lifetime <= Time.time && frozen) thaw();
	}
	
	void OnCollisionEnter2D(Collision2D col)
	{

		if (frozen) return;
		
		else if (col.gameObject.tag == "Yellow") 	changeDirection();
		else if (col.gameObject.tag == "Orange") 	freezeEnemy(col);
		
		if (col.gameObject.tag == "Projectile" || col.gameObject.tag == "Red" ||col.gameObject.tag == "Grenade"||
		    col.gameObject.tag == "Blue" || col.gameObject.tag == "Purple") 
		{
			if (col.gameObject.tag != "Grenade") Destroy (col.gameObject);
			Destroy (gameObject);
		} 
		//if 		(col.gameObject.tag == "Stage") 	changeDirection();
	}
	
	//for dash and bubble shield
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Projectile") 
		{
			Destroy(col.gameObject);
			Destroy(gameObject);
		}
	}
	void enemy_AI()
	{
		if (shooter && cooldown <= Time.time)
		{
			//this instantly kills the player? 
			/*
			Vector3 firePos = this.transform.position + new Vector3 (0, 5, 0);
			Rigidbody2D fireObj = Instantiate (freeze, firePos, Quaternion.Euler (new Vector3 (0, 5, 0))) as Rigidbody2D;
			fireObj.velocity = new Vector2 (0, 15);
			cooldown = Time.time + 3;*/
		}
		
		if (follow && !frozen)
		{
			Vector3 dir = Vector3.Normalize (GameObject.FindWithTag ("Player").transform.position - this.transform.position) * .1f;
			if(dir.x<0 && facingRight || dir.x>0 && !facingRight) Flip ();
			transform.position += dir * (Time.deltaTime * 10 * speed);
		}
		else if (direction == (int)directions.LEFT) 
		{
			this.transform.position = new Vector3(transform.position.x-speed * Time.deltaTime, transform.position.y, transform.position.z);
			if (transform.position.x <= initialPosition.x - moveRange) changeDirection();
		}
		else if (direction == (int)directions.RIGHT) 
		{
			this.transform.position = new Vector3(transform.position.x+speed * Time.deltaTime, transform.position.y, transform.position.z);
			if (transform.position.x >= initialPosition.x + moveRange) changeDirection();
		}
		else if (direction == (int)directions.UP) 
		{
			this.transform.position = new Vector3(transform.position.x, transform.position.y+ speed * Time.deltaTime, transform.position.z);
			if (transform.position.y >= initialPosition.y + moveRange) changeDirection();
		}
		else if (direction == (int)directions.DOWN)
		{
			this.transform.position = new Vector3(transform.position.x, transform.position.y - speed * Time.deltaTime, transform.position.z);
			if (transform.position.y <= initialPosition.y - moveRange) changeDirection();
		}
	}
	
	void changeDirection(){
		if (direction == (int)directions.DOWN)
			direction = (int)directions.UP;
		else if (direction == (int)directions.UP)
			direction = (int)directions.DOWN;
		else if (direction == (int)directions.LEFT) {
			direction = (int)directions.RIGHT;
			Flip ();
		} 
		else if (direction == (int)directions.RIGHT) {
			direction = (int)directions.LEFT;
			Flip ();
		}
	}
	public void groundPounded()
	{
		lifetime = Time.time + 10;
		frozen = true;
		this.rigidbody2D.isKinematic = true;
		speed = 5;
	}
	
	void freezeEnemy(Collision2D col)
	{
		this.rigidbody2D.isKinematic = true;
		frozen = true;
		speed = 0;
		lifetime = Time.time + 7;
		myRenderer.sprite = frozenPlatform;
		Destroy(col.gameObject);
	}

	void thaw()
	{
		this.rigidbody2D.isKinematic = false;
		frozen = false;
		speed = 5;
		myRenderer.sprite = frozenPlatform;
	}
	
	void Flip()
	{
		facingRight = !facingRight;
		Vector3 playScale = transform.localScale;
		playScale.x *= -1;
		transform.localScale = playScale;
	}
}
