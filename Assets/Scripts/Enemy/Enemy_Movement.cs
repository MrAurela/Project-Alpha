using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
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
        anim.Play("Idle_Right");
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
                            anim.Play("IdleRight");
                        else
                            anim.Play("IdleLeft");
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
            anim.Play("MoveRight");
        }
        else if (Enemy.velocity.x < 0)
        {
            anim.Play("MoveLeft");
        }
        else if (Enemy.velocity.y != 0)
        {
            if (facing)
                anim.Play("MoveRight");
            else
                anim.Play("MoveLeft");
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

