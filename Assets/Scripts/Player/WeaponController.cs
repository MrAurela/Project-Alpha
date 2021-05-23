using UnityEngine;

public class WeaponController : MonoBehaviour
{
    // Ref to child sprite renderer to set color later
    //[SerializeField] private string ShootingSoundName = "shooting_1";
    [SerializeField] AudioClip shootingSound;
    [SerializeField] float shootingSoundVolume = 1f;

    public Camera cam;
    public GameObject bullet;
    public float bulletSpawnDistance;
    private SpriteRenderer sprite;
    //private AudioSource shootSound;


    private void Start()
    {
        sprite = GetComponentsInChildren<SpriteRenderer>()[0];
       // shootSound = gameObject.AddComponent<AudioSource>();
        //shootSound.clip = Resources.Load(ShootingSoundName) as AudioClip;

    }

    private void Update()
    {
        // Rotate weapon to mouse
        // TODO: support for two camera setup. Use screen pos?
        Vector2 positionOnScreen = cam.WorldToViewportPoint(transform.position);
        Vector2 mouseOnScreen = (Vector2)cam.ScreenToViewportPoint(Input.mousePosition);

        float angle = Mathf.Atan2(positionOnScreen.y - mouseOnScreen.y, positionOnScreen.x - mouseOnScreen.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

        if (Quaternion.Euler(new Vector3(0f, 0f, angle)).z > 0)
            sprite.sortingOrder = 2;
        else if (Quaternion.Euler(new Vector3(0f, 0f, angle)).z < 0)
            sprite.sortingOrder = 0;

        // Color weapon red if mouse pressed
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 dir = positionOnScreen - mouseOnScreen;
            Fire(dir);
        }
    }

    private void Fire(Vector2 dir)
    {
        //shootSound.Play();
        FindObjectOfType<AudioPlayer>().PlayClip(shootingSound, shootingSoundVolume);

        GameObject bulletClone = (GameObject) Instantiate(bullet, transform.position, transform.rotation);
        bulletClone.transform.position += new Vector3(-dir.normalized.x, -dir.normalized.y, 0) * bulletSpawnDistance;
        bulletClone.GetComponent<Rigidbody2D>().velocity = -dir.normalized;
        //bulletClone.set_direction(dir.normalized);
    }
}
