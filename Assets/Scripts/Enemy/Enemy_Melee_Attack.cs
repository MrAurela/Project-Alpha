using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Melee_Attack : MonoBehaviour
{
    // Ref to child sprite renderer to set color later
    SpriteRenderer sprite;
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
        Vector2 playerOnScreen = Player.position;

        float angle = Mathf.Atan2(positionOnScreen.y - playerOnScreen.y, positionOnScreen.x - playerOnScreen.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

        // Color weapon red if mouse pressed
        if (Input.GetMouseButtonDown(0))
        {
            
            Attack();
        }
        else
        {
            sprite.color = new Color(1, 1, 1, 1);
        }
    }

    private void Attack()
    {
        sprite.color = new Color(1,0,0,1);
    }
}
