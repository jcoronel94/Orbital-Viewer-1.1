using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FilenameInputScript : MonoBehaviour {
    public static string filename = "";
	// Use this for initialization
	void Start () {
        var input = gameObject.GetComponent<InputField>();
        var se = new InputField.SubmitEvent();
        se.AddListener(SubmitFileName);
        input.onEndEdit = se;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private void SubmitFileName(string arg)
    {
        if (arg != null)
            filename = arg;

    }
}
