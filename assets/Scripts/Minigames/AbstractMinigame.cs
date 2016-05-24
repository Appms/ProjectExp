using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public abstract class AbstractMinigame : MonoBehaviour
{
	[SerializeField]
	[Tooltip("The time the tutorial image will be shown")]
	protected float _tutorialTime = 4.0f;

	[SerializeField]
	[Tooltip("The score you get per successfull action")]
	protected int _scorePerUnit = 10;

	[SerializeField]
	[Tooltip("The value of wich the combo multiplier increases per successful action")]
	[Range(0.0f, 1.0f)]
	protected float _multiplierPerCombo = 0.1f;

	[SerializeField]
	[Tooltip("The value the multiplier starts at")]
	[Range(0.0f, 1.0f)]
	protected float _startMuliplier = 1.0f;

	[SerializeField]
	[Tooltip("The time the minigame will be played")]
	[Range(1.0f, 30.0f)]
	protected float _playTime = 10.0f;

	[SerializeField]
	[Tooltip("The layer Raycasts will hit")]
	protected LayerMask _layer;

	[HideInInspector]
	public Camera MinigameCamera;

	protected bool _active = false;

	//The time the minigame ends
	private float _endTime;

	//The current score
	private float _score;

	//The current combo count
	private int _combo;

	protected virtual void Start()
	{
		if (GetComponent<Camera>() == null)
		{
			throw new MissingComponentException("Missing Camera component");
		}

		_endTime = Time.time + _tutorialTime;

		MinigameCamera = GetComponent<Camera>();
	}

	protected virtual void Update()
	{
		//TODO Replace with touch input
		if (_endTime <= Time.time || !_active && Input.GetMouseButtonDown(0))
		{
			if (_active)
			{
				//TODO Display an endscreen
				EndMinigame();
			}
			else
			{
				_active = true;
				GameObject.Find("TutorialImage").SetActive(false);
				_endTime = Time.time + _playTime;
			}
		}
	}

	protected virtual void OnDrawGUI()
	{

	}


	public bool GetActive()
	{
		return _active;
	}

	/// <summary>
	/// Calculates the current score.
	/// </summary>
	/// <param name="pIncludeCombo">Determines if the current combo score is included</param>
	/// <returns>The current score</returns>
	protected int GetScore(bool pIncludeCombo)
	{
		return pIncludeCombo ? (int)_score + GetComboScore(true) : (int)_score;
	}

	/// <summary>
	/// Calculates the current combo score
	/// </summary>
	/// <param name="pUseMultiplier">Determines if the score is muiltiplied by the combo multiplier</param>
	/// <returns>The current combo score</returns>
	public int GetComboScore(bool pUseMultiplier)
	{
		return pUseMultiplier ? (int)(_combo * _scorePerUnit * _combo * _multiplierPerCombo) : (int)(_combo * _scorePerUnit);
	}

	/// <summary>
	/// Adds to the combo counter
	/// </summary>
	/// <param name="pAmount">The amount added to the combo counter</param>
	public void AddCombo(int pAmount = 1)
	{
		_combo += pAmount;
	}

	/// <summary>
	/// Removes from the combo counter
	/// </summary>
	/// <param name="pAmount">The amount removed from the combo counter</param>
	public void RemoveCombo(int pAmount = 1)
	{
		_combo -= pAmount;
	}

	/// <summary>
	/// Sets the combo counter to zero and adds the combo score to the total score
	/// </summary>
	/// <param name="pUseMultiplier">Determines if the comco score is multiplied with the comco multiplier</param>
	public void EndCombo(bool pUseMultiplier = true)
	{
		_score += GetComboScore(pUseMultiplier);
		_combo = 0;
	}

	/// <summary>
	/// Ends the current minigame
	/// </summary>
	public void EndMinigame()
	{
		_active = false;

		EndCombo();

		DestroyDynamicObjects();

		try
		{
			MaingameManager.Instance.EndMinigame((int)_score);
		}
		catch(NullReferenceException)
		{
			Debug.LogError("No Maingamemanager was found!");
		}
    }

	protected virtual void DestroyDynamicObjects() { }

}
