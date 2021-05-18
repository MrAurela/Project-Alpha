using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Melee_Attack : MonoBehaviour
{
    // Ref to child sprite renderer to set color later
    public GameObject bullet;
    public Rigidbody2D enemy;
    public Transform Gun1;
    public Transform Gun2;
    public Transform Gun3;
    public Transform Gun4;
    public float angle;
    
    private void Start()
    {

        
    }
    //Sorry, I just stole the code from the player, but it works fine here :D
    private void Update()
    {
        transform.Rotate(0f, 0f, angle);
        Fire();
    }
    
    void Fire()
    {
        
        Vector2 positionOnScreen = enemy.position;
        Vector2 gunOnScreen = Gun1.position;
        Vector2 dir = positionOnScreen - gunOnScreen;
        GameObject bulletClone = (GameObject)Instantiate(bullet, transform.position, transform.rotation);
        bulletClone.transform.position += new Vector3(-dir.normalized.x, -dir.normalized.y, 0);
        bulletClone.GetComponent<Rigidbody2D>().velocity = -dir.normalized;

        gunOnScreen = Gun2.position;
        dir = positionOnScreen - gunOnScreen;
        bulletClone = (GameObject)Instantiate(bullet, transform.position, transform.rotation);
        bulletClone.transform.position += new Vector3(-dir.normalized.x, -dir.normalized.y, 0);
        bulletClone.GetComponent<Rigidbody2D>().velocity = -dir.normalized;

        gunOnScreen = Gun3.position;
        dir = positionOnScreen - gunOnScreen;
        bulletClone = (GameObject)Instantiate(bullet, transform.position, transform.rotation);
        bulletClone.transform.position += new Vector3(-dir.normalized.x, -dir.normalized.y, 0);
        bulletClone.GetComponent<Rigidbody2D>().velocity = -dir.normalized;

        gunOnScreen = Gun4.position;
        dir = positionOnScreen - gunOnScreen;
        bulletClone = (GameObject)Instantiate(bullet, transform.position, transform.rotation);
        bulletClone.transform.position += new Vector3(-dir.normalized.x, -dir.normalized.y, 0);
        bulletClone.GetComponent<Rigidbody2D>().velocity = -dir.normalized;

    }


}