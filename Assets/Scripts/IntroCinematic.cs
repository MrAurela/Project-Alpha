using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroCinematic : MonoBehaviour
{
    float cinematicLength = 55f;
    bool cinematicDone;

    // Start is called before the first frame update
    void Start()
    {
        cinematicDone = false;
        StartCoroutine(TimedDelay(cinematicLength));
    }

    // Update is called once per frame
    void Update()
    {
        if(cinematicDone)
        {
            OpenMainMenu();
        }
    }

    private IEnumerator TimedDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        cinematicDone = true;
    }

    void OpenMainMenu()
    {
        Destroy(GameObject.Find("QRandom"));
        //Destroy(GameObject.Find("Sounds"));
        SceneManager.LoadScene("Menu");

        FindObjectOfType<AudioPlayer>().InCombat(false);
    }
}
