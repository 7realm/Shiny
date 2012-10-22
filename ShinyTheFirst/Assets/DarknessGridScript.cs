using UnityEngine;
using System.Collections;

public class DarknessGridScript : MonoBehaviour {
	
	
	public static Particle[] particles;
	
	public static ParticleEmitter emitter;
	
	TwoDInArray<Particle> _particles;
	const int n = 20;
	const float maxSize = 6;
	const float maxDarknessSpread = .001f;
	const float gridStep = 1.3f;
	
	// Use this for initialization
	void Start () {
		
		float gridOffset = gridStep * n/2;
		
		particles = new Particle[n*n];// particleEmitter.particles;
		for (int i = 0; i < n; i++) {
			for (int j = 0; j < n; j++) {
				var p = new Particle();
				p.position = new Vector3(
					i*gridStep - gridOffset, 
					i%2 == 0 ? j*gridStep - gridOffset : j*gridStep - gridOffset + gridStep/2, 
					10);
				p.color = new Color(1, 1,1 ,1);
				if (j%2!=0)
					p.size = maxSize;
				else
					p.size = maxSize;//.2f;
				p.energy = 1E+20f;
				p.startEnergy = 1E+20f;
				particles[i*n + j] = p;
			}
		}
		
		//particles = particleEmitter.particles;
		particleEmitter.particles = particles;
		_particles = new TwoDInArray<Particle>(particles, n);
		emitter = particleEmitter;
	}
	
	
	
	// Update is called once per frame
	void Update () {
			
		float[] delta = new float[particles.Length];
		var deltas = new TwoDInArray<float>(delta, n);
		
		for (int i = 1; i < n -1; i++) {
			for (int j = 1; j < n - 1; j++) {
				float f = 0.01f * _particles[i, j].size;
				if (f > maxDarknessSpread) 
					f = maxDarknessSpread;
				
				//deltas[i - 1, j - 1] += f;
				deltas[i + 0, j - 1] += f;
				//deltas[i + 1, j - 1] += f;
				deltas[i + 1, j + 0] += f;
				deltas[i + 1, j + 1] += f;
				deltas[i + 0, j + 1] += f;
				deltas[i - 1, j + 1] += f;
				deltas[i - 1, j + 0] += f;
			}
		}	
		
		for (int i = 0; i < particles.Length; i++) {
			particles[i].size += delta[i];
			if (particles[i].size > maxSize) particles[i].size = maxSize;
		}
		
		particleEmitter.particles = particles;
		emitter = particleEmitter;
	}
	
	class TwoDInArray<T>
	{
		T[] _storage;
		int _width;
		
		
		public TwoDInArray(T[] storage, int width)
		{
			_storage = storage;
			_width = width;
		}
			
			
		public T this[int x, int y] { 
			get { return _storage[_width*y + x]; }
			set { _storage[_width*y + x] = value; }
		}
			
	}
}
