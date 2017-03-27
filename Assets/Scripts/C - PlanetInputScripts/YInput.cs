using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;

public class YInput : MonoBehaviour

{
    //EventSystem system;
    //public static InputField inputY;
    //public static bool flagY = false;
    void Start()
    {
        var inputY = gameObject.GetComponent<InputField>();
        //inputY.readOnly = true;
        var se = new InputField.SubmitEvent();
        se.AddListener(SubmitName);
        inputY.onEndEdit = se;
        //system = EventSystem.current;


        //or simply use the line below, 
        //input.onEndEdit.AddListener(SubmitName);  // This also works
    }


    public void Update()
    {

       
    }


    private void SubmitName(string arg0)
    {
        //Debug.Log(arg0);
        //float xpos = float.Parse(arg0);
        XInput.inputs[1] = arg0;
        //ZInput.inputZ.readOnly = false;
                   
    }
}

