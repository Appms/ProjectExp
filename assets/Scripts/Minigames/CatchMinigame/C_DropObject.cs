using UnityEngine;
using System.Collections;

public abstract class C_DropObject : MonoBehaviour
{
	protected CatchMinigame _manager;
	protected float _speed;

	protected bool _catched;
	protected bool _droped;

	private float _lifeTime;
	private float _destroyTime;


	protected virtual void Start()
	{
		if (_destroyTime == 0.0f)
		{
			_destroyTime = Mathf.Infinity;
		}
	}

	protected virtual void Update()
	{
		if (_destroyTime <= Time.time)
		{
			_manager.DestroyObject(this.gameObject, true);

			//TODO Maybe only whem no dump is used?
			_manager.EndCombo();
		}
	}

	public void SetValues(CatchMinigame pManager, float pSpeed, float pLifetime)
	{
		_manager = pManager;
		_speed = pSpeed;

		_lifeTime = pLifetime;
		_destroyTime = Time.time + pLifetime;
	}

	public bool CheckCatchable()
	{
		return !_droped && !_catched;
	}

	public virtual void Catch()
	{
		_catched = true;
		_destroyTime = Mathf.Infinity;
		transform.rotation = Quaternion.identity;
	}

	public virtual void Drop()
	{
		_droped = true;
		_destroyTime = Time.time + _lifeTime;
	}

	public virtual void Dump()
	{

	}
}
