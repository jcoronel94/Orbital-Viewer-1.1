using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class ZVelocity : MonoBehaviour

{

    //public static InputField inputZV;

    void Start()
    {
        var inputZV = gameObject.GetComponent<InputField>();
        //inputZ.readOnly = true;
        var se = new InputField.SubmitEvent();
        se.AddListener(SubmitName);
        inputZV.onEndEdit = se;

        //or simply use the line below, 
        //input.onEndEdit.AddListener(SubmitName);  // This also works
    }

    private void SubmitName(string arg0)
    {
        //Debug.Log(arg0);
        //float xpos = float.Parse(arg0);


        //if (Input.GetButtonDown("Submit"))
         XInput.inputs[5] = arg0;
            //MassInput.inputM.readOnly = false;
        
        //flagZ = true;
    }
}

