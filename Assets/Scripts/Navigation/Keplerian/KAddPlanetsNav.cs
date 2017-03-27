using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class KAddPlanetsNav : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void KAddPlanetsNavigation()
    {
        SceneManager.LoadScene(6);
    }
}
