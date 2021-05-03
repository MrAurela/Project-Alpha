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

    private float health;
    private float currentMaximumHealth;

    void Start()
    {
        this.health = maximumHealth;
        this.currentMaximumHealth = maximumHealth;
    }

    public float GetHealth()
    {
        return this.health;
    }

    public float GetHealthPercentage()
    {
        return this.health/this.currentMaximumHealth;
    }

    //Can be used to Heal with negative value;
    public void Damage(float damageTaken)
    {
        SetHealth(this.health - damageTaken);
        if (this.health <= 0f)
        {
            GetComponent<IDamageable>().Die();
        }
    }

    //Can be used to Increase with negative value;
    public void DecreaseMaximumHealth(float decrease)
    {
        SetMaximumHealth(this.currentMaximumHealth - decrease);
        if (this.currentMaximumHealth <= 0f)
        {
            GetComponent<IDamageable>();
        }
    }

    public void SetHealth(float newAmount)
    {
        this.health = Mathf.Clamp(newAmount, 0, this.currentMaximumHealth);
    }

    public void SetMaximumHealth(float newAmount)
    {
        this.currentMaximumHealth = Mathf.Clamp(newAmount, 0, this.currentMaximumHealth);
    }

}
