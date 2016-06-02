using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	[SerializeField]
	public Transform tTransform;

	[Range(0.1f,20)]
	public float smoothing = 1;

	//bool start = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
		//if(Input.GetKeyDown(KeyCode.Space)){
		//	start = true;
		//}
		//if(!start){return;}
		Vector3 targetVector = new Vector3(this.transform.position.x, this.tTransform.position.y, this.transform.position.z);

		this.transform.position = Vector3.Lerp(this.transform.position, targetVector, 0.9f);

	}
}
