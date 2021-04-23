using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevel : MonoBehaviour
{
    [SerializeField] GameObject camera1;
    [SerializeField] GameObject camera2;
    [SerializeField] Vector3 cameraOffset;
    [SerializeField] Vector3 world1Origin;
    [SerializeField] Vector3 world2Origin;
    [SerializeField] GameObject room;
    [SerializeField] int levelWidth;
    [SerializeField] int levelHeight;

    private GameObject[][] roomStructure; 
    private float roomHeight;
    private float roomWidth;

    // Start is called before the first frame update
    void Start()
    {
        roomHeight = room.GetComponent<RoomGeneration>().GetSize().y;
        roomWidth = room.GetComponent<RoomGeneration>().GetSize().x;
        Debug.Log(roomHeight + ", " + roomWidth);
        roomStructure = new GameObject[levelHeight][];
        GenerateLevelLayout();
        InstantiateLevel();
    }

    private void GenerateLevelLayout()
    {
        for (int y = 0; y < levelWidth; y++)
        {
            roomStructure[y] = new GameObject[levelWidth];
            for (int x = 0; x < levelHeight; x++)
            {
                roomStructure[y][x] = Instantiate(room);
                roomStructure[y][x].GetComponent<RoomGeneration>().SelectDoors();
            }
        }
    }

    private void InstantiateLevel()
    {
        Instantiate(camera1, world1Origin + cameraOffset, Quaternion.identity);
        Instantiate(camera2, world2Origin + cameraOffset, Quaternion.identity);

        for (int x = 0; x < levelWidth; x++)
        {
            for (int y = 0; y < levelHeight; y++)
            {
                Debug.Log(room.GetComponent<RoomGeneration>().hasDoorUp());
                Vector3 offset = new Vector3(x * roomWidth, y * roomHeight);
                roomStructure[y][x].transform.position = world1Origin + offset;
                Instantiate(roomStructure[y][x], world2Origin + offset, Quaternion.identity);
            }
        }
    }
}
