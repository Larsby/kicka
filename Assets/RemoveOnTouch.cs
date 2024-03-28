using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveOnTouch : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount > 0) {
			iTween.FadeTo (gameObject, iTween.Hash ("alpha", 0.0f, "time", 0.5f));
		 
		}
		if (Input.GetButtonDown ("Fire1")) {
			iTween.FadeTo (gameObject, iTween.Hash ("alpha", 0.0f, "time", 0.5f));

		}
	}
}
