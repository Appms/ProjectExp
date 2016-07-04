using UnityEngine;
using System.Collections;

public class EndGameStoryTeller : MonoBehaviour {

	[SerializeField]
	Transform StoryScreen;
	[SerializeField]
	RocketScript rocket;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			rocket.enabled = true;
			StoryScreen.gameObject.SetActive(false);
		}
	}
}
