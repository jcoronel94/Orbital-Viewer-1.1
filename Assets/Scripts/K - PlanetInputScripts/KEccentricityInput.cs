using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;

public class KEccentricityInput : MonoBehaviour
{

    public static string[] inputs = new string[7];
    //EventSystem system;
    // public static bool flagX = false;
    void Start()
    {

        var input = gameObject.GetComponent<InputField>();
        var se = new InputField.SubmitEvent();
        se.AddListener(SubmitName);
        input.onEndEdit = se;
        //system = EventSystem.current;
        //or simply use the line below, 
        //input.onEndEdit.AddListener(SubmitName);  // This also works
    }


    public void Update()
    {
        /*
        system = EventSystem.current;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();

            if (next != null)
            {

                InputField inputfield = next.GetComponent<InputField>();
                if (inputfield != null) inputfield.OnPointerClick(new PointerEventData(system));  //if it's an input field, also set the text caret

                system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
            }
            //else Debug.Log("next nagivation element not found");

        }
        */
    }


    private void SubmitName(string arg0)
    {

        // if (Input.GetButtonDown("Submit"))
        // {
        //Debug.Log("1: " + arg0);
        inputs[0] = arg0;
            
            // flagX = true;
            //YInput.inputY.readOnly = false;
       // }
        
    }
}

