using UnityEngine;
using System.Collections;

public class StarScript : MonoBehaviour {

	[SerializeField]
	private GameObject[] stars;

	private void Start(){

		foreach (GameObject star in stars) {
			star.SetActive (false);
		}
	}

	public void StartMiniGame(string pName){
		Debug.Log (pName);
		MaingameManager.Instance.StartMinigame (pName, this);
	}

	public void DisplayStars(int pStarCount){

		switch (pStarCount) 
		{
		case 3: 
			stars [2].SetActive (true);
			goto case 2;
		case 2: 
			stars [1].SetActive (true);
			goto case 1;
		case 1: 
			stars [0].SetActive (true);
			goto default;
		default:
			break;
		}
	}
}
