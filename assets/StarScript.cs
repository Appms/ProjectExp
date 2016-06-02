using UnityEngine;
using System.Collections;

public class StarScript : MonoBehaviour 
{
	[SerializeField]
	private int _firstStarScore;

	[SerializeField]
	private int _secondStartScore;

	[SerializeField]
	private int _thirdStarScore;

	[SerializeField]
	GameObject[] stars;

	private int _score;

	void Start(){

		foreach (GameObject star in stars) {
			star.SetActive (false);
		}
	}

	void Update(){

		if (_score < _firstStarScore) {
			//no star
			return;
		}
		if (_score >= _firstStarScore) {
			//1 star
			stars [0].SetActive (true);
		}
		if (_score >= _secondStartScore) {
			//2 star
			stars [1].SetActive (true);
		} 
		if (_score >= _thirdStarScore) {
			//3 star
			stars [2].SetActive (true);
		}
	}
}
