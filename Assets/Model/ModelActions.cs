using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Threading;
using System.IO;

public class ModelActions : MonoBehaviour {

	int nb=0;
	int nbmax=100;
	Vector3 [] x;
	Vector3 [] v;
	Vector3 [] a;
	float [] m;
	public GameObject bodyPRE;
	GameObject [] bodyGos;

	float G = 8.88e-10f;

	float threadDT=0.001f;
	float fixedDT=0.1f;
	public Thread modelThread;

	bool threadRunning=true;
	bool stepFree=true;
	bool stepRunning = false;
	float t = 0.0f;

	bool threaded = false;
	bool fastrun=true;

	bool paused = false;

	static readonly object _locker = new object();

	public bool GetThreaded() {
		return threaded;
	}

	public void pushObject(float mass, Vector3 position, Vector3 velocity) {
		lock(_locker) {
			stepFree=false;
			// would a lock be more efficient here?
			while(stepRunning) {} // wait for step to finish to avoid race condition
			// grow array if needed
			if(nb==nbmax) {
				nbmax *= 2;
				Vector3 [] temp_x = new Vector3[nbmax];
				Vector3 [] temp_v = new Vector3[nbmax];
				float [] temp_m = new float[nbmax];
				GameObject [] temp_g = new GameObject[nbmax];
				for(int j=0;j<nb;j++) temp_x[j]=x[j];
				for(int j=0;j<nb;j++) temp_v[j]=v[j];
				for(int j=0;j<nb;j++) temp_m[j]=m[j];
				for(int j=0;j<nb;j++) temp_g[j]=bodyGos[j];
				x = temp_x;
				v = temp_v;
				m = temp_m;
				bodyGos = temp_g;
				a = new Vector3[nbmax];
			}

			// add new object
			x[nb]=position;
			v[nb]=velocity;
			m[nb]=mass;
			Debug.Log ("Getting Here " + nb);
			bodyGos[nb] = (GameObject) Instantiate (bodyPRE,x[nb],Quaternion.identity);
			// adjust to conserve momentum
			float netMass = 0.0f;
			for(int j=0;j<nb;j++) netMass += m[j];
			for(int j=0;j<nb;j++) v[j] -= mass/netMass*velocity;
			//update number of objects
			nb++;
			stepFree=true;
		}


	}

	public void popObject(int j) {
		lock(_locker) {
			stepFree=false;
			// would a lock be more efficient here?
			while(stepRunning) {} // wait for step to finish to avoid race condition
			// grow array if needed



			GameObject temp = bodyGos[j];
			for(int k=j;k<nb-1;k++) {
				x[k]=x[k+1];
				v[k]=v[k+1];
				m[k]=m[k+1];
				bodyGos[k]=bodyGos[k+1];
			}
			nb--;

			Destroy (temp);
			stepFree=true;
		}


	}

	// Use this for initialization
	void Start () {
		// initialize problem
		x = new Vector3[nbmax];
		v = new Vector3[nbmax];
		a = new Vector3[nbmax];
		m = new float[nbmax];
		bodyGos = new GameObject[nbmax];


		StreamReader F = new StreamReader("Assets/input.txt");
		string line;
		while ((line = F.ReadLine())!=null) {
			Debug.Log(line);
			string [] keyval = line.Split (" \t".ToCharArray(), 2);
			for (int i = 0; i < keyval.Length; i++) {
				Debug.Log (keyval[i]);
			}
			if (keyval [0] == "OBJECT") {
				string[] values = keyval [1].Split (" \t".ToCharArray ());
				Vector3 position = new Vector3 (float.Parse(values [0]), float.Parse(values [1]), float.Parse(values [2]));
				Vector3 velocity = new Vector3 (float.Parse(values [3]),float.Parse( values [4]), float.Parse(values [5]));
				float mass = float.Parse(values [6]);
				pushObject (mass, position, velocity);
				//GameObject go = (GameObject) Instantiate (planet, position, Quaternion.identity);
				//PlanetBehaviour pb = go.GetComponent<PlanetBehaviour> ();
				//pb.velocity = velocity;
				//pb.mass = mass;
			}
		}
			
		bodyGos [0].GetComponent<Renderer> ().material.color = Color.yellow;
		//bodyGos [0].transform.localScale *= 5.0f;

		/*
		for(int j=0;j<n;j++) {
			x[j] = Random.insideUnitSphere*50.0f;
			v[j] = Random.insideUnitSphere*15.0f;
			a[j] = Vector3.zero;
			m[j] = 10000.0f/(float)n;
			bodyGos[j] = (GameObject) Instantiate (bodyPRE,x[j],Quaternion.identity);
		}*/


		if (threaded) {
			modelThread = new Thread (this.threadedActions);
			modelThread.Start ();
		}

	}

	public void threadedActions() {
		while (threadRunning) {

			try 
			{
				if(stepFree||fastrun) {
					stepRunning=true;

					//float threadDT=0.001;
					takeStep(threadDT);

					stepRunning=false;
				}
				if(!fastrun) lock(_locker) {
						stepFree=false;
					}
			} 
			catch (ThreadAbortException ex) 
			{
				threadRunning=false;
			}



		}
	}

	void Center() {
		lock(_locker) {
			stepFree=false;
			// would a lock be more efficient here?
			while(stepRunning) {} // wait for step to finish to avoid race condition

			Vector3 COM = Vector3.zero;
			Vector3 COP = Vector3.zero;
			float M = 0.0f;

			for(int j=0;j<nb;j++) {
				COM += m[j]*x[j];
				COP += m[j]*v[j];
				M += m[j];
			}
			COM /= M;
			COP /= M;

			for(int j=0;j<nb;j++) {
				x[j] -= COM;
				v[j] -= COP;
			}


			stepFree=true;
		}
	}

	void FixedUpdate() {
		if (threaded) {
			lock (_locker) {
				stepFree = true;
			}
		} else {
			stepRunning = true;

			//float threadDT=0.001;
			takeStep (fixedDT);

			stepRunning = false;
		}



		Vector3 diff = GameObject.Find ("Player").GetComponent<PlayerBehaviour> ().cc.transform.position - x [0];
		float RV = Vector3.Dot (diff / diff.magnitude, v [0]);

	}

	public void Pause(bool yesNo) {
		lock(_locker) {
			stepFree=false;
			// would a lock be more efficient here?
			while(stepRunning) {} // wait for step to finish to avoid race condition
			// grow array if needed
			paused=yesNo;
		}
	}

	void takeStep(float dt) {
		if (!paused) {
			for (int j=0; j<nb; j++) {
				a [j] = Vector3.zero;
			}

			for (int j=0; j<nb; j++) {
				for (int k=j+1; k<nb; k++) {
					Vector3 dx = (x [k] - x [j]);
					float dx2 = dx.sqrMagnitude;
					float dx3 = dx2 * dx.magnitude;
					if (dx3 > 1.0e-9) {
						a [j] += G * m [k] / dx3 * dx;
						a [k] -= G * m [j] / dx3 * dx;
						//Debug.Log ("STEP "+a[j].magnitude);
					}
				}
			}

			for (int j=0; j<nb; j++) {
				v [j] += a [j] * dt;
				x [j] += v [j] * dt;
			}
			for (int j=0; j<nb; j++) {
				bodyGos [j].GetComponent<PlanetBehaviour> ().Push (x [j]);
			}
			t += dt;
		}
	}




	// Update is called once per frame
	void Update () {
		for(int j=0;j<nb;j++) {
			bodyGos[j].transform.position=x[j];
		}
			
		//float timestep = Input.GetAxis ("Timestep");
		//threadDT *= (1.0f + 0.25f * timestep);
	
		for(int j=0;j<nb;j++) {
			if(x[j].sqrMagnitude>1000.0f) {
				popObject(j);
				break;
			}
		}

		Center ();

		float dt;
		if (threaded)
			dt = threadDT;
		else
			dt = Time.fixedDeltaTime;
		//GameObject.Find ("TimeDisplay").GetComponent<Text> ().text = string.Format ("Time = {0} dt = {1}", t, dt);

		/*
		if (Input.GetButtonDown ("Cancel")) {
			// clear all trail
			for(int j=0;j<nb;j++) {
				bodyGos[j].GetComponent<PlanetBehaviour>().Clear();
			}
		}
		*/


	}
}

