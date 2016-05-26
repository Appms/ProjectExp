using UnityEngine;
using System.Collections;

public class RocketScript : MonoBehaviour {

	[SerializeField]
	float maxSpeed;
	public float currentSpeed = 0;

	float lerpTime = 6f;
	float currentLerpTime;

	bool start = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)){
			start = true;
		}
		if(!start){return;}
		//reset when we press spacebar
        if (Input.GetKeyDown(KeyCode.Space)) {
            currentLerpTime = 0f;
        }
 
        //increment timer once per frame
        currentLerpTime += Time.deltaTime;
        if (currentLerpTime > lerpTime) {
            currentLerpTime = lerpTime;
        }
 
        //lerp!
        float perc = currentLerpTime / lerpTime;
        currentSpeed = Mathf.Lerp(0f, maxSpeed, perc);

		MoveY(currentSpeed);
	}

	void MoveY(float yValue){
		this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y + yValue, this.transform.position.z);
	}
}
