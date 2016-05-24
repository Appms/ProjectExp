using UnityEngine;
using System.Collections;
using System;

public class TestConditionWindow : ConditionObject
{
	//TODO Wait until animation is played before showing new condition
	protected override void InitTrue()
	{
		transform.position = new Vector3(0.0f, 0.0f, 0.0f);
	}

	protected override void InitFalse()
	{
		transform.position = new Vector3(0.0f, 0.75f, 0.0f);
	}

	protected override void TurnTrue()
	{
		transform.position = new Vector3(0.0f, 0.75f, 0.0f);
	}

	protected override void TurnFalse()
	{
		transform.position = new Vector3(0.0f, 0.0f, 0.0f);
	}
}
