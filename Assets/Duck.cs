using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : MonoBehaviour {
	float speed = 3f;
	float rotationSpeed = 9f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		CharacterController controller = GetComponent<CharacterController>();
		Vector3  moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		float moveMagnitude = moveDirection.magnitude;
		if (moveMagnitude > 1) {
			moveDirection.Normalize ();
			moveMagnitude = 1;
		}
		controller.Move (moveDirection * Time.deltaTime * speed);
		if (moveDirection.sqrMagnitude > 0) {
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (moveDirection), Time.deltaTime * rotationSpeed * moveMagnitude);
		}
	}
}
