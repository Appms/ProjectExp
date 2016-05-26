using UnityEngine;
using System.Collections;

public class TestRack : T_CatchObject
{
	[SerializeField]
	private float _lerpTime;

	[SerializeField]
	private Vector3 _targetPoint;

	private Vector3 _startPosition;

	private float _currentLerpTime;

	private bool _reachedEnd;

	private void Start()
	{
		_startPosition = transform.localPosition;
	}

	private void Update()
	{
		_currentLerpTime += Time.deltaTime;

		if (_currentLerpTime > _lerpTime)
		{
			_currentLerpTime = _lerpTime;
			_reachedEnd = true;
		}

		float perc = _currentLerpTime / _lerpTime;
		transform.localPosition = Vector3.Lerp(_startPosition, _targetPoint, perc);

		if (_reachedEnd)
		{
			Vector3 temp = _startPosition;
			_startPosition = _targetPoint;
			_targetPoint = temp;
			_currentLerpTime = 0.0f;
			_reachedEnd = false;
		}
	}

	public override void Dump()
	{
		base.Dump();
		GetComponent<Animator>().SetTrigger("SpawnObject");
	}
}
