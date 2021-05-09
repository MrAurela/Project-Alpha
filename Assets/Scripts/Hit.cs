using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour, IDamageable
{

    public void Die()
    {
        transform.root.GetComponent<RoomGeneration>().DecreaseNumberOfEnemies();
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            gameObject.GetComponent<Damageable>().Damage(10);
        }
    }
}
