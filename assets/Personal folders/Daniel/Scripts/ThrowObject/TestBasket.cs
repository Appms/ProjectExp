using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestBasket : T_SpawnObject
{	
	[SerializeField]
	private int numberOfTrajectoryPoints;
	[SerializeField]
	private float trajectoryPointSpacing;
	[SerializeField]
	private List<Transform> trajectoryPoints;
	[SerializeField]
	private GameObject trajectoryPointPrefab;

	protected override void Start(){
		base.Start();
		ParticleManager.InitParticles();

		for(int i=0; i < numberOfTrajectoryPoints; i++){
			GameObject dot = (GameObject)Instantiate(trajectoryPointPrefab);
			dot.GetComponent<Renderer>().enabled = false;
			trajectoryPoints.Insert(i, dot.transform);
		}
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

	protected override void OnHold(){
		setTrajectoryPoints(this.transform.position + _spawnOffset, -projectileVector * _throwingPower);
	}

	private void setTrajectoryPoints(Vector3 pStartPosition , Vector3 pVelocity )
	{
		float velocity = Mathf.Sqrt((pVelocity.x * pVelocity.x) + (pVelocity.y * pVelocity.y));
		float angle = Mathf.Rad2Deg*(Mathf.Atan2(pVelocity.y , pVelocity.x));
		float fTime = 0;
		fTime += trajectoryPointSpacing;
		for (int i = 0 ; i < numberOfTrajectoryPoints ; i++)
		{
			float dx = velocity * fTime * Mathf.Cos(angle * Mathf.Deg2Rad);
			float dy = velocity * fTime * Mathf.Sin(angle * Mathf.Deg2Rad) - (Physics2D.gravity.magnitude * fTime * fTime / 2.0f);
			Vector3 pos = new Vector3(pStartPosition.x + dx , pStartPosition.y + dy ,0);
			trajectoryPoints[i].transform.position = pos;
			trajectoryPoints[i].GetComponent<Renderer>().enabled = true;
			trajectoryPoints[i].transform.eulerAngles = new Vector3(0,0,Mathf.Atan2(pVelocity.y - (Physics.gravity.magnitude)*fTime,pVelocity.x)*Mathf.Rad2Deg);
			fTime += trajectoryPointSpacing;
		}
	}

	private void ResetSpawn(){
		//Debug.Log("ResetSpawn");
		//this.transform.position = new Vector3(	_originPos.x + Random.Range(Pos1.x, Pos2.x), 
		//										_originPos.y + Random.Range(Pos1.y, Pos2.y), 
		//										_originPos.z + Random.Range(Pos1.z, Pos2.z));
	}
}
