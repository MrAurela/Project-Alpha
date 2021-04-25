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
    [SerializeField] GameObject character1Prefab;
    [SerializeField] GameObject character2Prefab;
    [SerializeField] GameObject room;
    [SerializeField] int levelWidth;
    [SerializeField] int levelHeight;

    private Room[][] roomStructure;
    private float roomHeight;
    private float roomWidth;

    // Start is called before the first frame update
    void Start()
    {
        roomHeight = room.GetComponent<RoomGeneration>().GetSize().y;
        roomWidth = room.GetComponent<RoomGeneration>().GetSize().x;
        roomStructure = new Room[levelHeight][];
        GenerateLevelLayout();
        InstantiateLevel();
    }

    private void GenerateLevelLayout()
    {

        int[][] path = new int[levelHeight][];
        for (int y = 0; y < levelWidth; y++)
        {
            roomStructure[y] = new Room[levelWidth];
            path[y] = new int[levelWidth];
            for (int x = 0; x < levelHeight; x++)
            {
                roomStructure[y][x] = new Room();
                //roomStructure[y][x].SetDoors(0f);
            }
        }

        List<Vector2> nextCoordinates = new List<Vector2>();
        nextCoordinates.Add(new Vector2(0, 0));
        Vector2 selectedCoordinate = new Vector2(0, 0);

        roomStructure[0][0].IsStartRoom = true;

        Vector2 direction = new Vector2(0,0); 
        
        while (nextCoordinates.Count > 0)
        {
            Vector2 lastCoordinate = selectedCoordinate;
            selectedCoordinate = nextCoordinates[Random.Range(0, nextCoordinates.Count)];

            CreateDoorway(lastCoordinate, selectedCoordinate);

            path[(int)selectedCoordinate.y][(int)selectedCoordinate.x] = path[(int)lastCoordinate.y][(int)lastCoordinate.x] + 1;

            nextCoordinates = new List<Vector2>();
            if (notVisited(selectedCoordinate + Vector2.right))
            {
                nextCoordinates.Add(selectedCoordinate + Vector2.right);
            }
            if (notVisited(selectedCoordinate + Vector2.left))
            {
                nextCoordinates.Add(selectedCoordinate + Vector2.left);
            }
            if (notVisited(selectedCoordinate + Vector2.up))
            {
                nextCoordinates.Add(selectedCoordinate + Vector2.up);
            }
            if (notVisited(selectedCoordinate + Vector2.down))
            {
                nextCoordinates.Add(selectedCoordinate + Vector2.down);
            }

        }

        roomStructure[(int)selectedCoordinate.y][(int)selectedCoordinate.x].IsBossRoom = true;

        //Creating doors to unvisited rooms and some shortcuts
        for (int y = 0; y < levelWidth; y++)
        {
            for (int x = 0; x < levelHeight; x++)
            {
                Vector2 coordinate = new Vector2(x, y);
                if (path[y][x] == 0)
                {
                    nextCoordinates = new List<Vector2>();
                    if (validCoordinate(new Vector2(x, y + 1)) && !notVisited(new Vector2(x, y + 1))) nextCoordinates.Add(new Vector2(x, y + 1));
                    if (validCoordinate(new Vector2(x, y - 1)) && !notVisited(new Vector2(x, y - 1))) nextCoordinates.Add(new Vector2(x, y - 1));
                    if (validCoordinate(new Vector2(x + 1, y)) && !notVisited(new Vector2(x + 1, y))) nextCoordinates.Add(new Vector2(x + 1, y));
                    if (validCoordinate(new Vector2(x - 1, y)) && !notVisited(new Vector2(x - 1, y))) nextCoordinates.Add(new Vector2(x - 1, y));

                    if (nextCoordinates.Count == 0)
                    {
                        if (validCoordinate(new Vector2(x, y + 1))) nextCoordinates.Add(new Vector2(x, y + 1));
                        if (validCoordinate(new Vector2(x, y - 1))) nextCoordinates.Add(new Vector2(x, y - 1));
                        if (validCoordinate(new Vector2(x + 1, y))) nextCoordinates.Add(new Vector2(x + 1, y));
                        if (validCoordinate(new Vector2(x - 1, y))) nextCoordinates.Add(new Vector2(x - 1, y));
                    }

                    selectedCoordinate = nextCoordinates[Random.Range(0, nextCoordinates.Count)];
                    CreateDoorway(coordinate, selectedCoordinate);

                } else if (!roomStructure[(int)coordinate.y][(int)coordinate.x].IsBossRoom)
                {
                    nextCoordinates = new List<Vector2>();
                    if (validCoordinate(new Vector2(x, y + 1)) && closeEnough(new Vector2(x, y + 1), coordinate, 5)) nextCoordinates.Add(new Vector2(x, y + 1));
                    if (validCoordinate(new Vector2(x, y - 1)) && closeEnough(new Vector2(x, y - 1), coordinate, 5)) nextCoordinates.Add(new Vector2(x, y - 1));
                    if (validCoordinate(new Vector2(x + 1, y)) && closeEnough(new Vector2(x + 1, y), coordinate, 5)) nextCoordinates.Add(new Vector2(x + 1, y));
                    if (validCoordinate(new Vector2(x - 1, y)) && closeEnough(new Vector2(x - 1, y), coordinate, 5)) nextCoordinates.Add(new Vector2(x - 1, y));

                    if (nextCoordinates.Count > 0)
                    {
                        selectedCoordinate = nextCoordinates[Random.Range(0, nextCoordinates.Count)];
                        CreateDoorway(coordinate, selectedCoordinate);
                    }
                }

            }
        }

        Vector2 NeighbourCoordinates(Vector2 c, bool allowVisited=true)
        {
            return new Vector2(0, 0);
        }

        void CreateDoorway(Vector2 room1Coordinate, Vector2 room2Coordinate)
        {
            Vector2 direction = room2Coordinate - room1Coordinate;

            //Door from room1 to room2
            if (direction.x > 0) roomStructure[(int)room1Coordinate.y][(int)room1Coordinate.x].HasDoorRight = true;
            else if (direction.x < 0) roomStructure[(int)room1Coordinate.y][(int)room1Coordinate.x].HasDoorLeft = true;
            else if (direction.y > 0) roomStructure[(int)room1Coordinate.y][(int)room1Coordinate.x].HasDoorUp = true;
            else if (direction.y < 0) roomStructure[(int)room1Coordinate.y][(int)room1Coordinate.x].HasDoorDown = true;

            //Door from room2 to room1
            if (-direction.x > 0) roomStructure[(int)room2Coordinate.y][(int)room2Coordinate.x].HasDoorRight = true;
            else if (-direction.x < 0) roomStructure[(int)room2Coordinate.y][(int)room2Coordinate.x].HasDoorLeft = true;
            else if (-direction.y > 0) roomStructure[(int)room2Coordinate.y][(int)room2Coordinate.x].HasDoorUp = true;
            else if (-direction.y < 0) roomStructure[(int)room2Coordinate.y][(int)room2Coordinate.x].HasDoorDown = true;
        }

        bool validCoordinate(Vector2 c) {
            return (c.x >= 0 && c.x < levelWidth && c.y >= 0 && c.y < levelHeight);
        }

        bool notVisited(Vector2 c)
        {
            return (validCoordinate(c) && path[(int)c.y][(int)c.x] == 0);
        }

        bool closeEnough(Vector2 c1, Vector2 c2, int distance)
        {
            return (Mathf.Abs(path[(int)c1.y][(int)c1.x] - path[(int)c2.y][(int)c2.x]) <= distance);
        }
    }

    private void InstantiateLevel()
    {

        for (int x = 0; x < levelWidth; x++)
        {
            for (int y = 0; y < levelHeight; y++)
            {
                Vector3 offset = new Vector3(x * roomWidth, y * roomHeight, 0f);

                GameObject player1 = Instantiate(room, world1Origin + offset, Quaternion.identity);
                player1.GetComponent<RoomGeneration>().room = roomStructure[y][x];

                GameObject player2 = Instantiate(room, world2Origin + offset, Quaternion.identity);
                player2.GetComponent<RoomGeneration>().room = roomStructure[y][x];

                if (roomStructure[y][x].IsStartRoom)
                {
                    player1.GetComponent<RoomGeneration>().playerPrefab = character1Prefab;
                    player2.GetComponent<RoomGeneration>().playerPrefab = character2Prefab;
                }
                
            }
        }
    }
}
