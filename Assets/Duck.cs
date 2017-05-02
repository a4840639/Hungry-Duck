using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : MonoBehaviour {
	float speed = 3f;
	Vector3 velocity;
	float rotationSpeed = 9f;
	float dashtime = -1f;
	Vector3 accel;
	Vector3 direciton = Vector3.forward;
	CharacterController controller;
	Vector3 lastPos;
	Vector3 moveDirection;

    public AudioClip eatsound;
    public AudioClip swimsound;
    public AudioClip boostsound;
    public AudioClip bouncesound;
    public AudioClip jumpsound;
    private AudioSource source;
    private float vollowrange = .3f;
    private float volhighrange = .6f;
	float timethen;

	bool onair = false;
	shallow_wave wave_script;
	GameController game;

	// Use this for initialization
	void Start () {
		timethen = Time.time;
		controller = GetComponent<CharacterController>();
        source = GetComponent<AudioSource>(); 
		wave_script = GameObject.Find ("Water").GetComponent<shallow_wave>();
		game = GameObject.Find ("GameController").GetComponent<GameController>();
	}
     
	// Update is called once per frame
	void Update () {
		moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

		float moveMagnitude = moveDirection.magnitude;
		if (moveMagnitude > 1) {
			moveDirection.Normalize ();
			moveMagnitude = 1f;
		}
		float dash = Input.GetAxisRaw ("Fire1");
		float jump = Input.GetAxisRaw ("Fire2");


		if (onair) {
			velocity.y -= Time.deltaTime * 9.8f *speed;
		}

		if (jump > 0 && !onair) {
			velocity.y = 9.8f;
			onair = true;
            source.PlayOneShot(jumpsound, 1F);
		}

		if (dashtime > 0) {
			if (dashtime > 0.1f) {
				velocity.x = Mathf.SmoothDamp (velocity.x, velocity.x * 0.2f, ref accel.x, 0.3f);
				velocity.z = Mathf.SmoothDamp (velocity.z, velocity.z * 0.2f, ref accel.z, 0.3f);
			}
            dashtime -= Time.deltaTime;
		} else if (dash == 0) {
			velocity.x = moveDirection.x * speed;
			velocity.z = moveDirection.z * speed;
		} else if (!onair) {
			velocity.x = direciton.x * speed * 3f;
			velocity.z = direciton.z * speed * 3f;
			accel = velocity;
			dashtime = 0.4f;
            source.PlayOneShot(boostsound, 1f);
        }

		controller.Move (velocity * Time.deltaTime);
		if (moveDirection.sqrMagnitude > 0) {
			if (!onair && (moveDirection.normalized - direciton).magnitude > 0.3f && Time.time - timethen > 0.5f)
            {
                float vol = Random.Range(vollowrange, volhighrange);
                source.PlayOneShot(swimsound, vol);
				timethen = Time.time;
            }
			direciton = moveDirection.normalized;
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (moveDirection), Time.deltaTime * rotationSpeed * moveMagnitude);
		}


	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("Food"))
		{
			other.gameObject.SetActive (false);
			game.score += 100;
            source.PlayOneShot(eatsound, 1F);
        }
	}

	void OnControllerColliderHit(ControllerColliderHit other)
	{
		if (other.gameObject.CompareTag ("Wall")) {
			if (dashtime > 0) {
				dashtime = 0.4f;
				velocity.x = -0.6f * controller.velocity.x;
				velocity.z = -0.6f * controller.velocity.z;
				controller.Move (velocity * Time.deltaTime);
				accel = velocity;
				source.PlayOneShot (bouncesound, 1f);
			} else if (onair) {
//				if (Mathf.Abs(velocity.x) + Mathf.Abs(velocity.y) > 0.3f) {
//					velocity.x = direciton.x * 0.5f * speed;
//					velocity.z = direciton.z * 0.5f * speed;
//				} else if (moveDirection.sqrMagnitude < 0.1f*0.1f) {
//					velocity.x = 0.3f * velocity.x;
//					velocity.z = 0.3f * velocity.z;
//				}
//				float y = velocity.y;
//				velocity.y = 0;
//				controller.Move (velocity * Time.deltaTime);
//				velocity.y = y;
			}

        } else if (other.gameObject.CompareTag ("Plane") && onair) {
			velocity.y = 0;
			onair = false;
			wave_script.land = true;
			wave_script.duck = transform.position;
			source.PlayOneShot(swimsound, volhighrange);
		}
	}
}