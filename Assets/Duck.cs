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

	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController>();
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

		if (dashtime > 0) {
			if (dashtime > 0.1f) {
				velocity = Vector3.SmoothDamp (velocity, velocity * 0.2f, ref accel, 0.3f);
			}
			dashtime -= Time.deltaTime;
		} else if (dash == 0) {
			velocity = moveDirection * Time.deltaTime * speed;
		}
		else {
			velocity = direciton * Time.deltaTime * speed * 3f;
			accel = velocity * 0.9f - velocity;
			dashtime = 0.4f;
		}
			
		controller.Move (velocity);
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

	void OnControllerColliderHit()
	{
		if (dashtime > 0 && controller.velocity.magnitude > 0) {
			dashtime = 0.4f;
			velocity = -0.6f * velocity;
			controller.Move (velocity);
			accel = velocity * 0.9f - velocity;
		}
	}
}
