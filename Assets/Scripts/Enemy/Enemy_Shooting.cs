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
    public float FarDistanceMax;
    public float FarDistanceMin;
    public float CloseDistanceMax;
    public float CloseDistanceMin;
    private float FarDistance;
    private float CloseDistance;
    public float timer;
    public bool isBoss;
    private float startTime;
    private bool active;
    private float secondsUntilActivate = 1; // how many seconds should the enemy wait after spawning to begin attacking the player
    private void Start()
    {
        FarDistance = Random.Range(FarDistanceMin, FarDistanceMax);
        CloseDistance = Random.Range(CloseDistanceMin, CloseDistanceMax);
        sprite = GetComponentsInChildren<SpriteRenderer>()[0];
        active = false;
        Invoke("Activate",this.secondsUntilActivate);
    }
    //Sorry, I just stole the code from the player, but it works fine here :D
    private void Update()
    {
        if (Sees_player())
        {
            Vector2 positionOnScreen = enemy.position;
            Vector2 playerOnScreen = Player.position;

            float angle = Mathf.Atan2(positionOnScreen.y - playerOnScreen.y, positionOnScreen.x - playerOnScreen.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));


            if ((positionOnScreen - playerOnScreen).magnitude <= FarDistance & (positionOnScreen - playerOnScreen).magnitude >= CloseDistance)
            {
                if (Time.time - startTime >= timer)
                {
                    Vector2 dir = positionOnScreen - playerOnScreen;
                    Fire(dir);
                    if(this.isBoss)
                    {
                        Fire(Rotate(dir,3f));
                        Fire(Rotate(dir,-3f));
                    }
                    startTime = Time.time;
                }
            }
            else
            {
                startTime = Time.time;
            }
        }
        
    }
    private bool Sees_player()
    {

        LayerMask mask = LayerMask.GetMask("Wall");
        RaycastHit2D raycastHit2D = Physics2D.Linecast(enemy.position, Player.position, mask);
        if (raycastHit2D == false)
        {
            return true;
        }
        else
        {
            return false;
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

    public static Vector2 Rotate(Vector2 v, float delta)
    {
        float sin = Mathf.Sin(delta * Mathf.Deg2Rad);
        float cos = Mathf.Cos(delta * Mathf.Deg2Rad);
        
        float tx = (cos * v.x) - (sin * v.y);
        float ty = (sin * v.x) + (cos * v.y);
        return new Vector2(tx,ty);
    }

}
