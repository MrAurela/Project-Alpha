using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBulletScript : MonoBehaviour
{
    public int dmg = 1;
    public float timeToLive = 5;
    public float projectileSpeed = 15;
    private Rigidbody2D _rb;

    // fpublic Vector2 direction;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity *= projectileSpeed;
        Destroy(gameObject, timeToLive);
    }

    // Update is called once per frame
    void Update()
    {
        //_rb.velocity = new Vector2(projectileSpeed, 0);
        //_rb.velocity = direction;
        //_rb.AddForce(_rb.velocity, (ForceMode2D.Force));
    }
}
