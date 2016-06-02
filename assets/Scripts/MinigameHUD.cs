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
	private Text _endScreenScoreText;

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

	public void UpdateValues(string pTime, string pScore, string pCombo)
	{
		_timeText.text = pTime;
		_scoreText.text = pScore;
		_comboText.text = pCombo;
	}

	public void DisplayEndscreen()
	{
		_infoContainer.gameObject.SetActive(false);
		_endScreenContainer.gameObject.SetActive(true);
	}
}
