using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MaingameManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Camera that renders the house scene.")]
    private Camera _camera;

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

    private int _minigameUnlock;

	//Instance of this Singleton
    private static MaingameManager _instance = null;

	//The current score (playing minigames not included)
    private int _score;

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
	public static MaingameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MaingameManager>();

                if (_instance == null)
                {
                    Debug.LogError("You have no Maingame Manager in your Scene!");
                }
            }

            return _instance;
        }
    }




    private void Awake()
    {
		//TODO Have to do some research about touch input
		//Input.simulateMouseWithTouches = true;

        _arguments = new Arguments();

        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Debug.LogError("You had multiple Maingame Managers in your scene! " + this.gameObject.name + " got destroyed!");
            GameObject.Destroy(this.gameObject);
        }
    }

	private void Start()
	{
		//TODO Find out if getGameTime uses minutes or seconds
		_endTime = _arguments.getGameTime() + Time.time;
	}
	
	private void Update()
	{
		float _timer = _endTime - Time.time;
        _timer -= Time.unscaledDeltaTime;
        if (Mathf.FloorToInt(_timer % 60) < 10) _gameTimer.text = Mathf.FloorToInt(_timer / 60) + ":0" + Mathf.FloorToInt(_timer % 60);
        else _gameTimer.text = Mathf.FloorToInt(_timer / 60) + ":" + Mathf.FloorToInt(_timer % 60);

        //TODO Replace with touch Input
        if (Input.anyKey /*|| Input.GetTouch()*/)
		{
			_lastInputTime = Time.time;
		}

		float timeWithoutInput = Time.time - _lastInputTime;

		if (timeWithoutInput >= _timeToHelpMessage)
		{
			if (timeWithoutInput >= _timeToExitMessage)
			{
				if (timeWithoutInput >= _timeToExit && !_uploadedScore)
				{
					/*
					if (_currentMinigameName != "")
					{
						FindObjectOfType<AbstractMinigame>().EndMinigame();
					}
					*/

					_uploadedScore = true;
					//Application.Quit();
					//
					//StartCoroutine(uploadScore());
				}
				else
				{
					//TODO Display Exit Message
				}
			}
			else
			{
				//TODO Display Help Message
			}
		} 

		if (_endTime <= Time.time/* && !_uploadedScore*/)
		{
			_uploadedScore = true;
			//TODO StartCoroutine(uploadScore());
			UnityEngine.SceneManagement.SceneManager.LoadScene("CandyScene");
			//Application.Quit();
		}
	}
	
	private void OnDrawGUI()
	{
	
	}




    /// <summary>
    /// Starts a Minigame
    /// </summary>
    /// <param name="pName">The name of the minigame scene</param>
	public void StartMinigame(string pName, StarScript pStarScript)
    {
		_starScript = pStarScript;
        _camera.enabled = false;
        _hud.SetActive(false);
        UnityEngine.SceneManagement.SceneManager.LoadScene(pName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        _currentMinigameName = pName;
    }

    /// <summary>
    /// Ends the current Minigame
    /// </summary>
    /// <param name="pScore">The score from the Minigame</param>
	public void EndMinigame(int pScore, int pStarCount)
    {
        _camera.enabled = true;
        _hud.SetActive(true);
        UnityEngine.SceneManagement.SceneManager.UnloadScene(_currentMinigameName);
        _currentMinigameName = "";
        _score += pScore;

		_starScript.DisplayStars (pStarCount);
		_starScript = null;

        _minigames[Mathf.Clamp(++_minigameUnlock, 0, _minigames.Length - 1)].interactable = true;
        FindObjectOfType<ScoreAnimation>().UpdateScore(_score);
    }

	/// <summary>
	/// Uploads the score to the Mainframe
	/// </summary>
	/// <returns>To be specified</returns>
	private IEnumerator uploadScore()
    {
        //string full_url = _arguments.getConURL() + "insertScore.php?" + "userID=" + _arguments.getUserID() + "&gameID=" + _arguments.getGameID() + "&score=" + _score;
        //WWW post = new WWW(full_url);
        //yield return post;
        yield return null;
        Application.Quit();
    }
}
