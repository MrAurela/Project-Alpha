using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField] bool lockedUntilEnimiesKilled = true;
    [SerializeField] Color colorOpen;
    [SerializeField] Color colorClosed;

    private Room currentRoom;   //Room this door object is located at
    private Room connectedRoom; //Room that can be accessed trough this door
   
    void Update()
    {
        if (IsLocked())
        {
            GetComponent<SpriteRenderer>().color = colorClosed;
        } else
        {
            GetComponent<SpriteRenderer>().color = colorOpen;
        }
    }

    //Sets current room and connected room. Should be used when instantiating the Door objects to scene
    public void SetConnection(Room currentRoom, Room connectedRoom)
    {
        this.currentRoom = currentRoom;
        this.connectedRoom = connectedRoom;
    }

    //Player collision handling: move to the next room when player reaches the door
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !IsLocked())
        {
            //currentRoom.IsCleared = true;
            if (this.currentRoom.IsBossRoom)
            {
                SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
            } else
            {
                FindObjectOfType<GenerateLevel>().InstantiateRoom(connectedRoom.x, connectedRoom.y);
            }
            
            
        }
    }

    public bool IsLocked()
    {
        return (lockedUntilEnimiesKilled && !currentRoom.IsCleared());
    }
}
