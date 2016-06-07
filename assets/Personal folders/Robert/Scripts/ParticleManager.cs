using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleManager : MonoBehaviour {

	public enum Particles {GetPoints, PosFeedback}


	static Dictionary<Particles, string> particleDic = new Dictionary<Particles, string>();

	static public void InitParticles(){
		particleDic.Add(Particles.GetPoints, "Particles/GetPointsParticle");
		particleDic.Add(Particles.PosFeedback, "Particles/GreenSparkleParticle");
	}


	static public void CreateParticle(string path, float duration, Vector3 position){
		GameObject go = (GameObject)Instantiate(Resources.Load(path), position, Quaternion.identity);
		Destroy(go, duration);
	}

	static public void CreateParticle(Particles particle, float duration, Vector3 position){
		GameObject go = (GameObject)Instantiate(Resources.Load(particleDic[particle]), position, Quaternion.identity);
		Destroy(go, duration);
	}
}
