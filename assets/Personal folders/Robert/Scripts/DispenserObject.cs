using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DispenserObject : MonoBehaviour {

	//Reference to the list this Object is in
	public List<DispenserObject> list;
	public float deadZone;
	private bool _outOfGame = false;



	void Update () {
		if(this.transform.position.y < -1 && !_outOfGame){
			//Object removes itself from the list
			list.Remove(this);
			_outOfGame = true;
		}
		if(this.transform.position.y < -4){
			//Object destroys itself
			Destroy(this.gameObject);
		}

	}
}
