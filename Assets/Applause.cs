using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Applause : MonoBehaviour {

	public AudioClip[] applause;
	 
	AudioClip current;
	private AudioSource source;
	public float randMax = 0;
	bool audienceIsAlive = false;
	// Use this for initialization
	void Start ()
	{
		source = GetComponent<AudioSource> ();
	}


	public void stopAudience ()
	{
		source.Stop ();
		audienceIsAlive = false;
	}

	public void goAudience()
	{
		audienceIsAlive = true;
		 
	}

	public float Play ()
	{
	
	 	source.clip = applause [Random.Range (0, applause.Length)];
	 	source.Play ();

		source.pitch = Random.Range (1.0f - randMax, 1.0f + randMax);
		return source.clip.length;
	}

	void Update ()
	{
		if (!source.isPlaying) {
			if (audienceIsAlive) {
				Play();
			
			}
		}
	}
}
