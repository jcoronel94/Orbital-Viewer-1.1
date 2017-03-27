using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;

public class KSubmittoPlanetFile : MonoBehaviour {
    float xvel = 0f;
    float yvel = 0f;
    float zvel = 0f;
    float xh = 0f;
    float yh = 0f;
    float zh = 0f;
    //float mu = 0f;
    //float Sunmass = 0f;
    //float cheeseCounter = 0;

    //globals for cart
    float eglobal;
    float nodeglobal;
    float Mglobal;
    float wglobal;
    float incglobal;
    float semiglobal;

    
    public static int submitcount=0;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    

    public void Restart()
    {

		// check for null inputs
		if (KEccentricityInput.inputs [0] == null) { // eccentricity
			KEccentricityInput.inputs[0] = "0.0"; //default to zero
			Debug.Log("defaulting eccentricity to zero");
		}
		if (KEccentricityInput.inputs [1] == null) { // ascending node
			KEccentricityInput.inputs[1] = "0.0"; //default to zero
			Debug.Log("defaulting ascending node to zero");
		}
		if (KEccentricityInput.inputs [2] == null) { // semi major axis
			Debug.Log("DEFAULT FOR SEMI-MAJOR AXIS NOT ALLOWED");
		}
		if (KEccentricityInput.inputs [3] == null) { // omega
			KEccentricityInput.inputs[3] = "0.0"; //default to zero
			Debug.Log("defaulting omega to zero");
		}
		if (KEccentricityInput.inputs [4] == null) { // inclination
			KEccentricityInput.inputs[4] = "0.0"; //default to zero
			Debug.Log("defaulting inclination to zero");
		}
		if (KEccentricityInput.inputs [5] == null) { // mean anomoly
			KEccentricityInput.inputs[5] = "0.0"; //default to zero
			Debug.Log("defaulting mean anomoly to zero");
		}
		if (KEccentricityInput.inputs [6] == null) { // mass
			KEccentricityInput.inputs[6] = "1.0"; //default to 1.0
			Debug.Log("defaulting mass to 1.0");
		}


        kepler(float.Parse(KEccentricityInput.inputs[0]), float.Parse(KEccentricityInput.inputs[1]), float.Parse(KEccentricityInput.inputs[2]), float.Parse(KEccentricityInput.inputs[3]), float.Parse(KEccentricityInput.inputs[4]), float.Parse(KEccentricityInput.inputs[5]));

        Vector3 temp_pos = new Vector3(xh, yh, zh);
        Vector3 temp_vel = new Vector3(xvel, yvel, zvel);
        float temp_mass = float.Parse(KEccentricityInput.inputs[6]);



		ModelActions.KPlanetsPositions[submitcount] = temp_pos;
		ModelActions.KPlanetsVelocities[submitcount] = temp_vel;
		ModelActions.KPlanetsMasses[submitcount] = temp_mass;

        Debug.Log(temp_pos.x +" " + temp_pos.y +" "+ temp_pos.z);
        Debug.Log(temp_vel.x + " " + temp_vel.y + " " + temp_vel.z);

        Cartesian(temp_pos, temp_vel);



		ModelActions.PlanetsKfromCeccentricities[submitcount] = eglobal;
		ModelActions.PlanetsKfromClongnodes[submitcount] = nodeglobal;
		ModelActions.PlanetsKfromCsemimajors[submitcount] = semiglobal;
		ModelActions.PlanetsKfromCperiastrons[submitcount] = wglobal;
		ModelActions.PlanetsKfromCinclinations[submitcount] = incglobal;
		ModelActions.PlanetsKfromCMeanAnomalies[submitcount] = Mglobal;


		for (int j = 0; j < KEccentricityInput.inputs.Length; j++) {
			KEccentricityInput.inputs [j] = null;
		}

        submitcount++;

        SceneManager.LoadScene(6);
               
    }
    void kepler(float eV, float nodeV, float semiV, float wV, float incV, float MV)
    {
        float mu = 0f;
        //iterative ecc anomaly variables 
        float E0 = 0f;
        float E1 = 100f;
        float EV = 0f;
        float temp = 0f;
        //variables for true anamoly
        float trueA = 0f;
        //get that hill radius 


        //float xv = 0f;
        //float yv = 0f;
        //variables for position
        float r = 0f;

        //variables for velocity
        //float p; //a(1-e^2)

        float h = 0f;

        //--------------------------------------------------------------------------------------------------------------

        //float oldM;
        //calculate mean anomaly. mu is G*M
        //for (int j = 0; j < nb; j++) {
        //oldM = M [j];
        //M [j] = oldM + dt * Mathf.Sqrt (mu / semi [j] * semi [j] * semi [j]);
        //Debug.Log ("Mean:" +M [j]);
        //}

        //need to get better approx of mean eccentricity if eccentricity is higher than .06?. 

        EV = MV + eV * Mathf.Sin(MV) * (1.0f + eV * Mathf.Cos(MV));

        if (eV > .06f)
        {
            E0 = EV;

            while (Mathf.Abs(E1 - temp) >= .000001)
            {

                temp = E0;

                E1 = E0 - (E0 - MV - (eV * Mathf.Sin(E0))) / (1.0f - (eV * Mathf.Cos(E0)));

                E0 = E1;

            }
            EV = E0;
            
        }

        trueA = 2.0f * Mathf.Atan2(Mathf.Sqrt(1.0f + eV) * Mathf.Sin(EV / 2.0f), Mathf.Sqrt(1.0f - eV) * Mathf.Cos(EV / 2.0f));
        
        r = semiV * (1.0f - eV * Mathf.Cos(EV));
        
        StreamReader reader = new StreamReader("Assets/inputK.txt");
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            string[] keyval = line.Split(" \t".ToCharArray(), 2);
            
            if (keyval[0] == "STAR")
            {
                string[] values = keyval[1].Split(" \t".ToCharArray());
                mu = float.Parse(values[6]) * 8.88e-10f;
            }
        }

        h = Mathf.Sqrt((1f - eV * eV)) * (semiV * mu);
        Debug.Log(+h);

        xh = (r * Mathf.Cos(trueA)) * (Mathf.Cos(wV) * Mathf.Cos(nodeV) - Mathf.Sin(wV) * Mathf.Cos(incV) * Mathf.Sin(nodeV)) - (r * Mathf.Sin(trueA)) * (Mathf.Sin(wV) * Mathf.Cos(nodeV) + Mathf.Cos(wV) * Mathf.Cos(incV) * Mathf.Sin(nodeV));
        yh = (r * Mathf.Cos(trueA)) * (Mathf.Cos(wV) * Mathf.Sin(nodeV) + Mathf.Sin(wV) * Mathf.Cos(incV) * Mathf.Cos(nodeV)) + (r * Mathf.Sin(trueA)) * (Mathf.Cos(wV) * Mathf.Cos(incV) * Mathf.Cos(nodeV) - Mathf.Sin(wV) * Mathf.Sin(nodeV));
        zh = (r * Mathf.Cos(trueA)) * (Mathf.Sin(wV) * Mathf.Sin(incV)) + (r * Mathf.Sin(trueA)) * (Mathf.Cos(wV) * Mathf.Sin(incV));
        //xh = r * (Mathf.Cos (nodeV) * Mathf.Cos (trueA  + wV) - Mathf.Sin (nodeV ) * Mathf.Sin (trueA* wV)  * Mathf.Cos (incV));
        //yh = r * (Mathf.Sin (nodeV) * Mathf.Cos (trueA + wV) + Mathf.Cos (nodeV) * Mathf.Sin (trueA * wV) * Mathf.Cos (incV));
        //zh = r * (Mathf.Sin (trueA  + wV) * Mathf.Sin (incV));
        //Debug.Log(xh + " " + yh + " " + zh);
        //x [j] = new Vector3 (xh, yh, zh);
        //x [j] = new Vector3 (xh, yh, zh);
        //Debug.Log ("pos:" +x [j]);
        //calculate velocity vec
        if (h != 0)
        {
            xvel = (((-1.0f * Mathf.Sqrt(semiV * mu)) / r) * Mathf.Sin(EV)) * (Mathf.Cos(wV) * Mathf.Cos(nodeV) - Mathf.Sin(wV) * Mathf.Cos(incV) * Mathf.Sin(nodeV)) - (Mathf.Sqrt(mu * semiV) / r) * (Mathf.Sqrt(1.0f - eV * eV) * Mathf.Cos(EV)) * (Mathf.Sin(wV) * Mathf.Cos(nodeV) + Mathf.Cos(wV) * Mathf.Cos(incV) * Mathf.Sin(nodeV));
            yvel = (((-1.0f * Mathf.Sqrt(semiV * mu)) / r) * Mathf.Sin(EV)) * (Mathf.Cos(wV) * Mathf.Sin(nodeV) + Mathf.Sin(wV) * Mathf.Cos(incV) * Mathf.Cos(nodeV)) + (Mathf.Sqrt(mu * semiV) / r) * (Mathf.Sqrt(1.0f - eV * eV) * Mathf.Cos(EV)) * (Mathf.Cos(wV) * Mathf.Cos(incV) * Mathf.Cos(nodeV) - Mathf.Sin(wV) * Mathf.Sin(nodeV));
            zvel = (((-1.0f * Mathf.Sqrt(semiV * mu)) / r) * Mathf.Sin(EV)) * (Mathf.Sin(wV) * Mathf.Sin(incV)) + (Mathf.Sqrt(mu * semiV) / r) * (Mathf.Sqrt(1.0f - eV * eV) * Mathf.Cos(EV)) * (Mathf.Cos(wV) * Mathf.Sin(incV));
            //xvel = ((xh * h * eV) / (r * p)) * Mathf.Sin (trueA) - (h / r) * (Mathf.Cos (nodeV) * Mathf.Sin (trueA + wV) + Mathf.Sin (nodeV) * Mathf.Cos (trueA * wV) * Mathf.Cos (incV));
            //yvel = ((yh * h * eV) / (r * p)) * Mathf.Sin (trueA) - (h / r) * (Mathf.Sin (nodeV) * Mathf.Sin (trueA + wV) - Mathf.Cos (nodeV) * Mathf.Cos (trueA * wV) * Mathf.Cos (incV));
            //zvel = ((zh * h * eV) / (r * p)) * Mathf.Sin (trueA) + (h / r) * (Mathf.Sin (incV) * Mathf.Sin (trueA + wV));
        }
        else
        {
            xvel = 0;
            yvel = 0;
            zvel = 0;
        }
        //v [j] = new Vector3 (xvel, yvel, zvel);
        //Debug.Log(xvel + " " + yvel + " " + zvel);
        //cheeseCounter++;
        //------------------------------------------------------------------------------------------------------------
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
        hVec = Vector3.Cross(position, velocity);
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

        StreamReader reader = new StreamReader("Assets/inputK.txt");
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            string[] keyval = line.Split(" \t".ToCharArray(), 2);

            if (keyval[0] == "STAR")
            {
                string[] values = keyval[1].Split(" \t".ToCharArray());
                //Debug.Log(values[6]);
                mu = float.Parse(values[6]) * 8.88e-10f;
                //Debug.Log(mu);
            }
        }


        //get specific enegy
        E = (v * v) / 2f - (mu / r);
        //Debug.Log("E: " + E);

        //semi major and eccentricity (0-180)
        semiglobal = -mu / (2f * E);
        //Debug.Log("semi: " + semi);

        eglobal = Mathf.Sqrt((1f - (h * h) / (semiglobal * mu)));
        //Debug.Log("e: " + e);
        //inc cos(i) = h_z /h
        incglobal = Mathf.Acos(hVec.z / h);
        //Debug.Log("inc: " + inc);
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

        //wplusv = Mathf.Atan2((position.z / Mathf.Sin(incglobal)), ((position.x * Mathf.Cos(nodeglobal)) + position.y * Mathf.Sin(nodeglobal)));

        //Debug.Log("wplusv :" + wplusv);
        //compute anomaly (0-360)
        //trueA = Mathf.Acos((semi*(1-e*e)-r)/(e*r)); ?????

        p = semiglobal * (1.0f - eglobal * eglobal);
        //Vector3.Dot(velocity, position)
        trueA = Mathf.Atan2(Mathf.Sqrt(p / mu) * Vector3.Dot(velocity, position), p - r);

        //compute argument of periapse, lil omega (0-360)
        wglobal = wplusv - trueA;
        //Debug.Log("w: " + w);
        if (wplusv < 0)
        {
            wplusv = wplusv + Mathf.PI * 2;
        }

        //eccentric anomaly, EA, (0-360)

        EA = 2.0f * Mathf.Atan(Mathf.Sqrt((1 - eglobal) / (1 + eglobal)) * Mathf.Tan(trueA / 2.0f));
        //Debug.Log("EA: " + EA);

        Mglobal = EA - eglobal * Mathf.Sin(EA);
        //Debug.Log("MA: " + M);
    }
}
