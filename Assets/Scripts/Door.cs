using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Room currentRoom;   //Room this door object is located at
    private Room connectedRoom; //Room that can be accessed trough this door
   
    //Sets current room and connected room. Should be used when instantiating the Door objects to scene
    public void SetConnection(Room currentRoom, Room connectedRoom)
    {
        this.currentRoom = currentRoom;
        this.connectedRoom = connectedRoom;
    }

    //Player collision handling: move to the next room when player reaches the door
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            currentRoom.IsCleared = true;
            FindObjectOfType<GenerateLevel>().InstantiateRoom(connectedRoom.x, connectedRoom.y);
            
        }
    }
}
