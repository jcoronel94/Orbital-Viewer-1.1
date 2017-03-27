using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;

public class YVelocity : MonoBehaviour

{
    //EventSystem system;
    //public static InputField inputYV;
   
    void Start()
    {
        var inputYV = gameObject.GetComponent<InputField>();
        //inputZ.readOnly = true;
        var se = new InputField.SubmitEvent();
        se.AddListener(SubmitName);
        inputYV.onEndEdit = se;
        //system = EventSystem.current;

        //or simply use the line below, 
        //input.onEndEdit.AddListener(SubmitName);  // This also works
    }

    private void SubmitName(string arg0)
    {
        //Debug.Log(arg0);
        //float xpos = float.Parse(arg0);


        //if (Input.GetButtonDown("Submit"))
         XInput.inputs[4] = arg0;
            //ZVelocity.inputZV.readOnly = false;
        
        //flagZ = true;
    }
}

