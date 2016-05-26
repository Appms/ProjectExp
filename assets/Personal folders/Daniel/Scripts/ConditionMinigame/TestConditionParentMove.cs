using UnityEngine;
using System.Collections;

public class TestConditionParentMove : ConditionParent
{
	[SerializeField]
	private Vector3 _startPositionOffset;

	[SerializeField]
	private Vector3 _endPositionOffset;

	[SerializeField]
	private float _lerpTime;

	private float _currentLerpTime;

	private Vector3 _targetPosition;
	private Vector3 _startPosition;
	private bool _despawning;

	//TODO Make the animation move the wall to the left (later let it rotate around the camera)
	public override void Despawn()
	{
		base.Despawn();
		_startPosition = transform.localPosition;
		Debug.Log("D " + _startPosition);
		_targetPosition = transform.localPosition + _endPositionOffset;
		Debug.Log("D " + _targetPosition);
		_despawning = true;
		_currentLerpTime = 0.0f;
		_animationPlaying = true;
	}

	protected override void Start()
	{
		base.Start();
		_startPosition = transform.localPosition + _startPositionOffset;
		Debug.Log("S " + _startPosition);
		_targetPosition = transform.localPosition;
		Debug.Log("S " + _targetPosition);
		transform.localPosition = _startPosition;
		_animationPlaying = true;
		_currentLerpTime = 0.0f;
	}

	protected override void Update()
	{
		base.Update();

		if (_animationPlaying)
		{
			_currentLerpTime += Time.deltaTime;
			if (_currentLerpTime > _lerpTime)
			{
				_currentLerpTime = _lerpTime;
				_animationPlaying = false;
				if (_despawning)
				{
					GameObject.Destroy(this.gameObject);
				}
			}

			float perc = _currentLerpTime / _lerpTime;
			transform.localPosition = Vector3.Lerp(_startPosition, _targetPosition, perc);
		}
	}
}
