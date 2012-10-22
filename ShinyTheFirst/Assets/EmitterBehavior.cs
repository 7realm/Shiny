using UnityEngine;
using System.Collections;

public class EmitterBehavior : MonoBehaviour {

	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
		
		if (Input.GetMouseButton(0)) {
			var mousePos = Input.mousePosition;
			mousePos.z = 1.0f;
			var worldPos = Camera.main.ScreenToWorldPoint(mousePos);			
			
			//var direction = worldPos - this.transform.position;
			
			worldPos.z = 0;
			
			worldPos.Normalize();
			
			
			particleEmitter.worldVelocity = worldPos*5;
		}
		else {
			particleEmitter.worldVelocity = new Vector3();
			
		}
		
				
	
		if (Input.GetMouseButton(1)) {
			particleEmitter.rndVelocity = new Vector3(10, 10, 0);
			particleEmitter.minEmission = particleEmitter.maxEmission = 100;
		}
		else {
			particleEmitter.rndVelocity = new Vector3(1, 1, 0);
			particleEmitter.minEmission = particleEmitter.maxEmission = 10;
			
		}
		
		
		var particles = particleEmitter.particles;
		for (int i = 0; i < particles.Length; i++) {
			for (int j = 0; j < DarknessGridScript.particles.Length; j++) {
				
				var particle = particles[i];
				var darkParticle = DarknessGridScript.particles[j];
				
				var dist = (particle.position - darkParticle.position).sqrMagnitude;// - DarknessGridScript.particles[j].size;
				//dist *= 100;
				//if (dist < 1) dist = 1;
				if (dist < 2) 
				{
					float particleEnergy = (particle.size);
					
					float giveEnergy = Mathf.Min(0.2f, particleEnergy);
					
					giveEnergy = Mathf.Min(DarknessGridScript.particles[j].size, giveEnergy);
					
					DarknessGridScript.particles[j].size -= giveEnergy;
					
					
					if (DarknessGridScript.particles[j].size < 0) DarknessGridScript.particles[j].size = 0;
					particles[i].size -= giveEnergy;
					if (particles[i].size < 0) 
						particles[i].size = 0f;
				}
			}
		}
		
		particleEmitter.particles = particles;
		var m = DarknessGridScript.emitter;
		m.particles = DarknessGridScript.particles;
	}
}
