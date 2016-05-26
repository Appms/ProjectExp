using UnityEngine;
using System.Collections;

public abstract class ConditionParent : MonoBehaviour
{
	protected bool _animationPlaying;

	public bool AnimationPlaying
	{
		get { return _animationPlaying; }
	}

	public virtual void Despawn()
	{
		
	}

	protected virtual void Start()
	{
		
	}

	protected virtual void Update()
	{

	}
}
