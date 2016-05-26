using UnityEngine;
using System.Collections;

public class SelfDisablingScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
			this.gameObject.active = false;
		}
	}
}
