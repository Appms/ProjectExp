using UnityEngine;
using System.Collections;

public class TestBasket : T_SpawnObject
{	
	[SerializeField]
	Vector3 Pos1;
	[SerializeField]
	Vector3 Pos2;

	private Vector3 _originPos;

	protected override void Start(){
		base.Start();
		_originPos = this.transform.position;
		ParticleManager.InitParticles();
	}

	protected override void Spawn()
	{
		base.Spawn();
		GetComponent<Animator>().SetTrigger("SpawnObject");
	}

	protected override void OnRelease(){
		base.OnRelease();
		ResetSpawn();
	}
	private void ResetSpawn(){
		Debug.Log("ResetSpawn");
		this.transform.position = new Vector3(	_originPos.x + Random.Range(Pos1.x, Pos2.x), 
												_originPos.y + Random.Range(Pos1.y, Pos2.y), 
												_originPos.z + Random.Range(Pos1.z, Pos2.z));
	}
}
