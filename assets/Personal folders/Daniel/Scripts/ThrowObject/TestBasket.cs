using UnityEngine;
using System.Collections;

public class TestBasket : T_SpawnObject
{
	protected override void Spawn()
	{
		base.Spawn();
		GetComponent<Animator>().SetTrigger("SpawnObject");
	}
}
