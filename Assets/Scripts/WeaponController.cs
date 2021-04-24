using UnityEngine;

public class WeaponController : MonoBehaviour
{
    // Ref to child sprite renderer to set color later
    SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponentsInChildren<SpriteRenderer>()[0];
    }

    private void Update()
    {
        // Rotate weapon to mouse
        // TODO: support for two camera setup. Use screen pos?
        Vector3 mousePos = Input.mousePosition;
        // mousePos.z = 5.23f;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));


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
