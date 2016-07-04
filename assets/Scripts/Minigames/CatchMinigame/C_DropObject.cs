using UnityEngine;
using System.Collections;

public abstract class C_DropObject : MonoBehaviour {
	protected CatchMinigame _manager;
	protected float _speed;

	protected bool _catched;
	protected bool _droped;

	private float _lifeTime;
	private float _destroyTime;

	protected virtual void Start () {
		if (_destroyTime == 0.0f) {
			_destroyTime = Mathf.Infinity;
		}
	}

	protected virtual void Update () {
		if (_destroyTime <= Time.time) {
			_manager.DestroyObject(this.gameObject, true);

			if (FindObjectOfType<C_DumpObject>() == null) {
				_manager.EndCombo();
			}
		}
	}

	/// <summary>
	/// Set Values of this DropObject
	/// </summary>
	/// <param name="pManager">refrence to the Minigame manager</param>
	/// <param name="pSpeed">movement speed</param>
	/// <param name="pLifetime">lifetime of this object</param>
	/// <param name="pDropped">Determines if the plate can be catched</param>
	public void SetValues (CatchMinigame pManager, float pSpeed, float pLifetime, bool pDropped = false) {
		_droped = pDropped;

		_manager = pManager;
		_speed = pSpeed;

		_lifeTime = pLifetime;
		_destroyTime = Time.time + pLifetime;
	}

	/// <summary>
	/// Returns if the plate is catchable
	/// </summary>
	/// <returns>if the plate can be catched</returns>
	public bool CheckCatchable () {
		return !_droped && !_catched;
	}

	/// <summary>
	/// Gets called when this instacne gets catched
	/// </summary>
	public virtual void Catch () {
		_catched = true;
		_destroyTime = Mathf.Infinity;
		transform.rotation = Quaternion.identity;
	}

	/// <summary>
	/// Gets called if the plate gets dropped
	/// </summary>
	public virtual void Drop () {
		_droped = true;
		_destroyTime = Time.time + _lifeTime;
	}

	/// <summary>
	/// Gets called when this instance gets dumped
	/// </summary>
	public virtual void Dump () {

	}
}
