using UnityEngine;
using System.Collections;

public abstract class ConditionParent : MonoBehaviour {
	protected bool _animationPlaying;
	protected ConditionMinigame _manager;

	public bool AnimationPlaying {
		get { return _animationPlaying; }
	}

	public virtual void Despawn () {

	}

	protected virtual void Start () {
		_manager = FindObjectOfType<ConditionMinigame>();
	}

	protected virtual void Update () {

	}
}
