using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            transform.root.GetComponent<RoomGeneration>().DecreaseNumberOfEnemies();
            Destroy(gameObject);
        }
    }
}
