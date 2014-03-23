using UnityEngine;
using System.Collections;

public class AudioScript : MonoBehaviour {

	public AudioSource blankTrack;
	public AudioSource redTrack;
	public AudioSource blueTrack;
	public AudioSource yellowTrack;
	public AudioSource walking;
	public AudioClip death;
	public AudioClip jump;
	public AudioClip land;
	public AudioClip gun;
	public AudioClip paint;
	AudioSource current;
	AudioSource wait;
	// Use this for initialization
	void Start () {
		current = blankTrack;
		wait = redTrack;
	}
	
	// Update is called once per frame
	void Update () {
		//good for playing jumps, explosions, etc.
		//AudioSource.PlayClipAtPoint (clip, transform.position);

		//increase
		//if (current.volume != 1)
			//current.volume += 0.01f;

		//decrease
		if (wait.volume != 0) {
			//Debug.Log ("Decreasing");	
			wait.volume -= 0.01f;
		}

	}

	public void ChangeTrack(int color){
		//now red
		if (color == 1) {
			wait = current;
			current = redTrack;
		}
		//now orange
		if (color == 2) {
			
		}
		//now yellow
		if (color == 3) {
			wait = current;
			current = yellowTrack;
		}
		//now green
		if (color == 4) {
			
		}
		//now blue
		if (color == 5) {
			wait = current;
			current = blueTrack;
		}
		//now purple
		if (color == 6) {
			
		}
	}

	public void PlayWalk(){
		if (!walking.isPlaying)
			walking.Play ();
	}

	public void StopWalk(){
		if (walking.isPlaying)
			walking.Stop ();
	}
}