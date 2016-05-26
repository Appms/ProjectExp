using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour {

	[SerializeField]
	float timeInSecond = 180f;

	[SerializeField]
	int fontSize = 20;

	[SerializeField]
	int score = 0;

	GUIStyle style;

	[SerializeField]
	GameObject arrow;

	[SerializeField]
	Image bar;

	float width = 270f;
	float height = 10f;

	// Use this for initialization
	void Start () {

		//FONT
		style = new GUIStyle();
		style.fontSize = fontSize;
	}
	
	// Update is called once per frame
	void Update () {
		//TIMER + CLOCK + LIFE BAR
		if(timeInSecond <= 0){
			timeInSecond = 0;
			arrow.transform.rotation = Quaternion.Euler (Vector3.zero);
			bar.rectTransform.sizeDelta = new Vector2(270, height);
		} else {
			timeInSecond -= Time.deltaTime;
			arrow.transform.Rotate(new Vector3(0, 0, -2 * Time.deltaTime));
			width -= 1.5f * Time.deltaTime;
			bar.rectTransform.sizeDelta = new Vector2(width, height);
		}
	}

	void OnGUI(){

		//TIMER
		int minutes = Mathf.FloorToInt (timeInSecond / 60f);
		int seconds = Mathf.FloorToInt (timeInSecond - minutes * 60f);
		string timer = string.Format ("{0:0}:{1:00}", minutes, seconds);
		GUI.Label (new Rect(200, 10, 250, 100), "Time: " + timer, style);

		//SCORE
		GUI.Label (new Rect(350, 10, 250, 100), "Score: " + score.ToString(), style);

	}
}
