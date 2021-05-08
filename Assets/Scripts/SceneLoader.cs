using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] QRand qrand;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadQRand());
        //
    }

    IEnumerator LoadQRand()
    {
        yield return StartCoroutine(qrand.InitQRand());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

 
}
