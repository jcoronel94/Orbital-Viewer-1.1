﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class KIncMaxInput : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        var input = gameObject.GetComponent<InputField>();
        var se = new InputField.SubmitEvent();
        se.AddListener(SubmitName);
        input.onEndEdit = se;
    }

    // Update is called once per frame
    void Update()
    {


    }

    private void SubmitName(string arg0)
    {
        KAsteroidAmountInput.inputs[9] = arg0;

    }
}
