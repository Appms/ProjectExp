using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DispenserObject : MonoBehaviour {

	//Reference to the list this Object is in
	public List<DispenserObject> list;

	public float deadZone;

	private bool _outOfGame = false;

	//if the obejct has the same index as the box it lands in you get points
	public int index;

	private void Update () {
		this.transform.rotation = Quaternion.Euler(0, 90, 0);
		this.GetComponent<Rigidbody>().drag = 10;
		if (this.transform.position.y < -2 && !_outOfGame) {
			//Object removes itself from the list
			list.Remove(this);
			_outOfGame = true;
		}
		if (this.transform.position.y < -4) {
			//Object destroys itself
			Destroy(this.gameObject);
		}

	}

	private void OnTriggerEnter (Collider other) {
		SortingBox box = other.GetComponent<SortingBox>();
		if (box != null) {
			if (box.index == this.index) {
				//AddScore
				Debug.Log("AddingScore");
			} else {
				//AbortCombo
				//Debug.Log("AbortCombo");
			}
		}
	}
}
