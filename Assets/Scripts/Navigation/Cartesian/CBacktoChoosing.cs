using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CBacktoChoosing : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void BacktoCartesianMenu()
    {
        SceneManager.LoadScene(1);
    }
}
