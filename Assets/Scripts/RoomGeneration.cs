using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomGeneration : MonoBehaviour
{
    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] float unitSize;
    [SerializeField] int doorSize;
    [SerializeField] GameObject origin;
    [SerializeField] GameObject wall;
    [SerializeField] GameObject floor;
    [SerializeField] GameObject door;
    [SerializeField] GameObject bossPrefab;
    [SerializeField] GameObject enemyPrefab;
    //[SerializeField] Tilemap tilemap;
    //[SerializeField] TileBase wallTile, floorTile;
    public GameObject playerPrefab;

    public Room room;

    private int numberOfEnemies = 0;
    private QRand qrand;

    public void SetPlayerTo(GameObject character, int id)
    {
        playerPrefab = character;
        if (id == 1)
        {
            playerPrefab.transform.position = origin.transform.position + new Vector3(width / 2f, 3f, 0f);
        }
        else if (id == 3)
        {
            playerPrefab.transform.position = origin.transform.position + new Vector3(width / 2f, height - 3f, 0f);
        }     
        else if (id == 2)
        {
            playerPrefab.transform.position = origin.transform.position + new Vector3(3f, height / 2f, 0f);
        }
        else if (id == 4)
        {
            playerPrefab.transform.position = origin.transform.position + new Vector3(width - 3f, height / 2f, 0f);
        }  
        else {
            playerPrefab.transform.position = origin.transform.position + new Vector3(width / 2f, height / 2f, 0f);
        }

    }

    public float RandomFloat(float f1, float f2)
    {
        float rand = qrand.NextFloat();
        Debug.Log(rand);
        return rand*(f2-f1)+f1;
    }
    public int RandomFloat(int i1, int i2)
    {
        return Random.Range(i1, i2);
    }

    public void IncreaseNumberOfEnemies()
    {
        this.numberOfEnemies++;
    }

    public void DecreaseNumberOfEnemies()
    {
        this.numberOfEnemies--;
        if (this.numberOfEnemies == 0)
        {
            this.room.SetRoomCleared();
        }
    }

    void Awake()
    {
        qrand = FindObjectOfType<QRand>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Setup random generation with room seed
        //Random.InitState(this.room.Seed);
        qrand.InitState(this.room.Seed);


        //Instantiate outer structures: walls & doors
        CreateWalls();
        

        //Mirror the room layout when needed
        if (room.IsTVertical() && room.HasDoorUp)
            transform.GetChild(2).transform.localScale = new Vector3(1, -1, 1);
        if (room.IsTHorizontal() && room.HasDoorRight)
            transform.GetChild(2).transform.localScale = new Vector3(-1, 1, 1);
        if (room.IsL() && (room.HasDoorUp && room.HasDoorLeft))
            transform.GetChild(2).transform.localScale = new Vector3(-1, 1, 1);
        if (room.IsL() && (room.HasDoorDown && room.HasDoorLeft))
            transform.GetChild(2).transform.localScale = new Vector3(-1, -1, 1);
        if (room.IsL() && (room.HasDoorDown && room.HasDoorRight))
            transform.GetChild(2).transform.localScale = new Vector3(1, -1, 1);
        if (room.IsDEHorizontal() && room.HasDoorRight)
            transform.GetChild(2).transform.localScale = new Vector3(-1, 1, 1);
        if (room.IsDEVertical() && room.HasDoorDown)
            transform.GetChild(2).transform.localScale= new Vector3(1, -1, 1);
    }

    private void CreateWalls()
    {

        //tilemap = FindObjectOfType<Tilemap>();
        //GameObject tileSystem = Instantiate(tileSystem, new Vector3(0f, 0f, 0f), Quaternion.identity);
        //tilemap.SetTile(new Vector3Int(0, 0, 0), wallTile);

        for (int i = 0; i < width; i++)
        {
            float x = origin.transform.position.x + unitSize * i;
            float y1 = origin.transform.position.y;
            float y2 = origin.transform.position.y + (height-1) * unitSize;
            
            if (Mathf.Abs(x) > doorSize*unitSize || !room.HasDoorDown)
            {
                Instantiate(wall, new Vector3(x, y1, 0f), Quaternion.identity, transform);
            } else
            {
                GameObject newDoor = Instantiate(door, new Vector3(x, y1, 0f), Quaternion.identity, transform);
                newDoor.GetComponent<Door>().SetConnection(this.room, FindObjectOfType<GenerateLevel>().GetRoom(this.room.x, this.room.y - 1));
            }
            if (Mathf.Abs(x) > doorSize * unitSize || !room.HasDoorUp)
            {
                Instantiate(wall, new Vector3(x, y2, 0f), Quaternion.identity, transform);
            } else
            {
                GameObject newDoor = Instantiate(door, new Vector3(x, y2, 0f), Quaternion.identity, transform);
                newDoor.GetComponent<Door>().SetConnection(this.room, FindObjectOfType<GenerateLevel>().GetRoom(this.room.x, this.room.y + 1));
            }
        }


        for (int i = 0; i < height; i++)
        {
            float y = origin.transform.position.y + unitSize * i;
            float x1 = origin.transform.position.x;
            float x2 = origin.transform.position.x + (width-1) * unitSize;
            if (Mathf.Abs(y) > doorSize * unitSize || !room.HasDoorLeft)
            {
                Instantiate(wall, new Vector3(x1, y, 0f), Quaternion.identity, transform);
                
            } else
            {
                GameObject newDoor = Instantiate(door, new Vector3(x1, y, 0f), Quaternion.identity, transform);
                newDoor.GetComponent<Door>().SetConnection(this.room, FindObjectOfType<GenerateLevel>().GetRoom(this.room.x - 1, this.room.y));
            }
            if (Mathf.Abs(y) > doorSize * unitSize || !room.HasDoorRight)
            {
                Instantiate(wall, new Vector3(x2, y, 0f), Quaternion.identity, transform);
            } else
            {
                GameObject newDoor = Instantiate(door, new Vector3(x2, y, 0f), Quaternion.identity, transform);
                newDoor.GetComponent<Door>().SetConnection(this.room, FindObjectOfType<GenerateLevel>().GetRoom(this.room.x + 1, this.room.y));
            }
        }

        for (int i = 0; i < height - 1; i++)
        {
            for (int j = 0; j < width - 1; j++)
            {
                float y = origin.transform.position.y + i*unitSize;
                float x = origin.transform.position.x + j*unitSize;
                Instantiate(floor, new Vector3(x, y, 0f), Quaternion.identity, transform);
            }
        }
    }

    public Vector2 GetSize()
    {
        return new Vector2(width * unitSize, height * unitSize);
    }

}
