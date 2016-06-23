using UnityEngine;
using System.Collections.Generic;

public class TestConditionParentMove : ConditionParent
{
	[SerializeField]
	private Vector3 _startPositionOffset;

	[SerializeField]
	private Vector3 _endPositionOffset;

	[SerializeField]
	private float _lerpTime;

	[SerializeField]
	private GameObject _thermometer;

	private float _currentLerpTime;

	private Vector3 _targetPosition;
	private Vector3 _startPosition;
	private bool _despawning;

	//TODO Make the animation move the wall to the left (later let it rotate around the camera)
	public override void Despawn()
	{
		base.Despawn();
		_startPosition = transform.localPosition;
		_targetPosition = transform.localPosition + _endPositionOffset;
		_despawning = true;
		_currentLerpTime = 0.0f;
		_animationPlaying = true;
	}

	protected override void Start()
	{
		base.Start();

		if (FindObjectsOfType<TestConditionParentMove>().Length > 1)
		{
			_startPosition = transform.localPosition + _startPositionOffset;
			_targetPosition = transform.localPosition;
			transform.localPosition = _startPosition;
			_animationPlaying = true;
		}

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

		switch (CalcTemperature())
		{
			case 2:
				_thermometer.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
				break;

			case 1:
				_thermometer.GetComponent<Renderer>().material.SetColor("_Color", Color.red / 2);
				break;

			case 0:
				_thermometer.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
				break;

			case -1:
				_thermometer.GetComponent<Renderer>().material.SetColor("_Color", Color.blue / 2);
				break;

			case -2:
				_thermometer.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
				break;
		}
	}

	private int CalcTemperature()
	{
		int _temperature = 0;

		List<ConditionObject> allCos = new List<ConditionObject>(transform.GetComponentsInChildren<ConditionObject>());
		ConditionObject nonChangeableCo = allCos.Find(x => x.Changeable == false);
		List<ConditionObject> changeableCos = allCos.FindAll(x => x.Changeable == true);

		if (nonChangeableCo.State)
		{
			_temperature = -2;
		}
		else
		{
			_temperature = 2;
		}

		foreach (ConditionObject co in changeableCos)
		{
			if (nonChangeableCo.State)
			{
				if (co.State)
				{
					_temperature++;
				}
			}
			else
			{
				if (!co.State)
				{
					_temperature--;
				}
			}
		}

		return _temperature;
	}
}
