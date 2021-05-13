using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSelector : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    [SerializeField] Sprite downRightSprite;
    [SerializeField] Sprite overDoorSprite;

    public bool up = false;
    public bool right = false;
    public bool down = false;
    public bool left = false;

    void Update()
    {
        int spriteIndex = 0;
        if (!HasObjectInDirection("Floor", Vector2.up))
        {
            spriteIndex += 1;
            up = true;
        }
        if (!HasObjectInDirection("Floor", Vector2.right))
        {
            spriteIndex += 2;
            right = true;
        }
        if (!HasObjectInDirection("Floor", Vector2.down))
        {
            spriteIndex += 4;
            down = true;
        }
        if (!HasObjectInDirection("Floor", Vector2.left))
        {
            spriteIndex += 8;
            left = true;
        }

        GetComponent<SpriteRenderer>().sprite = sprites[spriteIndex];

        /*if (spriteIndex == 14 && !NeighbourHasObjectInDirection(Vector2.right, 2)) //Object on down-right
        {
            GetComponent<SpriteRenderer>().sprite = downRightSprite;
        }*/

        if (HasObjectInDirection("Door", Vector2.down)) {
            GetComponent<SpriteRenderer>().sprite = overDoorSprite;
        }
    }


    private bool HasObjectInDirection(string tag, Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(this.gameObject.transform.position, direction, 1f);
        return hit.collider != null && hit.collider.gameObject.tag == tag;
    }

    public bool HasObjectInDirection(int directionId)
    {
        if (directionId == 0) return this.up;
        else if (directionId == 1) return this.right;
        else if (directionId == 2) return this.down;
        else if (directionId == 3) return this.left;
        else return false;
    }

    private bool NeighbourHasObjectInDirection(Vector2 neighbourDirection, int directionId)
    {
        RaycastHit2D hit = Physics2D.Raycast(this.gameObject.transform.position, neighbourDirection, 1f);
        if (hit)
        {
            SpriteSelector selector = hit.collider.gameObject.GetComponent<SpriteSelector>();
            if (selector)
            {
                return selector.HasObjectInDirection(directionId);
            }
        }
        return false;
    }

}
