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

			StartCoroutine(WaitSomeSecs(1));

			rocket.enabled = true;
			StoryScreen.gameObject.SetActive(false);
		}
	}

	IEnumerator WaitSomeSecs(float time){
		yield return new WaitForSeconds(time);
		Debug.Log("Quitting");
	}
}
