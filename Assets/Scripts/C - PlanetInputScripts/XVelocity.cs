using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class XVelocity : MonoBehaviour

{

    //public static InputField inputXV;
    
    void Start()
    {
        var inputXV = gameObject.GetComponent<InputField>();
        //inputXV.readOnly = true;
        var se = new InputField.SubmitEvent();

     
        se.AddListener(SubmitName);
        inputXV.onEndEdit = se;

        //or simply use the line below, 
        //input.onEndEdit.AddListener(SubmitName);  // This also works
    }

    private void SubmitName(string arg0)
    {
        //Debug.Log(arg0);
        //float xpos = float.Parse(arg0);


        //if (Input.GetButtonDown("Submit"))
        XInput.inputs[3] = arg0;
            
            //YVelocity.inputYV.readOnly = false;
        

    
    }
}

