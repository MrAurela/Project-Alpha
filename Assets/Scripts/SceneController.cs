using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    //Goes to next scene or first scene if in last scene
    public void NextScene()
    {
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
    }

    public void GoToScene(string scene)
    {
        /*if(scene=="Menu")
        {
            Destroy(GameObject.Find("QRandom"));
            Destroy(GameObject.Find("Sounds"));
        }*/
        SceneManager.LoadScene(scene);
    }

    public void OpenMainMenu()
    {
        Destroy(GameObject.Find("QRandom"));
        //Destroy(GameObject.Find("Sounds"));
        SceneManager.LoadScene("Menu");

        FindObjectOfType<AudioPlayer>().InCombat(false);
    }
}
