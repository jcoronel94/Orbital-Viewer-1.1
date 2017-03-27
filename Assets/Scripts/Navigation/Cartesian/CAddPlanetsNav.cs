using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CAddPlanetsNav : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void CAddPlanetsNavigation()
    {
        SceneManager.LoadScene(4);
    }
}
