using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StarScript : MonoBehaviour {

	[SerializeField]
	private GameObject[] stars;

	Color transparent;
	Color nonTransparent;

	public bool Played;

	private void Start(){

		transparent.a = 0.3f;
		transparent.r = 1.0f;
		transparent.g = 1.0f;
		transparent.b = 1.0f;

		nonTransparent.a = 1.0f;
		nonTransparent.r = transparent.r;
		nonTransparent.g = transparent.g;
		nonTransparent.b = transparent.b;

		foreach (GameObject star in stars) {
			star.GetComponent<Image> ().color = transparent;
		}
	}

	public void StartMiniGame(string pName){
		Played = true;
		MaingameManager.Instance.StartMinigame (pName, this);
	}

	public void DisplayStars(int pStarCount){

		switch (pStarCount) 
		{
		case 3: 
			stars [2].GetComponent<Image> ().color = nonTransparent;
			goto case 2;
		case 2: 
			stars [1].GetComponent<Image> ().color = nonTransparent;
			goto case 1;
		case 1: 
			stars [0].GetComponent<Image> ().color = nonTransparent;
			goto default;
		default:
			break;
		}
	}
}
