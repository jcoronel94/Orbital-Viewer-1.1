using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class KSemiMajorInput : MonoBehaviour

{

    //public static InputField inputZ;
    //public static bool flagZ = false;
    void Start()
    {
        var inputZ = gameObject.GetComponent<InputField>();
        //inputZ.readOnly = true;
        var se = new InputField.SubmitEvent();
        se.AddListener(SubmitName);
        inputZ.onEndEdit = se;

        //or simply use the line below, 
        //input.onEndEdit.AddListener(SubmitName);  // This also works
    }

    private void SubmitName(string arg0)
    {
        //Debug.Log(arg0);
        //float xpos = float.Parse(arg0);


        //if (Input.GetButtonDown("Submit"))
        KEccentricityInput.inputs[2] = arg0;
            //XVelocity.inputXV.readOnly = false;
        
        //flagZ = true;
    }
}

