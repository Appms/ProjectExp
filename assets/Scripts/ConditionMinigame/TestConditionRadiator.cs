using UnityEngine;
using System.Collections;
using System;

public class TestConditionRadiator : ConditionObject
{
	protected override void InitTrue()
	{
		foreach (Renderer r in GetComponentsInChildren<Renderer>())
		{
			r.material.SetColor("_Color", Color.red);
		}
	}

	protected override void InitFalse()
	{
		foreach (Renderer r in GetComponentsInChildren<Renderer>())
		{
			r.material.SetColor("_Color", Color.gray);
		}
	}

	protected override void TurnTrue()
	{
		base.TurnTrue();

		foreach (Renderer r in GetComponentsInChildren<Renderer>())
		{
			r.material.SetColor("_Color", Color.gray);
		}
	}

	protected override void TurnFalse()
	{
		base.TurnFalse();

		foreach (Renderer r in GetComponentsInChildren<Renderer>())
		{
			r.material.SetColor("_Color", Color.red);
		}
	}
}
