using UnityEngine;
using System.Collections;

public class AudioScript : MonoBehaviour {


	public AudioClip death;
	public AudioClip jump;
	public AudioClip land;

	public AudioClip tick;
	public AudioClip boom;
	public AudioClip gun;
	public AudioClip hit;
	public AudioClip paint;
	public AudioClip dash;
	public AudioClip pop;
	public AudioClip punch;


	public AudioSource blankTrack;
	public AudioSource redTrack;
	public AudioSource orangeTrack;
	public AudioSource yellowTrack;
	public AudioSource greenTrack;
	public AudioSource blueTrack;
	public AudioSource purpleTrack;
	public AudioSource walking;
	public AudioSource sounds;

	AudioSource current;
	AudioSource wait;
	// Use this for initialization
	void Start () {
		current = blankTrack;
		current.volume = 0.0f;
		wait = redTrack;
		Debug.Log("MVOL: " + PlayerPrefs.GetFloat("Music")); 
	}
	
	// Update is called once per frame
	void Update () {
		//good for playing jumps, explosions, etc.
		//AudioSource.PlayClipAtPoint (clip, transform.position);
		//AudioSource.PlayOneShot(clip, volume);

		//increase
		float test = PlayerPrefs.GetFloat("Music");
		if (current.volume < test)
			current.volume += 0.01f;

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
			wait = current;
			current = orangeTrack;
		}
		//now yellow
		if (color == 3) {
			wait = current;
			current = yellowTrack;
		}
		//now green
		if (color == 4) {
			wait = current;
			current = greenTrack;
		}
		//now blue
		if (color == 5) {
			wait = current;
			current = blueTrack;
		}
		//now purple
		if (color == 6) {
			wait = current;
			current = purpleTrack;
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

	public void PauseTrack(){
		current.Pause ();
		if (!walking.isPlaying)
			walking.Pause ();
	}

	public void ResumeTrack(){
		current.Play ();
		walking.Play ();
	}
}