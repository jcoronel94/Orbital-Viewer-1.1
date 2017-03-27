using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;

public class KAsteroidAmountInput : MonoBehaviour {

    public static string[] inputs = new string[15];
    // Use this for initialization
    void Start () {
        var input = gameObject.GetComponent<InputField>();
        var se = new InputField.SubmitEvent();
        se.AddListener(SubmitName);
        input.onEndEdit = se;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    private void SubmitName(string arg0)
    {
        if (arg0 != null)
            inputs[0] = arg0;

    }

}
