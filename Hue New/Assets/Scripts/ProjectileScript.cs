using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour 
{
	public Sprite red;
	public Sprite orange;
	public Sprite yellow;
	public Sprite green;
	public Sprite blue;
	public Sprite purple;
	
	private Transform camera;
	private float lifetime = 0;
	private bool grenadeExploding;
	
	// Use this for initialization
	void Start() 
	{
		grenadeExploding = false;
		camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
		
		if 		(gameObject.tag == "Grenade")	{ Debug.Log ("Grenade create"); 	lifetime = Time.time + 3; }
		else if (gameObject.tag == "Orange") 	{ Debug.Log ("Orange create"); 		lifetime = Time.time + 10; }
		else if (gameObject.tag == "Yellow") 	{ Debug.Log ("Platform create"); 	lifetime = Time.time + 6; }
		else if (gameObject.tag == "Green") 	{ Debug.Log ("Dash create"); 		lifetime = Time.time + 2; }
		else if (gameObject.tag == "Blue") 		{ Debug.Log ("Bubble create"); 		lifetime = Time.time + 1; }
		else if (gameObject.tag == "Purple") 	{ Debug.Log ("Glove create"); 		lifetime = Time.time + (float)0.5; }
		else lifetime = Time.time + 10;
	}
	
	void OnCollisionEnter2D(Collision2D col)
	{
		if (gameObject.tag == "Purple" && col.gameObject.tag == "Player") { Debug.Log ("glove player"); Destroy(gameObject); }

		//ORANGE
		if (gameObject.tag == "Orange" && col.gameObject.tag == "Player") { Debug.Log ("Orange player"); Destroy(gameObject); }
		else if (gameObject.tag == "Orange") this.rigidbody2D.isKinematic = true;

		else if (gameObject.tag == "Purple" && col.gameObject.tag == "Stage") { Debug.Log ("glove wall"); Destroy(gameObject); }
		if (gameObject.tag == "Projectile"||
		    gameObject.tag == "Blue"||gameObject.tag == "Purple") { /*do nothing*/}
		

		
		/*if (col.gameObject.tag == "Player")
		{
			//player shot himself condition
			Debug.Log ("player hit own bullet");
		} */
		
		//make grenades go boom
		if (gameObject.tag == "Grenade")
		{
			if(col.gameObject.tag == "Enemy")
			{
				this.gameObject.rigidbody2D.isKinematic = true;
				lifetime = Time.time + (float)0.5;
				grenadeExploding = true;
			}
			//GameObject.FindWithTag("Backdrop").GetComponent<SplatterScript>().Splat (red,transform.position);
		}
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (transform.position.x > camera.transform.position.x+50f || transform.position.x < camera.transform.position.x-50f)
		{
			Debug.Log ("grenade out of range!");
			Destroy (gameObject);
		}
		
		//weapon lifetime is up
		if (lifetime > Time.time && grenadeExploding) 
		{
			Collider2D[] boom = Physics2D.OverlapCircleAll (gameObject.transform.position, 20f);
			int l = boom.GetLength (0);
			
			for (int i=0; i<l; i++) 
			{
				//Debug.Log ("Boom");
				GameObject hit = boom [i].gameObject;
				if (hit.rigidbody2D != null) 
				{
					if (hit.rigidbody2D.gameObject.tag == "Player")
					{
						hit.rigidbody2D.AddForce ((hit.transform.position - gameObject.transform.position) * 20f);
						Debug.Log ("Pushing!");
					}
					else if (hit.rigidbody2D.gameObject.tag == "Enemy")
					{
						Debug.Log("grenade blast hit enemy!");
						Destroy(hit.rigidbody2D.gameObject);
					}
				}
			}
		}
		else if (lifetime < Time.time && grenadeExploding)
		{
			Debug.Log ("grenade out of life");
			Destroy (this.gameObject);
		}
		else if (lifetime < Time.time && !grenadeExploding) 
		{ 
			//spawn color splat
			if (gameObject.tag == "Grenade") 
			{
				lifetime = Time.time + (float)0.5;
				grenadeExploding = true;
				
				GameObject.FindWithTag ("Backdrop").GetComponent<SplatterScript> ().Splat (red, transform.position);
			} 
			else if (gameObject.tag == "Orange")
				GameObject.FindWithTag ("Backdrop").GetComponent<SplatterScript> ().Splat (orange, transform.position);
			else if (gameObject.tag == "Yellow")
				GameObject.FindWithTag ("Backdrop").GetComponent<SplatterScript> ().Splat (yellow, transform.position);
			else if (gameObject.tag == "Green")
				GameObject.FindWithTag ("Backdrop").GetComponent<SplatterScript> ().Splat (green, transform.position);
			else if (gameObject.tag == "Blue")
				GameObject.FindWithTag ("Backdrop").GetComponent<SplatterScript> ().Splat (blue, transform.position);
			else if (gameObject.tag == "Purple")
				GameObject.FindWithTag ("Backdrop").GetComponent<SplatterScript> ().Splat (purple, transform.position);
			
			if(!grenadeExploding) 
			{
				Debug.Log ("Player Weapon Dissipated"); 
				Destroy (this.gameObject); 
			}
		} 
	}
	
}
