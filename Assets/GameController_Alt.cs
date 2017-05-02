using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
	public int score;
	float time;
	GameObject GO;
	bool paused;
	GameObject[] foods;

	// Use this for initialization
	void Start () {
		GO = GameObject.Find ("Gameover");
		Reset ();
	}

	// Update is called once per frame
	void Update () {


		if (Time.time - time > 60) {
			GO.SetActive (true);
			Text text = GameObject.Find ("TimeTxt").GetComponent<Text> ();
			text.text = "Your Score: " + score;
			paused = true;
			Time.timeScale = .0000001f;
		}



		if (paused && Input.GetAxis ("Fire2") > 0) {
			Reset ();
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}

	}

	void Reset () {
		time = Time.time;
		Time.timeScale = 1;
		paused = false;
		score = 0;
		time = 0;
		GO.SetActive (false);


	}
}
