using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Threading;
using System.IO;

public class ModelActions : MonoBehaviour
{
    /*
    public float mass;
    public Vector3 position;
    public Vector3 velocity;
    */

    //public static int totallinesK = 0;
    //public static int addedlinesK = 0;

    //public static int totallinesC = 0;
    //public static int addedlinesC = 0;

    //TEMPORARY ARRAYS

	public static Vector3[] CAsteroidsPositions = new Vector3[1000];
	public static Vector3[] CPlanetsPositions = new Vector3[1000];
	public static Vector3[] KAsteroidsPositions = new Vector3[1000];
	public static Vector3[] KPlanetsPositions = new Vector3[1000];

	public static Vector3[] CAsteroidsVelocities = new Vector3[1000];
	public static Vector3[] CPlanetsVelocities = new Vector3[1000];
	public static Vector3[] KAsteroidsVelocities = new Vector3[1000];
	public static Vector3[] KPlanetsVelocities = new Vector3[1000];

	public static float[] CAsteroidsMasses = new float[1000];
	public static float[] KAsteroidsMasses = new float[1000];
	public static float[] CPlanetsMasses = new float[1000];
	public static float[] KPlanetsMasses = new float[1000];

	//for converting back to Keplerian from Cartesian
	//planets
	public static float[] PlanetsKfromCeccentricities = new float[1000];
	public static float[] PlanetsKfromClongnodes = new float[1000];
	public static float[] PlanetsKfromCsemimajors = new float[1000];
	public static float[] PlanetsKfromCperiastrons = new float[1000];
	public static float[] PlanetsKfromCinclinations = new float[1000];
	public static float[] PlanetsKfromCMeanAnomalies = new float[1000];
	//asteroids
	public static float[] AsteroidsKfromCeccentricities = new float[1000];
	public static float[] AsteroidsKfromClongnodes = new float[1000];
	public static float[] AsteroidsKfromCsemimajors = new float[1000];
	public static float[] AsteroidsKfromCperiastrons = new float[1000];
	public static float[] AsteroidsKfromCinclinations = new float[1000];
	public static float[] AsteroidsKfromCMeanAnomalies = new float[1000];


	double [] dx;
	double [] dy;
	double [] dz;
	double [] dvx;
	double [] dvy;
	double [] dvz;
	double [] dax;
	double [] day;
	double [] daz;
	double [] dm;
	double [] dhr;
	int [] itd;

    int nb = 0;
    int nbmax = 100;
    Vector3d[] x;
    Vector3d[] v;
    Vector3d[] a;
    float[] hillRadius;
    bool[] toBeRemoved;


    float[] m;

    public float massThreshold = 1.0e-3f;
    float shieldRadius = 1.0e-3f;

    float xvel = 0f;
    float yvel = 0f;
    float zvel = 0f;
    float xh = 0f;
    float yh = 0f;
    float zh = 0f;
    float cheeseCounter = 0;

    public GameObject bodyPRE;
    GameObject[] bodyGos;

    public float timescale = 1.0f;

    float G = 8.88e-10f;


    float modelDT = 0.01f;

    public Thread modelThread;

    public bool threadRunning = false;
    public bool stepFree = true;
    public bool stepRunning = false;
    public float t = 0.0f;

    bool threaded = true;
    bool fastrun = true;

    bool paused = false;

	bool useDLL=true;

	public int centerObject = 0;

    static readonly object _locker = new object();

    public bool GetThreaded()
    {
        return threaded;
    }



    public void pushObject(float mass, Vector3 position, Vector3 velocity, float hillR)
    {
        lock (_locker)
        {
            stepFree = false;
            // would a lock be more efficient here?
            while (stepRunning) { } // wait for step to finish to avoid race condition
                                    // grow array if needed
            if (nb == nbmax)
            {
                nbmax *= 2;
                Vector3d[] temp_x = new Vector3d[nbmax];
                Vector3d[] temp_v = new Vector3d[nbmax];
                bool[] temp_remove = new bool[nbmax];
                float[] temp_m = new float[nbmax];
                float[] temp_hr = new float[nbmax];
                GameObject[] temp_g = new GameObject[nbmax];
                for (int j = 0; j < nb; j++) temp_x[j] = x[j];
                for (int j = 0; j < nb; j++) temp_v[j] = v[j];
                for (int j = 0; j < nb; j++) temp_m[j] = m[j];
                for (int j = 0; j < nb; j++) temp_remove[j] = toBeRemoved[j];
                for (int j = 0; j < nb; j++) temp_hr[j] = hillRadius[j];
                for (int j = 0; j < nb; j++) temp_g[j] = bodyGos[j];
                x = temp_x;
                v = temp_v;
                m = temp_m;
                hillRadius = temp_hr;
                toBeRemoved = temp_remove;
                bodyGos = temp_g;
                a = new Vector3d[nbmax];


				dx = new double[nbmax];
				dy = new double[nbmax];
				dz = new double[nbmax];
				dvx = new double[nbmax];
				dvy = new double[nbmax];
				dvz = new double[nbmax];
				dax = new double[nbmax];
				day = new double[nbmax];
				daz = new double[nbmax];
				dm = new double[nbmax];
				dhr = new double[nbmax];
				itd = new int[nbmax];
				for (int j = 0; j < nbmax; j++)
					itd [j] = 0;
            }

            // add new object
			x[nb].Set(position);
			v[nb].Set(velocity);
            m[nb] = mass;
            hillRadius[nb] = hillR;
            toBeRemoved[nb] = false;
            //Debug.Log("Getting Here " + nb);
			bodyGos[nb] = (GameObject)Instantiate(bodyPRE, x[nb].v3(), Quaternion.identity);
			bodyGos [nb].GetComponent<PlanetBehaviour> ().planetMass = m [nb];
            // adjust to conserve momentum
            float netMass = 0.0f;
            for (int j = 0; j < nb; j++) netMass += m[j];
            //Debug.Log("Getting Here ");
			Vector3d velocityd = new Vector3d(velocity);
            for (int j = 0; j < nb; j++) v[j] -= mass / netMass * velocityd;
            //update number of objects
            nb++;
            stepFree = true;
        }

    }

    public void popObject(int j)
    {
        lock (_locker)
        {
            stepFree = false;
            // would a lock be more efficient here?
            while (stepRunning) { } // wait for step to finish to avoid race condition
                                    // grow array if needed



            GameObject temp = bodyGos[j];
            for (int k = j; k < nb - 1; k++)
            {
                x[k] = x[k + 1];
                v[k] = v[k + 1];
                m[k] = m[k + 1];
                hillRadius[k] = hillRadius[k + 1];
				toBeRemoved[k] = toBeRemoved[k + 1];
				itd[k] = itd[k + 1];
                bodyGos[k] = bodyGos[k + 1];



            }
            nb--;

            Destroy(temp);
            stepFree = true;
        }


    }

    // Use this for initialization
    void Start()
    {
        // initialize problem
        x = new Vector3d[nbmax];
        v = new Vector3d[nbmax];
        a = new Vector3d[nbmax];
        m = new float[nbmax];

        float hillR=0f;
        hillRadius = new float[nbmax];
        toBeRemoved = new bool[nbmax];
        bodyGos = new GameObject[nbmax];
        Vector3 position;
        Vector3 velocity;
        float mass;
	
	
		dx = new double[nbmax];
		dy = new double[nbmax];
		dz = new double[nbmax];
		dvx = new double[nbmax];
		dvy = new double[nbmax];
		dvz = new double[nbmax];
		dax = new double[nbmax];
		day = new double[nbmax];
		daz = new double[nbmax];
		dm = new double[nbmax];
		dhr = new double[nbmax];
		itd = new int[nbmax];
		for (int j = 0; j < nbmax; j++)
			itd [j] = 0;

        //if(){
        //StreamReader F = new StreamReader("Assets/input.txt");
        //}

        StreamReader F;
        if (!CartesianNavigation.modeCheck)
        {
            F = new StreamReader("Assets/inputC.txt");
        }
        else
        {
            F = new StreamReader("Assets/inputK.txt");
        }

        string line;
        while ((line = F.ReadLine()) != null)
        {
            //totallines++;
            //Debug.Log(line);
            string[] keyval = line.Split(" \t".ToCharArray(), 2);
            //for (int i = 0; i < keyval.Length; i++)
            //{
                //Debug.Log (keyval[i]);
            //}
            if (keyval[0] == "PLANET")
            {
                string[] values = keyval[1].Split(" \t".ToCharArray());

                //if(cart){
                //Vector3 position = new Vector3 (float.Parse(values [0]), float.Parse(values [1]), float.Parse(values [2]));
                //Vector3 velocity = new Vector3 (float.Parse(values [3]),float.Parse( values [4]), float.Parse(values [5]));
                //}

                if (!CartesianNavigation.modeCheck)
                {
                    position = new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]));
                    velocity = new Vector3(float.Parse(values[3]), float.Parse(values[4]), float.Parse(values[5]));
                    mass = float.Parse(values[6]);
                    hillR = 0;
                }

                else
                {
                    float eV = float.Parse(values[0]);
                    float nodeV = float.Parse(values[1]);
                    float semiV = float.Parse(values[2]);
                    float wV = float.Parse(values[3]);
                    float incV = float.Parse(values[4]);
                    float MV = float.Parse(values[5]);
                    mass = float.Parse(values[6]);


                    
                    if (cheeseCounter >= 1)
                    {
                        hillR = semiV * (1 - eV) * Mathf.Pow(mass / (3.0f * m[0]), .3333f);
                    }
                    else
                    {
                        hillR = 0.004650467923f;
                        //Debug.Log(hillR);
                    }
                    kepler(eV, nodeV, semiV, wV, incV, MV);

                    position = new Vector3(xh, yh, zh);
                    //Debug.Log(position);
                    velocity = new Vector3(xvel, yvel, zvel);
                }
               
                pushObject(mass, position, velocity, hillR);
                
            }
            else
            {
                string[] values = keyval[1].Split(" \t".ToCharArray());

               

                if (!CartesianNavigation.modeCheck)
                {
                    position = new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]));
                    velocity = new Vector3(float.Parse(values[3]), float.Parse(values[4]), float.Parse(values[5]));
                    mass = float.Parse(values[6]);
                    hillR = 0;
                }

                else
                {
                    float eV = float.Parse(values[0]);
                    float nodeV = float.Parse(values[1]);
                    float semiV = float.Parse(values[2]);
                    float wV = float.Parse(values[3]);
                    float incV = float.Parse(values[4]);
                    float MV = float.Parse(values[5]);
                    mass = float.Parse(values[6]);



                    if (cheeseCounter >= 1)
                    {
                        hillR = semiV * (1 - eV) * Mathf.Pow(mass / (3.0f * m[0]), .3333f);
                    }
                    else
                    {
                        hillR = 0.004650467923f;
						cheeseCounter++;
                    }
                    kepler(eV, nodeV, semiV, wV, incV, MV);

                    position = new Vector3(xh, yh, zh);
                    
                    velocity = new Vector3(xvel, yvel, zvel);
                }
                
                pushObject(mass, position, velocity, hillR);
            }                   

        }
        for (int i = 0; i < KSubmittoPlanetFile.submitcount; i++)
        {
            pushObject(KPlanetsMasses[i], KPlanetsPositions[i], KPlanetsVelocities[i], 0);
        }
        
        for (int i = 0; i < KSubmitoAsteroidFile.Knumasteroids; i++)
        {
            pushObject(KAsteroidsMasses[i], KAsteroidsPositions[i], KAsteroidsVelocities[i], 0);
        }

        for (int i = 0; i < SubmitoAsteroidFile.Cnumasteroids; i++)
        {
            pushObject(CAsteroidsMasses[i], CAsteroidsPositions[i], CAsteroidsVelocities[i], 0);
        }

        for (int i = 0; i < SubmittoPlanetFile.submitcount; i++)
        {
            pushObject(CPlanetsMasses[i], CPlanetsPositions[i], CPlanetsVelocities[i], 0);
        }
        

        bodyGos[0].GetComponent<Renderer>().material.color = Color.yellow;
        for(int j = 1; j< nb; j++)
        {
			if (m [j] <= massThreshold) {
				bodyGos [j].GetComponent<Renderer> ().material.color = Color.green;
			} else {
				bodyGos [j].GetComponent<Renderer> ().material.color = Color.white;
			}
        }
                
        if (threaded)
        {
            modelThread = new Thread(this.threadedActions);
            modelThread.Start();
        }

    }

    public void threadedActions()
    {
        while (threadRunning)
        {
			Thread.Sleep (0);

            try
            {
                if (stepFree || fastrun)
                {
                    stepRunning = true;

                    //float threadDT=0.001;
                    takeStep(modelDT*timescale);

                    stepRunning = false;
                }
                if (!fastrun)
                    lock (_locker)
                    {
                        stepFree = false;
                    }
            }
            //(ThreadAbortException ex) 
            catch
            {
                threadRunning = false;
            }



        }
    }

    void Center()
    {
        lock (_locker)
        {
            stepFree = false;
            // would a lock be more efficient here?
            while (stepRunning) { } // wait for step to finish to avoid race condition

            Vector3d COM = Vector3d.zero;
            Vector3d COP = Vector3d.zero;
            float M = 0.0f;

            for (int j = 0; j < nb; j++)
            {
                COM += m[j] * x[j];
                COP += m[j] * v[j];
                M += m[j];
            }
            COM /= M;
            COP /= M;

            for (int j = 0; j < nb; j++)
            {
                x[j] -= COM;
                v[j] -= COP;
            }


            stepFree = true;
        }
    }

    void FixedUpdate()
    {
        if (threaded)
        {
            lock (_locker)
            {
                stepFree = true;
            }
        }
        else
        {
            stepRunning = true;

            //float threadDT=0.001;
            takeStep(modelDT);

            stepRunning = false;
        }



        //Vector3 diff = GameObject.Find ("Player").GetComponent<PlayerBehaviour> ().cc.transform.position - x [0];
        //float RV = Vector3.Dot (diff / diff.magnitude, v [0]);

    }

    public void Pause(bool yesNo)
    {
        lock (_locker)
        {
            stepFree = false;
            // would a lock be more efficient here?
            while (stepRunning) { } // wait for step to finish to avoid race condition
                                    // grow array if needed
            paused = yesNo;
        }
    }
		
    void takeStep(float dt)
    {
		hillRadius [0] = 0.0f;
		for (int j = 1; j < nb; j++) {
			if (m [j] > massThreshold) {
				double a = x [j].magnitude;
				hillRadius [j] = (float)(a * Mathd.Pow ((double)m [j] / (3.0*(double)m [0]), 0.33333));
			} else {
				hillRadius [j] = 0.0f;
			}
		}
		float captureMultiplier = 3.0f;
        if (!paused)
        {
			if (useDLL) {
				for (int j = 0; j < nb; j++) {
					dx [j] = x [j].x;
					dy [j] = x [j].y;
					dz [j] = x [j].z;
					dvx [j] = v [j].x;
					dvy [j] = v [j].y;
					dvz [j] = v [j].z;
					dm [j] = m [j];
					dhr [j] = captureMultiplier*hillRadius [j];
				}
				Wrappers.Wrapper_TakeStepDLL (itd, dx, dy, dz, dvx, dvy, dvz,
					dax, day, daz, dm, dhr, nb, (double)dt, (double)G, (double)shieldRadius, (double)massThreshold);
				for (int j = 0; j < nb; j++) {
					x [j].x = (float)dx [j];
					x [j].y = (float)dy [j];
					x [j].z = (float)dz [j];
					v [j].x = (float)dvx [j];
					v [j].y = (float)dvy [j];
					v [j].z = (float)dvz [j];
					if (itd [j] > 0) {
						toBeRemoved [j] = true;
					}
				}
			} else {
				for (int j = 0; j < nb; j++)
				{
					a[j] = Vector3d.zero;
				}

				for (int j = 0; j < nb; j++)
				{
					for (int k = j + 1; k < nb; k++)
					{
						if (m[k] > massThreshold || m[j] > massThreshold)
						{
							Vector3d dx = (x[k] - x[j]);
							double dxm = dx.magnitude;
							//float dx2 = dxm * dxm;
							double dx3 = dxm*dxm * dxm;
							if (m[j] <= massThreshold)
							{
								if (dxm < captureMultiplier*hillRadius[k])
								{
									toBeRemoved[j] = true;
								}
							}
							if (m[k] <= massThreshold)
							{
								if (dxm < captureMultiplier*hillRadius[j])
								{
									toBeRemoved[k] = true;
								}
							}
							if (dxm > shieldRadius)
							{
								if (m[k] > massThreshold)
								{
									a[j] += G * m[k] / dx3 * dx;
								}
								if (m[j] > massThreshold)
								{
									a[k] -= G * m[j] / dx3 * dx;
								}



							}
						}
					}
				}



				for (int j = 0; j < nb; j++)
				{
					v[j] += a[j] * dt;
					x[j] += v[j] * dt;
				}
			}
            
			/*
            for (int j = 0; j < nb; j++)
            {
                bodyGos[j].GetComponent<PlanetBehaviour>().Push(x[j]);
            }
            */
            t += dt;
        }
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
            //Debug.Log ("ECC:" +E [j]);
        }

        //}

        // use to get true anomoly and specific ang momemtum

        //xv = Mathf.Cos((E [nb])) - e [nb];
        //yv = Mathf.Sqrt (1f - e [nb] * e [nb]) * Mathf.Sin (E [nb]);


        trueA = 2.0f * Mathf.Atan2(Mathf.Sqrt(1.0f + eV) * Mathf.Sin(EV / 2.0f), Mathf.Sqrt(1.0f - eV) * Mathf.Cos(EV / 2.0f));
        //r [j] = Mathf.Sqrt (xv * xv + yv * yv);
        r = semiV * (1.0f - eV * Mathf.Cos(EV));
        //Debug.Log("r:" + r);

        //p = semiV * (1.0f - eV * eV);
        //specific angular momentum
        if (cheeseCounter >= 1)
        {
            //Debug.Log (m [0]);
            mu = m[0] * G; //TOTALLY FORGET TO THIS 
                           //Debug.Log(mu);
            h = Mathf.Sqrt((mu * semiV) * (1.0f - eV * eV)); //include reduced mass at some point

            //
        }
        //Debug.Log ("h:" + h);
        //Debug.Log ("node:" + nodeV);
        //Debug.Log ("trueA:" + trueA);
        //Debug.Log ("w:" + wV);
        //Debug.Log ("inc:" + incV);
        //Debug.Log ("eV:" + eV);
        //Debug.Log ("semi:" + semiV);
        //Debug.Log ("EV:" + EV);


        // calculate position vector 
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
        cheeseCounter++;
        //------------------------------------------------------------------------------------------------------------
    }






    // Update is called once per frame
    void Update()
    {
        for (int j = 0; j < nb; j++)
        {
			bodyGos [j].transform.position = (x [j] - x [centerObject]).v3 ();
			PlanetBehaviour pb = bodyGos [j].GetComponent<PlanetBehaviour> ();
			pb.x = x [j];
			pb.center = x [centerObject];
			pb.v = v [j];
			pb.mu = (float)(m [0] * G);
        }

        //float timestep = Input.GetAxis ("Timestep");
        //threadDT *= (1.0f + 0.25f * timestep);


        for (int j = 0; j < nb; j++)
        {
            if (x[j].sqrMagnitude > 1000.0f || toBeRemoved[j] == true)
            {
                popObject(j);
                break;
            }
        }

        //Center();


		float timeAxis = Input.GetAxis ("timeAxis");
		if (timeAxis != 0.0f) {
			modelDT *= (1.0f + Time.deltaTime*timeAxis * 1.0f);
			Debug.Log (modelDT);
		}

		if (Input.GetButtonDown ("FocusUp")) {
			centerObject++;
			centerObject = centerObject % nb;
		} 
		if (Input.GetButtonDown ("FocusDown")) {
			centerObject--;
			centerObject = centerObject % nb;
		} 


        //float dt;
        //if (threaded)
        //	dt = threadDT;
        //else
        //	dt = Time.fixedDeltaTime;

		GameObject.Find ("TimeDisplay").GetComponent<Text> ().text = string.Format ("DT = {0:####.0000}  ",modelDT);
		GameObject.Find ("FocusDisplay").GetComponent<Text> ().text = string.Format ("Focus = {0}  ",centerObject);
		GameObject.Find ("NDisplay").GetComponent<Text> ().text = string.Format ("N = {0}  ",nb);

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

