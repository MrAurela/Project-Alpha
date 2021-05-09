using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1;
    public Rigidbody2D rb;

    private Animator anim;
    private Vector2 moveDirection;
    private bool facing = true; // 1 = right, 0 = left
    
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        anim.Play("Idle_Right");
    }

    void Update()
    {
        processInputs();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
        if (moveDirection.x > 0)
        {
            anim.Play("Run_Right");
        }
        else if (moveDirection.x < 0)
        {
            anim.Play("Run_Left");
        }
        else if (moveDirection.y != 0)
        {
            if (facing)
                anim.Play("Run_Right");
            else
                anim.Play("Run_Left");
        }
        else
        {
            if (facing)
                anim.Play("Idle_Right");
            else
                anim.Play("Idle_Left");
        }
    }

    void processInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        if (moveX > 0)
            facing = true;
        else if (moveX < 0)
            facing = false;
        moveDirection = new Vector2(moveX, moveY).normalized;

        if (Input.GetMouseButtonDown(1))
        {
            FindObjectOfType<PlayerSpirit>().GetComponent<Damageable>().Damage(10f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Debug.Log("Player hit for 5 dmg");
            FindObjectOfType<PlayerSpirit>().GetComponent<Damageable>().Damage(5);
        }
    }
}
