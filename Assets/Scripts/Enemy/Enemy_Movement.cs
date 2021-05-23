using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    public bool isBoss = false;
    public Rigidbody2D Player;
    public Rigidbody2D Enemy;
    public int velocity = 1;
    private Vector2 moveDirection;
    private Vector2 playerGhost;
    public float FarDistanceMax;
    public float FarDistanceMin;
    public float CloseDistanceMax;
    public float CloseDistanceMin;
    private float FarDistance;
    private float CloseDistance;
    private bool facing = true; // 1 = right, 0 = left
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        FarDistance = Random.Range(FarDistanceMin, FarDistanceMax);
        CloseDistance = Random.Range(CloseDistanceMin, CloseDistanceMax);
        playerGhost = Enemy.position;

        // Animation
        anim = gameObject.GetComponent<Animator>();
        if (!isBoss)
            anim.Play("IdleRight");
        else
            anim.Play("BossIdleRight");
    }  
    // Update is called once per frame
    void Update()
    {
        if (Player != null)
        {
            if (Sees_player())
            {
                playerGhost = Player.position;
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

                    {
                        if (facing)
                            if (!isBoss)
                                anim.Play("IdleRight");
                            else
                                anim.Play("BossIdleRight");
                        else
                            if (!isBoss)
                                anim.Play("IdleLeft");
                            else
                                anim.Play("BossIdleLeft");
                    }
                }
            }
            else
            {
                MoveToGhost();
            }
            
        }
        // Movement animation
        if (Enemy.velocity.x > 0)
        {
            if (!isBoss)
                anim.Play("MoveRight");
            else
                anim.Play("BossMoveRight");
        }
        else if (Enemy.velocity.x < 0)
        {
            if (!isBoss)
                anim.Play("MoveLeft");
            else
                anim.Play("BossMoveLeft");
        }
        else if (Enemy.velocity.y != 0)
        {
            if (facing)
                if (!isBoss)
                    anim.Play("MoveRight");
                else
                    anim.Play("BossMoveRight");
            else
                if (!isBoss)
                    anim.Play("MoveLeft");
            else
                anim.Play("BossMoveLeft");
        }

    }
    bool CloseToPlayer()
    {
        float X = Player.position.x - Enemy.position.x;
        float Y = Player.position.y - Enemy.position.y;
        Vector2 distanceVector = new Vector2(X, Y);
        if (distanceVector.magnitude < CloseDistance)
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
        if (distanceVector.magnitude > FarDistance)
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
        
        LayerMask mask = LayerMask.GetMask("Wall");
        RaycastHit2D raycastHit2D = Physics2D.Linecast(Enemy.position, Player.position, mask);
        if (raycastHit2D == false)
        {
            return true;
        }
        else
        {
            return false;
        }
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
    void MoveToGhost()
    {
        float moveX = playerGhost.x - Enemy.position.x;
        float moveY = playerGhost.y - Enemy.position.y;
        moveDirection = new Vector2(moveX, moveY).normalized;
        Enemy.velocity = new Vector2(moveDirection.x * velocity, moveDirection.y * velocity);
    }
    void Attack()
    {
        Enemy.velocity = new Vector2(0, 0);
    }
}

