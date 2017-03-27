using UnityEngine;
using System.Collections;

public class PlanetBehaviour : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
    }

	public void Push(Vector3 x) {
		transform.position = x;
	}
		
}
