using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public abstract class T_ThrowObject : MonoBehaviour
{
	protected ThrowMinigame _manager;

	public bool Grabbed;

	protected virtual void Start()
	{
		_manager = FindObjectOfType<ThrowMinigame>();
	}

	protected virtual void Update()
	{

	}

	protected virtual void OnTriggerEnter(Collider pOther)
	{
		if (pOther.GetComponent<T_CatchObject>() != null)
		{
			_manager.AddCombo();
			pOther.GetComponent<T_CatchObject>().Dump();
			GameObject.Destroy(this.gameObject);
		}
	}

	protected virtual void OnCollisionEnter(Collision pColl)
	{
		//if (_manager.MinigameLayers == 1 << pColl.gameObject.layer)
		//{
			_manager.EndCombo();
			GameObject.Destroy(this.gameObject);
		//}
	}
}
