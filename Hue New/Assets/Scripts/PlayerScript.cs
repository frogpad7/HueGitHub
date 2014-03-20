using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour 
{
	public float maxSpeed = 20;
	bool facingRight = true;
	bool isDoubleJumping = false;
	bool alive = true;
	bool grounded = true;
	bool rBlocked = true;
	float groundRad = 0.1f;
	//controls bubble ability when in air & not in jump control
	bool flying = false;

	Animator anim;
	int color = 0;
	float cooldown = 0;
	float bubbleTime = 0;

	//public Rigidbody2D backdrop;
	public Rigidbody2D ball;
	public Rigidbody2D grenade;
	public Rigidbody2D freeze;
	public Rigidbody2D paintWall;
	public Rigidbody2D dash;
	public Rigidbody2D bubble;
	public Rigidbody2D bubbleSH;
	public Rigidbody2D glove;

	Rigidbody2D bubbleShield;

	public Transform abilityPos;
	public Transform groundCheck;
	public Transform rightCheck;
	public LayerMask whatIsGround;

	// Use this for initialization
	void Start () { anim = GetComponent<Animator>(); }
		
	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Enemy" && col.gameObject.rigidbody2D.isKinematic == false)
		{
			alive = false;
			GameObject.FindWithTag("MainCamera").GetComponent<CameraFollow>().enabled = false;

			//GetComponent<Transform>().DetachChildren();
			this.GetComponent<PlayerScript>().enabled = false;
			StartCoroutine("PlayerRestart");
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		//changes color when passing through the color object
		int c = color;
		if (col.gameObject.tag 		== "Red")		c = 1;
		else if (col.gameObject.tag == "Orange") 	c = 2;
		else if (col.gameObject.tag == "Yellow") 	c = 3;
		else if (col.gameObject.tag == "Green")  	c = 4;
		else if (col.gameObject.tag == "Blue")   	c = 5;
		else if (col.gameObject.tag == "Purple") 	c = 6;

		anim.SetInteger ("Color", c);
		
		if (c != color) 
		{
			GameObject.FindWithTag("Audio").GetComponent<AudioScript>().ChangeTrack(c);
			if(bubbleShield)
			{
				Destroy(bubbleShield.gameObject);
				bubbleShield = null;
			}
		}
		color = c;

		//transitions scene when reached ending/goal
		if (col.gameObject.tag == "ChamberEnd")
		{
			DontDestroyOnLoad (transform.gameObject);
			DontDestroyOnLoad (transform.FindChild("Audio"));
			StartCoroutine("PlayerSceneChange");
		}
		if (col.gameObject.tag == "Goal") 	Debug.Log ("Goal Reached");
		//backdrop.transform.position = new Vector3(0,0,-10);
	}
	
	IEnumerator PlayerRestart()
	{
		//animate death
		AutoFade.LoadLevel ("ColorRoom", 3, 1, Color.black);
		yield return new WaitForSeconds(1);
		Destroy (gameObject);
	}
	
	IEnumerator PlayerSceneChange()
	{
		AutoFade.LoadLevel ("Physics", 3, 1, Color.black);
		yield return new WaitForSeconds(3);
		transform.position = new Vector3(0,0,0);
	}

	// Update is called once per frame
	void FixedUpdate () 
	{
		//checks to see if player can jump
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundRad, whatIsGround);

		rBlocked = Physics2D.OverlapCircle(rightCheck.position, groundRad, whatIsGround);
		//make sure we can't triple jump
		if (grounded && isDoubleJumping)  isDoubleJumping = false;

		float move = Input.GetAxis("Horizontal");
		if(rBlocked && move > 0)
			move = 0;

		float moveY = Input.GetAxis("Vertical");
		float s = move;
		if (move < 0)
			s *= -1;
		anim.SetFloat("Speed", s);

		//make sure the bubble doesn't go running away
		if (bubbleShield) bubbleShield.transform.position = rigidbody2D.transform.position;

		if(bubbleShield && flying)
		{
			//if we have a bubble shield, we can fly
			rigidbody2D.velocity = new Vector2(move * maxSpeed, moveY * maxSpeed);
		}
		else
		{
			//moves player and the object held
			rigidbody2D.velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);
		}	

		//backdrop.rigidbody2D.velocity = new Vector2 (move * maxSpeed, rigidbody2D.velocity.y);

		//flips player if they change direction
		if (move > 0 && !facingRight) Flip ();
		else if (move < 0 && facingRight)	Flip ();
	}
		
	void Update()
	{
		//debugging color change
		if (Input.GetKeyDown (KeyCode.LeftControl)) 
		{
			color= (color % 6) + 1;
			anim.SetInteger("Color",color);
		}

		if (grounded) flying = false;
		else if (!grounded && (Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.DownArrow)))
			flying = true;

		//double jump Space key control
		if (!isDoubleJumping && Input.GetKeyDown (KeyCode.Space)) 
		{
			rigidbody2D.AddForce (new Vector2 (0, 3500f));
			isDoubleJumping = true;
			//Debug.Log (isDoubleJumping);
		}
		//jumping from the ground, and reset the double jump flag
		else if (grounded && Input.GetKeyDown(KeyCode.Space)) 
		{
			rigidbody2D.velocity= new Vector2(rigidbody2D.velocity.x,0f);
			rigidbody2D.AddForce (new Vector2 (0, 3500f));
			isDoubleJumping = false;
		} 

		//when we have a bubble active, this allows hover
		if (color == 5 && bubbleShield && !grounded && flying && bubbleTime >= Time.time) 
		{
			Debug.Log ("FLOATING");
			//Debug.Log (gameObject.rigidbody2D.velocity.y);
			//gameObject.rigidbody2D.isKinematic = true;
			gameObject.rigidbody2D.gravityScale = 0;

		}
		else if (bubbleShield && bubbleTime < Time.time) 
		{
			Debug.Log("Bubble Over!");
			Destroy(bubbleShield.gameObject);
			//splatter blue spray
			bubbleShield = null;
			flying = false;
			gameObject.rigidbody2D.gravityScale = 1;
		}
		else gameObject.rigidbody2D.gravityScale = 1;

		//else(grounded)
		//		isDoubleJumping = false;

		//ability button control
		if (Input.GetKeyDown (KeyCode.LeftShift)) 
		{
			//RED: GRENADE
			if(color == 1)
			{
				if(Input.GetKey(KeyCode.DownArrow) && cooldown <= Time.time)
				{
					Vector3 firePos;
					if(facingRight) 	firePos = transform.position + new Vector3(2,0,0);
					else				firePos = transform.position + new Vector3(-2,0,0);
					
					Rigidbody2D fireObj = Instantiate(grenade, firePos, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
					cooldown = 3  + Time.time;
				}
				else if(Input.GetKey(KeyCode.UpArrow) && cooldown <= Time.time)
				{
					Vector3 firePos = transform.position + new Vector3(0,2,0);
					Rigidbody2D fireObj = Instantiate(grenade, firePos, Quaternion.Euler(new Vector3(0,0,90f))) as Rigidbody2D;
					fireObj.velocity = new Vector2(0,60);
					cooldown = 3 + Time.time;
				}
				else if(facingRight && cooldown <= Time.time)
				{ 
					Vector3 firePos = transform.position + new Vector3(2,0,0);
					Rigidbody2D fireObj = Instantiate(grenade, firePos, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
					fireObj.velocity = new Vector2(30,30);
					cooldown = 3  + Time.time;
				}	
				else if (cooldown <= Time.time)
				{
					Vector3 firePos = transform.position + new Vector3(-2,0,0);
					Rigidbody2D fireObj = Instantiate (grenade, firePos, Quaternion.Euler (new Vector3(0,0,0))) as Rigidbody2D;
					fireObj.velocity = new Vector2(-30,30);
					cooldown = 3  + Time.time;
				}
			}

			//ORANGE: FREEZE
			if(color==2 && cooldown <= Time.time)
			{
				if(Input.GetKey(KeyCode.DownArrow)&&!grounded)
				{
					Vector3 firePos = transform.position + new Vector3(0,-2,0);
					Rigidbody2D fireObj = Instantiate(freeze, firePos, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
					fireObj.velocity = new Vector2(0,-15);
					cooldown = 1  + Time.time;	
				}
				else if(Input.GetKey(KeyCode.UpArrow))
				{
					Vector3 firePos = transform.position + new Vector3(0,2,0);
					Rigidbody2D fireObj = Instantiate(freeze, firePos, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
					fireObj.velocity = new Vector2(0,15);
					cooldown = 1  + Time.time;
				}
				else if(facingRight)
				{
					Vector3 firePos = transform.position + new Vector3(2,0,0);
					Rigidbody2D fireObj = Instantiate(freeze, firePos, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
					fireObj.velocity = new Vector2(15,0);
					cooldown = 1  + Time.time;
				}	
				else
				{
					Vector3 firePos = transform.position + new Vector3(-2,0,0);
					Rigidbody2D fireObj = Instantiate (freeze, firePos, Quaternion.Euler (new Vector3(0,0,0))) as Rigidbody2D;
					fireObj.velocity = new Vector2(-15,0);
					cooldown = 1  + Time.time;
				}
			}

			//YELLOW: The walls
			if(color == 3 && cooldown <= Time.time)
			{
				if(Input.GetKey(KeyCode.DownArrow))
				{
					Vector3 firePos;
					firePos = transform.position + new Vector3(0,-2,0);
					Rigidbody2D fireObj = Instantiate(paintWall, firePos, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
					cooldown = Time.time + 3;
				}
				else if(Input.GetKey(KeyCode.UpArrow))
				{
					Vector3 firePos = transform.position + new Vector3(0,5,0);
					Rigidbody2D fireObj = Instantiate(paintWall, firePos, Quaternion.Euler(new Vector3(0,0,90f))) as Rigidbody2D;
					fireObj.isKinematic = true;
					cooldown = Time.time + 3;
				}
				else if(facingRight)
				{
					Vector3 firePos = transform.position + new Vector3(4,0,0);
					Rigidbody2D fireObj = Instantiate(paintWall, firePos, Quaternion.Euler(new Vector3(0,0,90f))) as Rigidbody2D;
					fireObj.isKinematic = true;					
					cooldown = Time.time + 3;
				}	
				else
				{
					Vector3 firePos = transform.position + new Vector3(-4,0,0);
					Rigidbody2D fireObj = Instantiate (paintWall, firePos, Quaternion.Euler (new Vector3(0,0,90f))) as Rigidbody2D;
					fireObj.isKinematic = true;
					cooldown = Time.time + 3;				
				}
			}
	
			//GREEN: the warp-dash
			if(color == 4 && cooldown <= Time.time)
			{
				if(Input.GetKey(KeyCode.UpArrow))
				{
					//raycast logic
					Vector2 fwd = transform.TransformDirection(new Vector2(0,2));
					RaycastHit2D cast = Physics2D.Raycast(transform.position + new Vector3(0,2,0), fwd, 15);
					if (cast != null && cast.collider != null) 
					{
						if(cast.collider.tag == "Enemy")
						{
							Destroy(cast.collider.gameObject);
							Debug.Log("Green: target hit via raycast");
						}
						else Debug.Log ("Green: non-enemy collision");
					}

					Vector3 firePos = transform.position + new Vector3(0,2,0);
					Rigidbody2D fireObj = Instantiate(dash, firePos, Quaternion.Euler(new Vector3(0,0,90f))) as Rigidbody2D;
					fireObj.isKinematic = true;
					transform.position += new Vector3(0,15,0);
					cooldown = Time.time + 3;
				}
				else if(facingRight)
				{
					//raycast logic
					Vector2 fwd = transform.TransformDirection(new Vector2(0,2));
					RaycastHit2D cast = Physics2D.Raycast(transform.position + new Vector3(2,0,0), fwd, 15);
					if (cast != null && cast.collider != null) 
					{
						if(cast.collider.tag == "Enemy")
						{
							Destroy(cast.collider.gameObject);
							Debug.Log("Green: target hit via raycast");
						}
						else Debug.Log ("Green: non-enemy collision");
					}

					Vector3 firePos = transform.position + new Vector3(2,0,0);
					Rigidbody2D fireObj = Instantiate(dash, firePos, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
					fireObj.isKinematic = true;
					transform.position += new Vector3(15,0,0);
					cooldown = Time.time + 3;
				}
				else if(Input.GetKey(KeyCode.DownArrow) && !grounded)
				{
					//raycast logic
					Vector2 fwd = transform.TransformDirection(new Vector2(0,2));
					RaycastHit2D cast = Physics2D.Raycast(transform.position + new Vector3(0,-2,0), fwd, 15);
					if (cast != null && cast.collider != null) 
					{
						if(cast.collider.tag == "Enemy")
						{
							Destroy(cast.collider.gameObject);
							Debug.Log("Green: target hit via raycast");
						}
						else Debug.Log ("Green: non-enemy collision");
					}

					Debug.Log ("Down-Dash");
					Vector3 firePos = transform.position + new Vector3(0,-2,0);
					Rigidbody2D fireObj = Instantiate(dash, firePos, Quaternion.Euler(new Vector3(0,0,90f))) as Rigidbody2D;
					fireObj.isKinematic = true;
					transform.position += new Vector3(0,-15,0);
					cooldown = Time.time + 3;
				}
				else if(Input.GetKey(KeyCode.LeftArrow))
				{
					//raycast logic
					Vector2 fwd = transform.TransformDirection(new Vector2(0,2));
					RaycastHit2D cast = Physics2D.Raycast(transform.position + new Vector3(-2,0,0), fwd, 15);
					if (cast != null && cast.collider != null) 
					{
						if(cast.collider.tag == "Enemy")
						{
							Destroy(cast.collider.gameObject);
							Debug.Log("Green: target hit via raycast");
						}
						else Debug.Log ("Green: non-enemy collision");
					}

					Vector3 firePos = transform.position + new Vector3(-2,0,0);
					Rigidbody2D fireObj = Instantiate(dash, firePos, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
					fireObj.isKinematic = true;
					Destroy (fireObj.gameObject,.5f);
					transform.position += new Vector3(-15,0,0);
					cooldown = Time.time + 3;
				}
			}

			//BLUE: the bubble
			if(color==5)
			{
				//activate the bubble shield
				if(!bubbleShield && cooldown <= Time.time && bubbleTime < Time.time)
				{
					Vector3 pos = (Vector3)transform.position;
					bubbleShield = Instantiate (bubbleSH, pos, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
					bubbleTime = Time.time + 5;
					cooldown = Time.time + 10;
				}
			}
			
			//PURPLE
			if(color==6)
			{
				if(grounded && Input.GetKey(KeyCode.DownArrow)) rigidbody2D.AddForce (new Vector2 (0, 2000f));

				if(Input.GetKey(KeyCode.UpArrow))
				{
					Vector3 firePos = transform.position + new Vector3(0,2,0);
					Rigidbody2D fireObj = Instantiate(glove, firePos, Quaternion.Euler(new Vector3(0,0,90f))) as Rigidbody2D;
					fireObj.velocity = new Vector2(0,15);
					cooldown = Time.time + 1;
				}
				else if(facingRight && Input.GetKey(KeyCode.RightArrow))
				{
					Vector3 firePos = transform.position + new Vector3(2,0,0);
					Rigidbody2D fireObj = Instantiate(glove, firePos, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
					fireObj.velocity = new Vector2(15,0);
					cooldown = Time.time + 1;
				}	
				else if (Input.GetKey(KeyCode.LeftArrow))
				{
					Vector3 firePos = transform.position + new Vector3(-2,0,0);
					Rigidbody2D fireObj = Instantiate (glove, firePos, Quaternion.Euler (new Vector3(0,0,180f))) as Rigidbody2D;
					fireObj.velocity = new Vector2(-15,0);
					cooldown = Time.time + 1;
				}
			}
		}
	}

	void Flip()
	{
		facingRight = !facingRight;
		Vector3 playScale = transform.localScale;
		playScale.x *= -1;
		transform.localScale = playScale;

		//Vector3 backScale = GameObject.FindWithTag ("Backdrop").GetComponent<Transform> ().transform.localScale;
		//backScale.x *= -1;
		//GameObject.FindWithTag ("Backdrop").GetComponent<Transform> ().transform.localScale = backScale;
	}
}
