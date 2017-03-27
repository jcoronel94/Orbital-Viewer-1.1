using UnityEngine;
using System.Collections;

public class TracerBehaviour : MonoBehaviour {

	public static float tracerSize = 0.01f;
	public float tracerMass = 0.0f;
	int timer = 100;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (tracerMass < GameObject.Find ("Model").GetComponent<ModelActions> ().massThreshold) {
			transform.localScale = Vector3.one * tracerSize;
		} else {
			transform.localScale = Vector3.one * tracerSize * 5.0f;
		}
		timer -= 1;
		if (timer < 0)
			Destroy (gameObject);

	}
}
