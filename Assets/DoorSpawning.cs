using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSpawning : MonoBehaviour
{
    [SerializeField] bool doorUp;
    [SerializeField] bool doorRight;
    [SerializeField] bool doorDown;
    [SerializeField] bool doorLeft;

    Room room;

    // Start is called before the first frame update
    void Start()
    {
        room = transform.parent.GetComponent<RoomGeneration>().room;
    }

    // Update is called once per frame
    void Update()
    {
        if ((room.HasDoorUp && doorUp) || (room.HasDoorRight && doorRight) || (room.HasDoorDown && doorDown) || (room.HasDoorLeft && doorLeft))
        {
            SpawnDoor();
        }
    }

    private void SpawnDoor()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
    }
}
