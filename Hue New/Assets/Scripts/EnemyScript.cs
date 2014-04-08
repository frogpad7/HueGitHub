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
	public float speed = 0;
	public float initSpeed;
	private float lifetime = 0;

	bool facingRight = false;
	
	//enemy type flags
	public bool follow = false;
	public bool walker = false;
	public bool shooter = false;
	public bool frozen = false;
	//public bool jumper = false;
	public int dir;
	public bool faceL = true;
	
	//freeze & groundPound related
	public Rigidbody2D bullet;
	public Sprite frozenPlatform;
	private SpriteRenderer myRenderer;

	Animator anim;

	public Sprite pFreeze;
	public Sprite oFreeze;

	void Start () 
	{
		initSpeed = speed;

		if(gameObject.tag == "Enemy") anim = GetComponent<Animator> ();
		initialPosition = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
		direction = dir;
		myRenderer = gameObject.GetComponent<SpriteRenderer>();

		if (follow)		speed = 0;
		if (!faceL)     Flip ();
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

	void OnTriggerEnter2D(Collider2D col)
	{
		//For allowing follow enemies to move when player is in range
		if (follow) {
			if (col.gameObject.tag == "Player")
				speed = initSpeed;
		}

		//for dash and bubble shield
		if (col.gameObject.tag == "Blue") 
		{
			PlayerScript pScript = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
			if (pScript.cheatMode != true) Destroy(col.gameObject);
			Destroy(gameObject);
		}
	}

	//For stopping follow enemies from moving when player is out of range
	void OnTriggerExit2D(Collider2D col) {
		if (follow) {
			if (col.gameObject.tag == "Player")
				speed = 0;
		}
	}

	void enemy_AI()
	{
		Shoot ();
		if (follow && !frozen)
		{
			Vector3 dir = Vector3.Normalize (GameObject.FindWithTag ("Player").transform.position - this.transform.position) * .1f;
			if(dir.x<0 && facingRight || dir.x>0 && !facingRight) Flip ();
			transform.position += dir * (Time.deltaTime * 10 * speed);
		}
		else if (direction == (int)directions.LEFT) 
		{
			this.transform.position = new Vector3(transform.position.x-speed * Time.deltaTime, transform.position.y, transform.position.z);
			if (transform.position.x < initialPosition.x - moveRange) changeDirection();
		}
		else if (direction == (int)directions.RIGHT) 
		{
			this.transform.position = new Vector3(transform.position.x+speed * Time.deltaTime, transform.position.y, transform.position.z);
			if (transform.position.x > initialPosition.x + moveRange) changeDirection();
		}
		else if (direction == (int)directions.UP) 
		{
			this.transform.position = new Vector3(transform.position.x, transform.position.y+ speed * Time.deltaTime, transform.position.z);
			if (transform.position.y > initialPosition.y + moveRange) changeDirection();
		}
		else if (direction == (int)directions.DOWN)
		{
			this.transform.position = new Vector3(transform.position.x, transform.position.y - speed * Time.deltaTime, transform.position.z);
			if (transform.position.y < initialPosition.y - moveRange) changeDirection();
		}
	}

	void Shoot()
	{
		if (shooter && cooldown <= Time.time)
		{
			if (facingRight)
			{
				Vector3 firePos = this.transform.position + new Vector3 (5, 0, 0);
				Rigidbody2D fireObj = (Rigidbody2D)Instantiate (bullet, firePos, Quaternion.Euler (new Vector3 (0, 5, 0)));
				fireObj.velocity = new Vector2 (15, 0);
			}
			else if (!facingRight)
			{
				Vector3 firePos = this.transform.position + new Vector3 (-5, 0, 0);
				Rigidbody2D fireObj = (Rigidbody2D)Instantiate (bullet, firePos, Quaternion.Euler (new Vector3 (0, 5, 0)));
				fireObj.velocity = new Vector2 (-15, 0);
			}
			cooldown = Time.time + 2;
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
		myRenderer.sprite = pFreeze;
	}
	
	void freezeEnemy(Collision2D col)
	{
		this.rigidbody2D.isKinematic = true;
		frozen = true;
		speed = 0;
		lifetime = Time.time + 7;
		myRenderer.sprite = oFreeze;
		Destroy(col.gameObject);
	}

	void thaw()
	{
		this.rigidbody2D.isKinematic = false;
		frozen = false;
		speed = 5;
		//myRenderer.sprite = frozenPlatform;
	}
	
	void Flip()
	{
		facingRight = !facingRight;
		Vector3 playScale = transform.localScale;
		playScale.x *= -1;
		transform.localScale = playScale;
	}
}
