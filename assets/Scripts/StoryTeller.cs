using UnityEngine;
using System.Collections;

public class StoryTeller : MonoBehaviour {

	[SerializeField]
	Transform startScreen;

	[SerializeField]
	Texture[] slides;
	int page = 0;
	bool _startTelling = false;

	Material mat;

	// Use this for initialization
	void Start () {
		mat = GetComponent<Renderer>().material;
		mat.SetTexture("_MainTex", slides[page]);
	}
	
	// Update is called once per frame
	void Update () {
		

		if(_startTelling && Input.GetMouseButtonDown(0)){
			if(page+1 < slides.Length){
				page++;
				mat.SetTexture("_MainTex", slides[page]);
			}else{
				gameObject.active = false;
				UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("HouseScene");
			}
		}

		if(!_startTelling && Input.GetMouseButtonDown(0)){
			startScreen.gameObject.active = false;
			_startTelling = true;
		}
	}
}
