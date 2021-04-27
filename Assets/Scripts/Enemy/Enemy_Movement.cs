using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public Rigidbody2D Player;
    public Rigidbody2D Enemy;
    public int velocity = 1;
    private Vector2 moveDirection;
    

    // Update is called once per frame
    void Update()
    {
        if (CloseToPlayer())
        {
            MoveAway();
        }
        else if (FarFromPlayer())
        {
            MoveCloser();
        }
        else
        {
            Attack();
        }
    }
    bool CloseToPlayer()
    {
        float X = Player.position.x - Enemy.position.x;
        float Y = Player.position.y - Enemy.position.y;
        Vector2 distanceVector = new Vector2(X, Y);
        if (distanceVector.SqrMagnitude() < 5)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    bool FarFromPlayer()
    {
        float X = Player.position.x - Enemy.position.x;
        float Y = Player.position.y - Enemy.position.y;
        Vector2 distanceVector = new Vector2(X, Y);
        if (distanceVector.SqrMagnitude() > 20)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    bool Sees_player()
    {
        return true;
    }
    void MoveAway()
    {
        float moveX = - Player.position.x + Enemy.position.x;
        float moveY = - Player.position.y + Enemy.position.y;
        moveDirection = new Vector2(moveX, moveY).normalized;
        Enemy.velocity = new Vector2(moveDirection.x * velocity, moveDirection.y * velocity);
    }
    void MoveCloser()
    {
        float moveX = Player.position.x - Enemy.position.x;
        float moveY = Player.position.y - Enemy.position.y;
        moveDirection = new Vector2(moveX, moveY).normalized;
        Enemy.velocity = new Vector2(moveDirection.x * velocity, moveDirection.y * velocity);
    }
    void Attack()
    {
        Enemy.velocity = new Vector2(0, 0);
    }
}

