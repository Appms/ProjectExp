using UnityEngine;
using System.Collections;

public class SortingBox : MonoBehaviour {

	[SerializeField]
	SortingGameDispenser _manager;

	public int index;

	void OnTriggerEnter(Collider other){
		DispenserObject dObject = other.GetComponent<DispenserObject>();
		if(dObject != null){
			if(dObject.index == this.index){
				//AddScore
				_manager.AddCombo();
			}else{
				//AbortCombo
				_manager.EndCombo();
			}
		}
	}
}
