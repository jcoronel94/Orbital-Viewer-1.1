using UnityEngine;
using System.Collections;
using System.IO;

public class KSavetoFile : MonoBehaviour {
    
    // Use this for initialization
    int Cnumasters = 0;
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void KAsteroidsSaveIt()
    {
        //UIPanel.gameObject.SetActive(true);
        //string fileName = @"C:\Users\N\Documents\orbital-viewer\Orbital Viewer 1.1 - MANY CHANGES\Assets\KCurrentSession.txt";
        string fileName = @"C:\Users\N\Documents\orbital-viewer\Orbital Viewer 1.1 - MANY CHANGES\Assets\" + FilenameInputScript.filename + ".txt";

        if (!File.Exists(fileName))
        {
            File.Copy("Assets/inputK.txt", fileName);
        }
        
        File.AppendAllText(fileName, System.Environment.NewLine);

        Cnumasters = int.Parse(KAsteroidAmountInput.inputs[0]);
        for (int i = 0; i < Cnumasters; i++)
        {
            File.AppendAllText(fileName, "PLANET");
            File.AppendAllText(fileName, " ");
			File.AppendAllText(fileName, ModelActions.AsteroidsKfromCeccentricities[i].ToString("N5"));

            File.AppendAllText(fileName, " ");
			File.AppendAllText(fileName, ModelActions.AsteroidsKfromClongnodes[i].ToString("N5"));

            File.AppendAllText(fileName, " ");
			File.AppendAllText(fileName, ModelActions.AsteroidsKfromCsemimajors[i].ToString("N5"));

            File.AppendAllText(fileName, " ");
			File.AppendAllText(fileName, ModelActions.AsteroidsKfromCperiastrons[i].ToString("N5"));


            File.AppendAllText(fileName, " ");
			File.AppendAllText(fileName, ModelActions.AsteroidsKfromCinclinations[i].ToString("N5"));

            File.AppendAllText(fileName, " ");
			File.AppendAllText(fileName, ModelActions.AsteroidsKfromCMeanAnomalies[i].ToString("N5"));


            File.AppendAllText(fileName, " ");
			File.AppendAllText(fileName, ModelActions.KAsteroidsMasses[i].ToString("N5"));

            File.AppendAllText(fileName, System.Environment.NewLine);
        }
    }

    public void KPlanetSaveIt()
    {
        //string fileName = @"C:\Users\N\Documents\orbital-viewer\Orbital Viewer 1.1 - MANY CHANGES\Assets\KCurrentSession.txt";

        //UIPanel.gameObject.SetActive(true);
        string fileName = @"C:\Users\N\Documents\orbital-viewer\Orbital Viewer 1.1 - MANY CHANGES\Assets\" + FilenameInputScript.filename + ".txt";

        if (!File.Exists(fileName))
        {
            File.Copy("Assets/inputK.txt", fileName);
        }
        
        File.AppendAllText(fileName, System.Environment.NewLine);

        File.AppendAllText(fileName, "PLANET");
        File.AppendAllText(fileName, " ");
		File.AppendAllText(fileName, ModelActions.PlanetsKfromCeccentricities[KSubmittoPlanetFile.submitcount - 1].ToString("N5"));

        File.AppendAllText(fileName, " ");
		File.AppendAllText(fileName, ModelActions.PlanetsKfromClongnodes[KSubmittoPlanetFile.submitcount - 1].ToString("N5"));

        File.AppendAllText(fileName, " ");
		File.AppendAllText(fileName, ModelActions.PlanetsKfromCsemimajors[KSubmittoPlanetFile.submitcount - 1].ToString("N5"));

        File.AppendAllText(fileName, " ");
		File.AppendAllText(fileName, ModelActions.PlanetsKfromCperiastrons[KSubmittoPlanetFile.submitcount - 1].ToString("N5"));


        File.AppendAllText(fileName, " ");
		File.AppendAllText(fileName, ModelActions.PlanetsKfromCinclinations[KSubmittoPlanetFile.submitcount - 1].ToString("N5"));

        File.AppendAllText(fileName, " ");
		File.AppendAllText(fileName, ModelActions.PlanetsKfromCMeanAnomalies[KSubmittoPlanetFile.submitcount - 1].ToString("N5"));


        File.AppendAllText(fileName, " ");
		File.AppendAllText(fileName, ModelActions.KPlanetsMasses[KSubmittoPlanetFile.submitcount - 1].ToString("N5"));

        File.AppendAllText(fileName, System.Environment.NewLine);
    }
}
