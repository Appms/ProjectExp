using UnityEngine;
using System.Collections;
using System;

public abstract class ConditionObject : MonoBehaviour
{
	[SerializeField]
	[Tooltip("Determines if the object is switched with a tap")]
	private bool _changeable;

	private bool _state;

	protected bool _animationPlaying;

	public bool State
	{
		get { return _state; }
	}

	public bool AnimationPlaying
	{
		get { return _animationPlaying; }
	}

	public void SwitchState()
	{
		if (_changeable)
		{ 
			if (_state)
			{
				TurnTrue();
			}
			else
			{
				TurnFalse();
			}

			_state = !_state;
		}
	}

	protected virtual void Start()
	{
		_state = Convert.ToBoolean(UnityEngine.Random.Range(0, 2));

		if (_state)
		{
			InitTrue();
		}
		else
		{
			InitFalse();
		}
	}

	protected abstract void InitTrue();
	protected abstract void InitFalse();

	protected virtual void TurnTrue() { }
	protected virtual void TurnFalse() { }
}
