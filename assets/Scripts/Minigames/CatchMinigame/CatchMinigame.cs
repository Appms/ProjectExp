using UnityEngine;
using System.Collections.Generic;
using System;

public class CatchMinigame : AbstractMinigame
{
	[SerializeField]
	[Tooltip("The prefab for the drop object.")]
	private GameObject _dropObjectPrefab;

	[SerializeField]
	[Tooltip("The minimum speed an object drops with")]
	private float _minDropSpeed;

	[SerializeField]
	[Tooltip("The maximum speed an object drops with")]
	private float _maxDropSpeed;

	[SerializeField]
	[Tooltip("First point where the objects can spawn")]
	private Vector3 _minDropPosition;

	[SerializeField]
	[Tooltip("Seconds point where the objects can spawn")]
	private Vector3 _maxDropPosition;

	[SerializeField]
	[Tooltip("Minimum delay between plate spawn")]
	private float _minSpawnTime;

	[SerializeField]
	[Tooltip("Maximum delay between plate spawn")]
	private float _maxSpawnTime;

	[SerializeField]
	[Tooltip("The time until a objects gets destroyed")]
	private float _objectLifetime;

	[SerializeField]
	[Tooltip("The speed the Catchobject follows the finger")]
	private float _catchObjectFollowSpeed;

	[SerializeField]
	[Tooltip("Determines the CatchObjectSpeed that is required for a DropObject to drop")]
	private AnimationCurve _plateFallThreshold = new AnimationCurve(new Keyframe(0, 50), new Keyframe(40, 0));

	private float _nextSpawnTime;
	private List<GameObject> _dropObjects;
	private bool _usesDumpObject;

	protected override void Start()
	{
		base.Start();

		_dropObjects = new List<GameObject>();

		_usesDumpObject = FindObjectOfType<C_DumpObject>() != null;

		try
		{
			FindObjectOfType<C_CatchObject>().SetValues(this, _catchObjectFollowSpeed, _usesDumpObject);
		}
		catch (NullReferenceException)
		{
			Debug.LogError("There is no CatchObject in your scene!");
		}
	}

	protected override void Update()
	{
		if (_active)
		{
			if (_nextSpawnTime <= Time.time)
			{
				_nextSpawnTime = Time.time + UnityEngine.Random.Range(_minSpawnTime, _maxSpawnTime);
				GameObject obj = (GameObject)GameObject.Instantiate(_dropObjectPrefab, Vector3.Lerp(_minDropPosition, _maxDropPosition, UnityEngine.Random.Range(0.0f, 1000.0f) / 1000.0f), Quaternion.identity);
				obj.GetComponent<C_DropObject>().SetValues(this, UnityEngine.Random.Range(_minDropSpeed, _maxDropSpeed), _objectLifetime);
				_dropObjects.Add(obj);
			}
		}

		base.Update();
	}

	public void DestroyObject(GameObject pObject, bool pCheckForComboBreak = false)
	{
		if (pCheckForComboBreak && !_usesDumpObject)
		{
			EndCombo();
		}

		_dropObjects.Remove(pObject);
		GameObject.Destroy(pObject);
	}

	protected override void DestroyDynamicObjects()
	{
		foreach (GameObject go in _dropObjects)
		{
			DestroyObject(go);
		}
	}

	public bool EvaluateDrop(int pDropObjectCount, float pCatchObjectSpeed)
	{
		return _plateFallThreshold.Evaluate(pDropObjectCount) < pCatchObjectSpeed && pDropObjectCount > 0;
	}
}
