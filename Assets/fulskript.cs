using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fulskript : MonoBehaviour {
	Animator anim;

	public float animpos = 0.1f;
	private float old = 0.0f;
	public bool left = false;
	public bool idle = false;
	private bool inti = true;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		anim.speed = 0;
		anim.Play ("StrafeRight");

	}
	
	// Update is called once per frame
	void Update () {
		animpos-=0.01f;
		animpos %= 1f;
		string animName = "StrafeRight";
		if (left) {
			animName = "StrafeLeft";
		}
		if (idle) {
			animName = "Idle";
			anim.speed = 1;
			//anim.ResetTrigger ("Idle");
			anim.Play ("Idle");

		}
		if (old != animpos ) {
			anim.playbackTime = animpos;
			anim.Play (animName,-1,animpos);
			old = animpos;	
		}

	}

}
