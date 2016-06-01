using UnityEngine;
using System.Collections;

public class Cloud : MonoBehaviour {

	[SerializeField]
	int cloudSpeed;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
		if(transform.position.x < -1){
			transform.Translate (cloudSpeed * Time.deltaTime, 0, 0);
		} else if(transform.position.x > 1){
			transform.Translate (-cloudSpeed * Time.deltaTime, 0, 0);
		}
		else if(transform.position.x < 1 && transform.position.x > -1) {
			transform.Translate (0, 0, 0);
		}
	}
}
