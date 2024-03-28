using UnityEngine;
using System.Collections;

public class EnableDisableRacket : MonoBehaviour
{
	GameObject shadow;
	ShadowScript script;

	void Start ()
	{
		script = GetComponent<ShadowScript> ();

	}

	public void EnableRacket ()
	{

		shadow = script.ShadowVar;
		if (shadow != null) {
			shadow.GetComponent<Renderer> ().enabled = true;
		}
		GetComponent<Renderer> ().enabled = true;
		GetComponent<Collider2D> ().enabled = true;
		iTween.FadeTo (gameObject, iTween.Hash ("alpha", 1.0f, "time", 0.5f));

	}

	private void ActuallyDisableRacket ()
	{
		shadow = script.ShadowVar;
		if (shadow != null) {
			shadow.GetComponent<Renderer> ().enabled = false;
		}
		GetComponent<Renderer> ().enabled = false;
		Invoke ("EnableRacket", 4.5f);
	}

	public void DisableRacket (float time)
	{
		GetComponent<Collider2D> ().enabled = false;
		iTween.FadeTo (gameObject, iTween.Hash ("alpha", 0.0f, "time", 0.5f, "oncomplete", "ActuallyDisableRacket"));
		//	Invoke ("EnableRacket", time);


	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
