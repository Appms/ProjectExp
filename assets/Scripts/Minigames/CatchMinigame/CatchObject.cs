using UnityEngine;
using System.Collections.Generic;

public abstract class CatchObject : MonoBehaviour
{
	protected CatchMinigame _manager;
	private bool _usesDumpObject;
	private float _followSpeed;
	protected List<DropObject> _catchedObjects;
	private Vector3 _prevMousePos;

	protected virtual void Start()
	{
		_prevMousePos = Input.mousePosition;
		_catchedObjects = new List<DropObject>();
	}

	public void SetValues(CatchMinigame pManager, float pFollowSpeed, bool pUsedDumpObject)
	{
		_manager = pManager;
		_followSpeed = pFollowSpeed;
		_usesDumpObject = pUsedDumpObject;
	}

	protected virtual void Update()
	{
		if (_manager.GetActive())
		{
			if (_manager.EvaluateDrop(_catchedObjects.Count, Vector3.Distance(_prevMousePos, Input.mousePosition)))
			{
				DropObject drObj = _catchedObjects[_catchedObjects.Count - 1];
				drObj.Drop();
				_catchedObjects.Remove(drObj);
				_manager.RemoveCombo();
			}

			transform.position = Vector2.Lerp(transform.position, _manager.MinigameCamera.ScreenToWorldPoint(Input.mousePosition + new Vector3(0.0f, 0.0f, Mathf.Abs((transform.position - _manager.transform.position).z))), _followSpeed);
		}

		_prevMousePos = Input.mousePosition;
	}

	protected virtual void LateUpdate()
	{
		for (int i = 0; i < _catchedObjects.Count; i++)
		{
			_catchedObjects[i].transform.position = transform.position + new Vector3(0.0f, i * 0.1f, 0.0f);
		}
	}

	protected virtual void Catch() { }
	protected virtual void Dump() { }

	private void OnTriggerEnter(Collider pOther)
	{
		if (pOther.GetComponent<DropObject>() != null)
		{
			if (pOther.GetComponent<DropObject>().CheckCatchable())
			{
				pOther.GetComponent<DropObject>().Catch();
				_catchedObjects.Add(pOther.GetComponent<DropObject>());
				_manager.AddCombo();
				Catch();
			}
			
		}
		else if (_usesDumpObject && pOther.GetComponent<DumpObject>() != null)
		{
			_manager.EndCombo();
			Dump();
			pOther.GetComponent<DumpObject>().Dump();
		}
	}
}
