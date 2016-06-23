using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SortingGameDispenser : AbstractMinigame  {

	BertTimer _timer = new BertTimer();

	[Range(0.1f,5f)]
	public float minTime;
	[Range(0.1f,5f)]
	public float maxTime;
	[Range(0.0f,20f)]
	public float fallingDrag;

	List<DispenserObject> dispenserObjects = new List<DispenserObject>();

	[SerializeField]
	float distanceThreshold;
	[SerializeField]
	GameObject[] objects;
	[SerializeField]
	Transform spawnPosition;
	//[SerializeField]
	//Transform debugCube;

	//Control Variables
	//point where you cant control dispensed Objects anymore
	float lowerDeadZone = -2;
	float leftDeadZone = -1.5f;
	float rightDeadZone = 1.5f;

	Vector3 worldMousePos;
	DispenserObject currentObject;
	private bool _holding = false;

	// Use this for initialization
	override protected void Start () {
			//base must be first part in Start function
			base.Start();
			//
			ParticleManager.InitParticles();
			//setting the intveral calls Reset()
			_timer.Interval = 1f;
	}
	
	// Update is called once per frame
	override protected void Update () {
		if(GetActive()){
		
		worldMousePos = MinigameCamera.ScreenToWorldPoint(Input.mousePosition + new Vector3(0f, 0f, this.GetComponent<Camera>().WorldToScreenPoint(spawnPosition.transform.position).z));
		//debugCube.transform.position = worldMousePos;
		//Debug.Log(_timer.GetTime());
		if(!_timer.Run()){
			//Debug.Log("GO!");
			SpawnRandomItem();
			float rndNumber = Random.Range(minTime, maxTime);
			_timer.Interval = rndNumber;
		}
		if(Input.GetMouseButtonDown(0)){
			currentObject = GetClosestObject();
			if(currentObject != null){
			currentObject.GetComponent<Rigidbody>().useGravity = false;
			}
		}

		HandleGrabbedObject();


		if(Input.GetMouseButtonUp(0) && currentObject != null){
			if(dispenserObjects.Count != 0){
			
				Rigidbody rb = currentObject.transform.GetComponent<Rigidbody>();
				rb.drag = 0f;
				rb.useGravity = true;
				rb.AddForce(new Vector3(0,-500,0));
				
				ReleaseGrabbedObject();
			}
		}


		DrawDebugLines();
	}

		//base must be last part in Update funtion
		base.Update();
		//
	}

	private void HandleGrabbedObject(){
		if(currentObject == null){
			return;
		}
		Vector3 tempPos = worldMousePos;
		tempPos.y = Mathf.Clamp(tempPos.y,-1.5f,10);
		currentObject.transform.position = Vector3.Lerp(currentObject.transform.position, tempPos, Time.deltaTime*20);
	}

	private void ReleaseGrabbedObject(){
		dispenserObjects.Remove(currentObject);
		currentObject = null;
	}
	private DispenserObject GetClosestObject(){
		DispenserObject closestObjectSoFar = null;
		float closestDistance = 100f;
		foreach(DispenserObject o in dispenserObjects){
		float newDistance = (Vector3.Distance(o.transform.position, worldMousePos));

			if(newDistance < closestDistance && newDistance < distanceThreshold){
					closestObjectSoFar = o;
					closestDistance = newDistance;
			}
		}
		
		return closestObjectSoFar;
	}

	override protected void DestroyDynamicObjects(){
		foreach(DispenserObject o in dispenserObjects){
			Destroy(o.gameObject);
		}
	}

	private void SpawnRandomItem(){
		int rNum = Random.Range(1,100);
		if(0 < rNum &&  rNum <= 40){
			//Stuff between 0-45
			GameObject gO = (GameObject)Instantiate(objects[0], spawnPosition.position, Quaternion.identity);
			dispenserObjects.Add(gO.GetComponent<DispenserObject>());
			Rigidbody rb = gO.GetComponent<Rigidbody>();
			rb.drag = fallingDrag;
			gO.GetComponent<DispenserObject>().list = dispenserObjects;
			gO.GetComponent<DispenserObject>().deadZone = lowerDeadZone;
		}
		if(40 < rNum &&  rNum <= 80){
			//Stuff between 45-90
			GameObject gO = (GameObject)Instantiate(objects[1], spawnPosition.position, Quaternion.identity);
			dispenserObjects.Add(gO.GetComponent<DispenserObject>());
			Rigidbody rb = gO.GetComponent<Rigidbody>();
			rb.drag = fallingDrag;
			gO.GetComponent<DispenserObject>().list = dispenserObjects;
			gO.GetComponent<DispenserObject>().deadZone = lowerDeadZone;
		}
		if(80 < rNum &&  rNum <= 100){
			//Stuff between 90-100
			GameObject gO = (GameObject)Instantiate(objects[2], spawnPosition.position, Quaternion.identity);
			dispenserObjects.Add(gO.GetComponent<DispenserObject>());
			Rigidbody rb = gO.GetComponent<Rigidbody>();
			rb.drag = fallingDrag;
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
	  