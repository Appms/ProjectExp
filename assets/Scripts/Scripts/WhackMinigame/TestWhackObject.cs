using UnityEngine;
using System.Collections;
using System;

public class TestWhackObject : WhackObject
{
	protected override void InteractRight()
	{
		GetComponent<Renderer>().material.SetColor("_Color", new Color(1, 0, 0));
		GetComponent<Animator>().SetTrigger("Right");
	}

	protected override void InteractWrong()
	{
		GetComponent<Animator>().SetTrigger("Wrong");
	}

	protected override void TurnOn()
	{
		GetComponent<Renderer>().material.SetColor("_Color", new Color(0, 1, 0));
		GetComponent<Animator>().SetTrigger("Switch");
	}

	protected override void TurnOff()
	{
		GetComponent<Renderer>().material.SetColor("_Color", new Color(1, 0, 0));
		GetComponent<Animator>().SetTrigger("Switch");
	}
}
