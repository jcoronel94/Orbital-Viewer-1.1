using UnityEngine;
using System.Collections;

public class PlanetBehaviour : MonoBehaviour {

	public GameObject tracerPRE;
	public float planetMass = 0.0f;
	public Vector3d x;
	public Vector3d v;
	public Vector3d center;
	public float mu;


    // Use this for initialization
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
		/*
		GameObject go = (GameObject) Instantiate (tracerPRE, transform.position, Quaternion.identity);
		go.GetComponent<Renderer> ().material.color = GetComponent<Renderer> ().material.color;
		go.GetComponent<TracerBehaviour> ().tracerMass = planetMass;
		*/

		float planetScale=1.0f;
		if (planetMass < 1.0) {
			planetScale = 0.5f + 0.5f * planetMass;
		} else {
			planetScale = 1.0f + 0.2f*Mathf.Log10 (planetMass);
		}

		if (planetMass < GameObject.Find ("Model").GetComponent<ModelActions> ().massThreshold) {
			GetComponent<TrailRenderer> ().startWidth = TracerBehaviour.tracerSize;
			GetComponent<TrailRenderer> ().endWidth = TracerBehaviour.tracerSize;
			transform.localScale = Vector3.one * TracerBehaviour.tracerSize * 30.0f*planetScale;
		} else {
			GetComponent<TrailRenderer> ().startWidth = TracerBehaviour.tracerSize*3.0f*planetScale;
			GetComponent<TrailRenderer> ().endWidth = TracerBehaviour.tracerSize*3.0f*planetScale;
			transform.localScale = Vector3.one * TracerBehaviour.tracerSize * 30.0f*planetScale;

		}
		GetComponent<TrailRenderer> ().material.color = GetComponent<Renderer> ().material.color;


		if (Input.GetButtonDown ("Fire1")) {
			GetComponent<TrailRenderer> ().Clear ();
		}
		if (Input.GetButtonDown ("FocusUp")) {
			GetComponent<TrailRenderer> ().Clear ();

		} 
		if (Input.GetButtonDown ("FocusDown")) {
			GetComponent<TrailRenderer> ().Clear ();

		} 

		//Cartesian (x.v3(), v.v3());
    }

	public void Push(Vector3 x) {
		transform.position = x;
	}

	void Cartesian(Vector3 position, Vector3 velocity)
	{
		//need angular momentum
		Vector3 hVec;
		float h;
		//rad and vel vars
		float r;
		float v;
		//energy var
		float E;
		float mu = 0f;
		//arguemnt of latitude 
		float wplusv;
		//true anomaly var 
		float trueA;
		float EA; //eccentric anomaly

		//not sure what p stands for 
		float p;

		//Debug.Log("pos3: " + position);
		//Debug.Log("vel3: " + velocity);

		//agular momentum calculation
		hVec = Vector3.Cross(velocity, position);
		h = Mathf.Sqrt(hVec.x * hVec.x + hVec.y * hVec.y + hVec.z * hVec.z);


		//Debug.Log("x y z " + position.x + " " + position.y + " " + position.z + " ");
		// get radius and velocity
		r = Mathf.Sqrt((position.x * position.x) + (position.y * position.y) + (position.z * position.z));
		v = Mathf.Sqrt((velocity.x * velocity.x) + (velocity.y * velocity.y) + (velocity.z * velocity.z));
		//Debug.Log("r: " + r);
		//Debug.Log("v: " + v);

		/*
        if (cheeseCounter >= 1)
        {
            mu = m[0] * G; Debug.Log("mu: " + mu);


        }
        */

		//get specific enegy
		E = (v * v) / 2f - (mu / r);
		//Debug.Log("E: " + E);

		//semi major and eccentricity (0-180)
		float semiglobal = -mu / (2f * E);
		//Debug.Log("semi: " + semi);

		float eglobal = Mathf.Sqrt((1f - (h * h) / (semiglobal * mu)));
		//Debug.Log("e: " + e);
		//inc cos(i) = h_z /h
		float incglobal = Mathf.Acos(hVec.z / h);
		//Debug.Log("inc: " + inc);

		float nodeglobal;
		if (incglobal == 0f)
		{
			nodeglobal = 0f;
		}
		else
		{
			nodeglobal = Mathf.Atan2(hVec.x, -hVec.y);
		}

		if (incglobal == 0)
		{
			wplusv = Mathf.Atan2(position.y, position.x);
		}
		else
		{
			wplusv = Mathf.Atan2((position.z / Mathf.Sin(incglobal)), ((position.x * Mathf.Cos(nodeglobal)) + position.y * Mathf.Sin(nodeglobal)));
		}

		//right ascension of node (0-360)

		//nodeglobal = Mathf.Atan2(hVec.x, -hVec.y) + Mathf.PI;
		//Debug.Log("node: " + node);

		//compute argument of latitude, w + v, (0-360)



		//Debug.Log("wplusv :" + wplusv);
		//compute anomaly (0-360)
		//trueA = Mathf.Acos((semi*(1-e*e)-r)/(e*r)); ?????

		p = semiglobal * (1.0f - eglobal * eglobal);
		//Vector3.Dot(velocity, position)
		trueA = Mathf.Atan2(Mathf.Sqrt(p / mu) * Vector3.Dot(velocity, position), p - r);

		//compute argument of periapse, lil omega (0-360)
		float wglobal = wplusv - trueA;

		if (wplusv < 0)
		{
			wplusv = wplusv + Mathf.PI * 2;
		}
		//Debug.Log("w: " + w);

		//eccentric anomaly, EA, (0-360)

		EA = 2.0f * Mathf.Atan(Mathf.Sqrt((1 - eglobal) / (1 + eglobal)) * Mathf.Tan(trueA / 2.0f));
		//Debug.Log("EA: " + EA);

		float Mglobal = EA - eglobal * Mathf.Sin(EA);
		//Debug.Log("MA: " + M);
	}
		
}
