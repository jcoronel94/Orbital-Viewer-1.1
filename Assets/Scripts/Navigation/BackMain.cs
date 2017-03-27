using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BackMain : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ReturntoMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
