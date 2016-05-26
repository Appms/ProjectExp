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

	protected override void InitTrue()
	{
		transform.position = new Vector3(0.0f, 0.0f, 0.0f);
	}

	protected override void InitFalse()
	{
		transform.position = new Vector3(0.0f, 0.75f, 0.0f);
	}

	protected override void TurnTrue()
	{
		transform.position = new Vector3(0.0f, 0.75f, 0.0f);
		_animationPlaying = true;
	}

	protected override void TurnFalse()
	{
		transform.position = new Vector3(0.0f, 0.0f, 0.0f);
		_animationPlaying = true;
	}

	private void Update()
	{
		if (_animationPlaying)
		{
			_currentLerpTime += Time.deltaTime;
			if (_currentLerpTime > _lerpTime)
			{
				_currentLerpTime = _lerpTime;
				_animationPlaying = false;
			}

			float perc = _currentLerpTime / _lerpTime;
			transform.position = State ? Vector3.Lerp(_closedPosition, _openPosition, perc) : Vector3.Lerp(_openPosition, _closedPosition, perc);
		}
	}
}
