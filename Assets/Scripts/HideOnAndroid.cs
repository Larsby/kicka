using UnityEngine;
using System.Collections;

public class HideOnAndroid : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		
		#if UNITY_ANDROID
		gameObject.active = false;
		#endif
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
