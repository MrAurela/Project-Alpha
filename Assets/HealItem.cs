using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : MonoBehaviour
{
    [SerializeField] float health = 10f;

    void  OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            FindObjectOfType<PlayerSpirit>().gameObject.GetComponent<Damageable>().Heal(health);
            Destroy(gameObject);
        }
    }
}
