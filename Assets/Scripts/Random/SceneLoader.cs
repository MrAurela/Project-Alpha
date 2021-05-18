using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    //[SerializeField] QRand qrand;
    public QRand qrand;
    bool qrandDone;
    bool waitDone;

    // Start is called before the first frame update
    void Start()
    {
        qrandDone = false;
        waitDone = false;
        Debug.Log("Scene loading begun.");
        StartCoroutine(TimedDelay(7f));
        QRand qrand = FindObjectOfType<QRand>();
        if (qrand)
        {
            Debug.Log("Starting loadQRand() coroutine.");
            StartCoroutine(LoadQRand());
        }
    }

    void Update()
    {
        if(qrandDone && waitDone)
        {
            NextScene();
        }
    }

    IEnumerator LoadQRand()
    {
        yield return StartCoroutine(qrand.InitQRand());
        qrandDone = true;
    }

    private IEnumerator TimedDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        waitDone = true;
    }

    //Goes to next scene or first scene if in last scene
    public void NextScene()
    {
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
    }

 
}
