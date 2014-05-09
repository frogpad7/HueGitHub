using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour 
{
	//physics and attributes
	public bool grounded = true;
	float groundRad = 0.5f;
	bool fBlocked = true;
	bool flying = false;
	public float maxSpeed = 30;
	public bool alive = true;
	float move;
	float moveY;
	public int color = 0;
	float cooldown = 0;
	float bubbleTime = 0;
	bool onMovingPlatform = false;

	//audio
	public AudioScript audio;

	//animation
	bool facingRight = true;
	public Animator anim;
	int foot;
	int cycle;

	//cheat tools
	public bool cheatMode = false;

	//looks to play land sound
	bool landed = true;
	bool loading = false;
	bool atDoor = false;

	int stage;
	float sound = 1;
	float music = 1;

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
		//anim = GetComponent<Animator>();
		//audio = GameObject.FindWithTag ("Audio");
		DontDestroyOnLoad (transform.gameObject);
		DontDestroyOnLoad (transform.FindChild("Audio"));

		int level = PlayerPrefs.GetInt ("Level");
		stage = 1 + ((level - 1) * 2);
		//Debug.Log (stage);
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

		if (atDoor == true && Input.GetKey (KeyCode.UpArrow) && !loading) {
			StartCoroutine ("SceneChange");
			loading = true;		
		}
	}
	
	void inputProc_ability()
	{
		if(cooldown <= Time.time)
			anim.SetLayerWeight(1,0f);
		//ability button control
		if (Input.GetKeyDown (KeyCode.LeftShift) && cooldown <= Time.time) 
		{
			anim.SetTrigger("Shooting");
			anim.SetInteger("Foot",foot);
			anim.SetInteger("Cycle",cycle);

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
		move =  Input.GetAxis("Horizontal");
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
		
		if (grounded && s>0 && alive) audio.PlayWalk ();
		else                 audio.StopWalk ();
	}
	
	void inputProc_devCheats()
	{

		//cheats on/off control
		if (Input.GetKeyDown (KeyCode.Backspace))
		{
			audio.sounds.PlayOneShot(audio.jump, sound);
			//AudioSource.PlayClipAtPoint(audio.GetComponent<AudioScript>().jump,transform.position);
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
		fBlocked = Physics2D.OverlapCircle(frontCheck.position,  groundRad, whatIsGround);
		anim.SetBool ("Grounded", grounded);

		//jumping from the ground, and reset the double jump flag
		if (grounded)
		{
			if(!landed)	audio.sounds.PlayOneShot(audio.land, sound);
			if (Input.GetKeyDown(KeyCode.Space)) 
			{
				audio.sounds.PlayOneShot(audio.jump, sound);
				//AudioSource.PlayClipAtPoint(audio.GetComponent<AudioScript>().jump,transform.position);
				rigidbody2D.velocity= new Vector2(rigidbody2D.velocity.x,0f);
				if (!bubbleShield) rigidbody2D.AddForce (new Vector2 (0, 4550f));
				landed = false;
			} 
			if (!bubbleShield) flying = false;
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
				else rigidbody2D.velocity = new Vector2((move * (float)0.8) * maxSpeed, (moveY * (float)0.55) * maxSpeed);

				if (!grounded && bubbleTime >= Time.time) gameObject.rigidbody2D.gravityScale = 0;
				else if (bubbleTime < Time.time) 
				{
					//play pop
					float rand = Random.value * 360;
					GameObject.FindWithTag ("Backdrop").GetComponent<SplatterScript> ().Splat (5, transform.position, Quaternion.Euler(new Vector3(0,0,rand)));
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
				try
				{
					if (groundPoundBoom [i].gameObject != null) 
					{
						EnemyScript eScript = groundPoundBoom[i].GetComponent<EnemyScript>();
						if (groundPoundBoom [i].gameObject.rigidbody2D.gameObject.tag == "Enemy") 
						{
							groundPoundBoom[i].gameObject.rigidbody2D.velocity = new Vector3(0,0,0);
							eScript.groundPounded();
						}
					}
				}
				catch(System.Exception e)
				{
					Debug.Log("ERR: Ground Pound Problem:");
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
			Vector3 firePos = transform.position + new Vector3(0,4,0);
			fireObj = Instantiate(grenade, firePos, Quaternion.Euler(new Vector3(0,0,90f))) as Rigidbody2D;
			fireObj.velocity = new Vector2(0,160);
		}
		else if(facingRight)
		{ 
			Vector3 firePos = transform.position + new Vector3(2,0,0);
			fireObj = Instantiate(grenade, firePos, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
			fireObj.velocity = new Vector2(45,60);
		}	
		else
		{
			Vector3 firePos = transform.position + new Vector3(-2,0,0);
			fireObj = Instantiate (grenade, firePos, Quaternion.Euler (new Vector3(0,0,0))) as Rigidbody2D;
			fireObj.velocity = new Vector2(-45,60);
			
		}
		cooldown = 6  + Time.time;
		audio.sounds.PlayOneShot (audio.tick, sound);
		//AudioSource.PlayClipAtPoint (audio.GetComponent<AudioScript> ().tick, transform.position);
	}
	
	void ability_Orange()
	{
		audio.sounds.PlayOneShot (audio.gun, sound);
		//AudioSource.PlayClipAtPoint (audio.GetComponent<AudioScript> ().gun, transform.position);
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
			rigidbody2D.AddForce (new Vector2 (0, -4000f));
		}
		else if (facingRight)
		{
			Vector3 firePos = transform.position + new Vector3 (6, 0, 0);
			Rigidbody2D fireObj = Instantiate (freeze, firePos, Quaternion.Euler (new Vector3 (0, 0, 0))) as Rigidbody2D;
			fireObj.velocity = new Vector2 (60, 0);
			rigidbody2D.AddForce (new Vector2 (-4000f, 0));
		}
		else 
		{
			Vector3 firePos = transform.position + new Vector3 (-6, 0, 0);
			Rigidbody2D fireObj = Instantiate (freeze, firePos, Quaternion.Euler (new Vector3 (0, 0, 0))) as Rigidbody2D;
			fireObj.velocity = new Vector2 (-60, 0);
			rigidbody2D.AddForce (new Vector2 (4000f, 0));
		}
		cooldown = 1.5f + Time.time;
	}
	
	void ability_Yellow()
	{		
		audio.sounds.PlayOneShot (audio.paint, sound);
		//AudioSource.PlayClipAtPoint (audio.GetComponent<AudioScript> ().paint, transform.position);
		if(Input.GetKey(KeyCode.DownArrow))
		{
			Vector3 firePos;
			firePos = transform.position + new Vector3(0,-3,0);
			Rigidbody2D fireObj = Instantiate(paintWall, firePos, Quaternion.Euler(new Vector3(0,0,90f))) as Rigidbody2D;
			cooldown = Time.time + 3;
		}
		else if(Input.GetKey(KeyCode.UpArrow))
		{
			Vector3 firePos = transform.position + new Vector3(0,10,0);
			Rigidbody2D fireObj = Instantiate(paintWall, firePos, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
			fireObj.isKinematic = true;
			cooldown = Time.time + 6;
		}
		else if(facingRight)
		{
			Vector3 firePos = transform.position + new Vector3(10,1,0);
			Rigidbody2D fireObj = Instantiate(paintWall, firePos, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
			fireObj.isKinematic = true;
			cooldown = Time.time + 6;
		}	
		else
		{
			Vector3 firePos = transform.position + new Vector3(-10,1,0);
			Rigidbody2D fireObj = Instantiate (paintWall, firePos, Quaternion.Euler (new Vector3(0,0,0))) as Rigidbody2D;
			fireObj.isKinematic = true;
			cooldown = Time.time + 6;
		}

	}
	
	void ability_Green()
	{
		//sound for green
		if (onMovingPlatform) this.transform.parent = null;
		if (Input.GetKey (KeyCode.UpArrow))
		{
			Collider2D[] warpDash = Physics2D.OverlapCircleAll (transform.position, 35f);
			
			for (int i=0; i<warpDash.GetLength(0); i++) 
			{
				if (warpDash[i].gameObject.rigidbody2D != null) 
				{
					if (warpDash[i].gameObject.rigidbody2D.gameObject.tag == "Enemy") 
					{
						if (Mathf.Abs(transform.position.x - warpDash[i].gameObject.transform.position.x)<= 5)
							if (transform.position.y < warpDash[i].gameObject.transform.position.y) Destroy (warpDash[i].gameObject);
					}
				}
			}
			
			Vector3 firePos = transform.position + new Vector3 (0, 12.5f, 0);
			//Rigidbody2D fireObj = Instantiate (dash, firePos, Quaternion.Euler (new Vector3 (0, 0, 90f))) as Rigidbody2D;
			//fireObj.isKinematic = true;
			GameObject.FindWithTag ("Backdrop").GetComponent<SplatterScript> ().Splat (4, firePos, Quaternion.Euler(new Vector3(0,0,90)));
			transform.position += new Vector3 (0, 25, 0);
		}
		
		else if (Input.GetKey (KeyCode.DownArrow)) 
		{
			Collider2D[] warpDash = Physics2D.OverlapCircleAll (transform.position, 35f);
			for (int i=0; i<warpDash.GetLength(0); i++) 
			{
				if (warpDash[i].gameObject.rigidbody2D != null) 
				{
					if (warpDash[i].gameObject.rigidbody2D.gameObject.tag == "Enemy") 
					{
						if (Mathf.Abs(transform.position.x - warpDash[i].gameObject.transform.position.x)<= 5)
							if (transform.position.y > warpDash[i].gameObject.transform.position.y) Destroy (warpDash[i].gameObject);
					}
				}
			}
			
			Vector3 firePos = transform.position + new Vector3 (0, -12.5f, 0);
			GameObject.FindWithTag ("Backdrop").GetComponent<SplatterScript> ().Splat (4, firePos, Quaternion.Euler(new Vector3(0,0,270)));
			transform.position += new Vector3 (0, -25, 0);
		} 
		else if (!facingRight)
		{
			Collider2D[] warpDash = Physics2D.OverlapCircleAll (transform.position, 35f);
			
			for (int i=0; i<warpDash.GetLength(0); i++) 
			{
				if (warpDash[i].gameObject.rigidbody2D != null) 
				{
					if (warpDash[i].gameObject.rigidbody2D.gameObject.tag == "Enemy") 
					{
						if (Mathf.Abs(transform.position.y - warpDash[i].gameObject.transform.position.y)<= 5)
							if (transform.position.x > warpDash[i].gameObject.transform.position.x) Destroy (warpDash[i].gameObject);
					}
				}
			}
			
			Vector3 firePos = transform.position + new Vector3 (-12.5f, 0, 0);
			GameObject.FindWithTag ("Backdrop").GetComponent<SplatterScript> ().Splat (4, firePos, Quaternion.Euler(new Vector3(0,0,180)));
			transform.position += new Vector3 (-25, 0, 0);
		}
		else if (facingRight) 
		{
			Collider2D[] warpDash = Physics2D.OverlapCircleAll (transform.position, 35f);
			
			for (int i=0; i<warpDash.GetLength(0); i++) 
			{
				if (warpDash[i].gameObject.rigidbody2D != null) 
				{
					if (warpDash[i].gameObject.rigidbody2D.gameObject.tag == "Enemy") 
					{
						if (Mathf.Abs(transform.position.y - warpDash[i].gameObject.transform.position.y)<= 5)
							if (transform.position.x < warpDash[i].gameObject.transform.position.x) Destroy (warpDash[i].gameObject);
					}
				}
			}
			
			Vector3 firePos = transform.position + new Vector3 (12.5f, 0, 0);
			//Rigidbody2D fireObj = Instantiate (dash, firePos, Quaternion.Euler (new Vector3 (0, 0, 0))) as Rigidbody2D;
			//fireObj.isKinematic = true;
			GameObject.FindWithTag ("Backdrop").GetComponent<SplatterScript> ().Splat (4, firePos, new Quaternion());
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
			cooldown = Time.time + 8;
		}
	}
	
	void ability_Purple()
	{
		//sound for purple
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
			cooldown = Time.time + 4;
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
		audio.sounds.PlayOneShot (audio.death, sound);
		//AudioSource.PlayClipAtPoint(audio.GetComponent<AudioScript>().death,transform.position);
		alive = false;
		anim.SetBool ("Alive", false);
		GameObject.FindWithTag("MainCamera").GetComponent<CameraFollow>().enabled = false;
		this.rigidbody2D.isKinematic = true;
		this.GetComponent<PlayerScript>().enabled = false;
		StartCoroutine("PlayerRestart");
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (!cheatMode)
		{
			if      (flying  && (col.gameObject.tag == "Projectile" || col.gameObject.tag == "Enemy")) { Destroy(col.gameObject); bubbleTime = 0; }
			else if (col.gameObject.tag == "Projectile") { Destroy(col.gameObject); death(); }
			else if (col.gameObject.tag == "Enemy" && col.gameObject.rigidbody2D.isKinematic == false && alive) death();
			else if (col.gameObject.tag == "Spikes" && alive) death();
		}
		else if (flying && col.gameObject.tag == "Enemy") Destroy(col.gameObject);
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if (col.gameObject.tag == "rightleft" || col.gameObject.tag == "updown") {
			onMovingPlatform = false;
			this.transform.parent = null;
		}

		if (col.gameObject.tag == "ChamberEnd")
			EndGame ();
		else if (col.gameObject.name == "Finish" && !loading) 
			atDoor = false;
	}

	GameObject platParent;

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "rightleft" || col.gameObject.tag == "updown") 
		{
			onMovingPlatform = true;
			platParent = col.gameObject;
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
			audio.ChangeTrack(c);
			if(bubbleShield)
			{
				bubbleTime = 0;
			}
		}
		color = c;

		if (col.gameObject.name == "Finish" && !loading) 
			atDoor = true;

		if (col.gameObject.tag == "Foreground") {
			//Turn block red
			if (color == 1)
				col.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1f, 0f, 0f);
		
			//Turn block orange
			else if (color == 2) 
				col.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1f, 0.5f, 0f);
		
			//Turn block yellow
			else if (color == 3)
				col.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 0f);
		
			//Turn block green
			else if (color == 4)
				col.gameObject.GetComponent<SpriteRenderer> ().color = new Color (0f, 1f, 0f);
		
			//Turn block blue
			else if (color == 5)
				col.gameObject.GetComponent<SpriteRenderer> ().color = new Color (0f, 0f, 1f);
		
			//Turn block purple
			else if (color == 6)
				col.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1f, 0f, 1f);
		}
	}
	
	IEnumerator PlayerRestart()
	{
		//animate death

		int i = 0;
		while (i<256) {
			//Debug.Log (i);
			PlayerDying(i);	
			//GetComponent<SpriteRenderer> ().material.color = new Color (255 - i, 255 - i, 255 - i);
			i++;
		}
		//GetComponent<SpriteRenderer> ().material.color = new Color (255 - 255, 255 - 255, 255 - 255);
		bubbleTime = 0;
		if (stage % 2 == 0) {
			AutoFade.LoadLevel ("ColorRoom", 5, 1, Color.black);
			stage = 1;		
		}
		else
			AutoFade.LoadLevel (stage, 5, 1, Color.black);
		PlayerPrefs.SetInt ("Level", 1);
		yield return new WaitForSeconds(5);
		alive = true;
		anim.SetBool ("Alive", true);
		this.rigidbody2D.isKinematic = false;
		this.GetComponent<PlayerScript>().enabled = true;
		transform.position = new Vector3(0,0,0);
		GetComponent<SpriteRenderer> ().material.color = new Color (255,255,255);
		color = 0;
		anim.SetInteger ("Color", 0);
		audio.ChangeTrack(0);
	}

	IEnumerator PlayerDying(int c)
	{
		yield return new WaitForSeconds(1f);
		GetComponent<SpriteRenderer> ().material.color = new Color (255 - c, 255 - c, 255 - c);
	}
	
	IEnumerator SceneChange()
	{
		bubbleTime = 0;
		stage++;
		maxSpeed = 0;
		AutoFade.LoadLevel (stage, 3, 1, Color.black);
		PlayerPrefs.SetInt ("Level", stage);
		Debug.Log (PlayerPrefs.GetInt ("Level"));
		yield return new WaitForSeconds(3);
		maxSpeed = 30;
		transform.position = new Vector3(0,0,0);
		loading = false;
		atDoor = false;
	}

	IEnumerator EndGame()
	{
		GameObject.FindWithTag("MainCamera").GetComponent<CameraFollow>().enabled = false;
		GameObject.FindWithTag("MainCamera").GetComponent<Camera>().orthographicSize = 500;
		Destroy(GameObject.FindWithTag("L5Structure"));
		this.GetComponent<PlayerScript>().enabled = false;
		yield return new WaitForSeconds(5);
		AutoFade.LoadLevel ("Credits", 7, 3, Color.white);
	}

	void LeftFoot  (){ foot = 0; }
	void CenterFoot(){ foot = 1; }
	void RightFoot (){ foot = 2; }

	void IdleCycle(){ cycle = 0; }
	void LeftCycle(){ cycle = 1; }
	void RightCycle(){ cycle = 2; }
	void JumpCycle(){ cycle = 3; }

	void WhiteOut(){
		anim.SetLayerWeight (1, 1f);
	}
}
