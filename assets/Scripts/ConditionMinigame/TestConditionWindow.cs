using UnityEngine;
using System.Collections;
using System;

public class TestConditionWindow : ConditionObject
{
	[SerializeField]
	[Tooltip("The time the window needs to move up or down")]
	private float _lerpTime;

	[SerializeField]
	[Tooltip("The open position of the window")]
	private Vector3 _openPosition;

	[SerializeField]
	[Tooltip("The closed position of the window")]
	private Vector3 _closedPosition;

	private float _currentLerpTime;

	private Vector3 _startPosition;
	private Vector3 _targetPosition;

	private float _realLerpTime;

	protected override void InitTrue()
	{
		transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
	}

	protected override void InitFalse()
	{
		transform.localPosition = new Vector3(0.0f, 0.75f, 0.0f);
	}

	protected override void TurnTrue()
	{
		_currentLerpTime = 0.0f;
		_startPosition = transform.localPosition;
		_targetPosition = _openPosition;
		_realLerpTime = Vector3.Distance(_startPosition, _targetPosition) / Vector3.Distance(_openPosition, _closedPosition) * _lerpTime;
		_animationPlaying = true;
	}

	protected override void TurnFalse()
	{
		_currentLerpTime = 0.0f;
		_startPosition = transform.localPosition;
		_targetPosition = _closedPosition;
		_realLerpTime = Vector3.Distance(_startPosition, _targetPosition) / Vector3.Distance(_openPosition, _closedPosition) * _lerpTime;
		_animationPlaying = true;
	}

	private void Update()
	{
		if (_animationPlaying)
		{
			_currentLerpTime += Time.deltaTime;
			if (_currentLerpTime > _realLerpTime)
			{
				_currentLerpTime = _realLerpTime;
				_animationPlaying = false;
			}

			float perc = _currentLerpTime / _realLerpTime;
			transform.localPosition = Vector3.Lerp(_startPosition, _targetPosition, perc);
		}
	}
}
