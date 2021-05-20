using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour, IDamageable
{
    [SerializeField] int damage = 10;
    [SerializeField] GameObject dropItem;
    [SerializeField] float dropProbability;

    public void Die()
    {
        transform.root.GetComponent<RoomGeneration>().DecreaseNumberOfEnemies();
        float random = transform.root.GetComponent<RoomGeneration>().RandomFloat(0f, 1f);
        if (random <= dropProbability)
        {
            Instantiate(dropItem, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            gameObject.GetComponent<Damageable>().Damage(damage);
        }
    }
}
