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
	bool onair = false;
	shallow_wave wave_script;

	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController>();
		wave_script = GameObject.Find ("Water").GetComponent<shallow_wave>();
	}

	// Update is called once per frame
	void Update () {
		Vector3  moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
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
			print ("Jump");
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
		}

		controller.Move (velocity * Time.deltaTime);
		if (moveDirection.sqrMagnitude > 0) {
			direciton = moveDirection.normalized;
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (moveDirection), Time.deltaTime * rotationSpeed * moveMagnitude);
		}


	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("Food"))
		{
			other.gameObject.SetActive (false);
		} 


	}

	void OnControllerColliderHit(ControllerColliderHit other)
	{
		if (other.gameObject.CompareTag ("Wall") && dashtime > 0 && controller.velocity.magnitude > 0) {
			dashtime = 0.4f;
			velocity = -0.6f * velocity;
			controller.Move (velocity * Time.deltaTime);
			accel = velocity;
		} else if (other.gameObject.CompareTag ("Plane") && onair) {
			onair = false;
			wave_script.land = true;
			wave_script.duck = transform.position;
		}
	}
}