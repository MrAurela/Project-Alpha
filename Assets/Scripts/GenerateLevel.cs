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
        roomStructure = new GameObject[levelHeight][];
        GenerateLevelLayout();
        InstantiateLevel();
    }

    private void GenerateLevelLayout()
    {

        bool[][] isVisited = new bool[levelHeight][];
        for (int y = 0; y < levelWidth; y++)
        {
            roomStructure[y] = new GameObject[levelWidth];
            isVisited[y] = new bool[levelWidth];
            for (int x = 0; x < levelHeight; x++)
            {
                roomStructure[y][x] = Instantiate(room);
                roomStructure[y][x].GetComponent<RoomGeneration>().SelectDoors(0f,0f,0f,0f);
            }
        }

        List<Vector2> nextCoordinates = new List<Vector2>();
        nextCoordinates.Add(new Vector2(0, 0));
        Vector2 selectedCoordinate = new Vector2(0, 0);

        Vector2 direction = new Vector2(0,0); 
        
        while (nextCoordinates.Count > 0)
        {
            Vector2 lastCoordinate = selectedCoordinate;
            selectedCoordinate = nextCoordinates[Random.Range(0, nextCoordinates.Count)];

            direction = selectedCoordinate - lastCoordinate;

            //Door from the last room on current room
            if (direction.x > 0) roomStructure[(int)lastCoordinate.y][(int)lastCoordinate.x].GetComponent<RoomGeneration>().doorRight = true;
            else if (direction.x < 0) roomStructure[(int)lastCoordinate.y][(int)lastCoordinate.x].GetComponent<RoomGeneration>().doorLeft = true;
            else if (direction.y > 0) roomStructure[(int)lastCoordinate.y][(int)lastCoordinate.x].GetComponent<RoomGeneration>().doorUp = true;
            else if (direction.y < 0) roomStructure[(int)lastCoordinate.y][(int)lastCoordinate.x].GetComponent<RoomGeneration>().doorDown = true;

            //Door from current room to the last room
            if (-direction.x > 0) roomStructure[(int)selectedCoordinate.y][(int)selectedCoordinate.x].GetComponent<RoomGeneration>().doorRight = true;
            else if (-direction.x < 0) roomStructure[(int)selectedCoordinate.y][(int)selectedCoordinate.x].GetComponent<RoomGeneration>().doorLeft = true;
            else if (-direction.y > 0) roomStructure[(int)selectedCoordinate.y][(int)selectedCoordinate.x].GetComponent<RoomGeneration>().doorUp = true;
            else if (-direction.y < 0) roomStructure[(int)selectedCoordinate.y][(int)selectedCoordinate.x].GetComponent<RoomGeneration>().doorDown = true;
            

            isVisited[(int)selectedCoordinate.y][(int)selectedCoordinate.x] = true;

            nextCoordinates = new List<Vector2>();
            if (validCoordinate(selectedCoordinate + Vector2.right))
            {
                nextCoordinates.Add(selectedCoordinate + Vector2.right);
            }
            if (validCoordinate(selectedCoordinate + Vector2.left))
            {
                nextCoordinates.Add(selectedCoordinate + Vector2.left);
            }
            if (validCoordinate(selectedCoordinate + Vector2.up))
            {
                nextCoordinates.Add(selectedCoordinate + Vector2.up);
            }
            if (validCoordinate(selectedCoordinate + Vector2.down))
            {
                nextCoordinates.Add(selectedCoordinate + Vector2.down);
            }

        }

        /*for (int y = 0; y < levelWidth; y++)
        {
            roomStructure[y] = new GameObject[levelWidth];
            for (int x = 0; x < levelHeight; x++)
            {
                if (roomStructure[y][x] == null) {
                    //roomStructure[y][x] = Instantiate(room);
                    //roomStructure[y][x].GetComponent<RoomGeneration>().SelectDoors(0f,0f,0f,0f);
                }
            }
        }*/

        bool validCoordinate(Vector2 c) {
            return (c.x >= 0 && c.x < levelWidth && c.y >= 0 && c.y < levelHeight && !isVisited[(int)c.y][(int)c.x]);
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
                Vector3 offset = new Vector3(x * roomWidth, y * roomHeight);
                roomStructure[y][x].transform.position = world1Origin + offset;
                Instantiate(roomStructure[y][x], world2Origin + offset, Quaternion.identity);
            }
        }
    }
}
