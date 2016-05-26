using UnityEngine;
using System.Collections;

public class ThrowMinigame : AbstractMinigame
{
	[SerializeField]
	[Tooltip("The object that needs to be thrown")]
	private GameObject _throwObjectPrefab;

	public GameObject ThrowObjectPrefab
	{
		get { return _throwObjectPrefab; }
	}

	protected override void Start()
	{
		base.Start();
	}

	protected override void Update()
	{
		base.Update();
	}

	protected override void DestroyDynamicObjects()
	{
		base.DestroyDynamicObjects();
		T_ThrowObject[] obj = FindObjectsOfType<T_ThrowObject>();

		foreach (T_ThrowObject t in obj)
		{
			GameObject.Destroy(t.gameObject);
		}
	}
}
