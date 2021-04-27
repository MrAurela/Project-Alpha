using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Shooting : MonoBehaviour
{
    // Ref to child sprite renderer to set color later
    SpriteRenderer sprite;
    public GameObject bullet;
    public Rigidbody2D enemy;
    public Rigidbody2D Player;

    private void Start()
    {
        sprite = GetComponentsInChildren<SpriteRenderer>()[0];
    }
    //Sorry, I just stole the code from the player, but it works fine here :D
    private void Update()
    {
        
        Vector2 positionOnScreen = enemy.position;
        Vector2 mouseOnScreen = Player.position;

        float angle = Mathf.Atan2(positionOnScreen.y - mouseOnScreen.y, positionOnScreen.x - mouseOnScreen.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

        // Color weapon red if mouse pressed
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 dir = positionOnScreen - mouseOnScreen;
            Fire(dir);
        }
    }

    private void Fire(Vector2 dir)
    {
        GameObject bulletClone = (GameObject)Instantiate(bullet, transform.position, transform.rotation);
        bulletClone.transform.position += new Vector3(-dir.normalized.x, -dir.normalized.y, 0);
        bulletClone.GetComponent<Rigidbody2D>().velocity = -dir.normalized;
    }
}
