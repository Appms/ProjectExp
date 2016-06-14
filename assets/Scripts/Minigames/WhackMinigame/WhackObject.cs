using UnityEngine;
using System.Collections;

public abstract class WhackObject : MonoBehaviour
{
	private bool _state = false;

	private float _switchTime = Mathf.Infinity;

	private float _initialTime = 0.0f;

	public void SwitchState()
	{
		if (_state)
		{
			TurnOff();
		}
		else
		{
			TurnOn();
		}

		_state = !_state;
	}

	public bool State
	{
		get { return _state; }
	}

	public float SwitchTime
	{
		get { return _switchTime; }
		set
		{
			_switchTime = value;
			_initialTime = value - Time.time;
		}
	}

	public float HitTime
	{
		get
		{
			return _initialTime - (_switchTime - Time.time);
		}
	}

	public float LeftTime
	{
		get
		{
			return _switchTime - Time.time;
		}
	}

	public void Interact()
	{
		if (_state)
		{
			_state = !_state;
			InteractRight();
		}
		else
		{
			InteractWrong();
		}
	}

	protected abstract void InteractRight();
	protected abstract void InteractWrong();
	protected abstract void TurnOn();
	protected abstract void TurnOff();
}
