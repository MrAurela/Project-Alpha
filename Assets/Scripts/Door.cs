using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Room connectedRoom;
    private int x, y;
   
    public void SetConnection(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void ConnectTo(Room room)
    {
        connectedRoom = room;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(x + ", " + y);

        if (collision.gameObject.tag == "Player")
        {
            FindObjectOfType<GenerateLevel>().InstantiateRoom(x, y);
        }

    }
}
