using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class SceneController : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(PingServerToWake());
    }

    IEnumerator PingServerToWake()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get("https://quantum-seed-generator.herokuapp.com/ping"))
        {
            yield return webRequest.SendWebRequest();
        }
    }

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
