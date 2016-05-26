using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameManager : MonoBehaviour {
	private int _score;
	private string _currentMinigame;
	private PlayerWalk _player;

	[SerializeField]
	private AnimationCurve _animCurve = new AnimationCurve(new Keyframe(1, 3), new Keyframe(99, 20));

	[SerializeField] private float _helpTime = 5.0f;
	[SerializeField] private float _exitMessageTime = 5.0f;
	[SerializeField] private float _exitTime= 5.0f;
	private int _state = 0;
	private float _timer = 0.0f;

	[SerializeField] private GameObject[] _minigames;
	private int _finishedMinigamesInTier = 0;
	private int _currentMinigameTier = 0;

	[SerializeField] private GameObject[] _doors;
	[SerializeField] private GameObject cam;

	int specificDoor = 0;

	private void Start()
	{
		_player = FindObjectOfType<PlayerWalk>();
		cam.SetActive (false);
		ManageMinigameTier();
	}

	private void Update()
	{
		if (!Input.GetMouseButton(0))
		{
			switch (_state)
			{
				case 0:
					_timer = Time.time + _helpTime;
					_state = 1;
					break;

				case 1:
					if (Time.time >= _timer)
					{
						_state = 2;
						_timer = Time.time + _exitMessageTime;
						Debug.Log("Display help message");
					}
					break;

				case 2:
					if (Time.time >= _timer)
					{
						_state = 3;
						_timer = Time.time + _exitTime;
						Debug.Log("Display exit message");
					}
					break;

				case 3:
					if (Time.time >= _timer)
					{
						Debug.Log("Exit that darn game");
					}
					break;
			}
		}
		else
		{
			_state = 0;
		}
	}

	private void ManageMinigameTier()
	{
		if (_finishedMinigamesInTier == _minigames[_currentMinigameTier].transform.childCount)
		{
			_currentMinigameTier++;
			_finishedMinigamesInTier = 0;

			MinigameStarter[] starter = _minigames[_currentMinigameTier].GetComponentsInChildren<MinigameStarter>();

			foreach (MinigameStarter start in starter)
			{
				start.Startable = true;
			}
		}
	}

	public void StartMinigame(string pSceneName)
	{
		_player.CanWalk = false;
		//Camera.main.GetComponent<CameraMove>().IsActive = false;
		_currentMinigame = pSceneName;
		UnityEngine.SceneManagement.SceneManager.LoadScene(pSceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
	}

	public void FinishMinigame(int pScore)
	{
		//Camera.main.GetComponent<CameraMove>().IsActive = true;
		//StartCoroutine(LookAtDoor(_doors[specificDoor]));
		UnityEngine.SceneManagement.SceneManager.UnloadScene(_currentMinigame);
		_currentMinigame = "";
		_player.CanWalk = true;
		_score += pScore;
		//_finishedMinigamesInTier++;
		//specificDoor++;
		//ManageMinigameTier();
	}

    /*
	IEnumerator LookAtDoor(GameObject door)
	{
		yield return new WaitForSeconds (1);
		cam.SetActive (true);
		Camera.main.GetComponent<CameraMove>().IsActive = false;
		cam.transform.position = new Vector3 (door.transform.position.x, 5, door.transform.position.z);
		cam.transform.LookAt (door.transform.position);
		yield return new WaitForSeconds(1);
		door.SetActive(false);
		yield return new WaitForSeconds (2);
		cam.SetActive (false);
		Camera.main.GetComponent<CameraMove>().IsActive = true;
		_player.CanWalk = true;
	}
    */
}
