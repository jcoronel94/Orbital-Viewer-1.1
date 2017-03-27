using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class KeplerianNav : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void KeplerianNavigation()
    {
        SceneManager.LoadScene(2);
	CartesianNavigation.modeCheck = true;
    }
}
