using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour 
{
	//physics and attributes
	bool grounded = true;
	float groundRad = 0.5f;
	bool fBlocked = true;
	bool flying = false;
	public float maxSpeed = 20;
	public bool alive = true;
	float move;
	float moveY;
	int color = 0;
	float cooldown = 0;
	float bubbleTime = 0;
	bool onMovingPlatform = false;

	//audio
	GameObject audio;
	int foot;

	//animation
	bool facingRight = true;
	Animator anim;

	//cheat tools
	public bool cheatMode = false;

	//looks to play land sound
	bool landed = true;

	//ridgidbodies
	public Rigidbody2D ball;
	public Rigidbody2D grenade;
	public Rigidbody2D freeze;
	public Rigidbody2D paintWall;
	public Rigidbody2D dash;
	public Rigidbody2D bubble;
	public Rigidbody2D bubbleSH;
	public Rigidbody2D glove;
	public Sprite groundPoundPlatform;
	private Rigidbody2D bubbleShield;

	//transforms
	public Transform abilityPos;
	public Transform groundCheck;
	public Transform frontCheck;
	public LayerMask whatIsGround;
	
	//ground pound variables
	private bool groundPounding;
	private float groundPoundTime = 0;
	private Vector3 groundPoundPos;
	public Collider2D[] groundPoundBoom;
	public Vector3 pushForce = new Vector3(0,100,0);
	
	// Use this for initialization
	void Start () 
	{ 
		anim = GetComponent<Animator>();
		audio = GameObject.FindWithTag ("Audio");
	}
	
	// Update is called once per frame
	void Update()
	{
		inputProc_devCheats();				//debugging color change & double jump

		inputProc_movement();				//arrow key controls
		inputProc_jump();					//jumping controls
		inputProc_ability();				//ability use button
		
		ability_bubbleShield();				//fly if we have a bubble
		ability_groundPound();				//check to see if we're pounding
		
		//flip the player sprite
		if (move > 0 && !facingRight) 		Flip ();
		else if (move < 0 && facingRight)	Flip ();
	}
	
	void inputProc_ability()
	{
		//ability button control
		if (Input.GetKeyDown (KeyCode.LeftShift) && cooldown <= Time.time) 
		{
			anim.SetTrigger("Shooting");
			anim.SetInteger ("Foot",foot);

			if     (color == 1) ability_Red();
			else if(color == 2) ability_Orange();
			else if(color == 3) ability_Yellow();
			else if(color == 4) ability_Green();
			else if(color == 5) ability_Blue();
			else if(color == 6) ability_Purple();
		}
	}
	
	void inputProc_movement()
	{
		move = Input.GetAxis("Horizontal");
		moveY = Input.GetAxis("Vertical");

		if (fBlocked)
		{
			if (facingRight && move > 0)        move = 0;
			else if (!facingRight && move < 0)  move = 0;
		}

		float s = move * maxSpeed;
		if (move < 0) s *= -1;
		anim.SetFloat   ("Speed", s);
		anim.SetInteger ("Direction", Mathf.RoundToInt(moveY));
		anim.SetBool    ("FacingR", facingRight);
		
		if (grounded && s>0) audio.GetComponent<AudioScript> ().PlayWalk ();
		else                 audio.GetComponent<AudioScript> ().StopWalk ();
	}
	
	void inputProc_devCheats()
	{
		//cheats on/off control
		if (Input.GetKeyDown (KeyCode.Backspace))
		{
			AudioSource.PlayClipAtPoint(audio.GetComponent<AudioScript>().jump,transform.position);
			if (cheatMode) cheatMode = false;
			else cheatMode = true;
		}

		//if cheats are off, we're done here.
		if (!cheatMode) return;

		//color change cheat
		if (Input.GetKeyDown (KeyCode.LeftControl)) 
		{
			color= (color % 6) + 1;
			anim.SetInteger("Color",color);
		}
		
		//cheat-jump Space key control
		if (!grounded && Input.GetKeyDown (KeyCode.Space)) rigidbody2D.AddForce (new Vector2 (0, 3500f));
		cooldown = 0;
	}
	
	void inputProc_jump()
	{
		//checks to see if player can jump
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundRad, whatIsGround);
		fBlocked = Physics2D.OverlapCircle(frontCheck.position, groundRad, whatIsGround);
		anim.SetBool ("Grounded", grounded);

		//jumping from the ground, and reset the double jump flag
		if (grounded)
		{
			if (Input.GetKeyDown(KeyCode.Space)) 
			{
				AudioSource.PlayClipAtPoint(audio.GetComponent<AudioScript>().jump,transform.position);
				rigidbody2D.velocity= new Vector2(rigidbody2D.velocity.x,0f);
				if (!bubbleShield) rigidbody2D.AddForce (new Vector2 (0, 4400f));
				landed = false;
			} 
			if (!bubbleShield) flying = false;
			if(!landed)	AudioSource.PlayClipAtPoint(audio.GetComponent<AudioScript>().land,transform.position);
		}
		else if (!grounded && (bubbleShield)) flying = true;
		landed = grounded;
	}
	
	void ability_bubbleShield()
	{
		if (bubbleShield) 
		{
			//make sure the bubble doesn't go running away
			bubbleShield.transform.position = rigidbody2D.transform.position;
		
			if(flying)
			{
				//bubble flight control
				if (cheatMode) rigidbody2D.velocity = new Vector2((move * (float)2.5) * maxSpeed, (moveY * (float)2.5) * maxSpeed);
				else rigidbody2D.velocity = new Vector2((move * (float)0.75) * maxSpeed, (moveY * (float)0.5) * maxSpeed);

				if (!grounded && bubbleTime >= Time.time) gameObject.rigidbody2D.gravityScale = 0;
				else if (bubbleTime < Time.time) 
				{
					Destroy(bubbleShield.gameObject);
					bubbleShield = null;
					flying = false;
					gameObject.rigidbody2D.gravityScale = 1;
				}
				else gameObject.rigidbody2D.gravityScale = 1;
			}
		}
		//movement for the player and the object held
		else rigidbody2D.velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);
	}
	
	void ability_groundPound()
	{
		if (groundPoundTime < Time.time && groundPounding) 
		{
			//stop the enemy movement to make them hover
			for (int i=0; i<groundPoundBoom.GetLength(0); i++) 
			{
				if (groundPoundBoom [i].gameObject.rigidbody2D != null) 
				{
					EnemyScript eScript = groundPoundBoom[i].GetComponent<EnemyScript>();
					if (groundPoundBoom [i].gameObject.rigidbody2D.gameObject.tag == "Enemy") 
					{
						groundPoundBoom[i].gameObject.rigidbody2D.velocity = new Vector3(0,0,0);
						eScript.groundPounded();
					}
				}
			}
			groundPounding = false;
		}
	}
	
	void ability_Red()
	{
		Rigidbody2D fireObj;
		if(Input.GetKey(KeyCode.DownArrow))
		{
			Vector3 firePos;
			if(facingRight) 	firePos = transform.position + new Vector3(2,0,0);
			else				firePos = transform.position + new Vector3(-2,0,0);
			
			fireObj = Instantiate(grenade, firePos, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
		}
		else if(Input.GetKey(KeyCode.UpArrow))
		{
			Vector3 firePos = transform.position + new Vector3(0,2,0);
			fireObj = Instantiate(grenade, firePos, Quaternion.Euler(new Vector3(0,0,90f))) as Rigidbody2D;
			fireObj.velocity = new Vector2(0,80);
		}
		else if(facingRight)
		{ 
			Vector3 firePos = transform.position + new Vector3(2,0,0);
			fireObj = Instantiate(grenade, firePos, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
			fireObj.velocity = new Vector2(40,40);
		}	
		else
		{
			Vector3 firePos = transform.position + new Vector3(-2,0,0);
			fireObj = Instantiate (grenade, firePos, Quaternion.Euler (new Vector3(0,0,0))) as Rigidbody2D;
			fireObj.velocity = new Vector2(-40,40);
			
		}
		cooldown = 3  + Time.time;
		fireObj.GetComponent<AudioSource> ().Play ();
	}
	
	void ability_Orange()
	{
		AudioSource.PlayClipAtPoint (audio.GetComponent<AudioScript> ().gun, transform.position);
		if (Input.GetKey (KeyCode.DownArrow) && !grounded) 
		{
			Vector3 firePos = transform.position + new Vector3 (0, -6, 0);
			Rigidbody2D fireObj = Instantiate (freeze, firePos, Quaternion.Euler (new Vector3 (0, 0, 0))) as Rigidbody2D;
			fireObj.velocity = new Vector2 (0, -60);
			rigidbody2D.AddForce (new Vector2 (0, 500f));
		}
		else if (Input.GetKey (KeyCode.UpArrow))
		{
			Vector3 firePos = transform.position + new Vector3 (0, 6, 0);
			Rigidbody2D fireObj = Instantiate (freeze, firePos, Quaternion.Euler (new Vector3 (0, 0, 0))) as Rigidbody2D;
			fireObj.velocity = new Vector2 (0, 60);
			rigidbody2D.AddForce (new Vector2 (0, -2000f));
		}
		else if (facingRight)
		{
			Vector3 firePos = transform.position + new Vector3 (6, 0, 0);
			Rigidbody2D fireObj = Instantiate (freeze, firePos, Quaternion.Euler (new Vector3 (0, 0, 0))) as Rigidbody2D;
			fireObj.velocity = new Vector2 (60, 0);
			rigidbody2D.AddForce (new Vector2 (-2000f, 0));
		}
		else 
		{
			Vector3 firePos = transform.position + new Vector3 (-6, 0, 0);
			Rigidbody2D fireObj = Instantiate (freeze, firePos, Quaternion.Euler (new Vector3 (0, 0, 0))) as Rigidbody2D;
			fireObj.velocity = new Vector2 (-60, 0);
			rigidbody2D.AddForce (new Vector2 (2000f, 0));
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
			Rigidbody2D fireObj = Instantiate(paintWall, firePos, Quaternion.Euler(new Vector3(0,0,90f))) as Rigidbody2D;
			cooldown = Time.time + 3;
		}
		else if(Input.GetKey(KeyCode.UpArrow))
		{
			Vector3 firePos = transform.position + new Vector3(0,5,0);
			Rigidbody2D fireObj = Instantiate(paintWall, firePos, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
			fireObj.isKinematic = true;
			cooldown = Time.time + 6;
		}
		else if(facingRight)
		{
			Vector3 firePos = transform.position + new Vector3(5,1,0);
			Rigidbody2D fireObj = Instantiate(paintWall, firePos, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
			fireObj.isKinematic = true;
			cooldown = Time.time + 6;
		}	
		else
		{
			Vector3 firePos = transform.position + new Vector3(-5,1,0);
			Rigidbody2D fireObj = Instantiate (paintWall, firePos, Quaternion.Euler (new Vector3(0,0,0))) as Rigidbody2D;
			fireObj.isKinematic = true;
			cooldown = Time.time + 6;
		}

	}
	
	void ability_Green()
	{
		if (onMovingPlatform) return;
		if (Input.GetKey (KeyCode.UpArrow))
		{
			Collider2D[] warpDash = Physics2D.OverlapCircleAll (transform.position, 30f);
			
			for (int i=0; i<warpDash.GetLength(0); i++) 
			{
				if (warpDash[i].gameObject.rigidbody2D != null) 
				{
					if (warpDash[i].gameObject.rigidbody2D.gameObject.tag == "Enemy") 
					{
						if (Mathf.Abs(transform.position.x - warpDash[i].gameObject.transform.position.x)<= 2)
							if (transform.position.y < warpDash[i].gameObject.transform.position.y) Destroy (warpDash[i].gameObject);
					}
				}
			}
			
			Vector3 firePos = transform.position + new Vector3 (0, 2, 0);
			//Rigidbody2D fireObj = Instantiate (dash, firePos, Quaternion.Euler (new Vector3 (0, 0, 90f))) as Rigidbody2D;
			//fireObj.isKinematic = true;
			transform.position += new Vector3 (0, 25, 0);
		}
		
		else if (Input.GetKey (KeyCode.DownArrow) && !grounded) 
		{
			Collider2D[] warpDash = Physics2D.OverlapCircleAll (transform.position, 30f);
			for (int i=0; i<warpDash.GetLength(0); i++) 
			{
				if (warpDash[i].gameObject.rigidbody2D != null) 
				{
					if (warpDash[i].gameObject.rigidbody2D.gameObject.tag == "Enemy") 
					{
						if (Mathf.Abs(transform.position.x - warpDash[i].gameObject.transform.position.x)<= 2)
							if (transform.position.y > warpDash[i].gameObject.transform.position.y) Destroy (warpDash[i].gameObject);
					}
				}
			}
			
			Vector3 firePos = transform.position + new Vector3 (0, -2, 0);
			transform.position += new Vector3 (0, -25, 0);
		} 
		else if (!facingRight)
		{
			Collider2D[] warpDash = Physics2D.OverlapCircleAll (transform.position, 30f);
			
			for (int i=0; i<warpDash.GetLength(0); i++) 
			{
				if (warpDash[i].gameObject.rigidbody2D != null) 
				{
					if (warpDash[i].gameObject.rigidbody2D.gameObject.tag == "Enemy") 
					{
						if (Mathf.Abs(transform.position.y - warpDash[i].gameObject.transform.position.y)<= 2)
							if (transform.position.x > warpDash[i].gameObject.transform.position.x) Destroy (warpDash[i].gameObject);
					}
				}
			}
			
			Vector3 firePos = transform.position + new Vector3 (-2, 0, 0);
			transform.position += new Vector3 (-25, 0, 0);
		}
		else if (facingRight) 
		{
			Collider2D[] warpDash = Physics2D.OverlapCircleAll (transform.position, 30f);
			
			for (int i=0; i<warpDash.GetLength(0); i++) 
			{
				if (warpDash[i].gameObject.rigidbody2D != null) 
				{
					if (warpDash[i].gameObject.rigidbody2D.gameObject.tag == "Enemy") 
					{
						if (Mathf.Abs(transform.position.y - warpDash[i].gameObject.transform.position.y)<= 2)
							if (transform.position.x < warpDash[i].gameObject.transform.position.x) Destroy (warpDash[i].gameObject);
					}
				}
			}
			
			Vector3 firePos = transform.position + new Vector3 (2, 0, 0);
			//Rigidbody2D fireObj = Instantiate (dash, firePos, Quaternion.Euler (new Vector3 (0, 0, 0))) as Rigidbody2D;
			//fireObj.isKinematic = true;
			transform.position += new Vector3 (25, 0, 0);
		}
		cooldown = Time.time + 3;
	}
	
	void ability_Blue()
	{
		//activate the bubble shield
		if(!bubbleShield && cooldown <= Time.time && bubbleTime < Time.time)
		{
			rigidbody2D.AddForce (new Vector2 (0, 2200f));
			Vector3 pos = (Vector3)transform.position;
			bubbleShield = Instantiate (bubbleSH, pos, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;

			if (cheatMode) bubbleTime = Time.time + 60;
			else           bubbleTime = Time.time + 4;
			cooldown = Time.time + 9;
		}
	}
	
	void ability_Purple()
	{
		if(grounded && Input.GetKey(KeyCode.DownArrow) && !groundPounding)
		{
			groundPoundPos = transform.position;
			groundPoundBoom = Physics2D.OverlapCircleAll (groundPoundPos, 20f);
			
			int l = groundPoundBoom.GetLength (0);
			for (int i=0; i<l; i++) 
			{
				if (groundPoundBoom [i].gameObject.rigidbody2D != null) 
				{
					if (groundPoundBoom [i].gameObject.rigidbody2D.gameObject.tag == "Enemy") 
					{
						EnemyScript eScript = groundPoundBoom[i].GetComponent<EnemyScript>();
						if (eScript.walker)
						{
							eScript.frozen = true;
							groundPoundBoom [i].rigidbody2D.gravityScale = -4;
							groundPoundBoom [i].gameObject.rigidbody2D.drag = 2;
						}
						else if (eScript.follow) eScript.frozen = true;
					}
				}
			}
			groundPoundTime = Time.time + 1;
			cooldown = Time.time + 7;
			groundPounding = true;
			
		}
		
		else if(Input.GetKey(KeyCode.UpArrow))
		{
			Vector3 firePos = transform.position + new Vector3(0,7,0);
			Rigidbody2D     fireObj;
			if(facingRight)	fireObj = Instantiate(glove, firePos, Quaternion.Euler(new Vector3(0,0,90f))) as Rigidbody2D;
			else            fireObj = Instantiate(glove, firePos, Quaternion.Euler(new Vector3(180f,0,270f))) as Rigidbody2D;
			fireObj.velocity = new Vector2(0,30);
			cooldown = Time.time + (float)0.3;
		}
		else if(facingRight)
		{
			Vector3 firePos = transform.position + new Vector3(6,0,0);
			Rigidbody2D fireObj = Instantiate(glove, firePos, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
			fireObj.velocity = new Vector2(30,0);
			cooldown = Time.time + (float)0.3;
		}	
		else if (!facingRight)
		{
			Vector3 firePos = transform.position + new Vector3(-6,0,0);
			Rigidbody2D fireObj = Instantiate (glove, firePos, Quaternion.Euler (new Vector3(180f,0,180f))) as Rigidbody2D;
			fireObj.velocity = new Vector2(-30,0);
			cooldown = Time.time + (float)0.3;
		}
	}
	
	void Flip()
	{
		facingRight = !facingRight;
		Vector3 playScale = transform.localScale;
		playScale.x *= -1;
		transform.localScale = playScale;
	}

	void death()
	{
		Destroy (GameObject.FindWithTag ("Pause"));
		AudioSource.PlayClipAtPoint(audio.GetComponent<AudioScript>().death,transform.position);
		alive = false;
		GameObject.FindWithTag("MainCamera").GetComponent<CameraFollow>().enabled = false;
		this.rigidbody2D.isKinematic = true;
		this.GetComponent<PlayerScript>().enabled = false;
		StartCoroutine("PlayerRestart");
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (!cheatMode)
		{
			if      (flying &(col.gameObject.tag == "Projectile" || col.gameObject.tag == "Enemy")) { Destroy(col.gameObject); bubbleTime = 0; }
			else if (col.gameObject.tag == "Projectile")                                            { Destroy(col.gameObject); death(); }
			else if (col.gameObject.tag == "Enemy" && col.gameObject.rigidbody2D.isKinematic == false && alive) death();
		}
		else if (flying && col.gameObject.tag == "Enemy") Destroy(col.gameObject);
	}

	void OnTriggerExit2D(Collider2D col)
	{
		onMovingPlatform = false;
		if (col.gameObject.tag == "rightleft") col.transform.DetachChildren();
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "rightleft" || col.gameObject.tag == "updown") 
		{
			onMovingPlatform = true;
			this.transform.parent = col.transform;
		}
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

		if (col.gameObject.tag == "Level1")
		{
			DontDestroyOnLoad (transform.gameObject);
			DontDestroyOnLoad (transform.FindChild("Audio"));
			StartCoroutine("Scene1Change");
		}
		if (col.gameObject.tag == "Level2")
		{
			DontDestroyOnLoad (transform.gameObject);
			DontDestroyOnLoad (transform.FindChild("Audio"));
			StartCoroutine("Scene2Change");
		}
		if (col.gameObject.tag == "Level3")
		{
			DontDestroyOnLoad (transform.gameObject);
			DontDestroyOnLoad (transform.FindChild("Audio"));
			StartCoroutine("Scene3Change");
		}
		if (col.gameObject.tag == "Level4")
		{
			DontDestroyOnLoad (transform.gameObject);
			DontDestroyOnLoad (transform.FindChild("Audio"));
			StartCoroutine("Scene4Change");
		}
		if (col.gameObject.tag == "Level5")
		{
			DontDestroyOnLoad (transform.gameObject);
			DontDestroyOnLoad (transform.FindChild("Audio"));
			StartCoroutine("Scene5Change");
		}
	}
	
	IEnumerator PlayerRestart()
	{
		//animate death
		bubbleTime = 0;
		AutoFade.LoadLevel ("ColorRoom", 5, 1, Color.black);
		PlayerPrefs.SetInt ("Level", 1);
		yield return new WaitForSeconds(1);
		Destroy (gameObject);
	}
	
	IEnumerator Scene1Change()
	{
		bubbleTime = 0;
		AutoFade.LoadLevel ("Level1", 3, 1, Color.black);
		yield return new WaitForSeconds(3);
		transform.position = new Vector3(0,0,0);
	}
	IEnumerator Scene2Change()
	{
		bubbleTime = 0;
		AutoFade.LoadLevel ("Level2", 3, 1, Color.black);
		yield return new WaitForSeconds(3);
		transform.position = new Vector3(0,0,0);
	}
	IEnumerator Scene3Change()
	{
		bubbleTime = 0;
		AutoFade.LoadLevel ("Level3", 3, 1, Color.black);
		yield return new WaitForSeconds(3);
		transform.position = new Vector3(0,0,0);
	}
	IEnumerator Scene4Change()
	{
		bubbleTime = 0;
		AutoFade.LoadLevel ("Level4", 3, 1, Color.black);
		yield return new WaitForSeconds(3);
		transform.position = new Vector3(0,0,0);
	}
	IEnumerator Scene5Change()
	{
		bubbleTime = 0;
		AutoFade.LoadLevel ("Level5", 3, 1, Color.black);
		yield return new WaitForSeconds(3);
		transform.position = new Vector3(0,0,0);
	}

	void CenterFoot(){ foot = 0; }
	void RightFoot (){ foot = 1; }
	void LeftFoot  (){ foot = 2; }
}
