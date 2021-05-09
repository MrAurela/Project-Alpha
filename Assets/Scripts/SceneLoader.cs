using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    //[SerializeField] QRand qrand;
    public QRand qrand;

    // Start is called before the first frame update
    void Start()
    {
        QRand qrand = FindObjectOfType<QRand>();
        if (qrand)
        {
            StartCoroutine(LoadQRand());
        }
        //
    }

    IEnumerator LoadQRand()
    {
        yield return StartCoroutine(qrand.InitQRand());
        NextScene();
    }

    //Goes to next scene or first scene if in last scene
    public void NextScene()
    {
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
    }

 
}
