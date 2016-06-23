using UnityEngine;
using System.Collections;
using System;

public class FinalConditionSeason : ConditionObject {
	[SerializeField]
	private GameObject _summer;

	[SerializeField]
	private GameObject _winter;

	protected override void InitTrue()
	{
		_summer.SetActive(false);
		_winter.SetActive(true);
	}

	protected override void InitFalse()
	{
		_summer.SetActive(true);
		_winter.SetActive(false);
	}
}
