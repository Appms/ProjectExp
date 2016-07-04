using UnityEngine;
using System.Collections;
using System;

public class TestConditionRadiator : ConditionObject {
	[SerializeField]
	private float _onSaturation;

	[SerializeField]
	private float _offSaturation;

	protected override void InitTrue () {
		transform.GetComponentInChildren<ParticleSystem>().Play();

		foreach (Renderer r in GetComponentsInChildren<Renderer>()) {
			r.material.SetFloat("_S", _onSaturation);
		}
	}

	protected override void InitFalse () {
		transform.GetComponentInChildren<ParticleSystem>().Stop();

		foreach (Renderer r in GetComponentsInChildren<Renderer>()) {
			r.material.SetFloat("_S", _offSaturation);
		}
	}

	protected override void TurnTrue () {
		base.TurnTrue();

		transform.GetComponentInChildren<ParticleSystem>().Stop();

		foreach (Renderer r in GetComponentsInChildren<Renderer>()) {
			r.material.SetFloat("_S", _offSaturation);
		}
	}

	protected override void TurnFalse () {
		base.TurnFalse();

		transform.GetComponentInChildren<ParticleSystem>().Play();

		foreach (Renderer r in GetComponentsInChildren<Renderer>()) {
			r.material.SetFloat("_S", _onSaturation);
		}
	}
}
