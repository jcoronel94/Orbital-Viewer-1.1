using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.SceneManagement;


public class SubmitoAsteroidFile : MonoBehaviour {
    //public static int addedlines = 0;
    public static int Cnumasteroids;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SubmissiontoFile()
    {
        Cnumasteroids = int.Parse(AsteroidAmountInput.inputs[0]);
        for (int i=0; i<Cnumasteroids; i++)
        {
            
            float xpos = Random.Range(float.Parse(AsteroidAmountInput.inputs[4]), float.Parse(AsteroidAmountInput.inputs[3]));
            //Debug.Log(xpos);
            float ypos = Random.Range(float.Parse(AsteroidAmountInput.inputs[8]), float.Parse(AsteroidAmountInput.inputs[7]));
            //Debug.Log(ypos);
            float zpos = Random.Range(float.Parse(AsteroidAmountInput.inputs[12]), float.Parse(AsteroidAmountInput.inputs[11]));
            //Debug.Log(zpos);
            float xvel = Random.Range(float.Parse(AsteroidAmountInput.inputs[6]), float.Parse(AsteroidAmountInput.inputs[5]));
            //Debug.Log(xvel);
            float yvel = Random.Range(float.Parse(AsteroidAmountInput.inputs[10]), float.Parse(AsteroidAmountInput.inputs[9]));
            //Debug.Log(yvel);
            float zvel = Random.Range(float.Parse(AsteroidAmountInput.inputs[14]), float.Parse(AsteroidAmountInput.inputs[13]));
            //Debug.Log(zvel);
            float mass = Random.Range(float.Parse(AsteroidAmountInput.inputs[2]), float.Parse(AsteroidAmountInput.inputs[1]));
            //Debug.Log(mass);

            Vector3 temp_pos = new Vector3(xpos, ypos, zpos);
            Vector3 temp_vel = new Vector3(xvel, yvel, zvel);
            float temp_mass = mass;


			ModelActions.CAsteroidsPositions[i] = temp_pos;
			ModelActions.CAsteroidsVelocities[i] = temp_vel;
			ModelActions.CAsteroidsMasses[i] = temp_mass;
                        
            SceneManager.LoadScene(3);
        }
        
        //SceneManager.LoadScene(3);
    }
}
