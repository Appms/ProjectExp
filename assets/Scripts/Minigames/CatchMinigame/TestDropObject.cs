using UnityEngine;
using System.Collections;

public class TestDropObject : C_DropObject {
	private float _rotationSpeed = 180.0f;

	protected override void Start () {
		base.Start();
	}

	protected override void Update () {
		base.Update();

		if (!_catched || _droped) {
			transform.Translate(0, (_droped ? -_speed * 2 : -_speed) * Time.deltaTime, 0, Space.World);
			transform.Rotate(new Vector3(1, 0, 0), _rotationSpeed * Time.deltaTime);
		}
	}

	/// <summary>
	/// Gets called when this instacne gets catched
	/// </summary>
	public override void Catch () {
		base.Catch();
	}

	/// <summary>
	/// Gets called when this instance gets dropped
	/// </summary>
	public override void Drop () {
		base.Drop();
	}

	/// <summary>
	/// Gets called when this instance gets dumped
	/// </summary>
	public override void Dump () {
		_manager.dropper.SetActive(true);
		_manager.DestroyObject(this.gameObject);
	}
}
