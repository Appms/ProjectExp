using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class ConditionMinigame : AbstractMinigame
{
	[SerializeField]
	[Tooltip("The prefab that is shown to the player")]
	private GameObject _conditionPrefab;

	[SerializeField]
	[Tooltip("The amount of time action feedback is shown")]
	private float _showFeedbackTime;

	//TODO Maybe find a way to do this more elegant
	private Image _rightFeedback;
	private Image _wrongFeedback;

	private GameObject _currentConditionPrefab;
	private List<ConditionObject> _currentConditionObjects;
	private float _feedbackEndTime;
	private bool _evaluated;

	protected override void Start()
	{
		base.Start();

		_feedbackEndTime = Mathf.Infinity;

		_rightFeedback = GameObject.Find("RightFeedback").GetComponent<Image>();
		_wrongFeedback = GameObject.Find("WrongFeedback").GetComponent<Image>();

		_rightFeedback.gameObject.SetActive(false);
		_wrongFeedback.gameObject.SetActive(false);

		newElement();
	}

	protected override void Update()
	{
		if (_active)
		{
			if (!_evaluated)
			{
				//TODO Only Execute this part when all parent are finished animating
				if (Input.GetMouseButtonDown(0))
				{
					foreach (ConditionObject co in _currentConditionObjects)
					{
						co.SwitchState();
					}

					evaluate();
					//newElement();
				}
				else if (Input.GetMouseButtonDown(1))
				{
					//TODO Execute newElement only if all objects are finished animating
					evaluate();
					//newElement();
				}

				if (_feedbackEndTime <= Time.time)
				{
					_rightFeedback.gameObject.SetActive(false);
					_wrongFeedback.gameObject.SetActive(false);
					_feedbackEndTime = Mathf.Infinity;
				}
			}
			else if(objectAnimationsFinished())
			{
				newElement();

				if (parentAnimationsFinished())
				{
					_evaluated = false;
				}
			}
		}

		base.Update();
	}

	private void evaluate()
	{
		_evaluated = true;

		bool result = _currentConditionObjects.ToArray().All(x => x.State == true);

		if (!result)
		{
			result = _currentConditionObjects.ToArray().All(x => x.State == false);
		}

		//TODO Show animation indication if right or wrong
		if (result)
		{
			_rightFeedback.gameObject.SetActive(true);
			_feedbackEndTime = Time.time + _showFeedbackTime;
			AddCombo();
		}
		else
		{
			_wrongFeedback.gameObject.SetActive(true);
			_feedbackEndTime = Time.time + _showFeedbackTime;
			EndCombo();
		}
	}

	private bool objectAnimationsFinished()
	{
		return FindObjectsOfType<ConditionObject>().All(x => x.AnimationPlaying == false);
	}

	private bool parentAnimationsFinished()
	{
		return FindObjectsOfType<ConditionParent>().All(x => x.AnimationPlaying == false);
	}

	public void newElement()
	{
		Debug.Log("New Element initiated");

		if (_currentConditionPrefab != null)
		{
			_currentConditionPrefab.GetComponent<ConditionParent>().Despawn();
		}

		_evaluated = false;
		_currentConditionPrefab = (GameObject)GameObject.Instantiate(_conditionPrefab, Vector3.zero, Quaternion.identity);
		_currentConditionObjects = new List<ConditionObject>(_currentConditionPrefab.GetComponentsInChildren<ConditionObject>());
	}

	public void RightButtonPressed()
	{
		Debug.Log("Right Button");
	}

	public void WrongButtonPressed()
	{
		Debug.Log("Wrong Button");
	}
}
