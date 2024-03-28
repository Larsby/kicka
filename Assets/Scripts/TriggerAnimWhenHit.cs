using UnityEngine;
using System.Collections;

public class TriggerAnimWhenHit : MonoBehaviour
{

	// Use this for initialization
	public GameObject target = null;
	public string animationTriggerName;
	private Animator animator = null;

	void Start ()
	{
		if (target == null) {
		
			target = gameObject;
		}
		animator = target.GetComponent<Animator> ();
	}

	void DisableRenderer ()
	{
		target.GetComponent<Renderer> ().enabled = false;

	}

	void OnCollisionEnter2D (Collision2D col)
	{
		animator.SetTrigger (animationTriggerName);
		Invoke ("DisableRenderer", 2f);
	}
	// Update is called once per frame
	void Update ()
	{

	}
}
