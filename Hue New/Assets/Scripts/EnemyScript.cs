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
	private bool isPlatform = false;
	public float speed = 0;
	public float initSpeed;
	private float lifetime = 0;

	bool facingRight = false;
	
	//enemy type flags
	public bool follow = false;
	public bool walker = false;
	public bool shooter = false;
	public bool downShooter = false;
	public bool frozen = false;
	//public bool jumper = false;
	public int dir;
	public bool faceL = true;
	
	//freeze & groundPound related
	public Rigidbody2D bullet;
	public Sprite frozenPlatform;
	SpriteRenderer myRenderer;
	PlayerScript pScript;

	Animator anim;
	bool orange;

	public Sprite pFreeze;
	public Sprite oFreeze;

	void Start () 
	{
		do { pScript = GameObject.FindWithTag ("Player").GetComponent<PlayerScript> (); }
		while(!GameObject.FindWithTag("Player"));

		if (downShooter) 
		{
			shooter = false;
			Debug.Log("ERR : An ENEMY cannot be both a down-shooter and a shooter!");
			Debug.Log("WARN: ENEMY defaulted to regular shooter!");
		}

		initSpeed = speed;

		if(gameObject.tag == "Enemy") anim = GetComponent<Animator> ();
		initialPosition = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
		direction = dir;
		myRenderer = this.gameObject.GetComponent<SpriteRenderer>();

		if (follow)		speed = 0;
		if (!faceL)     Flip ();
		if (walker) 	rigidbody2D.gravityScale=1; 
		else			rigidbody2D.gravityScale=0;
	}
	// Update is called once per frame
	void Update () 
	{
		if (gameObject.tag == "Enemy") anim.SetFloat ("Speed", speed);
		if (!frozen && pScript.alive)  enemy_AI();
		else if (lifetime <= Time.time && isPlatform) thaw();
	}
	
	void OnCollisionEnter2D(Collision2D col)
	{
		if (frozen && col.gameObject.tag != "Player") Destroy(col.gameObject);
		
		else if (col.gameObject.tag == "Yellow") 	changeDirection();
		else if (col.gameObject.tag == "Orange") 	freezeEnemy(col);
		else if (col.gameObject.tag == "Purple") 	{ 
			Destroy (gameObject); 
			Destroy(col.gameObject); 
			float rand = Random.value * 360;
			GameObject.FindWithTag ("Backdrop").GetComponent<SplatterScript> ().Splat (6, transform.position, Quaternion.Euler(new Vector3(0,0,rand)));
		}
		
	//if 		(col.gameObject.tag == "Stage") 	changeDirection();
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		//For allowing follow enemies to move when player is in range
		if (follow) 
			if (col.gameObject.tag == "Player")	speed = initSpeed;

	}

	//For stopping follow enemies from moving when player is out of range
	void OnTriggerExit2D(Collider2D col) 
	{
		if (follow)
			if (col.gameObject.tag == "Player")	speed = 0;
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
		else if (downShooter && cooldown <= Time.time)
		{
			Vector3 firePos = this.transform.position + new Vector3 (0, -5, 0);
			Rigidbody2D fireObj = (Rigidbody2D)Instantiate (bullet, firePos, Quaternion.Euler (new Vector3 (0, -5, 0)));
			fireObj.velocity = new Vector2 (0, -15);
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
		lifetime = Time.time + 6;
		frozen = true;
		isPlatform = true;
		orange = false;
		this.rigidbody2D.isKinematic = true;
		speed = 5;
		anim.SetBool ("Platform", false);
		anim.SetBool ("Frozen", frozen);
	}
	
	void freezeEnemy(Collision2D col)
	{
		lifetime = Time.time + 7;
		frozen = true;
		isPlatform = true;
		orange = true;
		this.rigidbody2D.isKinematic = true;
		anim.SetBool ("Platform", true);
		anim.SetBool ("Frozen", frozen);
		//speed = 5;

		//myRenderer.sprite = oFreeze;
		Destroy(col.gameObject);
	}

	void thaw()
	{
		Debug.Log("Thawing Enemy");
		this.rigidbody2D.isKinematic = false;
		frozen = false;
		anim.SetBool ("Frozen", frozen);
		//speed = 5;
		if (walker) this.rigidbody2D.gravityScale = 1;
		isPlatform = false;
		if (orange)
			GameObject.FindWithTag ("Backdrop").GetComponent<SplatterScript> ().Splat (2, transform.position, new Quaternion ());
		else {
			float rand = Random.value * 360;
			GameObject.FindWithTag ("Backdrop").GetComponent<SplatterScript> ().Splat (6, transform.position, Quaternion.Euler (new Vector3(0,0,rand)));
		}
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