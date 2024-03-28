using UnityEngine;
using System.Collections;


public class RundpingisBall : MonoBehaviour
{
	 




	public AudioClip[] impact;
	AudioSource audioSource;
	AudioSource audioSourcePowerup;
	Rigidbody2D myRigidbody;
	SpriteRenderer sr;
	bool destroyNext = false;
	public GameControl gc;
	public Sprite[] ballSprites;
	bool shrinkNext = false;
	bool expandNext = false;
	GameObject ballShadowVar;
	public GameObject ballShadowType;
	moveRackets moves;

	public PlayRandomSound ooh;

	public AudioClip bomb;
	public AudioClip grow;
	public AudioClip growball;
	public AudioClip multiball;
	public AudioClip rotate;
	public AudioClip shrink;
	public AudioClip slow;
	public AudioClip slowball;
	public AudioClip expand;

	Vector2 magnus_direction;
	public bool enableMagnus = false;
	ParticleSystem ballsplosion;

	public GameObject sphere;

 

	void Start ()
	{
		ballShadowVar = Instantiate (ballShadowType);
		GetComponent<SpriteRenderer> ().color = Color.white;
		ParticleSystem ps = GetComponent<ParticleSystem> ();
		if (ps != null) {
			
			ps.startColor = Color.white;
		}
		//	moves = RacketObject.GetComponent<moveRackets> ();

	}

	public void setGameControl (GameControl inGC)
	{
		gc = inGC;
		 
	}

	public void setRackets (moveRackets inR)
	{
		moves = inR;
	}

	public void setParticles (ParticleSystem inR)
	{
		ballsplosion = inR;
	}


	static float map (float value,
	                  float istart,
	                  float istop,
	                  float ostart,
	                  float ostop)
	{
		return ostart + (ostop - ostart) * ((value - istart) / (istop - istart));
	}

	void Update ()
	{
		//	Vector2 moveDirection = gameObject.GetComponent<Rigidbody2D> ().velocity;
		/*	if (moveDirection != Vector2.zero)
		{
			float angle = Mathf.Atan2 (moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
		}
*/

		float shadowsink = map (Mathf.Abs (transform.position.x), 0, 20, 1.5f, 0.5f);
		ballShadowVar.transform.position = new Vector3 (transform.position.x, transform.position.y - shadowsink, transform.position.z + 1);
		ballShadowVar.transform.rotation = transform.rotation;

		 

		//Johan sätt in rätt storleksförändring på bollen här
		float dist = Vector3.Distance (new Vector3 (0, 0, 0), transform.position);

		//float ballsize = map (Mathf.Abs (dist), 0, 6.5f, 8.3f, 11.6f);
	//	float ballsize = map (Mathf.Abs (dist), 0, 6.5f, 8.3f+5.0f, 8.6f+5.0f);

	//	transform.localScale = new Vector3 (ballsize, ballsize, ballsize);
	
	 //	gc.AddForceToBall (Random.Range (20, 50));
		 gc.AddForceToBall (40);

		if (enableMagnus)
			myRigidbody.AddRelativeForce (magnus_direction);
		
	
		//sphere.transform.Rotate( myRigidbody.velocity.y*0.2f, myRigidbody.velocity.x*0.2f,0,  Space.World );
		//sphere.transform.Rotate( 0, (0.1f-myRigidbody.velocity.normalized.x)*5.0f,0,  Space.World );
		sphere.transform.Rotate( (myRigidbody.velocity.normalized.y)*5.0f,(0.1f-myRigidbody.velocity.normalized.x)*5.0f ,0,  Space.World );

	
		//transform.LookAt (Camera.main.transform.position, -Vector3.up);
	}

	public void GoBall (bool begin)
	{
		audioSource = GetComponent<AudioSource> ();
		audioSourcePowerup = GetComponents<AudioSource> () [1];
		myRigidbody = GetComponent<Rigidbody2D> ();
		//myRigidbody.velocity = new Vector2 (Random.Range (-1.0F, 1.0F), Random.Range (-1.0F, 1.0F)) * speed;  
		float speed = gc.GetBallSpeed ();
		Vector2 velocity;
		if (begin) {
			Debug.Log ("Beginner");
			velocity = new Vector2 ((1.0f - Random.Range (-0.1F, .10F)) * (speed / 10), (0.0f - Random.Range (-0.1F, .10F)) * (speed / 10));
		} else {
			velocity = new Vector2 ((1.0f - Random.Range (-0.2F, .20F)) * speed, (0.2f - Random.Range (-0.0F, .20F)) * speed);  
		}
		velocity *= 0.5f;
		myRigidbody.velocity = velocity;
		//	Debug.Log ("Velocity" + velocity);
		sr = gameObject.GetComponent<SpriteRenderer> (); 
		sr.sprite = ballSprites [(int)Random.Range (0.0f, ballSprites.Length)];

	}



	void OnCollisionExit2D (Collision2D col)
	{
		
		gc.AddForceToBall (52580);
		if (destroyNext) {
			destroyNext = false;
	
			col.gameObject.GetComponent<EnableDisableRacket> ().DisableRacket (5f);
			//gc.TriggerPowerupCollisionAnim (col.gameObject.);

			//play poof here?
			GetComponent<SpriteRenderer> ().color = Color.white;
			ParticleSystem ps = GetComponent<ParticleSystem> ();
			if (ps != null) {

				ps.startColor = Color.white;
			}
		}

 

	 // sphere.transform.LookAt(Vector3.zero, Vector3.forward);

	//	sphere.transform.Rotate (Vector3.forward);
		/*
		Vector3 dir = <your movedirection (_movement)>.normalized;
		transform.forward = dir;
		transform.Translate(Vector3.forward * <speed (_movementspeed)> * Time.deltaTime)>);*/	
	}

 
	void playSplosion (Collider2D col)
	{
		if (ballsplosion != null) {
			ballsplosion.transform.position = col.gameObject.transform.position;
			if (ballsplosion.isStopped)
				ballsplosion.Play ();
		}

	}
	IEnumerator ToSize(GameObject obj, Vector3 size, float delay) {

		yield return new WaitForSeconds (delay);
		iTween.ScaleTo (obj, new Vector3 (size.x , size.y , size.y),0.3f);
	}
	void OnTriggerExit2D (Collider2D col)
	{

	 
	

		if (col.gameObject.tag == "clone") {
			gc.TriggerPowerupCollisionAnim (col.gameObject);
			playSplosion (col);

		}
		if (col.gameObject.name == "bomb(Clone)") {
			audioSourcePowerup.PlayOneShot (bomb);

			GetComponent<SpriteRenderer> ().color = Color.red;
			ParticleSystem ps = GetComponent<ParticleSystem> ();
			if (ps != null) {

				ps.startColor = Color.red;
			}

	
			destroyNext = true;
			gc.initSpawning ();
			playSplosion (col);


		} else if (col.gameObject.name == "multiball(Clone)") {
			audioSourcePowerup.PlayOneShot (multiball);

			gc.MultiBall ();
			gc.initSpawning ();
			playSplosion (col);


		} else if (col.gameObject.name == "zzz(Clone)") {
			audioSourcePowerup.PlayOneShot (slow);

			gc.MakeBallSlower ();
			gc.initSpawning ();
			playSplosion (col);


		} else if (col.gameObject.name == "shrink(Clone)") {
			audioSourcePowerup.PlayOneShot (shrink);
			playSplosion (col);

			shrinkNext = true;
			gc.initSpawning ();
		} else if (col.gameObject.name == "expand(Clone)") {
			audioSourcePowerup.PlayOneShot (expand);
			expandNext = true;
			playSplosion (col);

		} else if (col.gameObject.name == "biggerball(Clone)") {
			audioSourcePowerup.PlayOneShot (growball);

			gc.initSpawning ();
			playSplosion (col);
			StartCoroutine (ToSize (gameObject, new Vector3 (gameObject.transform.localScale.x * 2f, gameObject.transform.localScale.y * 2f, gameObject.transform.localScale.z * 2f), 0.0f));
			StartCoroutine (ToSize (gameObject, new Vector3 (gameObject.transform.localScale.x , gameObject.transform.localScale.y , gameObject.transform.localScale.z ), 15.0f));

		
			//scale shadow here?
		 
		} else if (col.gameObject.name == "smallerball(Clone)") {
			audioSourcePowerup.PlayOneShot (slowball);
			if (ballsplosion != null)
				ballsplosion.Play ();


			StartCoroutine (ToSize (gameObject, new Vector3 (gameObject.transform.localScale.x * 0.75f, gameObject.transform.localScale.y * 0.75f, gameObject.transform.localScale.z * 0.75f), 0.0f));
			StartCoroutine (ToSize (gameObject, new Vector3 (gameObject.transform.localScale.x , gameObject.transform.localScale.y , gameObject.transform.localScale.z ), 15.0f));
			//scale shadow here?

		} else if (col.gameObject.name == "zplus(Clone)") {
			audioSourcePowerup.PlayOneShot (rotate);
			playSplosion (col);

			//	Destroy (col.gameObject);
			gc.initSpawning ();
			gc.set_zRotation (0.1f);

		} else if (col.gameObject.name == "zminus(Clone)") {
			audioSourcePowerup.PlayOneShot (rotate);

			//	Destroy (col.gameObject);
			gc.initSpawning ();
			gc.set_zRotation (-0.1f);
			playSplosion (col);

		}

	 
	}


	void OnCollisionEnter2D (Collision2D col)
	{
 
		if (Random.Range (0, 10) == 5) {
		
			ooh.Play ();
		}
		if (moves != null) {
			myRigidbody.angularVelocity = 0.0f;
			myRigidbody.AddTorque (moves.dist * 0.5f, ForceMode2D.Force);
 	
		}

		if (shrinkNext) {
			if (col.gameObject.tag == "HitArea") {
				//iTween.ScaleTo (col.gameObject, new Vector3 (col.gameObject.transform.localScale.x * 0.75f, col.gameObject.transform.localScale.y * 1.0f, col.gameObject.transform.localScale.z), 0.3f);
				GameObject target = col.transform.parent.gameObject;
				gc.ScaleToSmall (target);
					
			}
			shrinkNext = false;

		}
		if (expandNext) {
			if (col.gameObject.tag == "HitArea") {
				GameObject target = col.transform.parent.gameObject;
				gc.ScaleToBig (target);
			}
			expandNext = false;

		}

		if (col.gameObject.name == "BackGround") {
			Destroy (ballShadowVar);
			Destroy (gameObject);
			gc.LostBall ();
			 
		} else {
			if (audioSource.isPlaying == false) {
				audioSource.pitch = Random.Range (0.9f, 1.1f);
				audioSource.PlayOneShot (impact [(int)Random.Range (0.0f, impact.Length)]);

	  
				myRigidbody.angularVelocity = 0; 
		 
				float dir = Random.Range (-0.09f, 0.09f);

				magnus_direction = new Vector2 (dir, 0);
				/*
				if (col.gameObject.GetComponent<iTween> () == null) {
					//iTween.PunchScale (col.gameObject, new Vector3 (-1.0f, 1.0f, 0.0f), 0.4f);
					 
			
					iTween.MoveBy (col.gameObject, iTween.Hash (
						"amount", new Vector3 (1.0f, -1.0f, 0.0f),
						"time", 0.4
					)
					);

					iTween.MoveBy (col.gameObject, iTween.Hash (
						"amount", new Vector3 (-1.0f, 1.0f, 0.0f),
						"delay", 0.4,
						"time", 0.4
					)
					);
				}
*/
				gc.IncreaseScore ();
			}
		}
	}
}
