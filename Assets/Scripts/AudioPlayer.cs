using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] AudioClip bg1;
    [SerializeField] AudioClip bg2;

    private bool ambient;

    // Start is called before the first frame update
    void Start()
    {
        
        if (FindObjectsOfType<AudioPlayer>().Length > 1)
        {
            Destroy(this);
        } else
        {
            DontDestroyOnLoad(this);
        }

        GetComponent<AudioSource>().clip = bg1;
        GetComponent<AudioSource>().Play();

        ambient = true;  
    }

    
    public void SetFightMusicOn()
    {
        if (ambient)
        {
            ambient = false;
            GetComponent<AudioSource>().clip = bg2;
            GetComponent<AudioSource>().Play();
        }
    }

    public void SetAmbientMusicOn()
    {
        if (!ambient)
        {
            ambient = true;
            StartCoroutine(Fade(bg1, 1.5f));
        }
    }


    //https://forum.unity.com/threads/fade-out-audio-source.335031/
    private IEnumerator Fade(AudioClip clip, float FadeTime)
    {
        AudioSource audioSource = GetComponent<AudioSource>();

        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }

        audioSource.clip = clip;
        audioSource.volume = startVolume;
        audioSource.Play();
        
    }


}
