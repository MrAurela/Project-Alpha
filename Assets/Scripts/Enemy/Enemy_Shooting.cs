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
    public int FarDistance;
    public int CloseDistance;
    public float timer;
    private float startTime;
    private bool active;
    private float secondsUntilActivate = 1; // how many seconds should the enemy wait after spawning to begin attacking the player
    private void Start()
    {
        sprite = GetComponentsInChildren<SpriteRenderer>()[0];
        active = false;
        Invoke("Activate",this.secondsUntilActivate);
    }
    //Sorry, I just stole the code from the player, but it works fine here :D
    private void Update()
    {
        Vector2 positionOnScreen = enemy.position;
        Vector2 playerOnScreen = Player.position;

        float angle = Mathf.Atan2(positionOnScreen.y - playerOnScreen.y, positionOnScreen.x - playerOnScreen.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

        
        if ((positionOnScreen-playerOnScreen).magnitude <= FarDistance & (positionOnScreen - playerOnScreen).magnitude >= CloseDistance)
        {
            if (Time.time-startTime >= timer)
            {
                Vector2 dir = positionOnScreen - playerOnScreen;
                Fire(dir);
                startTime = Time.time;
            }            
        }
        else
        {
            startTime = Time.time;
        }
    }

    private void Activate()
    {
        this.active = true;
    }

    private void Fire(Vector2 dir)
    {
        if(this.active)
        {    
            GameObject bulletClone = (GameObject)Instantiate(bullet, transform.position, transform.rotation);
            bulletClone.transform.position += new Vector3(-dir.normalized.x, -dir.normalized.y, 0);
            bulletClone.GetComponent<Rigidbody2D>().velocity = -dir.normalized;
        }
    }

}
