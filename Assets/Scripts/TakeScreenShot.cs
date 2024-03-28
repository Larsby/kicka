using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TakeScreenShot : MonoBehaviour
{

	// Use this for initialization
	public bool capture = true;
	public string screenshotname = "RoundPingis";
	private static int index = 1;

	void Start ()
	{
		index = 1;
	}



	void Capture ()
	{
		Application.CaptureScreenshot (Application.dataPath + "/InGameIShots/" + screenshotname + index + ".png");
		index++;

	}

	// Update is called once per frame
	void Update ()
	{
		if (capture) {
			Capture ();
			//capture = false;
		}
	}
}
