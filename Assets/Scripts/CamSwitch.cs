using UnityEngine;
using System.Collections;

public class CamSwitch : MonoBehaviour
{


    public Camera MainCamera;
    public Camera third;


    private Camera[] cameras;
    private int currentCameraIndex = 0;
    private Camera currentCamera;

    // Use this for initialization
    void Start()
    {
        cameras = new Camera[] { MainCamera, third };//this is the array of cameras
        currentCamera = third; //When the program start the main camera is selected as the default camera
        ChangeView();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p") || Input.GetKeyDown("joystick button 9"))
        {
            currentCameraIndex++;
            if (currentCameraIndex > cameras.Length - 1)
                currentCameraIndex = 0;
            ChangeView();
        }
    }
    void ChangeView()
    {
        currentCamera.enabled = false;
        currentCamera = cameras[currentCameraIndex];
        currentCamera.enabled = true;
    }
}
