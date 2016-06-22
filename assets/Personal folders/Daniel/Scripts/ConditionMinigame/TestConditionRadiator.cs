using UnityEngine;
using System.Collections;
using System;

public class TestConditionRadiator : ConditionObject
{
	[SerializeField]
	private Color _onColor;

	[SerializeField]
	private Color _offColor;

	protected override void InitTrue()
	{
		transform.GetComponentInChildren<ParticleSystem>().Play();

		foreach (Renderer r in GetComponentsInChildren<Renderer>())
		{
			r.material.SetColor("_Color", _onColor);
		}
	}

	protected override void InitFalse()
	{
		transform.GetComponentInChildren<ParticleSystem>().Stop();

		foreach (Renderer r in GetComponentsInChildren<Renderer>())
		{
			r.material.SetColor("_Color", _offColor);
		}
	}

	protected override void TurnTrue()
	{
		base.TurnTrue();

		transform.GetComponentInChildren<ParticleSystem>().Stop();

		foreach (Renderer r in GetComponentsInChildren<Renderer>())
		{
			r.material.SetColor("_Color", _offColor);
		}
	}

	protected override void TurnFalse()
	{
		base.TurnFalse();

		transform.GetComponentInChildren<ParticleSystem>().Play();

		foreach (Renderer r in GetComponentsInChildren<Renderer>())
		{
			r.material.SetColor("_Color", _onColor);
		}
	}
}
