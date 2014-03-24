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

	GameObject audio;
	Animator anim;
	int color = 0;
	float cooldown = 0;
	float bubbleTime = 0;
	
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
	void Start () { 
		anim = GetComponent<Animator>(); 
		audio = GameObject.FindWithTag ("Audio");
	}
		
	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Enemy" && col.gameObject.rigidbody2D.isKinematic == false && alive)
		{
			alive = false;
			GameObject.FindWithTag("MainCamera").GetComponent<CameraFollow>().enabled = false;
			this.rigidbody2D.isKinematic = true;
			this.GetComponent<PlayerScript>().enabled = false;
			//animate death
			AudioSource.PlayClipAtPoint(audio.GetComponent<AudioScript>().death,transform.position);
			StartCoroutine("PlayerRestart");
		}
		if (col.gameObject.tag == "Floor") 
		{
			//make landing sound		
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

		//change audio track based on the color
		if (c != color) 
		{
			audio.GetComponent<AudioScript>().ChangeTrack(c);
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
	}
	
	IEnumerator PlayerRestart()
	{
		//animate death
		AutoFade.LoadLevel ("ColorRoom", 7, 1, Color.black);
		yield return new WaitForSeconds(7);
		Destroy (gameObject);
	}
	
	IEnumerator PlayerSceneChange()
	{
		AutoFade.LoadLevel ("Level1", 3, 1, Color.black);
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
		if(rBlocked && move > 0) move = 0;

		float moveY = Input.GetAxis("Vertical");
		float s = move;
		if (move < 0)
			s *= -1;
		anim.SetFloat("Speed", s);

		if (grounded && s>0)
			audio.GetComponent<AudioScript> ().PlayWalk ();
		else
			audio.GetComponent<AudioScript> ().StopWalk ();

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

		//flips player if they change direction
		if (move > 0 && !facingRight) Flip ();
		else if (move < 0 && facingRight)	Flip ();
	}

	void inputProc_devCheats()
	{
		//color change cheat
		if (Input.GetKeyDown (KeyCode.LeftControl)) 
		{
			color= (color % 6) + 1;
			anim.SetInteger("Color",color);
		}

		//double jump Space key control
		if (!isDoubleJumping && Input.GetKeyDown (KeyCode.Space)) 
		{
			rigidbody2D.AddForce (new Vector2 (0, 3500f));
			isDoubleJumping = true;
			//Debug.Log (isDoubleJumping);
		}
	}

	void inputProc_jump()
	{
		//jumping from the ground, and reset the double jump flag
		if (grounded && Input.GetKeyDown(KeyCode.Space)) 
		{
			AudioSource.PlayClipAtPoint(audio.GetComponent<AudioScript>().jump,transform.position);
			rigidbody2D.velocity= new Vector2(rigidbody2D.velocity.x,0f);
			rigidbody2D.AddForce (new Vector2 (0, 3500f));
			isDoubleJumping = false;
		} 

		if (grounded) flying = false;
		else if (!grounded && (Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.DownArrow)))
			flying = true;
	}

	void ability_bubbleShield()
	{
		//when we have a bubble active, this allows hover
		if (color == 5 && bubbleShield && !grounded && flying && bubbleTime >= Time.time) 
		{
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
	}

	void ability_Red()
	{
		if(Input.GetKey(KeyCode.DownArrow))
		{
			Vector3 firePos;
			if(facingRight) 	firePos = transform.position + new Vector3(2,0,0);
			else				firePos = transform.position + new Vector3(-2,0,0);
			
			Rigidbody2D fireObj = Instantiate(grenade, firePos, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
		}
		else if(Input.GetKey(KeyCode.UpArrow))
		{
			Vector3 firePos = transform.position + new Vector3(0,2,0);
			Rigidbody2D fireObj = Instantiate(grenade, firePos, Quaternion.Euler(new Vector3(0,0,90f))) as Rigidbody2D;
			fireObj.velocity = new Vector2(0,60);
		}
		else if(facingRight)
		{ 
			Vector3 firePos = transform.position + new Vector3(2,0,0);
			Rigidbody2D fireObj = Instantiate(grenade, firePos, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
			fireObj.velocity = new Vector2(30,30);
		}	
		else
		{
			Vector3 firePos = transform.position + new Vector3(-2,0,0);
			Rigidbody2D fireObj = Instantiate (grenade, firePos, Quaternion.Euler (new Vector3(0,0,0))) as Rigidbody2D;
			fireObj.velocity = new Vector2(-30,30);

		}
		cooldown = 3  + Time.time;
	}

	void ability_Orange()
	{
		AudioSource.PlayClipAtPoint (audio.GetComponent<AudioScript> ().gun, transform.position);
		if (Input.GetKey (KeyCode.DownArrow) && !grounded) 
		{
				Vector3 firePos = transform.position + new Vector3 (0, -2, 0);
				Rigidbody2D fireObj = Instantiate (freeze, firePos, Quaternion.Euler (new Vector3 (0, 0, 0))) as Rigidbody2D;
				fireObj.velocity = new Vector2 (0, -15);
					
		}
		else if (Input.GetKey (KeyCode.UpArrow))
		{
				Vector3 firePos = transform.position + new Vector3 (0, 2, 0);
				Rigidbody2D fireObj = Instantiate (freeze, firePos, Quaternion.Euler (new Vector3 (0, 0, 0))) as Rigidbody2D;
				fireObj.velocity = new Vector2 (0, 15);
		}
		else if (facingRight)
		{
				Vector3 firePos = transform.position + new Vector3 (2, 0, 0);
				Rigidbody2D fireObj = Instantiate (freeze, firePos, Quaternion.Euler (new Vector3 (0, 0, 0))) as Rigidbody2D;
				fireObj.velocity = new Vector2 (15, 0);
		}
		else 
		{
				Vector3 firePos = transform.position + new Vector3 (-2, 0, 0);
				Rigidbody2D fireObj = Instantiate (freeze, firePos, Quaternion.Euler (new Vector3 (0, 0, 0))) as Rigidbody2D;
				fireObj.velocity = new Vector2 (-15, 0);
		}
		cooldown = 1 + Time.time;
	}

	void ability_Yellow()
	{
		AudioSource.PlayClipAtPoint (audio.GetComponent<AudioScript> ().paint, transform.position);
		if(Input.GetKey(KeyCode.DownArrow))
		{
			Vector3 firePos;
			firePos = transform.position + new Vector3(0,-2,0);
			Rigidbody2D fireObj = Instantiate(paintWall, firePos, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;

		}
		else if(Input.GetKey(KeyCode.UpArrow))
		{
			Vector3 firePos = transform.position + new Vector3(0,5,0);
			Rigidbody2D fireObj = Instantiate(paintWall, firePos, Quaternion.Euler(new Vector3(0,0,90f))) as Rigidbody2D;
			fireObj.isKinematic = true;
		}
		else if(facingRight)
		{
			Vector3 firePos = transform.position + new Vector3(4,0,0);
			Rigidbody2D fireObj = Instantiate(paintWall, firePos, Quaternion.Euler(new Vector3(0,0,90f))) as Rigidbody2D;
			fireObj.isKinematic = true;					
		}	
		else
		{
			Vector3 firePos = transform.position + new Vector3(-4,0,0);
			Rigidbody2D fireObj = Instantiate (paintWall, firePos, Quaternion.Euler (new Vector3(0,0,90f))) as Rigidbody2D;
			fireObj.isKinematic = true;
		}
		cooldown = Time.time + 3;
	}

	void ability_Green()
	{
		if (Input.GetKey (KeyCode.UpArrow)) 
		{
			//raycast logic
			Vector2 fwd = transform.TransformDirection (new Vector2 (0, 2));
			RaycastHit2D cast = Physics2D.Raycast (transform.position + new Vector3 (0, 2, 0), fwd, 15);
			if (cast != null && cast.collider != null) 
				if (cast.collider.tag == "Enemy") Destroy (cast.collider.gameObject);

			Vector3 firePos = transform.position + new Vector3 (0, 2, 0);
			Rigidbody2D fireObj = Instantiate (dash, firePos, Quaternion.Euler (new Vector3 (0, 0, 90f))) as Rigidbody2D;
			fireObj.isKinematic = true;
			transform.position += new Vector3 (0, 15, 0);
		} 
		else if (facingRight) 
		{
			//raycast logic
			Vector2 fwd = transform.TransformDirection (new Vector2 (0, 2));
			RaycastHit2D cast = Physics2D.Raycast (transform.position + new Vector3 (2, 0, 0), fwd, 15);
			if (cast != null && cast.collider != null) 
				if (cast.collider.tag == "Enemy") Destroy (cast.collider.gameObject);
						
			Vector3 firePos = transform.position + new Vector3 (2, 0, 0);
			Rigidbody2D fireObj = Instantiate (dash, firePos, Quaternion.Euler (new Vector3 (0, 0, 0))) as Rigidbody2D;
			fireObj.isKinematic = true;
			transform.position += new Vector3 (15, 0, 0);
		}
		else if (Input.GetKey (KeyCode.DownArrow) && !grounded) 
		{
			//raycast logic
			Vector2 fwd = transform.TransformDirection (new Vector2 (0, 2));
			RaycastHit2D cast = Physics2D.Raycast (transform.position + new Vector3 (0, -2, 0), fwd, 15);
			if (cast != null && cast.collider != null)
				if (cast.collider.tag == "Enemy") Destroy (cast.collider.gameObject);
											
			Vector3 firePos = transform.position + new Vector3 (0, -2, 0);
			Rigidbody2D fireObj = Instantiate (dash, firePos, Quaternion.Euler (new Vector3 (0, 0, 90f))) as Rigidbody2D;
			fireObj.isKinematic = true;
			transform.position += new Vector3 (0, -15, 0);
		} 
		else
		{
			//raycast logic
			Vector2 fwd = transform.TransformDirection (new Vector2 (0, 2));
			RaycastHit2D cast = Physics2D.Raycast (transform.position + new Vector3 (-2, 0, 0), fwd, 15);
			if (cast != null && cast.collider != null)
				if (cast.collider.tag == "Enemy") Destroy (cast.collider.gameObject);

			Vector3 firePos = transform.position + new Vector3 (-2, 0, 0);
			Rigidbody2D fireObj = Instantiate (dash, firePos, Quaternion.Euler (new Vector3 (0, 0, 0))) as Rigidbody2D;
			fireObj.isKinematic = true;
			Destroy (fireObj.gameObject, .5f);
			transform.position += new Vector3 (-15, 0, 0);

		}
		cooldown = Time.time + 3;
	}

	void ability_Blue()
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

	void ability_Purple()
	{
		if(grounded && Input.GetKey(KeyCode.DownArrow)) rigidbody2D.AddForce (new Vector2 (0, 2000f));
		
		if(Input.GetKey(KeyCode.UpArrow))
		{
			Vector3 firePos = transform.position + new Vector3(0,2,0);
			Rigidbody2D fireObj = Instantiate(glove, firePos, Quaternion.Euler(new Vector3(0,0,90f))) as Rigidbody2D;
			fireObj.velocity = new Vector2(0,15);
			cooldown = Time.time + 1;
		}
		else if(facingRight)
		{
			Vector3 firePos = transform.position + new Vector3(2,0,0);
			Rigidbody2D fireObj = Instantiate(glove, firePos, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
			fireObj.velocity = new Vector2(15,0);
			cooldown = Time.time + 1;
		}	
		else
		{
			Vector3 firePos = transform.position + new Vector3(-2,0,0);
			Rigidbody2D fireObj = Instantiate (glove, firePos, Quaternion.Euler (new Vector3(0,0,180f))) as Rigidbody2D;
			fireObj.velocity = new Vector2(-15,0);
			cooldown = Time.time + 1;
		}
	}
		
	void Update()
	{
		//debugging color change & double jump
		inputProc_devCheats();
		inputProc_jump();
		ability_bubbleShield();

		//ability button control
		if (Input.GetKeyDown (KeyCode.LeftShift) && cooldown <= Time.time) 
		{
			if(color == 1) ability_Red();
			else if(color == 2) ability_Orange();
			else if(color == 3) ability_Yellow();
			else if(color == 4) ability_Green();
			else if(color == 5) ability_Blue();
			else if(color == 6) ability_Purple();
		}
	}

	void Flip()
	{
		facingRight = !facingRight;
		Vector3 playScale = transform.localScale;
		playScale.x *= -1;
		transform.localScale = playScale;
	}
}
