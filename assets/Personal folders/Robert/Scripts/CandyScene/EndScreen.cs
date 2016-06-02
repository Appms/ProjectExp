using UnityEngine;
using System.Collections;

public class EndScreen : MonoBehaviour {

	[SerializeField]
	CameraFollow tCamera;

	float distanceThreshold = 1;

	// Use this for initialization
	void Start () {
	Debug.Log("Yo replace score with real Score");
	float score = 200;
	this.transform.position = new Vector3(this.transform.position.x,Mathf.Clamp(score,0,200));
	}
	
	// Update is called once per frame
	void Update () {
		float distance = Mathf.Abs(this.transform.position.y - tCamera.transform.position.y);
		if(distance < distanceThreshold){
			tCamera.tTransform = this.transform;
		}
	}
}
