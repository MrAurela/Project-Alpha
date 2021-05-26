using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] float masterVolumeBackground = 1f; //modifier multiplied with the volume of background music
    [SerializeField] float masterVolumeEffects = 1f; //modifier multiplied with every sound effect 
    [SerializeField] AudioSource combatBG;
    [SerializeField] AudioSource exploreBG;

    private bool combatFadingOut;
    private bool inCombat = false;

    // Start is called before the first frame update
    void Awake()
    {
        if (FindObjectsOfType<AudioPlayer>().Length > 1)
        {
            Destroy(gameObject);
        } else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        exploreBG.volume = masterVolumeBackground;
        combatBG.volume = masterVolumeBackground;
    }

    void Update()
    {
        if (inCombat && exploreBG.isPlaying) exploreBG.Stop();
        if (inCombat && !combatBG.isPlaying) combatBG.Play();
        if (!inCombat && !exploreBG.isPlaying) exploreBG.Play();
        if (!inCombat && combatBG.isPlaying && !combatFadingOut) StartCoroutine(FadeOutCombat());
    }

    public void InCombat(bool value)
    {
        inCombat = value;
    }

    public void PlayClip(AudioClip clip, float volume = 1f)
    {
        if (clip) AudioSource.PlayClipAtPoint(clip, transform.position, volume * masterVolumeEffects);
    }

    //Fades away the combat music so that there is no silence before the ambient music "kicks in". 
    //Constant 2.5 was selected as the fade duration simply by testing what sounds the best
    //Based on: https://forum.unity.com/threads/fade-out-audio-source.335031/
    private IEnumerator FadeOutCombat()
    {
        combatFadingOut = true;

        float fadeTime = 2.5f;
        float startVolume = combatBG.volume;

        //Loop until sound is fully off
        //Can break early if combat mode activates again
        while (combatBG.volume > 0 && !inCombat)
        {
            combatBG.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }

        //Either set the combat music to stop or starts it again based on the current situation
        if (!inCombat) combatBG.Stop();
        combatBG.volume = startVolume;

        combatFadingOut = false;
    }


}
