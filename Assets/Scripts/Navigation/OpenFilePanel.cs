using UnityEngine;
using System.Collections;

public class OpenFilePanel : MonoBehaviour {
    [SerializeField]
    Transform UIPanel;
    // Use this for initialization
    void Start () {
        UIPanel.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    public void ActivatePanel()
    {
        UIPanel.gameObject.SetActive(true);
    }
    public void ClosePanel()
    {
        UIPanel.gameObject.SetActive(false);
    }
}
