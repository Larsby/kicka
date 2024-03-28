using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateRandomWhenHit : MonoBehaviour {

	public bool isOn = false;
	void OnCollisionExit2D(Collision2D coll) {

		if (isOn) {
			float collChange = 0.1f;
			transform.rotation = new Quaternion (transform.rotation.x, transform.rotation.y, Random.Range (90-collChange, 90+collChange), transform.rotation.w);
		}
	
	}
}
