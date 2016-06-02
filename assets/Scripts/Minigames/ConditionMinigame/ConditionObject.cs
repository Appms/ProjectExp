using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Collider))]
public abstract class ConditionObject : MonoBehaviour
{
	[SerializeField]
	private bool _changeable;

	private bool _state;

	protected bool _animationPlaying;

	protected ConditionMinigame _manager;

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

	protected virtual void Start()
	{
		_manager = FindObjectOfType<ConditionMinigame>();

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

	protected virtual void OnMouseDown()
	{
		if (_changeable && _manager.GetActive())
		{
			SwitchState();
			_manager.Evaluate();
		}
	}
}
