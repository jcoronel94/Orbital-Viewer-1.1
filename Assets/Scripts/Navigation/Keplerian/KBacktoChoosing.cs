using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class KBacktoChoosing : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void BacktoKeplerian()
    {
        SceneManager.LoadScene(2);
    }
}
