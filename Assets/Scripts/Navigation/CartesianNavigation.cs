using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CartesianNavigation : MonoBehaviour {
	public static bool modeCheck;
	// Use this for initialization
	void Start () {
	

	}
	
	// Update is called once per frame 
	void Update () {
	
	}

    public void CartesianScreenNav()
    {
        SceneManager.LoadScene(1);
	modeCheck = false;

    }
}
