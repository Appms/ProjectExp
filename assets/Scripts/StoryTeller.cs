using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StoryTeller : MonoBehaviour {

	float lerpTime = 1f;
    float currentLerpTime;
 
	[SerializeField]
	Transform startScreen;
	[SerializeField]
	Transform storyScreen;

	[SerializeField]
	public List<Storyslide> storySlides;
	private int storyPositionIndex = 0;
	private Vector3 _originPos;
	private Vector3 _targetPos;

	int page = 0;
	bool _startTelling = false;

	Material mat;
	Camera _camera;
	Vector3 _oldCameraPos;

	//Inspector
	private int inspectorPageIndex = 0;

	// Use this for initialization
	void Start () {
		mat = storyScreen.GetComponent<Renderer>().material;
		mat.SetTexture("_MainTex", storySlides[page].texture);
		_camera = this.transform.GetComponent<Camera>();
		_oldCameraPos = this.transform.position;
		_originPos = this.transform.position;
		_targetPos = _originPos + storySlides[page].Positions[storyPositionIndex];
	}
	
	void OnDrawGizmosSelected() {

		float z = 0;
		foreach(Storyslide slide in storySlides){
			foreach(Vector3 pos in slide.Positions){
				Gizmos.color = new Color(1, 1, 0, 1F);
	        	Gizmos.DrawCube(this.transform.position + pos + new Vector3(0,0,z), new Vector3(1, 1, 1));
			}
			z= z + 5;
        }
    }

	// Update is called once per frame
	void Update () {
		
		MoveCamera(_targetPos);
		
		if(!_startTelling && Input.GetMouseButtonDown(0)){
			startScreen.gameObject.SetActive(false);
			_startTelling = true;
		}
		if(_startTelling && Input.GetMouseButtonDown(0)){
			if(!NextPosition()){
				if(!NextPage()){
					UnityEngine.SceneManagement.SceneManager.LoadScene("HouseSceneAndrés");
				}
			}
		}
	}

	bool NextSlide(){
		page++;
		if(page >= storySlides.Count){
			//No slides left
			this.enabled = false;
			return false;
		}else{
			ReloadSlide();
			return true;
		}
		
	}
	void ReloadSlide(){
		mat.SetTexture("_MainTex", storySlides[page].texture);
	}
	void MoveCamera(Vector3 targetPos){
		 //reset when we press spacebar
        /*if (Input.GetKeyDown(KeyCode.Space)) {
            currentLerpTime = 0f;
        }
 
        //increment timer once per frame
        currentLerpTime += Time.deltaTime;
        if (currentLerpTime > lerpTime) {
            currentLerpTime = lerpTime;
        }
 
        //lerp!
        float perc = currentLerpTime / lerpTime;*/
        transform.position = Vector3.Lerp(transform.position, targetPos, 0.4f);
	}
	private bool NextPosition()
	{
		//Check if next position is valid
		if((storyPositionIndex + 1 < storySlides[page].Positions.Length))
		{
			storyPositionIndex++;
			_targetPos = _originPos + storySlides[page].Positions[storyPositionIndex];
			return true;
		}
		return false;
	}
	private bool NextPage()
	{
		//Check if next position is valid
		if((page + 1 < storySlides.Count))
		{	
			//not Last page
			page++;
			storyPositionIndex = 0;
			_targetPos = _originPos + storySlides[page].Positions[storyPositionIndex];
			return true;
		}
		//Last page
		return false;
	}
}
