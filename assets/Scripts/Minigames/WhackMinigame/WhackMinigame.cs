using UnityEngine;
using System.Collections.Generic;

public class WhackMinigame : AbstractMinigame
{
	[SerializeField]
	[Tooltip("The minimum time until an on object switches off")]
	private float _minTurnOffTime = 1.0f;

	[SerializeField]
	[Tooltip("The maximum time until an on object switches off")]
	private float _maxTurnOffTime = 3.0f;

	[SerializeField]
	[Tooltip("The minimum time until an active object switches on")]
	private float _minTurnOnTime = 2.0f;

	[SerializeField]
	[Tooltip("The maximum time until an active object switches on")]
	private float _maxTurnOnTime = 4.5f;

	[SerializeField]
	[Tooltip("The amount of objects that can turn on at the same time")]
	private int _activeObjectAmount = 4;

	//A list of all whack objects
	private List<WhackObject> _whackObjects = new List<WhackObject>();


	protected override void Start()
	{
		base.Start();

		_whackObjects = new List<WhackObject>(FindObjectsOfType<WhackObject>());

		if (_whackObjects.Count == 0)
		{
			Debug.LogError("You have no WhackObjects in your Scene!");
		}
	}

	protected override void Update()
	{
		if (_active)
		{
			//Determine wich object is ready for a state change
			foreach (WhackObject obj in _whackObjects)
			{
				if (obj.SwitchTime <= Time.time)
				{
					if (obj.State)
					{
						obj.SwitchTime = Mathf.Infinity;
						EndCombo();
					}
					else if (!obj.State)
					{
						obj.SwitchTime = Random.Range(_minTurnOffTime, _maxTurnOffTime) + Time.time;
					}

					obj.SwitchState();
				}

				//Gametime check
				base.Update();
			}

			int counter = 0;

			while (GetTurningOnObjectCount() < _activeObjectAmount)
			{
				counter++;

				int index = Random.Range(0, _whackObjects.Count);

				int safetyCounter = 0;

				while (_whackObjects[index].State && safetyCounter < _whackObjects.Count)
				{
					index++;
					safetyCounter++;

					if (index >= _whackObjects.Count)
					{
						index = 0;
					}
				}

				_whackObjects[index].SwitchTime = Random.Range(_minTurnOnTime, _maxTurnOnTime) + Time.time;

				if (counter >= 100)
					break;
			}


			//TODO Replace with Touch Input
			if (Input.GetMouseButtonDown(0))
			{
				Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
				List<RaycastHit> hits = new List<RaycastHit>(Physics.RaycastAll(ray, Mathf.Infinity, _layer));
				RaycastHit hit = hits.Find(x => x.collider.GetComponent<WhackObject>() != null);

				if (hit.collider != null)
				{
					WhackObject obj = hit.collider.GetComponent<WhackObject>();

					obj.SwitchTime = Mathf.Infinity;
					obj.Interact();

					if (obj.State)
					{
						AddCombo();
					}
					else
					{
						EndCombo();
					}
				}
			}
		}

		base.Update();
	}

	private int GetTurningOnObjectCount()
	{
		int count = 0;

		foreach (WhackObject obj in _whackObjects)
		{
			if (!obj.State && obj.SwitchTime < Mathf.Infinity)
			{
				count++;
			}
		}

		return count;
	}
}
