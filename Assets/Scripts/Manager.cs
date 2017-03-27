using UnityEngine;
using System.Collections;
using UnityEngine.UI; //Need this for calling UI scripts
using UnityEngine.SceneManagement;
using System.IO;

public class Manager : MonoBehaviour
{

    [SerializeField]
    Transform UIPanel; //Will assign our panel to this variable so we can enable/disable it

    //[SerializeField]
    //Text timeText; //Will assign our Time Text to this variable so we can modify the text it displays.

    bool isPaused; //Used to determine paused state

    void Start()
    {
        UIPanel.gameObject.SetActive(false); //make sure our pause menu is disabled when scene starts
        isPaused = false; //make sure isPaused is always false when our scene opens
    }

    void Update()
    {

        //timeText.text = "Time Since Startup: " + Time.timeSinceLevelLoad; //Tells us the time since the scene loaded
        //Debug.Log(timeText.text);

        //If player presses escape and game is not paused. Pause game. If game is paused and player presses escape, unpause.
        if (Input.GetButtonDown("Pauser") && !isPaused)
            Pause();
        else if (Input.GetButtonDown("Pauser") && isPaused)
            UnPause();
    }

    public void Pause()
    {
        isPaused = true;
        UIPanel.gameObject.SetActive(true); //turn on the pause menu
        Time.timeScale = 0.0F; //pause the game
        GameObject.Find("Model").GetComponent<ModelActions>().timescale = 0.0f;
    }

    public void UnPause()
    {
        isPaused = false;
        UIPanel.gameObject.SetActive(false); //turn off pause menu
        Time.timeScale = 1.0F; //resume game
        GameObject.Find("Model").GetComponent<ModelActions>().timescale = 1.0f;

    }

    public void QuitGame()
    {
        

            GameObject model = GameObject.Find("Model");
            if (model.GetComponent<ModelActions>().GetThreaded())
            {
                model.GetComponent<ModelActions>().threadRunning = false;
                System.Threading.Thread.Sleep(1000);
                model.GetComponent<ModelActions>().modelThread.Abort();
            }
            Application.Quit();
        
        
    }

    public void Restart()
    {
        SceneManager.LoadScene(7);
        UnPause();

    }

    public void BackMain()
    {
        SceneManager.LoadScene(0);
        UnPause();

        /*

        if (!CartesianNavigation.modeCheck)
        {
            string line = null;
            int line_number = 0;
            int first_line_to_delete = ModelActions.totallinesC - ModelActions.addedlinesC;

            using (StreamReader reader = new StreamReader(@"C:\Users\N\Documents\orbital-viewer\Orbital Viewer 1.0\Assets\inputC.txt"))
            {
                using (StreamWriter writer = new StreamWriter(@"C:\Users\N\Documents\orbital-viewer\Orbital Viewer 1.0\Assets\inputC.txt"))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        line_number++;

                        if (line_number != first_line_to_delete)
                        {
                            continue;
                        }

                        writer.WriteLine(line);
                        first_line_to_delete++;

                    }
                }
            }

        }
        else
        {
            string line = null;
            int line_number = 0;
            int first_line_to_delete = ModelActions.totallinesK - ModelActions.addedlinesK;

            using (StreamReader reader = new StreamReader(@"C:\Users\N\Documents\orbital-viewer\Orbital Viewer 1.0\Assets\inputK.txt"))
            {
                using (StreamWriter writer = new StreamWriter(@"C:\Users\N\Documents\orbital-viewer\Orbital Viewer 1.0\Assets\inputK.txt"))
                {
                    while ((line = reader.ReadLine()) != null)
                    {

                        line_number++;
                        if (line_number != first_line_to_delete)
                        {
                            continue;
                        }

                        
                        writer.WriteLine(line);
                        first_line_to_delete++;                       

                    }
                    //writer.CloseOutput = true;
                    writer.Close();
                }
                reader.Close();
            }
            
        }
        */
    }
}