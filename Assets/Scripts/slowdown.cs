using UnityEngine;
using System.Collections;

public class slowdown : MonoBehaviour
{

	public float speedchanger = 0.001f;
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnTriggerExit2D (Collider2D col)
	{

		float forceAmount = speedchanger;
		col.gameObject.GetComponent<Rigidbody2D> ().AddForce (col.gameObject.GetComponent<Rigidbody2D> ().velocity.normalized * forceAmount, ForceMode2D.Impulse);
	 
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		float forceAmount = -speedchanger;
		col.gameObject.GetComponent<Rigidbody2D> ().AddForce (col.gameObject.GetComponent<Rigidbody2D> ().velocity.normalized * forceAmount, ForceMode2D.Impulse);

	}

}
