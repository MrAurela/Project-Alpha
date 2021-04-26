using UnityEngine;

public class WeaponController : MonoBehaviour
{
    // Ref to child sprite renderer to set color later
    SpriteRenderer sprite;
    public Camera cam;

    private void Start()
    {
        sprite = GetComponentsInChildren<SpriteRenderer>()[0];
    }

    private void Update()
    {
        // Rotate weapon to mouse
        // TODO: support for two camera setup. Use screen pos?
        Vector2 positionOnScreen = cam.WorldToViewportPoint(transform.position);
        Vector2 mouseOnScreen = (Vector2)cam.ScreenToViewportPoint(Input.mousePosition);

        float angle = Mathf.Atan2(positionOnScreen.y - mouseOnScreen.y, positionOnScreen.x - mouseOnScreen.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

        // Color weapon red if mouse pressed
        if (Input.GetMouseButtonDown(0))
        {
            sprite.color = new Color(1, 0, 0, 1);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            sprite.color = new Color(1, 1, 1, 1);
        }
    }
}
