using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MinigameHUD : MonoBehaviour
{
	[SerializeField]
	private RectTransform _tutorialImageContainer;

	[SerializeField]
	private RectTransform _infoContainer;

	[SerializeField]
	private Text _timeText;

	[SerializeField]
	private Text _scoreText;

	[SerializeField]
	private Text _comboText;

	[SerializeField]
	private RectTransform _endScreenContainer;

	[SerializeField]
	private RectTransform _starContainer;

	[SerializeField]
	private Text _endScreenScoreText;

	private float _displayTime;
	private float _totalTime;

	private int _displayScore;
	private int _totalScore;

	private AbstractMinigame _manager;

	public void DisplayTutorial()
	{
		_tutorialImageContainer.gameObject.SetActive(true);
		_infoContainer.gameObject.SetActive(false);
		_endScreenContainer.gameObject.SetActive(false);
	}

	public void HideTutorial()
	{
		_tutorialImageContainer.gameObject.SetActive(false);
		_infoContainer.gameObject.SetActive(true);
	}

	public void UpdateValues(string pTime, string pScore, string pCombo, string pComboScore)
	{
		_timeText.text = pTime;
		_scoreText.text = pScore + " + " + pComboScore;
		_comboText.text = pCombo;
	}

	public void DisplayEndscreen(int pScore, float pDisplayTime)
	{
		//_endScreenScoreText.text = pScore;
		_totalScore = pScore;
		_totalTime = pDisplayTime - 2.0f;

		_infoContainer.gameObject.SetActive(false);
		_endScreenContainer.gameObject.SetActive(true);
	}

	private void Start()
	{
		_manager = FindObjectOfType<AbstractMinigame>();
	}

	private void Update()
	{
		if (_endScreenContainer.gameObject.activeSelf)
		{
			_displayTime += Time.deltaTime;
			_displayScore = Mathf.Min((int)(_totalScore * (_displayTime / _totalTime)), _totalScore);
			_endScreenScoreText.text = _displayScore.ToString();

			if (_manager.GetStarCount(_displayScore) > 0)
			{
				_starContainer.GetChild(_manager.GetStarCount(_displayScore) - 1).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
			}
		}
	}
}
