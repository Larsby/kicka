using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;

using Facebook.Unity;


#if UNITY_ANDROID
//using GooglePlayGames;
#endif
using System.Collections.Generic;

public class GameControl: MonoBehaviour
{

	public static GameControl control;

	public int HighScore = 0;
	public Text scoreText;
	public Text highScoreText;
	int score = 0;
	int numPlayed = 0;
	bool isNoob = true;
	public GameObject StartPanel;
	public GameObject ScorePanel;
	public RundpingisBall rb;
	public GameObject[] paddletypes;
	public GameObject[] PowerUpDowns;
	public GameObject powerCircle;
	public moveRackets rackets;

	public Text statusScoreText;
	public Text statusHighScoreText;
	public Text statusHighScoreTextR;
	public Text statusHighScoreTextG;
	 
	public Text statusNumPlaysText;

	public GameObject buyButton;
	public GameObject visitButton;
	int numberOfBalls = 0;
	bool isIniting = false;
	bool hasplayedvideo = false;

	public GameObject IntroVideo;
	public GameObject CurrentPowerUp;
	private bool runPowerupOutAnim;
	//GameObject introvideovar;
	public PlayRandomSound[] gameSounds;
	public AdManager adManager;
	public Applause applause;
	private bool gameOver = false;
	GameObject top;
	GameObject bottom;
	GameObject left;
	GameObject right;




	public Sprite backgroundSprite;
	public GameObject background;

 
	 public AudioClip ingameAudio;
	//public AudioClip ingameAudio2;


	private string roundsText = null;
	private GameObject ball = null;
	float zRotation = 0f;
	AudioSource myAudio;
	public float speed = 30;
	float forceAmount = .0015f;
	GameObject MainCamera = null;
	string leaderboardID;
	public bool dontdie = false;
	public GameObject centerFloor;
	public GameObject UI;
	public TextMesh scoreTextMesh;

	public Animator kicking = null;
	private enum PlayerLevel
	{
		NOOB,
		BEGINNER,
		INTERMEDIATE,
		GOOD,
		EXPERT,
		INSANE,
		GODLIKE
	}

	PlayerLevel skill;

	public float GetBallSpeed ()
	{
		return 80f;
	}

	private PlayerLevel GetPlayerSkill ()
	{
		// simple implementation for now now
		if (score > 300) {
			return  PlayerLevel.GODLIKE;
		}
		if (score > 200) {
			return PlayerLevel.INSANE;

		}

		if (score > 100) {
			return PlayerLevel.EXPERT;
		}
		if (score > 50) { 
			return   PlayerLevel.INTERMEDIATE;
		}
		if (score > 80) {
			return   PlayerLevel.GOOD;
		}
		if (score > 30) {
			return PlayerLevel.BEGINNER;
		}
		return PlayerLevel.NOOB;
	}

	public float GetBallForce ()
	{
		switch (GetPlayerSkill ()) {
		case PlayerLevel.NOOB:
			return forceAmount;
		case PlayerLevel.BEGINNER:
			return forceAmount + 0.00001f;
		case PlayerLevel.INTERMEDIATE:
			return forceAmount + 0.00003f;

		case PlayerLevel.GOOD:
			return forceAmount + 0.00004f;
		case PlayerLevel.EXPERT:
			return forceAmount + 0.00005f;
		case PlayerLevel.INSANE:
			return forceAmount + 0.00004f;
		case PlayerLevel.GODLIKE:
			return forceAmount + 0.00003f;
		}
		
		   
		return forceAmount;
	}

	void initStartPage ()
	{
		
		statusScoreText.text = "" + score;
		statusHighScoreText.text = "" + HighScore;
		statusHighScoreTextR.text = statusHighScoreText.text;
		statusHighScoreTextG.text = statusHighScoreText.text;
		if(scoreTextMesh != null) 
		scoreTextMesh.text = "" + score;
		//	statusNumPlaysText.text = "ROUND " + numPlayed;
		//	statusNumPlaysText.text = "ROUND ";
		statusNumPlaysText.text = roundsText + " " + numPlayed;
		StartPanel.SetActive (true);
		ScorePanel.SetActive (true);
		ResetRotation ();
	
		if (powerCircle != null) {
			powerCircle.GetComponent<Animator> ().ResetTrigger ("CircleIn");
			powerCircle.GetComponent<Animator> ().enabled = false;
			Destroy (powerCircle);
			powerCircle = null;
		}
		runPowerupOutAnim = false;
			
	}

	public void AddForceToBall (int maxMagnitude)
	{
		if (ball == null) {
			ball = GameObject.Find ("Ball(Clone)");

		}		 
		Rigidbody2D ballBody = ball.GetComponent<Rigidbody2D> ();
		if (ballBody.velocity.magnitude < maxMagnitude) {
			Vector2 ballVelocity = new Vector2(ballBody.velocity.normalized.x,ballBody.velocity.normalized.y );
			ballVelocity.x += UnityEngine.Random.Range (0.1f, .16f);
			ballVelocity.y += UnityEngine.Random.Range (0.1f, .16f);
			ballVelocity*=2f;
			ballBody.AddForce (ballVelocity * GetBallForce (), ForceMode2D.Impulse);
			ballBody.position = new Vector2 (UnityEngine.Random.Range (ballBody.position.x - 0.1f, ballBody.position.x + 0.1f), ballBody.position.y);
		}
	}

	public void MakeBallSlower ()
	{
		Rigidbody2D ballBody = ball.GetComponent<Rigidbody2D> ();
		ballBody.velocity = new Vector2 (ballBody.velocity.x / 2.0f, ballBody.velocity.y / 2.0f);
	}

	void FixedUpdate ()
	{
		
		if (Screen.orientation != ScreenOrientation.LandscapeLeft) {
			Screen.orientation = ScreenOrientation.LandscapeLeft;
		}
		if (!hasplayedvideo) {
			Load ();
			if (roundsText == null) {
				roundsText = statusNumPlaysText.text;
			}


			initStartPage ();
			//	iTween.FadeTo (StartPanel, 0.0f, 0.5f);
		    
		}
		if (MainCamera == null) {
			MainCamera = GameObject.Find ("Main Camera");
		}
		if (zRotation < 0 || zRotation > 0) {
			MainCamera.transform.Rotate (0.0f, 0.0f, zRotation);
		}
	
	/*	if (!myAudio.isPlaying) {
			if ((int)UnityEngine.Random.Range (0.0f, 2.0f) == 1) {
				myAudio.clip = ingameAudio2;
			} else {
				myAudio.clip = ingameAudio;
			}
			myAudio.Play ();
 
		}


		if (myAudio.pitch != 1.0) {
			myAudio.pitch += 0.02f;
			if (myAudio.pitch > 1.0f)
				myAudio.pitch = 1.0f;
		}
*/
	

		if (!myAudio.isPlaying) {
 
				myAudio.clip = ingameAudio;
		 
			myAudio.Play ();

		}


		if (myAudio.pitch != 1.0) {
			myAudio.pitch += 0.02f;
			if (myAudio.pitch > 1.0f)
				myAudio.pitch = 1.0f;
		}


		AdjustPaddle (top);
		AdjustPaddle (bottom);
		AdjustPaddle (left);
		AdjustPaddle (right);
	}

	void AdjustPaddle (GameObject paddle)
	{
		/*	float adjustspeed = 0.01f;
		
		if (paddle != null) {
			if (paddle.transform.localScale.x < 10) {
				paddle.transform.localScale = new Vector3 (paddle.transform.localScale.x + adjustspeed, paddle.transform.localScale.y, paddle.transform.localScale.z);

			}
			if (paddle.transform.localScale.x > 10) {
				paddle.transform.localScale = new Vector3 (paddle.transform.localScale.x - adjustspeed, paddle.transform.localScale.y, paddle.transform.localScale.z);

			}

			if (paddle.transform.localScale.y < 10) {
				paddle.transform.localScale = new Vector3 (paddle.transform.localScale.x, paddle.transform.localScale.y + adjustspeed, paddle.transform.localScale.z);

			}
			if (paddle.transform.localScale.y > 10) {
				paddle.transform.localScale = new Vector3 (paddle.transform.localScale.x, paddle.transform.localScale.y - adjustspeed, paddle.transform.localScale.z);

			}

		}
*/
	}


	void Awake ()
	{
 
		#if UNITY_IOS 
		Application.targetFrameRate = 600;
		#endif

		if (!hasplayedvideo) {
			ScorePanel.SetActive (false);
			//introvideovar = Instantiate (IntroVideo);
 
		}
		
		if (control == null) {
			DontDestroyOnLoad (gameObject);
			control = this;
		
		 
		} else if (control != this) {
			// There can be only one!
			Destroy (gameObject);
		}
//		gameObject.GetComponent
	}

	public void Share ()
	{

		Facebook.Unity.FB.FeedShare ("",
			new Uri ("http://www.pastille.se/"),
			"Get Kicka",
			"Join me and get Kicka",
			"Can you beat my high score of " + HighScore + "?",
			null, null);
	
	}

	public void Rate ()
	{

		UniRate r = GameObject.FindObjectOfType<UniRate> ();
		r.ShowPrompt ();
	}



	public void moreFromPastille ()
	{
		Application.OpenURL ("http://www.pastille.se");
	}

	public void openPastilleWebpage ()
	{ 
		Application.OpenURL ("http://www.pastille.se");
	}


    public void pojkFotbollFacebook()
    {
        Application.OpenURL("https://www.facebook.com/PojkfotbolliSkane/");
    }

    public void pojkFotbollInstagram()
    {
        Application.OpenURL("https://www.instagram.com/pojkfotbolliskane/?hl=sv");
    }




	void Start ()
	{
		myAudio = GetComponent<AudioSource> ();
		socialstuff ();
		kicking = null;
	}




	void socialstuff ()
	{
		
	
		Facebook.Unity.FB.Init ();

	
	}








	void OnDestroy ()
	{
		Save ();
	}

	Vector3 GetSpawnPosition (Vector3 position)
	{
		float randomrange = 19f;
		bool flip = false;
		float xpos = UnityEngine.Random.Range (-randomrange, randomrange);
		float ypos = UnityEngine.Random.Range (-randomrange, randomrange);
		// if ball and spawn is in the same "pie slice" then flip x and y of spwan
		if (xpos < 0.0f && position.x < 0.0f && ypos > 0.0f && position.y > 0.0f) {
			flip = true;
		
		} else if (xpos > 0.0f && position.x > 0.0f && ypos > 0.0f && position.y > 0.0f) {
			flip = true;
		} else if (xpos < 0.0f && position.x < 0.0f && ypos < 0.0f && position.y < 0.0f) {
			flip = true;
		} else if (xpos > 0.0f && position.x > 0.0f && ypos < 0.0f && position.y < 0.0f) {
			flip = true;

		}
		if (flip) {
			xpos *= -1.0f;
			ypos *= -1.0f;
		}
			
		return new Vector3 (xpos, ypos, 1f);	

	}

	

	void Spawn ()
	{

		if (score < 9) {
			initSpawning ();
		} else {
			
			int powerupdowntype = (int)UnityEngine.Random.Range (0.0f, PowerUpDowns.Length);


			//	powerupdowntype = 8;
			GameObject bollen = GameObject.Find ("Ball(Clone)");

			if (bollen != null) {
				
				Vector3 spawnpos = GetSpawnPosition (bollen.transform.position);
				GameObject powerupdown = Instantiate (PowerUpDowns [powerupdowntype]);
				if (powerCircle != null) {
					powerCircle.GetComponent<Animator> ().enabled = false;
					Destroy (powerCircle);
					powerCircle = null;
				}
				powerupdown.GetComponent<Renderer> ().enabled = true;
				//	Debug.Log ("Power updownd type " + powerupdowntype);
				powerCircle = (GameObject)Instantiate (Resources.Load ("PowerCircle", typeof(GameObject)));
				//		Debug.Log ("Instantiating pwercircle" + powerupdowntype);
				powerCircle.GetComponent<Renderer> ().enabled = false;
				powerCircle.GetComponent<Animator> ().SetTrigger ("CircleIn");
				runPowerupOutAnim = true;
			

				powerCircle.transform.position = new Vector3(spawnpos.x,spawnpos.y,0.0f);
				powerupdown.transform.position = new Vector3(spawnpos.x,spawnpos.y,0.0f);
				CurrentPowerUp = powerupdown;
			
			}
		
			Invoke ("RemovePowerup", 7.0f);
		}

	}



	public void DestroyPowerup (object target)
	{
		Destroy ((GameObject)target);
	}

	public void TriggerPowerupCollisionAnim (GameObject target)
	{
		runPowerupOutAnim = false;
		if (target != null) {
			Collider2D c = target.GetComponent<Collider2D> ();
			if (c != null) {
				c.enabled = false;
			}
			iTween.FadeTo (target, iTween.Hash ("alpha", 0.0f, "time", 0.5f, "oncomplete", "DestroyPowerup", "oncompleteparams", target, "oncompletetarget", gameObject));

		}
	
	}

	float deltaTime = 0.0f;

	void Update ()
	{
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;

	}
	/*
	void OnGUI ()
	{
		int w = Screen.width, h = Screen.height;

		GUIStyle style = new GUIStyle ();

		Rect rect = new Rect (0, 0, w, h * 2 / 100);
		style.alignment = TextAnchor.UpperLeft;
		style.fontSize = h * 2 / 100;
		style.normal.textColor = new Color (0.0f, 0.0f, 0.5f, 1.0f);
		float msec = deltaTime * 1000.0f;
		float fps = 1.0f / deltaTime;
		string text = string.Format ("{0:0.0} ms ({1:0.} fps)", msec, fps);
		GUI.Label (rect, text, style);
	}
*/
	public void RemovePowerup ()
	{
		if (powerCircle != null) {
			
			powerCircle.GetComponent<Animator> ().ResetTrigger ("CircleIn");
			powerCircle.GetComponent<Animator> ().enabled = false;
			Destroy (powerCircle);
			powerCircle = null;
		}
		Debug.Log ("cleaning up powerups");
		var clones = GameObject.FindGameObjectsWithTag ("clone");
		foreach (var clone in clones) {
			TriggerPowerupCollisionAnim (clone);
		}
		initSpawning ();
	}




	public void initSpawning ()
	{
		CancelInvoke ("Spawn");
		Invoke ("Spawn", UnityEngine.Random.Range (10.0f, 20.0f));

	}

	IEnumerator PlayCoachTalk(float delay) {
		yield return new WaitForSeconds (delay);
		if (gameOver == false) {
			float time = gameSounds [3].Play ();
			StartCoroutine (PlayCoachTalk (time+ UnityEngine.Random.Range(1.0f,3.0f)));
		}
	}

	public void LostBall ()
	{
		//return;
		numberOfBalls--;

		if (numberOfBalls == 0) {
			if (dontdie == true) {
				createBall ();
				return;
			}
			GameOver ();

			//createBall ();
		}
	}
	 IEnumerator PlayCoachInsult() {
		yield return new WaitForSeconds (0.5f);
		gameSounds [2].Play ();
	}

	 IEnumerator ShowGameOverScreen() {
		
		UI.SetActive (true);
		iTween.PunchScale (StartPanel, new Vector3 (-1.0f, 1.5f, 0.0f), 0.5f);
		yield return new WaitForSeconds (0.5f);
		centerFloor.SetActive (false);
		//scoreText.text = "" + score;
		numPlayed++;
		if (numPlayed < 10)
			isNoob = false;
		if (score < 45)
			isNoob = false;

	//	scoreText.text = "" + score;

		initStartPage (); 
		//iTween.PunchScale (StartPanel, new Vector3 (-1.0f, 1.0f, 0.0f), 0.5f);

		cleanPlayfield ();
	
		if (score > HighScore) {
			HighScore = score;
			UniRate.Instance.LogEvent (true);
			UnityEngine.Analytics.Analytics.CustomEvent ("highScore", new Dictionary<string, object> {
				{ "highscore", HighScore }
			});

	
		}
		Save ();
		RemovePowerup ();
		initStartPage ();
		CancelInvoke ("Spawn"); //kill all old spawns
	}

	public void GameOver ()
	{  
		gameOver = true;
		StopCoroutine ("PlayCoachTalk");
		StartCoroutine (PlayCoachInsult ());
		StartCoroutine (ShowGameOverScreen ());
		applause.stopAudience ();
		adManager.PlayAd ();

	}

	public void StartGame ()
	{
 
		if (!hasplayedvideo) {
			initStartPage ();
			//	iTween.FadeTo (StartPanel, 0.0f, 0.5f);
			background.GetComponent<SpriteRenderer> ().sprite = backgroundSprite;

	
		}



		iTween.PunchScale (StartPanel, new Vector3 (-1.0f, 1.0f, 0.0f), 0.5f);
		isIniting = true;
	
		Invoke ("initPlayfield", 0.4f);
		Invoke ("Spawn", UnityEngine.Random.Range (1.0f, 2.0f));

		Invoke("removeUI", 0.5f);
	//	scoreText.text = "" + score;
		if(scoreTextMesh != null) 
		scoreTextMesh.text = "" + score;
		myAudio.pitch = UnityEngine.Random.Range (0.4f, 0.5f);


		applause.goAudience ();
		if (!hasplayedvideo) {
			Load ();
			hasplayedvideo = true;
		}

	}

	void removeUI()
	{	UI.SetActive (false);
		centerFloor.SetActive (true);
		
	}

	public void IncreaseScore ()
	{
		score += 1;
	//	scoreText.text = "" + score;
		if(scoreTextMesh != null) 
		scoreTextMesh.text = "" + score;
	}


	public void Save ()
	{
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Open (Application.persistentDataPath + "/gameInfo.dat", FileMode.OpenOrCreate);
		PlayerData data = new PlayerData ();
		data.HighScore = HighScore;
		data.numPlayed = numPlayed;
		data.LastScore = score;
		if (numPlayed < 10)
			isNoob = false;
		
		data.isNoob = isNoob;


		bf.Serialize (file, data);
		file.Close ();
	}

	public void Load ()
	{
		if (File.Exists (Application.persistentDataPath + "/gameInfo.dat")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/gameInfo.dat", FileMode.Open);
			PlayerData data = (PlayerData)bf.Deserialize (file);
			file.Close ();
			score = data.LastScore;
			HighScore = data.HighScore;
			numPlayed = data.numPlayed;
			isNoob = data.isNoob;
		}
	}

	void cleanPlayfield ()
	{

		Transform parent = GameObject.Find ("rackets").transform;

		foreach (Transform child in parent) {
			Destroy (child.gameObject);
		}

		var clones = GameObject.FindGameObjectsWithTag ("clone");
		foreach (var clone in clones) {
			Destroy (clone);
		}

	}

	public void ResetRotation ()
	{
		zRotation = 0.0f;
	}

	public void set_zRotation (float zrot)
	{
		zRotation = zrot;
		Invoke ("ResetRotation", 10);
	}

	public void MultiBall ()
	{
		for (int i = 0; i < 5; i++) {
			RundpingisBall ball = Instantiate (rb);
			ball.setGameControl (this);
			ball.GoBall (false);



			GameObject bollen = GameObject.Find ("Ball(Clone)");

			ball.transform.position = bollen.transform.position;


			ball.setGameControl (this);
			numberOfBalls++;
		}
	}
	public void ScaleToSmall(GameObject target) {
		Vector3 shrinkVector = new Vector3(10.0f,10.0f,10.0f);
		Vector3 normalVector = new Vector3 (20.0f, 20.0f, 20.0f);
		StartCoroutine(ScaleTo(target,shrinkVector,0.5f,0.0f));
		StartCoroutine(ScaleTo(target,normalVector,0.5f,15.0f));
	}
	public void ScaleToBig(GameObject target) {
		Vector3 ExpandkVector = new Vector3(40.0f,40.0f,40.0f);
		Vector3 normalVector = new Vector3 (20.0f, 20.0f, 20.0f);
		StartCoroutine(ScaleTo(target,ExpandkVector,0.5f,0.0f));
		StartCoroutine(ScaleTo(target,normalVector,0.5f,15.0f));
	
	}
	 IEnumerator ScaleTo(GameObject target, Vector3 scaleVector, float duration, float delay) {
		yield return new WaitForSeconds (delay);
		iTween.ScaleTo (target, scaleVector, duration);
	}
	private void createBall ()
	{
		RundpingisBall ball = Instantiate (rb);
		ball.setGameControl (this);

		if (isNoob) {
			// for the noob
			// for the noob
			ball.GoBall (true);
			//	left.transform.localScale = new Vector3 (20, 20, top.transform.localScale.z);
			//	right.transform.localScale = new Vector3 (20, 20, top.transform.localScale.z);

		} else {
			//this is for the expert player
			//this is for the expert player
			ball.GoBall (false);
			 
		}
		ParticleSystem ballsplosion = GameObject.Find ("Ballsplosion").GetComponent<ParticleSystem> ();
		;
		if (ballsplosion != null) {
			ball.setParticles (ballsplosion);
		}

		ball.setRackets (rackets);
		numberOfBalls++;
	}

	public void initPlayfield ()
	{
		if (!isIniting)
			return;
	
		gameOver = false;
		int paddletype = (int)UnityEngine.Random.Range (0.0f, paddletypes.Length);
	 
		float screenRatio = (float)(Screen.width) / (float)(Screen.height);
 
		if (screenRatio < 1.0) {
			
			GameObject.Find ("rackets").transform.localScale = new Vector3 (12f, 12f, 4f);
		}
		top = Instantiate (paddletypes [0]);
		bottom = Instantiate (paddletypes [0]);
		left = Instantiate (paddletypes [1]);
		right = Instantiate (paddletypes [1]);

		int edge = 40;

		top.transform.position = new Vector3 (0, edge, -1);
		top.transform.eulerAngles = new Vector3 (0, 90, -90);

		bottom.transform.position = new Vector3 (0, -edge, -1);
		bottom.transform.eulerAngles = new Vector3 (180, 90, -90);

		left.transform.position = new Vector3 (-edge, 0, -1);
		left.transform.eulerAngles = new Vector3 (270, 90, -90);

		right.transform.position = new Vector3 (edge, 0, -1);
		right.transform.eulerAngles = new Vector3 (90, 90, -90);

	
		top.transform.parent = GameObject.Find ("rackets").transform;
		bottom.transform.parent = GameObject.Find ("rackets").transform;
		left.transform.parent = GameObject.Find ("rackets").transform;
		right.transform.parent = GameObject.Find ("rackets").transform;


		createBall ();
		float time = gameSounds [0].Play ();
		StartCoroutine (PlayCoachTalk (time));

		if (screenRatio < 1.0) {
			GameObject.Find ("rackets").transform.localScale = new Vector3 (6f, 6f, 4f);
		}
	
		statusHighScoreText.text = "" + HighScore;
		statusHighScoreTextR.text = statusHighScoreText.text;
		statusHighScoreTextG.text = statusHighScoreText.text;

		score = 0;
	//	statusScoreText.text = "" + score;
		//scoreText.text = "" + score;
		if(scoreTextMesh != null) 
		scoreTextMesh.text = "" + score;
		StartPanel.SetActive (false);
		isIniting = false;
		if (score > HighScore)
			HighScore = score;
		Save ();
	 
	}

}

[Serializable]
class PlayerData
{
	public int numPlayed = 0;
	public int HighScore = 0;
	public int LastScore = 0;
	public bool isNoob = true;

 
}

