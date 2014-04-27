using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour 
{
	
	private Transform camera;
	private float lifetime = 0;
	private bool grenadeExploding;
	
	// Use this for initialization
	void Start() 
	{
		grenadeExploding = false;
		camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
		
		if 		(gameObject.tag == "Grenade")	lifetime = Time.time + 3;
		else if (gameObject.tag == "Orange") 	lifetime = Time.time + 10;
		else if (gameObject.tag == "Yellow") 	lifetime = Time.time + 6;
		else if (gameObject.tag == "Green") 	lifetime = Time.time + 2;
		else if (gameObject.tag == "Blue") 		lifetime = Time.time + 1000;
		else if (gameObject.tag == "Purple") 	lifetime = Time.time + (float)0.5;
		else lifetime = Time.time + 10;
	}
	
	void OnCollisionEnter2D(Collision2D col)
	{
		if (gameObject.tag == "Projectile")
		{
			if (col.gameObject.tag == "Projectile") { Destroy(col.gameObject); Destroy(col.gameObject); }
			else if (col.gameObject.tag == "Orange") Destroy(col.gameObject);
			else if (col.gameObject.tag == "Stage" || col.gameObject.tag == "Floor" || col.gameObject.tag == "Yellow") Destroy(gameObject);
			else if (col.gameObject.tag == "Projectile"  || col.gameObject.tag == "Purple") { Destroy(col.gameObject); Destroy(gameObject); } 
			else if (col.gameObject.tag != "Player") Destroy(gameObject);
		}

		//player shot himself in the face condition
		else if (col.gameObject.tag == "Player")
		{
			if (gameObject.tag == "Purple" || gameObject.tag == "Orange" ) Destroy(gameObject);
		}

		//else if (gameObject.tag == "Orange") this.rigidbody2D.isKinematic = true;
		else if (gameObject.tag == "Purple" && (col.gameObject.tag == "Stage" || col.gameObject.tag == "Floor")) Destroy(gameObject);
	
		//make grenades go boom
		else if (gameObject.tag == "Grenade")
		{
			if(col.gameObject.tag == "Enemy")
			{
				this.gameObject.rigidbody2D.isKinematic = true;
				lifetime = Time.time + (float)0.5;
				grenadeExploding = true;
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		//if (transform.position.x > camera.transform.position.x+50f || transform.position.x < camera.transform.position.x-50f)
		//{
		//	Debug.Log ("grenade out of range!");
		//	Destroy (gameObject);
		//}
		
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
					}
					else if (hit.rigidbody2D.gameObject.tag == "Enemy")
					{
						Destroy(hit.rigidbody2D.gameObject);
					}
				}
			}
		}
		else if (lifetime < Time.time && grenadeExploding)
		{
			Debug.Log ("grenade out of life");
			Destroy (this.gameObject);
			GameObject.FindWithTag ("Backdrop").GetComponent<SplatterScript> ().Splat (1, transform.position, new Quaternion());
		}
		else if (lifetime < Time.time && !grenadeExploding) 
		{ 
			//spawn color splat
			if (gameObject.tag == "Grenade") 
			{
				lifetime = Time.time + (float)0.5;
				grenadeExploding = true;
			} 
			else if (gameObject.tag == "Orange"){
				GameObject.FindWithTag ("Backdrop").GetComponent<SplatterScript> ().Splat (2, transform.position, new Quaternion());
			}
			else if (gameObject.tag == "Yellow"){
				GameObject.FindWithTag ("Backdrop").GetComponent<SplatterScript> ().Splat (3, transform.position, transform.rotation);
			}
			//else if (gameObject.tag == "Green")
			//	GameObject.FindWithTag ("Backdrop").GetComponent<SplatterScript> ().Splat (4, transform.position);
			//else if (gameObject.tag == "Blue"){
			//	GameObject.FindWithTag ("Backdrop").GetComponent<SplatterScript> ().Splat (5, transform.position);
			//	Debug.Log ("Pop");
			//}
			else if (gameObject.tag == "Purple"){
				GameObject.FindWithTag ("Backdrop").GetComponent<SplatterScript> ().Splat (6, transform.position, new Quaternion());
			}
			if(!grenadeExploding) Destroy (this.gameObject); 
		} 
	}
	
}
