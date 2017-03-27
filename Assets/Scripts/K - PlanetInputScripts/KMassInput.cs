using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class KMassInput : MonoBehaviour

{

    //public static InputField inputM;

    void Start()
    {
        var inputM = gameObject.GetComponent<InputField>();
        //inputZ.readOnly = true;
        var se = new InputField.SubmitEvent();
        se.AddListener(SubmitName);
        inputM.onEndEdit = se;

        //or simply use the line below, 
        //input.onEndEdit.AddListener(SubmitName);  // This also works
    }

    private void SubmitName(string arg0)
    {
        //Debug.Log(arg0);
        //float xpos = float.Parse(arg0);


        //if (Input.GetButtonDown("Submit"))
        KEccentricityInput.inputs[6] = arg0;

        
        //flagZ = true;
    }
}

