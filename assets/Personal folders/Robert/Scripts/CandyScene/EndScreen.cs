using UnityEngine;
using System.Collections;

public class EndScreen : MonoBehaviour {

	[SerializeField]
	CameraFollow tCamera;

	float distanceThreshold = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float distance = Mathf.Abs(this.transform.position.y - tCamera.transform.position.y);
		if(distance < distanceThreshold){
			tCamera.tTransform = this.transform;
		}
	}
}
