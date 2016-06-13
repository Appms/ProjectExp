using UnityEngine;
using System.Collections;

public class ParticleTestingScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.K)){
			ParticleManager.CreateParticle("Particles/GetPointsParticle", 2f, new Vector3(0,0,0));
		}
		if(Input.GetKeyDown(KeyCode.J)){
			ParticleManager.CreateParticle(ParticleManager.Particles.PosFeedback, 2f, new Vector3(0,0,0));
		}
		if(Input.GetKeyDown(KeyCode.H)){
			ParticleManager.CreateParticle(ParticleManager.Particles.GetPoints, 2f, new Vector3(0,0,0));
		}
		if(Input.GetKeyDown(KeyCode.I)){
			ParticleManager.InitParticles();
		}
	}
}
