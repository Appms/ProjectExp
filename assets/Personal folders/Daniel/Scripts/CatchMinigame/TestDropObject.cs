using UnityEngine;
using System.Collections;

public class TestDropObject : DropObject
{
	private float _rotationSpeed = 15.0f;

	protected override void Start()
	{
		base.Start();
	}

	protected override void Update()
	{
		base.Update();

		if (!_catched)
		{
			transform.Translate(0, -_speed * Time.deltaTime, 0, Space.World);
			transform.Rotate(new Vector3(1, 0, 0), _rotationSpeed * Time.deltaTime);
		}
	}
}
