using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void ToExperiment()
    {
        SceneManager.LoadScene("ExperimentScen");
    }
    
    public void ToVisuell()
    {
        SceneManager.LoadScene("VisuellScen");
    }

    public void backToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
