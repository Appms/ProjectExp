using UnityEngine;
using System.Collections;

public class SortingBox : MonoBehaviour {

	[SerializeField]
	SortingGameDispenser _manager;

    [SerializeField]
    Transform _particlePosition;

    public int index;

	void OnTriggerEnter(Collider other){
		DispenserObject dObject = other.GetComponent<DispenserObject>();
		if(dObject != null){
			if(dObject.index == this.index){
				//AddScore
				_manager.AddCombo();
				ParticleManager.CreateParticle(ParticleManager.Particles.PosFeedback,2f,_particlePosition.position);
			}else{
				//AbortCombo
				_manager.EndCombo();
			}
		}
	}
}
