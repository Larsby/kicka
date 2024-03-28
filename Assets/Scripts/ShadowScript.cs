using UnityEngine;
using System.Collections;

public class ShadowScript : MonoBehaviour
{

	static float map (float value,
	                  float istart,
	                  float istop,
	                  float ostart,
	                  float ostop)
	{
		return ostart + (ostop - ostart) * ((value - istart) / (istop - istart));
	}

	public GameObject ShadowType;
	public GameObject ShadowVar;

	// Use this for initialization
	void Start ()
	{
		
		ShadowVar = Instantiate (ShadowType);
		ShadowVar.transform.parent = gameObject.transform;
		ShadowVar.transform.parent = GameObject.Find ("rackets").transform;

	}

	
	// Update is called once per frame
	void Update ()
	{
		float shadowsink = 2.0f;
		if (ShadowVar != null) {
			ShadowVar.transform.localScale = transform.localScale;
			ShadowVar.transform.rotation = transform.rotation;
			//	ShadowVar.transform.position = new Vector3 (transform.position.x, transform.position.y - shadowsink, 1);
			ShadowVar.transform.position = new Vector3 (transform.position.x, transform.position.y - shadowsink, transform.position.z + 1);

		}
	 
	}

	void OnDestroy ()
	{
		Destroy (ShadowVar);
	}

}
