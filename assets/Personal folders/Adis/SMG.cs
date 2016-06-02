using UnityEngine;
using System.Collections;

public class SMG : AbstractMinigame {

	float mouseX;
	Transform cloud;

	// Use this for initialization
	override protected void Start () {
		
		base.Start ();
	}
	
	// Update is called once per frame
	override protected void Update () {

		if (_active) {
			
			if (Input.GetMouseButtonDown (0) && cloud == null) {
				RaycastHit rayHit = new RaycastHit ();
				if (Physics.Raycast (MinigameCamera.ScreenPointToRay (Input.mousePosition), out rayHit)) {
					cloud = rayHit.transform;
				}
			}

			if (Input.GetMouseButton (0) && cloud != null) {
				RaycastHit rayHit = new RaycastHit ();
				if (Physics.Raycast (MinigameCamera.ScreenPointToRay (Input.mousePosition), out rayHit)) {
					mouseX = rayHit.point.x;
				}
				cloud.position = new Vector3 (mouseX, cloud.position.y, cloud.position.z);
			}

			if (Input.GetMouseButtonUp (0)) {
				if (cloud.position.x > 100 || cloud.position.x < -100) {
					Destroy (cloud.gameObject);
				} else {
					cloud = null;
				}
			}
		}

		base.Update ();
	}

	protected override void DestroyDynamicObjects ()
	{
		base.DestroyDynamicObjects ();
		cloud = null;
		Cloud[] clouds = FindObjectsOfType<Cloud> ();
		foreach (Cloud c in clouds) {
			GameObject.Destroy (c);
		}
	}

}
