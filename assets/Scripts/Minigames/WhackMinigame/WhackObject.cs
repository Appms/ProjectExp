using UnityEngine;
using System.Collections;

public abstract class WhackObject : MonoBehaviour {
	private bool _state = false;

	private float _switchTime = Mathf.Infinity;

	private float _initialTime = 0.0f;

	/// <summary>
	/// Switches the state of given whackObject
	/// </summary>
	public void SwitchState () {
		if (_state) {
			TurnOff();
		} else {
			TurnOn();
		}

		_state = !_state;
	}

	/// <summary>
	/// Return the current state
	/// </summary>
	public bool State {
		get { return _state; }
	}

	/// <summary>
	/// Gets or sets the time for next state switch
	/// </summary>
	public float SwitchTime {
		get { return _switchTime; }
		set {
			_switchTime = value;
			_initialTime = value - Time.time;
		}
	}

	/// <summary>
	/// Returns the time needed to click on this whack objects
	/// </summary>
	public float HitTime {
		get {
			return _initialTime - (_switchTime - Time.time);
		}
	}

	/// <summary>
	/// Return the time remaining until the state switch
	/// </summary>
	public float LeftTime {
		get {
			return _switchTime - Time.time;
		}
	}

	/// <summary>
	/// Switches the state when the player interacts and playes the according animation
	/// </summary>
	public void Interact () {
		if (_state) {
			_state = !_state;
			InteractRight();
		} else {
			InteractWrong();
		}
	}

	protected abstract void InteractRight ();
	protected abstract void InteractWrong ();
	protected abstract void TurnOn ();
	protected abstract void TurnOff ();
}
