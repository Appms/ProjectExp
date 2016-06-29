using UnityEngine;
using System.Collections;
using System;

public class TestConditionSeason : ConditionObject
{
	[SerializeField]
	private Sprite _summerSprite;

	[SerializeField]
	private Sprite _winterSprite;

	protected override void InitTrue()
	{
		GetComponent<SpriteRenderer>().sprite = _winterSprite;
	}

	protected override void InitFalse()
	{
		GetComponent<SpriteRenderer>().sprite = _summerSprite;
	}
}
