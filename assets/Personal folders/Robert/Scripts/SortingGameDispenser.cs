using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SortingGameDispenser : AbstractMinigame  {

	BertTimer _timer = new BertTimer();

	[Range(0.1f,5f)]
	public float minTime;
	[Range(0.1f,5f)]
	public float maxTime;

	List<DispenserObject> dispenserObjects = new List<DispenserObject>();

	[SerializeField]
	GameObject[] objects;
	[SerializeField]
	Transform spawnPosition;

	//Control Variables
	//point where you cant control dispensed Objects anymore
	float lowerDeadZone = -1;
	float leftDeadZone = -1.5f;
	float rightDeadZone = 1.5f;

	Vector3 worldMousePos;

	// Use this for initialization
	override protected void Start () {
			//base must be first part in Start function
			base.Start();
			//

			//setting the intveral calls Reset()
			_timer.Interval = 1f;
	}
	
	// Update is called once per frame
	override protected void Update () {

		if(!_timer.Run()){
			SpawnRandomItem();
			_timer.Interval = Random.Range(minTime, maxTime);
		}
		if(Input.GetMouseButtonUp(0)){
			if(dispenserObjects.Count <= 10000){
			worldMousePos = MinigameCamera.ScreenToWorldPoint(Input.mousePosition + new Vector3(0f, 0f, 10f));
			Debug.Log(worldMousePos);
			if(worldMousePos.x < leftDeadZone){
				//Do stuff if mouse is on the left side
				dispenserObjects[0].transform.GetComponent<Rigidbody>().AddForce(new Vector3(-600+(dispenserObjects[0].transform.position.y*100f,300,0));
				}else if(worldMousePos.x > rightDeadZone){
				//Do stuff if mouse is on the left side	
				dispenserObjects[0].transform.GetComponent<Rigidbody>().AddForce(new Vector3(600-dispenserObjects[0].transform.position.y*100f,300,0));
				}
				dispenserObjects.RemoveAt(0);
			}
		}


		DrawDebugLines();

		//base must be last part in Update funtion
		base.Update();
		//
	}


	private void SpawnRandomItem(){
		int rNum = Random.Range(1,100);
		if(0 < rNum &&  rNum <= 45){
			//Stuff between 0-45
			GameObject gO = (GameObject)Instantiate(objects[0], spawnPosition.position, Quaternion.identity);
			dispenserObjects.Add(gO.GetComponent<DispenserObject>());
			gO.GetComponent<DispenserObject>().list = dispenserObjects;
			gO.GetComponent<DispenserObject>().deadZone = lowerDeadZone;
		}
		if(45 < rNum &&  rNum <= 90){
			//Stuff between 45-90
			GameObject gO = (GameObject)Instantiate(objects[1], spawnPosition.position, Quaternion.identity);
			dispenserObjects.Add(gO.GetComponent<DispenserObject>());
			gO.GetComponent<DispenserObject>().list = dispenserObjects;
			gO.GetComponent<DispenserObject>().deadZone = lowerDeadZone;
		}
		if(90 < rNum &&  rNum <= 100){
			//Stuff between 90-100
			GameObject gO = (GameObject)Instantiate(objects[2], spawnPosition.position, Quaternion.identity);
			dispenserObjects.Add(gO.GetComponent<DispenserObject>());
			gO.GetComponent<DispenserObject>().list = dispenserObjects;
			gO.GetComponent<DispenserObject>().deadZone = lowerDeadZone;
		}
	}

	void DrawDebugLines(){
		Debug.DrawLine(new Vector3(leftDeadZone, lowerDeadZone, 0), new Vector3(rightDeadZone, lowerDeadZone, 0), Color.red);
		Debug.DrawLine(new Vector3(leftDeadZone, 5, 0), new Vector3(leftDeadZone, -5, 0), Color.grey);
		Debug.DrawLine(new Vector3(rightDeadZone, 5, 0), new Vector3(rightDeadZone, -5, 0), Color.grey);

    	
	}
}
	  