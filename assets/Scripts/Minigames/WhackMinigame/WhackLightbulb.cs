using UnityEngine;
using System.Collections;
using System;

public class WhackLightbulb : WhackObject
{
	[SerializeField]
	private Material _offMaterial;

	[SerializeField]
	private Material _onMaterial;

	protected override void TurnOn()
	{
		//GetComponent<Light>().enabled = true;
		GetComponent<Animator>().SetTrigger("Switch");
		GetComponent<Renderer>().material = _onMaterial;
	}

	protected override void TurnOff()
	{
		//GetComponent<Light>().enabled = false;
		GetComponent<Animator>().SetTrigger("Switch");
		GetComponent<Renderer>().material = _offMaterial;
	}

	protected override void InteractRight()
	{
		//GetComponent<Light>().enabled = false;
		GetComponent<Animator>().SetTrigger("Right");
		GetComponent<Renderer>().material = _offMaterial;
	}

	protected override void InteractWrong()
	{
		GetComponent<Animator>().SetTrigger("Wrong");
	}
}
