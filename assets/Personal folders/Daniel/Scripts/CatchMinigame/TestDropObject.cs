using UnityEngine;
using System.Collections;

public class TestDropObject : C_DropObject
{
	private float _rotationSpeed = 145.0f;

	protected override void Start()
	{
		base.Start();
	}

	protected override void Update()
	{
		base.Update();

		if (!_catched || _droped)
		{
			transform.Translate(0, -_speed * Time.deltaTime, 0, Space.World);
			transform.Rotate(new Vector3(1, 0, 0), _rotationSpeed * Time.deltaTime);
		}
	}

	public override void Catch()
	{
		base.Catch();
	}

	public override void Drop()
	{
		base.Drop();
	}

	public override void Dump()
	{
		_manager.DestroyObject(this.gameObject);
	}
}
