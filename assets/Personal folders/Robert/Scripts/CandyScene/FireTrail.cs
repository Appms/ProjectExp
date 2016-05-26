using UnityEngine;
using System.Collections;

public class FireTrail : MonoBehaviour {

	[SerializeField]
	RocketScript rocket;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.localScale = new Vector3(this.transform.localScale.x, rocket.currentSpeed*8, this.transform.localScale.z);
	}
}
