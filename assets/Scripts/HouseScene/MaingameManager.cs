﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MaingameManager : MonoBehaviour {
	[SerializeField]
	[Tooltip("Camera that renders the house scene.")]
	private Camera _camera;

	//[SerializeField]
	//[Tooltip("Camera's audio listener.")]
	//private AudioListener _audioListener;

	[SerializeField]
	[Tooltip("Main House Scene lights.")]
	private GameObject _lights;

	[SerializeField]
	[Tooltip("Main House Scene HUD.")]
	private GameObject _hud;

	[SerializeField]
	[Tooltip("UI text that displays the time.")]
	private Text _gameTimer;

	[SerializeField]
	[Tooltip("Determines the time until a help message is displayed.")]
	private float _timeToHelpMessage;

	[SerializeField]
	[Tooltip("Determines the time until an exit message is displayed.")]
	private float _timeToExitMessage;

	[SerializeField]
	[Tooltip("Dtermines the time until the game quits.")]
	private float _timeToExit;

	[SerializeField]
	[Tooltip("Minigames displayed in the order they are supposed to be unlocked.")]
	private Button[] _minigames;

	[SerializeField]
	private GameObject _props;

	[SerializeField]
	private GameObject _endMessage;

	private int _minigameUnlock;

	//Instance of this Singleton
	private static MaingameManager _instance = null;

	//The current score (playing minigames not included)
	//private int _score;
	private Dictionary<string, int> _score = new Dictionary<string, int>();

	//The time the game ends
	private float _endTime;

	//The name of the minigame that is currently active
	private string _currentMinigameName;

	//Instance to class of Mainframe arguments
	private Arguments _arguments;

	//The last time the player gave input
	private float _lastInputTime;

	//Safety bool to only upload score once
	private bool _uploadedScore = false;

	private StarScript _starScript;

	/// <summary>
	/// Return the current instance of the Maingame Manager
	/// </summary>
	public static MaingameManager Instance {
		get {
			if (_instance == null) {
				_instance = FindObjectOfType<MaingameManager>();

				if (_instance == null) {
					Debug.LogError("You have no Maingame Manager in your Scene!");
				}
			}

			return _instance;
		}
	}




	private void Awake () {
		Screen.SetResolution(960, 540, true);
		// Have to do some research about touch input
		//Input.simulateMouseWithTouches = true;

		_arguments = new Arguments();

		if (_instance == null) {
			_instance = this;
		} else if (_instance != this) {
			Debug.LogError("You had multiple Maingame Managers in your scene! " + this.gameObject.name + " got destroyed!");
			GameObject.Destroy(this.gameObject);
		}
	}

	private void Start () {
		//Find out if getGameTime uses minutes or seconds
		_endTime = _arguments.getGameTime() + Time.time;
	}

	private void Update () {
		float _timer = _endTime - Time.time;
		_timer -= Time.unscaledDeltaTime;
		if (Mathf.FloorToInt(_timer % 60) < 10)
			_gameTimer.text = Mathf.FloorToInt(_timer / 60) + ":0" + Mathf.FloorToInt(_timer % 60);
		else
			_gameTimer.text = Mathf.FloorToInt(_timer / 60) + ":" + Mathf.FloorToInt(_timer % 60);

		//Replace with touch Input
		if (Input.anyKey /*|| Input.GetTouch()*/) {
			_lastInputTime = Time.time;
		}

		float timeWithoutInput = Time.time - _lastInputTime;

		if (timeWithoutInput >= _timeToHelpMessage) {
			if (timeWithoutInput >= _timeToExitMessage) {
				if (timeWithoutInput >= _timeToExit && !_uploadedScore) {
					//if (_currentMinigameName != "") {
					//	FindObjectOfType<AbstractMinigame>().EndMinigame();
					//}

					_uploadedScore = true;
					//Application.Quit();
					//
					StartCoroutine(uploadScore());
				} else {
					_endMessage.SetActive(true);
				}
			} else {
				//FIX Display Help Message
			}
		} else {
			_endMessage.SetActive(false);
		}

		if (_endTime <= Time.time && _currentMinigameName == ""/* && !_uploadedScore*/) {
			_uploadedScore = true;
			StartCoroutine(uploadScore());
			UnityEngine.SceneManagement.SceneManager.LoadScene("CandyScene");
		}
	}


	/// <summary>
	/// Starts a Minigame
	/// </summary>
	/// <param name="pName">The name of the minigame scene</param>
	public void StartMinigame (string pName, StarScript pStarScript) {
		_starScript = pStarScript;
		UnityEngine.SceneManagement.SceneManager.LoadScene(pName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
		_camera.gameObject.SetActive(false);
		_hud.SetActive(false);
		_lights.SetActive(false);
		_props.SetActive(false);
		_currentMinigameName = pName;
	}

	private int getScore() {
		int result = 0;
		foreach (KeyValuePair<string, int> s in _score) {
			result += s.Value;
		}
		return result;
	}

	/// <summary>
	/// Ends the current Minigame
	/// </summary>
	/// <param name="pScore">The score from the Minigame</param>
	public void EndMinigame (int pScore, int pStarCount) {
		UnityEngine.SceneManagement.SceneManager.UnloadScene(_currentMinigameName);
		_camera.gameObject.SetActive(true);
		_hud.SetActive(true);
		_props.SetActive(true);
		_lights.SetActive(true);
		_currentMinigameName = "";

		//TODO lol raplce this
		//_score += pScore;
		if (!_score.ContainsKey(_currentMinigameName) || _score[_currentMinigameName] < pScore) {
			_score[_currentMinigameName] = pScore;
		}

		_starScript.DisplayStars(pStarCount);
		_starScript = null;

		_minigames[Mathf.Clamp(++_minigameUnlock, 0, _minigames.Length - 1)].interactable = true;
		FindObjectOfType<ScoreAnimation>().UpdateScore(getScore());
	}

	/// <summary>
	/// Uploads the score to the Mainframe
	/// </summary>
	/// <returns>To be specified</returns>
	private IEnumerator uploadScore () {
		string full_url = _arguments.getConURL() + "insertScore.php?" + "userID=" + _arguments.getUserID() + "&gameID=" + _arguments.getGameID() + "&score=" + _score;
		WWW post = new WWW(full_url);
		yield return post;
		yield return null;
		Application.Quit();
	}
}
