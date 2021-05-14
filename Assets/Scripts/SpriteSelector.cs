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

    public string upS;
    public string rightS;
    public string downS;
    public string leftS;

    void Update()
    {
        int spriteIndex = 0;
        if (!HasObjectInDirection("Floor", "Default", Vector2.up) || HasObjectInDirection("Wall", "Wall", Vector2.up))
        {
            spriteIndex += 1;
            up = true;
        }
        if (!HasObjectInDirection("Floor", "Default", Vector2.right) || HasObjectInDirection("Wall", "Wall", Vector2.right))
        {
            spriteIndex += 2;
            right = true;
        }
        if (!HasObjectInDirection("Floor", "Default", Vector2.down) || HasObjectInDirection("Wall", "Wall", Vector2.down))
        {
            spriteIndex += 4;
            down = true;
        }
        if (!HasObjectInDirection("Floor", "Default", Vector2.left) || HasObjectInDirection("Wall", "Wall", Vector2.left))
        {
            spriteIndex += 8;
            left = true;
        }

        upS = ObjectInDirection(Vector2.up);
        rightS = ObjectInDirection(Vector2.right);
        downS = ObjectInDirection(Vector2.down);
        leftS = ObjectInDirection(Vector2.left);

        GetComponent<SpriteRenderer>().sprite = sprites[spriteIndex];

        /*if (spriteIndex == 14 && !NeighbourHasObjectInDirection(Vector2.right, 2)) //Object on down-right
        {
            GetComponent<SpriteRenderer>().sprite = downRightSprite;
        }*/

        if (HasObjectInDirection("Door", "Door", Vector2.down)) {
            GetComponent<SpriteRenderer>().sprite = overDoorSprite;
        }
    }


    private bool HasObjectInDirection(string tag, string layer, Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(this.gameObject.transform.position, direction, 1f, LayerMask.GetMask(layer));
        return hit.collider != null && hit.collider.gameObject.tag == tag;
    }

    private string ObjectInDirection(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(this.gameObject.transform.position, direction, 1f);
        if (hit.collider)
        {
            return hit.collider.gameObject.tag;
        } else
        {
            return "";
        }
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
