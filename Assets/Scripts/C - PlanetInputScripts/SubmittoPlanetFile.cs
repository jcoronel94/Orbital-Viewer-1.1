using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;

public class SubmittoPlanetFile : MonoBehaviour {
    public static int submitcount = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    public void Restart()
    {
        
        //GUI.Label (new Rect(25, 25, 100, 30), "Label");
        //Display
        /*
        File.AppendAllText(@"C:\Users\N\Documents\orbital-viewer\Orbital Viewer 1.1 - MANY CHANGES\Assets\inputC.txt", System.Environment.NewLine);
        File.AppendAllText(@"C:\Users\N\Documents\orbital-viewer\Orbital Viewer 1.1 - MANY CHANGES\Assets\inputC.txt", "OBJECT");
        File.AppendAllText(@"C:\Users\N\Documents\orbital-viewer\Orbital Viewer 1.1 - MANY CHANGES\Assets\inputC.txt", " ");
        File.AppendAllText(@"C:\Users\N\Documents\orbital-viewer\Orbital Viewer 1.1 - MANY CHANGES\Assets\inputC.txt", XInput.inputs[0]);

        File.AppendAllText(@"C:\Users\N\Documents\orbital-viewer\Orbital Viewer 1.1 - MANY CHANGES\Assets\inputC.txt", " ");
        File.AppendAllText(@"C:\Users\N\Documents\orbital-viewer\Orbital Viewer 1.1 - MANY CHANGES\Assets\inputC.txt", XInput.inputs[1]);

        File.AppendAllText(@"C:\Users\N\Documents\orbital-viewer\Orbital Viewer 1.1 - MANY CHANGES\Assets\inputC.txt", " ");
        File.AppendAllText(@"C:\Users\N\Documents\orbital-viewer\Orbital Viewer 1.1 - MANY CHANGES\Assets\inputC.txt", XInput.inputs[2]);

        File.AppendAllText(@"C:\Users\N\Documents\orbital-viewer\Orbital Viewer 1.1 - MANY CHANGES\Assets\inputC.txt", " ");
        File.AppendAllText(@"C:\Users\N\Documents\orbital-viewer\Orbital Viewer 1.1 - MANY CHANGES\Assets\inputC.txt", XInput.inputs[3]);


        File.AppendAllText(@"C:\Users\N\Documents\orbital-viewer\Orbital Viewer 1.1 - MANY CHANGES\Assets\inputC.txt", " ");
        File.AppendAllText(@"C:\Users\N\Documents\orbital-viewer\Orbital Viewer 1.1 - MANY CHANGES\Assets\inputC.txt", XInput.inputs[4]);

        File.AppendAllText(@"C:\Users\N\Documents\orbital-viewer\Orbital Viewer 1.1 - MANY CHANGES\Assets\inputC.txt", " ");
        File.AppendAllText(@"C:\Users\N\Documents\orbital-viewer\Orbital Viewer 1.1 - MANY CHANGES\Assets\inputC.txt", XInput.inputs[5]);


        File.AppendAllText(@"C:\Users\N\Documents\orbital-viewer\Orbital Viewer 1.1 - MANY CHANGES\Assets\inputC.txt", " ");
        File.AppendAllText(@"C:\Users\N\Documents\orbital-viewer\Orbital Viewer 1.1 - MANY CHANGES\Assets\inputC.txt", XInput.inputs[6]);
        */

		// parse null input for position
		if (XInput.inputs [0] == null || XInput.inputs [1] == null || XInput.inputs [2] == null) {
			//if all are null, return error, otherwise set nulls to zero.
			if (XInput.inputs [0] == null && XInput.inputs [1] == null && XInput.inputs [2] == null) {
				Debug.Log ("CANNOT LEAVE ALL POSITIONS BLANK");
			} else {
				Debug.Log ("SETTING BLANKS TO ZERO");

				for(int j=0;j<3;j++)
					if (XInput.inputs [j] == null)
						XInput.inputs [j] = "0";
			}
				
		}
		Vector3 temp_pos = new Vector3(float.Parse(XInput.inputs[0]), float.Parse(XInput.inputs[1]), float.Parse(XInput.inputs[2]));


		// parse null input for velocity
		if (XInput.inputs [3] == null || XInput.inputs [4] == null || XInput.inputs [5] == null) {
			//if all are null, find default, otherwise set nulls to zero.
			if (XInput.inputs [3] == null && XInput.inputs [4] == null && XInput.inputs [5] == null) {
				Debug.Log ("ALL VELOCITIES BLANK DETERMINE DEFAULT FROM X");
				// change me to something good

				float mu=0.0f;
		
				StreamReader reader = new StreamReader("Assets/inputC.txt");
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					string[] keyval = line.Split(" \t".ToCharArray(), 2);

					if (keyval[0] == "STAR")
					{
						string[] values = keyval[1].Split(" \t".ToCharArray());
						mu = float.Parse (values [6]) * 8.88e-10f;
					}
				}

				Vector3 velocity = GetVelocityFromPosition (mu, temp_pos);
				XInput.inputs [3] = velocity.x.ToString();
				XInput.inputs [4] = velocity.y.ToString();
				XInput.inputs [5] = velocity.z.ToString();

			} else {

				Debug.Log ("SETTING BLANKS TO ZERO");

				for(int j=3;j<6;j++)
					if (XInput.inputs [j] == null)
						XInput.inputs [j] = "0";
			}

		}
		// set default mass
		if (XInput.inputs [6] == null) {
			Debug.Log ("SETTING MASS TO DEFAULT");

			XInput.inputs [6] = "1.0";
		}

        Vector3 temp_vel = new Vector3(float.Parse(XInput.inputs[3]), float.Parse(XInput.inputs[4]), float.Parse(XInput.inputs[5]));
        


		float temp_mass = float.Parse(XInput.inputs[6]);



		ModelActions.CPlanetsPositions[submitcount] = temp_pos;
		ModelActions.CPlanetsVelocities[submitcount] = temp_vel;
		ModelActions.CPlanetsMasses[submitcount] = temp_mass;

		for (int j = 0; j < XInput.inputs.Length; j++) {
			XInput.inputs [j] = null;
		}

        submitcount++;

        SceneManager.LoadScene(4);
       
               
    }


	static Vector3 GetVelocityFromPosition(float mu,Vector3 position){


		Vector3 velocity;

		float vmag = Mathf.Sqrt(mu/position.magnitude);


		velocity.z = 0;
		if (position.x != 0 && position.y != 0) {
			velocity.y = (1 / Mathf.Sqrt ((1 + ((position.y * position.y) / (position.x * position.x))))) * vmag;
			velocity.x = -Mathf.Sqrt (((vmag * vmag) - (velocity.y * velocity.y)));
		} else if (position.x == 0) {
			velocity.x = -vmag;
			velocity.y = 0;
		} else {
			velocity.y = vmag;
			velocity.x = 0;
		}
	
		return velocity;

	}
}
