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
	[Tooltip("The time the endscreen will be shown")]
	protected float _endScreenTime = 4.0f;

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

	[SerializeField]
	[Tooltip("The score for 1 star")]
	protected int _firstStarScore;

	[SerializeField]
	[Tooltip("The score for 2 stars")]
	protected int _secondStartScore;

	[SerializeField]
	[Tooltip("The score for 3 stars")]
	protected int _thirdStarScore;

	[HideInInspector]
	private Camera _minigameCamera;

	public Camera MinigameCamera
	{
		get
		{
			if (_minigameCamera == null)
			{
				_minigameCamera = GetComponent<Camera>();
			}

			return _minigameCamera;
		}
	}

	protected bool _active = false;

	protected bool _ended = false;

	//The time the minigame ends
	private float _endTime;

	//The current score
	private float _score;

	//The current combo count
	private int _combo;

	private MinigameHUD _hudManager;

	public LayerMask MinigameLayers
	{
		get { return _layer; }
	}

	protected virtual void Start()
	{
		if (GetComponent<Camera>() == null)
		{
			throw new MissingComponentException("Missing Camera component on Minigame Manager!");
		}

		_endTime = Time.time + _tutorialTime;

		_hudManager = FindObjectOfType<MinigameHUD>();

		if (_hudManager == null)
		{
			throw new MissingComponentException("You have no minigame HUD manager in your scene!");
		}

		_hudManager.DisplayTutorial();
	}

	protected virtual void Update()
	{
		if (!_active && !_ended)
		{
			if (_endTime <= Time.time /*|| Input.GetMouseButtonDown(0)*/)
			{
				_hudManager.HideTutorial();
				_endTime = Time.time + _playTime;
				_active = true;
			}
		}
		else if (_active && !_ended)
		{
			if (_endTime <= Time.time)
			{
				EndCombo();
				_hudManager.DisplayEndscreen(GetScore(false).ToString());
				_endTime = Time.time + _endScreenTime;
				_active = false;
				_ended = true;
			}
		}
		else if (!_active && _ended)
		{
			if (_endTime <= Time.time /*|| Input.GetMouseButtonDown(0)*/)
			{
				EndMinigame();
			}
		}
	}

	protected virtual void OnGUI()
	{
		_hudManager.UpdateValues(Mathf.Round(_endTime - Time.time).ToString(), GetScore(false).ToString(), GetComboScore(false).ToString() + " x " + GetMultiplier() + " = " + GetComboScore(true).ToString());
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
		return pUseMultiplier ? (int)(_combo * _scorePerUnit * (_combo * _multiplierPerCombo + _startMuliplier)) : (int)(_combo * _scorePerUnit);
	}

	public float GetMultiplier()
	{
		return _combo * _multiplierPerCombo + _startMuliplier;
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
		DestroyDynamicObjects();

		try
		{
			MaingameManager.Instance.EndMinigame((int)_score, starCount());
		}
		catch(NullReferenceException)
		{
			Debug.LogError("No Maingamemanager was found!");
		}
    }

	private int starCount(){
		if (_score >= _firstStarScore) {
			if (_score >= _secondStartScore) {
				if (_score >= _thirdStarScore) {
					return 3;
				}
				return 2;
			} 
			return 1;
		}

		return 0;
	}

	protected virtual void DestroyDynamicObjects() { }

}
