using UnityEngine;
using System.Collections;

public class TestRack : T_CatchObject
{
	[SerializeField]
	private float _lerpTime;

	[SerializeField]
	private Vector3 Pos1;
	[SerializeField]
	private Vector3 Pos2;

	private Vector3 _originPos;

	private void Start()
	{
		_originPos = transform.position;
	}

	private void Update()
	{
		/*_currentLerpTime += Time.deltaTime;

		if (_currentLerpTime > _lerpTime)
		{
			_currentLerpTime = _lerpTime;
			_reachedEnd = true;
		}

		float perc = _currentLerpTime / _lerpTime;
		transform.localPosition = Vector3.Lerp(_startPosition, _targetPoint, perc);

		if (_reachedEnd)
		{
			Vector3 temp = _startPosition;
			_startPosition = _targetPoint;
			_targetPoint = temp;
			_currentLerpTime = 0.0f;
			_reachedEnd = false;
		}*/
	}

	public override void Dump()
	{
		base.Dump();
		GetComponent<Animator>().SetTrigger("SpawnObject");
		ResetPos();
	}
	private void ResetPos(){
		Debug.Log("ResetSpawn");
		this.transform.position = new Vector3(	_originPos.x + Random.Range(Pos1.x, Pos2.x), 
												_originPos.y + Random.Range(Pos1.y, Pos2.y), 
												_originPos.z + Random.Range(Pos1.z, Pos2.z));
	}
}
