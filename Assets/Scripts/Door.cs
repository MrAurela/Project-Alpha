using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Room currentRoom;
    private Room connectedRoom;
   
    public void SetConnection(Room currentRoom, Room connectedRoom)
    {
        this.currentRoom = currentRoom;
        this.connectedRoom = connectedRoom;
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            FindObjectOfType<GenerateLevel>().InstantiateRoom(connectedRoom.x, connectedRoom.y);
            currentRoom.IsCleared = true;
        }
    }
}
