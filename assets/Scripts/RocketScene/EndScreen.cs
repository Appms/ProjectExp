using UnityEngine;
using System.Collections;

public class EndScreen : MonoBehaviour {

	[SerializeField]
	CameraFollow tCamera;

	//ending
	bool _youWon;
	BertTimer _timer;

	float distanceThreshold = 1;

	float timer;

	// Use this for initialization
	void Start () {
	_timer = new BertTimer();
	//Debug.Log("Yo replace score with real Score");
	float score = 200;
		timer = Time.time + 3.0f;
	this.transform.position = new Vector3(this.transform.position.x,Mathf.Clamp(score,0,200), this.transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		float distance = Mathf.Abs(this.transform.position.y - tCamera.transform.position.y);
		if(distance < distanceThreshold){
			tCamera.tTransform = this.transform;
			_timer.Interval = 3f;
			_youWon = true;
		}
		if(_youWon && timer <= Time.time)
		{
			//Debug.Log("Quitting");
			Application.Quit();
		}
	}
}
