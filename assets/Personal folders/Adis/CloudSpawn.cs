using UnityEngine;
using System.Collections;

public class CloudSpawn : MonoBehaviour {

	GameObject cloud = null;

	float timer;

	[SerializeField]
	GameObject canvas;

	[SerializeField]
	int minSec;
	[SerializeField]
	int maxSec;

	// Use this for initialization
	void Start () {
		timer = Random.Range(minSec, maxSec + 1);
		SpawnClouds ();
	}
	
	// Update is called once per frame
	void Update () {
		timer = timer - 1 * Time.deltaTime;
		
		if (timer <= 0) {
			SpawnClouds ();
			timer = Random.Range(minSec, maxSec + 1);
		}
	}

	void SpawnClouds(){
		
		cloud = Instantiate (Resources.Load("Cloud"), this.transform.position, Quaternion.identity) as GameObject;
		cloud.transform.SetParent (canvas.transform);
	}

}
