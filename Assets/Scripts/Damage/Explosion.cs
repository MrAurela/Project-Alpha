using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    [SerializeField] float damage = 30f;
    [SerializeField] float secondsAlive = 1f;
    [SerializeField] float force = 5f;

    private float lifetime;

    // Start is called before the first frame update
    void Start()
    {
        lifetime = secondsAlive;
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        //Do damage to everything close (having Damageable component or the Player character)
        Damageable damageable = collider.transform.gameObject.GetComponent<Damageable>();
        if (damageable)
        {
            Debug.Log("Damage");
            damageable.Damage(damage);
        } else if (collider.gameObject.tag == "Player")
        {
            FindObjectOfType<PlayerSpirit>().gameObject.GetComponent<Damageable>().Damage(damage);
        }

        //Add explosive force to everything close (having rigidbody)
        Rigidbody2D rb = collider.transform.gameObject.GetComponent<Rigidbody2D>();
        if (rb)
        {
            rb.AddForce(force * (rb.position - (Vector2) transform.position).normalized);
        }
    }
}
