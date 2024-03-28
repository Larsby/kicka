using UnityEngine;
using System.Collections;


public class moveRackets : MonoBehaviour
{

	public float time = 0.25f;
	public float degree = 0.125f;
	Vector2 delta = Vector3.zero;
	Vector2 lastPos = Vector3.zero;
	bool wasLifted = true;
	private bool useTouch;
	public float dist = 0;
	public Vector2 touchVect;
	public Vector2 lastTouch;
	float speed = 5.0f;
	private float previousRotation;
	public float rotationDiff;
	private float prevRotation;
	private float animpos = 0;
	private GameControl game;
	// Use this for initialization
	void Start ()
	{
		useTouch = false;

		GameObject obj = GameObject.FindGameObjectWithTag ("GameControl");	
		game = obj.GetComponent<GameControl> ();

		animpos = 0;
		#if UNITY_TVOS
		useTouch = true;
		speed = 1.2f;


		#endif
		#if UNITY_IOS
		useTouch = true;
		#endif
		#if UNITY_ANDROID
		useTouch = true;
		#endif
		#if UNITY_EDITOR
		useTouch = false;
		#endif

		if (SystemInfo.deviceModel.Contains ("Amazon AFT")) {
			useTouch = false;
		}
	 
	}

	void RotateGameObject (float degree)
	{
		iTween.RotateBy (gameObject, iTween.Hash ("z", degree, "easeType", "spring", "time", time));
	}

	int GetRotationDirection(Vector2 position, Vector2 lastPosition)
	{
		int result = -1;
		float a = lastPosition.x - position.x;
		float b = lastPosition.y - position.y;

		float d = a;
		float e = b;
		if (d < 0) {
			d = d * -1;
		}
		if (e < 0) {
			e = e * -1;
		}
		if (e > d) {
			a = b;
		}
		if (a < 0) {

			result = 1;
		}
		if (a > 0) {
			result = 0;

		}
		return result;
	}
		

	int Rotate (Vector2 position, Vector2 lastPosition)
	{
		int result = -1;
		delta = position - lastPosition;
		lastPos = touchVect;

		float h = 0;
		float v = 0;


		h = Input.GetAxisRaw ("Horizontal");
		v = Input.GetAxisRaw ("Vertical");
		
		h += delta.x / 20.0f;
		v += delta.y / 20.0f;
		result = GetRotationDirection (position, lastPosition);
		float zRotation = transform.rotation.z;
		transform.Rotate (new Vector3 (0f, 0f, (h + v) * speed));
		rotationDiff  = transform.rotation.z - zRotation;
		if (rotationDiff < 0) {
			rotationDiff *= -1;
		}
		rotationDiff = rotationDiff * 100;

		return result;
	}

	int Move ()
	{  
		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch (0);

			touchVect = touch.position;
			if (touch.phase == TouchPhase.Began) {
				lastPos = touch.position;
			}
			if (touch.phase == TouchPhase.Moved && wasLifted == false) {
				return Rotate (touchVect, lastPos);

			}
			if (touch.phase == TouchPhase.Ended) {
				wasLifted = true;
			}
		}
		return -1;
	}
	void AnimateChildren(string trigger, int id, float speed) {
		//Debug.Log ("Triggger" + trigger);
		foreach (Transform child in transform) {
			GameObject target = child.GetChild (0).gameObject;
			Animator animator = target.GetComponent<Animator> ();
			if (!animator == game.kicking) {
				AnimatorStateInfo asi = animator.GetCurrentAnimatorStateInfo (0);
				/*
			AnimatorClipInfo [] aci = animator.GetCurrentAnimatorClipInfo (0);
			AnimatorTransitionInfo ati =  animator.GetAnimatorTransitionInfo (0);
			Animation anim = target.GetComponent<Animation> ();
			AnimationClip [] clips = animator.runtimeAnimatorController.animationClips;
			*/
				if (asi.IsName ("Pass")) {
					float t = animator.GetCurrentAnimatorStateInfo (0).normalizedTime ;
					if (t < 0.6f)
						return;
					Debug.Log ("" + t);
				
				}
				if (asi.IsName ("StrafeLeft") && id == 0) {
					//	Debug.Log ("Yeah!!");
					animator.speed = 0;
			
				} else if (asi.IsName ("StrafeRight") && id == 1) {
					//	Debug.Log ("yeah2!");
					animator.speed = 0;

			
				} else {
					if (id > -1) {
						animator.ResetTrigger ("Idle");
					} else {
						animator.speed = 1.0f;
						animator.SetTrigger (trigger);
						break;
					}
					animator.speed = 1.0f;
					//animator.SetTrigger (trigger);


				}
				animpos -= 0.01f;
				animpos %= 1f;
				animator.Play (trigger, -1, animpos);
				//	Debug.Log ("!!!!" + asi.fullPathHash);


			}
		}
	}
	void Update ()
	{	
		int dir = -1;
		if (Input.GetButtonUp ("Fire1") || Input.GetButtonUp ("Fire2") || Input.GetButtonUp ("Fire3") || Input.GetMouseButtonUp (0)) {
			wasLifted = true; 
		} else {
			wasLifted = false;
		}


		if (useTouch) {
			dir =	Move ();

		}
		if (!useTouch) {
			dist = Vector2.Distance (Input.mousePosition, lastPos);

			if (wasLifted) {
				lastPos = new  Vector2 (Input.mousePosition.x, Input.mousePosition.y);
				wasLifted = false;

			}  
			dir = GetRotationDirection (Input.mousePosition, lastPos);
			delta = new Vector2 (Input.mousePosition.x, Input.mousePosition.y) - lastPos;
			lastPos = new  Vector2 (Input.mousePosition.x, Input.mousePosition.y);

			float h = 0;
			float v = 0;
			float factor = 20.0f;
	

			h = Input.GetAxisRaw ("Horizontal");
			v = Input.GetAxisRaw ("Vertical");
		
			h += delta.x / factor;
			v += delta.y / factor;
			float zRotation = transform.rotation.z;

			transform.Rotate (new Vector3 (0f, 0f, (h + v) * speed));
			rotationDiff  = transform.rotation.z - zRotation;
			if (rotationDiff < 0) {
				rotationDiff *= -1;
			}
			rotationDiff = rotationDiff * 100;
		}
		float rotationSpeed = 0.5f;
		if (rotationDiff > 0.8f) {
			rotationSpeed = 1.0f;
		}
		if (dir == 0) {
			AnimateChildren ("StrafeLeft",0,rotationSpeed);

		}
		if (dir == 1) {
			AnimateChildren ("StrafeRight",1,rotationSpeed);
		
		}
		if (dir == -1) {
			AnimateChildren ("Idle",-1,0);
		}
		}




}

