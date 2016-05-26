using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {
	[SerializeField] private Vector2 _cornerDistancePercent;
	[SerializeField] private float _moveSpeed;

	private bool _active = true;
	private bool _isMoving;

	private void Update()
	{
		_isMoving = false;

		if (_active && Input.GetMouseButton(0))
		{
			//Debug.Log(_cornerDistancePercent.x * Screen.width);
			if (Input.mousePosition.x < _cornerDistancePercent.x * Screen.width)
			{
				transform.Translate(new Vector3(-_moveSpeed * Time.deltaTime, 0.0f, 0.0f));
				_isMoving = true;
			}

			if (Input.mousePosition.x > Screen.width - _cornerDistancePercent.x * Screen.width)
			{
				transform.Translate(new Vector3(_moveSpeed * Time.deltaTime, 0.0f, 0.0f));
				_isMoving = true;
			}

			if (Input.mousePosition.y < _cornerDistancePercent.y * Screen.height)
			{
				transform.Translate(new Vector3(0.0f, 0.0f, -_moveSpeed * Time.deltaTime), Space.World);
				_isMoving = true;
			}

			if (Input.mousePosition.y > Screen.height - _cornerDistancePercent.y * Screen.height)
			{
				transform.Translate(new Vector3(0.0f, 0.0f, _moveSpeed * Time.deltaTime), Space.World);
				_isMoving = true;
			}
		}
	}

	public bool IsMoving
	{
		get { return _isMoving; }
	}

	public bool IsActive
	{
		set { _active = value; }
	}
}
