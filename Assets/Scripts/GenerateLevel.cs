using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevel : MonoBehaviour
{
    //Variables that can be changed in the editor
    [SerializeField] Vector3 world1Origin, world2Origin;
    [SerializeField] GameObject character1Prefab, character2Prefab;
    [SerializeField] GameObject room;
    [SerializeField] int levelWidth, levelHeight;
    [SerializeField] GameObject[] verticalDE, horizontalDE, verticalI, horizontalI, L, verticalT, horizontalT, X, boss, start;
    //[SerializeField] Tilemap tilemap;
    //[SerializeField] TileBase wallTile, floorTile;

    //2D layout of the rooms that will be randomly generated. Adjacent room can be connected with doors
    private Room[][] roomStructure;

    private float roomHeight;
    private float roomWidth;

    //Current room coordinates of the player
    private int locationX;
    private int locationY;

    //Instantiated character objects:
    private GameObject character1, character2;

    //Currently instantiated room layout objects:
    private GameObject room1, room2;

    private QRand qrand;

    // Start is called before the first frame update
    void Start()
    {
        qrand = FindObjectOfType<QRand>();

        roomHeight = room.GetComponent<RoomGeneration>().GetSize().y;
        roomWidth = room.GetComponent<RoomGeneration>().GetSize().x; 

        //Characters are instantieated
        character1 = Instantiate(character1Prefab, new Vector3(0, 0, 0), Quaternion.identity);
        character2 = Instantiate(character2Prefab, new Vector3(0, 0, 0), Quaternion.identity);

        //Character map location is set
        //locationX = Random.Range(0,levelWidth);
        //locationY = Random.Range(0,levelHeight);
        locationX = qrand.NextInt(levelWidth);
        locationY = qrand.NextInt(levelHeight);

        //Connections of the rooms will be generated. Rooms are saved to roomStructure variable
        roomStructure = new Room[levelHeight][];
        GenerateLevelLayout();

        //First room will be instantiated
        InstantiateRoom(locationX, locationY, true);  
    }

    //DEBUG: keys to change rooms quickly
    void Update()
    {
        try
        {
            if (Input.GetKeyDown("t"))
            {
                FindObjectOfType<GenerateLevel>().InstantiateRoom(locationX, locationY + 1);
            } else if(Input.GetKeyDown("h"))
            {
                FindObjectOfType<GenerateLevel>().InstantiateRoom(locationX + 1, locationY);
            } else if (Input.GetKeyDown("g"))
            {
                FindObjectOfType<GenerateLevel>().InstantiateRoom(locationX, locationY - 1);
            }
            else if (Input.GetKeyDown("f"))
            {
                FindObjectOfType<GenerateLevel>().InstantiateRoom(locationX - 1, locationY);
            }
        }
        catch (Exception e)
        {
            Debug.LogFormat("Likely out of bounds (maybe other error).\nException: {0}",e);
        }
        
    }

    //Generates the main layout of the level
    private void GenerateLevelLayout()
    {
        List<Vector2> pathToBoss = new List<Vector2>();

        //Room object are created and placed at roomStructure variabel
        int[][] path = new int[levelHeight][];
        for (int y = 0; y < levelWidth; y++)
        {
            roomStructure[y] = new Room[levelWidth];
            path[y] = new int[levelWidth];
            for (int x = 0; x < levelHeight; x++)
            {
                roomStructure[y][x] = new Room();
                roomStructure[y][x].x = x;
                roomStructure[y][x].y = y;
                roomStructure[y][x].Seed = qrand.NextInt(10000000);
                Debug.LogFormat("LevelLayout Seed: {0}",qrand.GetCurrentSeedInt());

                Debug.Log("Room created at: " + x + ", " + y);
            }
        }

        //nextCoordinates is a list of possible room coordinates that can be visited in the main path
        List<Vector2> nextCoordinates = new List<Vector2>();
        nextCoordinates.Add(new Vector2(locationX, locationY));

        //selectedCoordinate is the coordinates of the room that was selected to be visited next. Initialized with starting coordinates
        Vector2 selectedCoordinate = new Vector2(locationX, locationY);

        //These coordinates also determine the Start room
        roomStructure[locationY][locationX].IsStartRoom = true;
        roomStructure[locationY][locationX].IsSafeRoom = true; 
        roomStructure[locationY][locationX].SetRoomCleared(2); //2=both rooms are ready

        //The main path continues as long as at least on neighbour room exists that has not yet been visited
        while (nextCoordinates.Count > 0)
        {
            //Select random neighbour room
            Vector2 lastCoordinate = selectedCoordinate;
            selectedCoordinate = nextCoordinates[qrand.NextInt(nextCoordinates.Count)];

            //Create door between the rooms
            CreateDoorway(lastCoordinate, selectedCoordinate);
            path[(int)selectedCoordinate.y][(int)selectedCoordinate.x] = path[(int)lastCoordinate.y][(int)lastCoordinate.x] + 1;

            //List of unvisited neighbours
            nextCoordinates = NeighbourCoordinates(selectedCoordinate, false,true);


            // Debug.Log(selectedCoordinate);
        }

        //The last room on the path will be the Boss room
        roomStructure[(int)selectedCoordinate.y][(int)selectedCoordinate.x].IsBossRoom = true;
        Debug.Log("Boss: " + selectedCoordinate.x + ", " + selectedCoordinate.y);


        //Create doors to rooms that are not part of the main path and add some random extra doors between rooms
        for (int y = 0; y < levelWidth; y++)
        {
            for (int x = 0; x < levelHeight; x++)
            {
                Vector2 coordinate = new Vector2(x, y);
                if (path[y][x] == 0) //If room was not found in previous step, combine it to some found room
                {
                    //List of visited neighbour
                    nextCoordinates = NeighbourCoordinates(coordinate,true);

                    if (nextCoordinates.Count == 0)
                    {
                        //List of all neighbors
                        nextCoordinates = NeighbourCoordinates(coordinate); //Any neighbours
                    }

                    //Select random neighbour
                    //selectedCoordinate = nextCoordinates[Random.Range(0, nextCoordinates.Count)];
                    selectedCoordinate = nextCoordinates[qrand.NextInt(nextCoordinates.Count-1)];

                    //Create door
                    CreateDoorway(coordinate, selectedCoordinate);

                } else if (!roomStructure[(int)coordinate.y][(int)coordinate.x].IsBossRoom) //Randomly add shortcuts for other rooms
                {
                    nextCoordinates = new List<Vector2>();
                    if (validCoordinate(new Vector2(x, y + 1)) && closeEnough(new Vector2(x, y + 1), coordinate, 5)) nextCoordinates.Add(new Vector2(x, y + 1));
                    if (validCoordinate(new Vector2(x, y - 1)) && closeEnough(new Vector2(x, y - 1), coordinate, 5)) nextCoordinates.Add(new Vector2(x, y - 1));
                    if (validCoordinate(new Vector2(x + 1, y)) && closeEnough(new Vector2(x + 1, y), coordinate, 5)) nextCoordinates.Add(new Vector2(x + 1, y));
                    if (validCoordinate(new Vector2(x - 1, y)) && closeEnough(new Vector2(x - 1, y), coordinate, 5)) nextCoordinates.Add(new Vector2(x - 1, y));

                    if (nextCoordinates.Count > 0)
                    {
                        //Select random neighbour
                        //selectedCoordinate = nextCoordinates[Random.Range(0, nextCoordinates.Count)];
                        selectedCoordinate = nextCoordinates[qrand.NextInt(nextCoordinates.Count)];

                        //Create door
                        CreateDoorway(coordinate, selectedCoordinate);
                    }
                }

            }


        }

        List<Vector2> NeighbourCoordinates(Vector2 c, bool mustBeVisited=false, bool mustBeNotVisited=false)
        {
            List<Vector2> nextCoordinates = new List<Vector2>();
            if (mustBeNotVisited)
            {
                if (notVisited(new Vector2(c.x, c.y + 1))) nextCoordinates.Add(new Vector2(c.x, c.y + 1));
                if (notVisited(new Vector2(c.x, c.y - 1))) nextCoordinates.Add(new Vector2(c.x, c.y - 1));
                if (notVisited(new Vector2(c.x + 1, c.y))) nextCoordinates.Add(new Vector2(c.x + 1, c.y));
                if (notVisited(new Vector2(c.x - 1, c.y))) nextCoordinates.Add(new Vector2(c.x - 1, c.y));
            } else if (mustBeVisited)
            {
                if (validCoordinate(new Vector2(c.x, c.y + 1)) && !notVisited(new Vector2(c.x, c.y + 1)))
                    nextCoordinates.Add(new Vector2(c.x, c.y + 1));
                if (validCoordinate(new Vector2(c.x, c.y - 1)) && !notVisited(new Vector2(c.x, c.y - 1)))
                    nextCoordinates.Add(new Vector2(c.x, c.y - 1));
                if (validCoordinate(new Vector2(c.x + 1, c.y)) && !notVisited(new Vector2(c.x + 1, c.y)))
                    nextCoordinates.Add(new Vector2(c.x + 1, c.y));
                if (validCoordinate(new Vector2(c.x - 1, c.y)) && !notVisited(new Vector2(c.x - 1, c.y)))
                    nextCoordinates.Add(new Vector2(c.x - 1, c.y));
            } else
            {
                if (validCoordinate(new Vector2(c.x, c.y + 1))) nextCoordinates.Add(new Vector2(c.x, c.y + 1));
                if (validCoordinate(new Vector2(c.x, c.y - 1))) nextCoordinates.Add(new Vector2(c.x, c.y - 1));
                if (validCoordinate(new Vector2(c.x + 1, c.y))) nextCoordinates.Add(new Vector2(c.x + 1, c.y));
                if (validCoordinate(new Vector2(c.x - 1, c.y))) nextCoordinates.Add(new Vector2(c.x - 1, c.y));
            } 
            return nextCoordinates;
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

    //Returns the Room object at map coordinates x,y
    public Room GetRoom(int x, int y)
    {
        return this.roomStructure[y][x];
    }

    //Instantiates the room at map coordinates x,y.
    //If sameRoom=true, instantiation happens even if the the same room has already been instantiated
    public void InstantiateRoom(int x, int y, bool sameRoom=false)
    {
        if (x == locationX && y == locationY && !sameRoom)
        {
            Debug.Log("ERROR! Cannot instantiate the same room unless in speacial circumstances!");
            return;
        }

        Debug.Log("Instantiating room " + x + ", " + y);
        Debug.Log("Room object: " + this.roomStructure[y][x]);

        if (room1) Destroy(room1);
        if (room2) Destroy(room2);

        Room room = roomStructure[y][x];
        GameObject roomTemplate = SelectRoomTemplate(room);

        room1 = Instantiate(roomTemplate, world1Origin, Quaternion.identity);
        Debug.Log(roomTemplate);
        Debug.Log(room1);
        Debug.Log(room1.GetComponent<RoomGeneration>());
        Debug.Log(room1.GetComponent<RoomGeneration>().room);
        room1.GetComponent<RoomGeneration>().room = room;
        Debug.Log(room1.GetComponent<RoomGeneration>().room);

        room2 = Instantiate(roomTemplate, world2Origin, Quaternion.identity);
        room2.GetComponent<RoomGeneration>().room = room;

        room1.GetComponent<RoomGeneration>().playerPrefab = character1;
        room2.GetComponent<RoomGeneration>().playerPrefab = character2;

        int id = 0;
        if (y > locationY) id = 1;
        else if (x > locationX) id = 2;
        else if (y < locationY) id = 3;
        else if (x < locationX) id = 4;

        room1.GetComponent<RoomGeneration>().SetPlayerTo(character1, id);
        room2.GetComponent<RoomGeneration>().SetPlayerTo(character2, id);

        locationX = x;
        locationY = y;
    }


    //Randomly selects and returns room template based on the shape of the room
    private GameObject SelectRoomTemplate(Room room)
    {
        GameObject[] choices = new GameObject[] { };

        if (room.IsBossRoom) choices = boss;
        else if (room.IsStartRoom) choices = start;
        else if (room.IsX()) choices = X;
        else if (room.IsTVertical()) choices = verticalT;
        else if (room.IsTHorizontal()) choices = horizontalT;
        else if (room.IsL()) choices = L;
        else if (room.IsIVertical()) choices = verticalI;
        else if (room.IsIHorizontal()) choices = horizontalI;
        else if (room.IsDEVertical()) choices = verticalDE;
        else if (room.IsDEHorizontal()) choices = horizontalDE;
        else
        {
            //This should never happen!
            Debug.LogFormat("This should not have happened.\n{0}",room.ToString());
            return null;
        }

        //Random.InitState(room.Seed); //Initialize random generator with the room seed to select the same room type every time this room is visited
        //return choices[Random.Range(0, choices.Length)];

        qrand.InitState(room.Seed);
        return choices[qrand.NextInt(choices.Length)];
    }
}
