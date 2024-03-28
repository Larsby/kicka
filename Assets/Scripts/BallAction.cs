using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAction : MonoBehaviour
{

	public GameObject actor;
	private GameControl game;
	// Use this for initialization
	void Start ()
	{
	GameObject obj = GameObject.FindGameObjectWithTag ("GameControl");	
		game = obj.GetComponent<GameControl> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		

	}

	void KickIsDone() {
		game.kicking = null;
		actor.GetComponent<Animator> ().ResetTrigger ("Idle");
	}
	void OnTriggerEnter2D (Collider2D other)
	{

		 	 
			
		if (!actor.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("HeadbuttBall")) {
			
			string trigger = "HeadbuttBall";
			if (other.gameObject.transform.localScale.x <= 20.0f) {
				trigger = "Pass";
			}
			game.kicking = actor.GetComponent<Animator> ();
			actor.GetComponent<Animator> ().speed = 1.0f;
			actor.GetComponent<Animator> ().Play ("Pass");
	//		actor.GetComponent<Animator> ().SetTrigger (trigger);
			Invoke("KickIsDone",1.5f);
		}

		
	}
	void OnTriggerExit2D (Collider2D other)
	{




		actor.GetComponent<Animator> ().ResetTrigger ("Idle");
			game.kicking = null;
			




	}


}
