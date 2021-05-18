using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This could be in a separate file, but it looked so lonely I just added it here
public interface IDamageable
{
    public void Die();
}

public class Damageable : MonoBehaviour
{
    [SerializeField] float maximumHealth;
    [SerializeField] AudioClip damageSound;
    [SerializeField] AudioClip healSound;
    [SerializeField] AudioClip deathSound;
    
    public SpriteRenderer whiteRenderer = null;

    private float health;
    private float currentMaximumHealth;
    private AudioPlayer audioPlayer;

    void Start()
    {
        this.health = maximumHealth;
        this.currentMaximumHealth = maximumHealth;

        audioPlayer = FindObjectOfType<AudioPlayer>();
    }

    public float GetHealth()
    {
        return this.health;
    }

    public float GetHealthPercentage()
    {
        return this.health/this.currentMaximumHealth;
    }

    public void Damage(float damageTaken)
    {
        audioPlayer.PlayClip(damageSound);
        if (whiteRenderer != null)
            StartCoroutine(flashWhite());
        SetHealth(this.health - damageTaken);
        if (this.health <= 0f)
        {
            audioPlayer.PlayClip(deathSound);
            GetComponent<IDamageable>().Die();
        }
    }

    public void Heal(float amountToHeal)
    {
        audioPlayer.PlayClip(healSound);
        SetHealth(this.health + amountToHeal);
    }

    //Can be used to Increase with negative value;
    public void DecreaseMaximumHealth(float decrease)
    {
        SetMaximumHealth(this.currentMaximumHealth - decrease);
        SetHealth(this.health); //ensures the health does not go over range;
    }

    public void SetHealth(float newAmount)
    {
        this.health = Mathf.Clamp(newAmount, 0, this.currentMaximumHealth);
    }

    public void SetMaximumHealth(float newAmount)
    {
        this.currentMaximumHealth = Mathf.Clamp(newAmount, 0, this.currentMaximumHealth);
    }
    IEnumerator flashWhite()
    {
        whiteRenderer.material.SetFloat("_FlashAmount", 1.0f);
        whiteRenderer.material.SetFloat("_SelfIllum", 1.0f);
        yield return new WaitForSeconds(0.1f);
        whiteRenderer.material.SetFloat("_FlashAmount", 0.0f);
        whiteRenderer.material.SetFloat("_SelfIllum", 1.0f);
    }
}
