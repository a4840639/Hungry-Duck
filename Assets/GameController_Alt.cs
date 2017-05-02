using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController_Alt : MonoBehaviour {
	public int score;
	float time;
	GameObject GO;
	bool paused;
	GameObject[] foods;
	GameObject[] food;
	int index;
	bool initial;

	// Use this for initialization
	void Start () {
		food = new GameObject[8];
		GO = GameObject.Find ("Gameover");
		foods = new GameObject[5];
		for (int i = 0; i < 5; i++) {
			foods[i] = GameObject.Find ("Food" + (i + 1));
			foods [i].SetActive (false);
		}
		Reset ();
	}

	// Update is called once per frame
	void Update () {


		if (Time.time - time > 60 && !paused) {
			GO.SetActive (true);
			Text text = GameObject.Find ("TimeTxt").GetComponent<Text> ();
			text.text = "Your Score: " + score;
			paused = true;
			Time.timeScale = .0000001f;
		}



		if (paused && Input.GetAxis ("Fire2") > 0) {
			time = Time.time;
			Reset ();
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}

	}

	void Reset () {
		initial = true;
		index = 0;
		time = Time.time;
		Time.timeScale = 1;
		paused = false;
		score = 0;
		time = 0;
		GO.SetActive (false);
		newRound ();
	}

	void newRound () {
		if (!initial) {
			for (int i = 0; i < 8; i++) {
				food [i].SetActive (true);
			}
		}
		for (int i = 0; i < 5; i++) {
			foods [i].SetActive (false);
		}
		int r = Random.Range (0, 4);
		foods [r].SetActive (true);
	}

	public void eat (GameObject foo) {
		score += 100;
		food [index] = foo;
		index = (index + 1) % 8;
		if (index == 0) {
			initial = false;
			newRound();
		}
	}
}
