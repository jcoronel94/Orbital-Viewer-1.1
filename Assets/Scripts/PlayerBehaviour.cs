using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Threading;


public class PlayerBehaviour : MonoBehaviour {

	public CharacterController cc;

    public GameObject planetPRE;

    float lookSpeed = 100.0f;
	float moveSpeed = 30.0f;
    

    // Use this for initialization
    void Start () {
		cc = GetComponent<CharacterController> ();
       // cam1.enabled = true;
     //   cam2.enabled = false;
    }


	// Update is called once per frame
	/// <summary>
    /// 
    /// </summary>
    void Update () {

       
        float mouseX = Input.GetAxis ("Mouse X");
		float mouseY = Input.GetAxis ("Mouse Y");
		float roll = Input.GetAxis ("Roll");
		float forward = Input.GetAxis ("Vertical");
		float strafe = Input.GetAxis("Horizontal");
		float jump = Input.GetAxis ("Jump");
        if (jump > 0)
        {
            jump = 1;
            //Input.ResetInputAxes();
        } else if(jump<0.0)
        {
            jump = -1;
            //Input.ResetInputAxes();
        }

		transform.Rotate (0.5f*mouseY*Time.deltaTime*lookSpeed,
							mouseX*Time.deltaTime*lookSpeed,
							roll*Time.deltaTime*lookSpeed);

		cc.Move (moveSpeed*forward * transform.forward * Time.deltaTime
			+moveSpeed*strafe*transform.right*Time.deltaTime
			+moveSpeed*jump*transform.up*Time.deltaTime);

        //if (Input.GetKeyUp(KeyCode.Return))
        //{
            //cam1.enabled = !cam1.enabled;
          //  cam2.enabled = !cam2.enabled;


        //}

        if(Input.GetButtonDown("Fire1"))
        {
			//GameObject.Find ("Model").GetComponent<ModelActions> ().pushObject (33300.0f, transform.position + 0.5f * transform.forward,
				                  // 0.0172f * transform.forward);
        }

		if(Input.GetButtonDown("Fire2")) {
			TracerBehaviour.tracerSize *= 2.0f;
		}
		if(Input.GetButtonDown("Fire3")) {
			TracerBehaviour.tracerSize /= 2.0f;
		}
		if (Input.GetButton("Cancel"))
		{
			GameObject model = GameObject.Find("Model");
			if (model.GetComponent<ModelActions>().GetThreaded())
			{
				model.GetComponent<ModelActions>().threadRunning = false;
				System.Threading.Thread.Sleep(100);
				model.GetComponent<ModelActions>().modelThread.Abort();
			}
			Application.Quit();
		}
		if (Input.GetButtonDown ("Reset")) {
			transform.position = Vector3.zero;
		}




    }
}
