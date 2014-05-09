using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour 
{
	
	private Transform camera;
	private float lifetime = 0;
	private bool grenadeExploding;
	private bool grenadeSplat = true;
	
	// Use this for initialization
	void Start() 
	{
		grenadeExploding = false;
		camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
		
		if 		(gameObject.tag == "Grenade")	lifetime = Time.time + 1.3f;
		else if (gameObject.tag == "Orange") 	lifetime = Time.time + 10;
		else if (gameObject.tag == "Yellow") 	lifetime = Time.time + 6;
		else if (gameObject.tag == "Green") 	lifetime = Time.time + 2;
		else if (gameObject.tag == "Blue") 		lifetime = Time.time + 1000;
		else if (gameObject.tag == "Purple") 	lifetime = Time.time + (float)0.5;
		else lifetime = Time.time + 10;
	}
	
	void OnCollisionEnter2D(Collision2D col)
	{
		if (gameObject.tag == "Orange" && col.gameObject.tag != "Enemy") { Destroy(gameObject); GameObject.FindWithTag ("Backdrop").GetComponent<SplatterScript> ().Splat (2, transform.position, new Quaternion ()); }
		else if (gameObject.tag == "Projectile")
		{
			if (col.gameObject.tag == "Projectile") { Destroy(col.gameObject); }
			else if (col.gameObject.tag == "Enemy") { Destroy(col.gameObject); }
			else if (col.gameObject.tag == "Orange") Destroy(col.gameObject);
			else if (col.gameObject.tag == "Stage" || col.gameObject.tag == "Floor" || col.gameObject.tag == "Yellow") Destroy(gameObject);
			else if (col.gameObject.tag == "Purple") { Destroy(col.gameObject); Destroy(gameObject); } 
			else if (col.gameObject.tag != "Player") Destroy(gameObject);
		}

		//player shot himself in the face condition
		else if (col.gameObject.tag == "Player")
		{
			if (gameObject.tag == "Purple" || gameObject.tag == "Orange" ) Destroy(gameObject);
		}

		else if (gameObject.tag == "Yellow" && col.gameObject.tag == "Enemy") { 
			col.gameObject.collider2D.enabled = false;
			col.gameObject.GetComponent<EnemyScript>().anim.SetBool("Alive",false); 
			lifetime = Time.time + 1; 
		}

		else if (gameObject.tag == "Purple" && (col.gameObject.tag == "Stage" || col.gameObject.tag == "Floor")) Destroy(gameObject);
	
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
			if (grenadeSplat)
			{
				this.gameObject.GetComponent<PolygonCollider2D> ().enabled = false;
				this.gameObject.GetComponent<Rigidbody2D> ().isKinematic = true;
				this.gameObject.GetComponent<Animator> ().enabled = true;
				float rand = Random.value * 360;
				GameObject.FindWithTag ("Backdrop").GetComponent<SplatterScript> ().Splat (1, transform.position, Quaternion.Euler(new Vector3(0,0,rand)));
				grenadeSplat = false;
			}
			Collider2D[] boom = Physics2D.OverlapCircleAll (gameObject.transform.position, 18f);
			int l = boom.GetLength (0);
			
			for (int i=0; i<l; i++) 
			{
				//Debug.Log ("Boom");
				GameObject hit = boom [i].gameObject;
				if (hit.rigidbody2D != null) 
				{
					if (hit.rigidbody2D.gameObject.tag == "Player")
					{
						if (!GameObject.FindWithTag("Player").GetComponent<PlayerScript>().grounded) hit.rigidbody2D.AddForce ((hit.transform.position - gameObject.transform.position) * 20f);
					}
					else if (hit.rigidbody2D.gameObject.tag == "Enemy" )
					{
						//animate death
						hit.collider2D.enabled = false;
						hit.gameObject.GetComponent<EnemyScript>().anim.SetBool ("Alive", false);
					}
				}
			}
		}
		else if (lifetime < Time.time && grenadeExploding)
		{
			Debug.Log ("grenade out of life");
			grenadeSplat = true;
			Destroy (gameObject);
		}
		else if (lifetime < Time.time && !grenadeExploding) 
		{ 
			//spawn color splat
			if (gameObject.tag == "Grenade") 
			{
				//turn off ticking
				//gameObject.GetComponent<AudioSource>().PlayOneShot()
				lifetime = Time.time + (float)1.0;
				grenadeExploding = true;
			} 
			else if (gameObject.tag == "Orange"){
				GameObject.FindWithTag ("Backdrop").GetComponent<SplatterScript> ().Splat (2, transform.position, new Quaternion());
			}
			else if (gameObject.tag == "Yellow"){
				GameObject.FindWithTag ("Backdrop").GetComponent<SplatterScript> ().Splat (3, transform.position, transform.rotation);
			}
			else if (gameObject.tag == "Purple"){
				float rand = Random.value * 360;
				GameObject.FindWithTag ("Backdrop").GetComponent<SplatterScript> ().Splat (6, transform.position, Quaternion.Euler(new Vector3(0,0,rand)));
			}
			if(!grenadeExploding) Destroy (this.gameObject); 
		} 
	}

	void EndAnim() {
		this.gameObject.GetComponent<Animator> ().enabled = false;
	}
}
