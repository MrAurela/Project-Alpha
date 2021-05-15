using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSelector : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    [SerializeField] Sprite downRightSprite;
    [SerializeField] Sprite overDoorSprite;

    public bool wallUp = false;
    public bool wallRight = false;
    public bool wallDown = false;
    public bool wallLeft = false;

    public bool floorUp = false;
    public bool floorRight = false;
    public bool floorDown = false;
    public bool floorLeft = false;
    public bool wallLeftDown = false;
    public bool wallRightDown = false;

    public bool doorDown = false;

    void Update()
    {
        wallUp = false;
        wallRight = false;
        wallDown = false;
        wallLeft = false;

        floorUp = false;
        floorRight = false;
        floorDown = false;
        floorLeft = false;
        wallRightDown = false;
        wallLeftDown = false;

        doorDown = false;

        /*if (!HasObjectInDirection("Floor", "Default", Vector2.up) || HasObjectInDirection("Wall", "Wall", Vector2.up))
        {
            up = true;
        }
        if (!HasObjectInDirection("Floor", "Default", Vector2.right) || HasObjectInDirection("Wall", "Wall", Vector2.right))
        {
            right = true;
        }
        if (!HasObjectInDirection("Floor", "Default", Vector2.down) || HasObjectInDirection("Wall", "Wall", Vector2.down))
        {
            down = true;
        }
        if (!HasObjectInDirection("Floor", "Default", Vector2.left) || HasObjectInDirection("Wall", "Wall", Vector2.left))
        {
            left = true;
        }*/

        if (HasObjectInDirection("Wall", "Wall", Vector2.up)) wallUp = true;
        if (HasObjectInDirection("Wall", "Wall", Vector2.right)) wallRight = true;
        if (HasObjectInDirection("Wall", "Wall", Vector2.down)) wallDown = true;
        if (HasObjectInDirection("Wall", "Wall", Vector2.left)) wallLeft = true;
        if (NeighbourHasWallInDirection(Vector2.right, 2)) wallRightDown = true;
        if (NeighbourHasWallInDirection(Vector2.left, 2)) wallLeftDown = true;

        if (HasObjectInDirection("Floor", "Default", Vector2.up)) floorUp = true;
        if (HasObjectInDirection("Floor", "Default", Vector2.right)) floorRight = true;
        if (HasObjectInDirection("Floor", "Default", Vector2.down)) floorDown = true;
        if (HasObjectInDirection("Floor", "Default", Vector2.left)) floorLeft = true;
        

        if (HasObjectInDirection("Door", "Door", Vector2.down)) doorDown = true;

        GetComponent<SpriteRenderer>().sprite = sprites[GetSpriteIndex()];


       /* if (HasObjectInDirection("Door", "Door", Vector2.down)) {
            GetComponent<SpriteRenderer>().sprite = overDoorSprite;
        }*/

    }

    private int GetSpriteIndex()
    {
        int si = 0;
        if (this.wallUp) si += 1;
        if (this.wallRight) {
            /*if (this.wallDown && !this.wallRightDown)
            {
                si += 16;
            } else
            {
                si += 2;
            }*/
            si += 2;
        }
        if (this.wallDown || this.doorDown) si += 4;
        if (this.wallLeft) {
            /*if (this.wallDown && !this.wallLeftDown) {
                si += 32;
            } else
            {
                si += 8;
            }*/
            si += 8;
        }

        if ((si == 14 || si == 15) && !this.wallLeftDown) si = 20;
        if ((si == 14 || si == 15) && !this.wallRightDown) si = 21;
        if (si == 15 && !this.wallRightDown && !this.wallLeftDown) si = 22;


        return si;
    }

    private bool HasObjectInDirection(string tag, string layer, Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(this.gameObject.transform.position, direction, 1f, LayerMask.GetMask(layer));
        return hit.collider != null && hit.collider.gameObject.tag == tag;
    }

   /*private string ObjectInDirection(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(this.gameObject.transform.position, direction, 1f);
        if (hit.collider)
        {
            return hit.collider.gameObject.tag;
        } else
        {
            return "";
        }
    }*/

    public bool HasWallInDirection(int directionId)
    {
        if (directionId == 0) return this.wallUp;
        else if (directionId == 1) return this.wallRight;
        else if (directionId == 2) return this.wallDown;
        else if (directionId == 3) return this.wallLeft;
        else return false;
    }

    private bool NeighbourHasWallInDirection(Vector2 neighbourDirection, int directionId)
    {
        RaycastHit2D hit = Physics2D.Raycast(this.gameObject.transform.position, neighbourDirection, 1f, LayerMask.GetMask("Wall"));
        if (hit)
        {
            SpriteSelector selector = hit.collider.gameObject.GetComponent<SpriteSelector>();
            if (selector)
            {
                return selector.HasWallInDirection(directionId);
            }
        }
        return false;
    }

}
