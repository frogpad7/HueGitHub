using UnityEngine;
using System.Collections;

public class EnemyTrigger : MonoBehaviour 
{
	EnemyScript eScript;
	public GameObject parentEnemy;

	// Use this for initialization
	void Start() 
	{
		parentEnemy = this.transform.parent.gameObject;
		eScript = parentEnemy.GetComponent<EnemyScript>();
		eScript.moving = false;
		Debug.Log ("parent:" + parentEnemy.gameObject.tag);
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player") 
		{
			Debug.Log ("exit enemy trigger");
			eScript.moving = false;
		}
	}
	
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player") 
		{
			Debug.Log ("enter enemy trigger");
			eScript.moving = true;
		}
	}
	// Update is called once per frame
	void Update () 
	{
	
	}
}
