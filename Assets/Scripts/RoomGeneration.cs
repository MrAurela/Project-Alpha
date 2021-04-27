using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGeneration : MonoBehaviour
{
    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] float unitSize;
    [SerializeField] int doorSize;
    [SerializeField] GameObject origin;
    [SerializeField] GameObject wall;
    [SerializeField] GameObject bossPrefab;
    [SerializeField] GameObject enemyPrefab;
    public GameObject playerPrefab;

    public Room room;
    

    // Start is called before the first frame update
    void Start()
    {
        //Instantiate outer structures: walls & doors
        CreateWalls();

        SetEnemyTargets();

        //Instantiate players
        if (room.IsStartRoom)
        {
            Instantiate(playerPrefab, origin.transform.position + new Vector3(width / 2f, height / 2f, 0f), Quaternion.identity);
        } else if(room.IsBossRoom)
        {
            Instantiate(bossPrefab, origin.transform.position + new Vector3(width / 2f, height / 2f, 0f), Quaternion.identity);
        }

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
        //Horizontal walls & doors
        for (int i = 0; i <= width; i++)
        {
            float x = origin.transform.position.x + unitSize * i;
            float y1 = origin.transform.position.y;
            float y2 = origin.transform.position.y + height * unitSize;
            
            if (Mathf.Abs(width / 2 - doorSize*i) > doorSize || !room.HasDoorDown)
            {
                Instantiate(wall, new Vector3(x, y1, 0f), Quaternion.identity, transform);
            }
            if (Mathf.Abs(width / 2 - doorSize * i) > doorSize || !room.HasDoorUp)
            {
                Instantiate(wall, new Vector3(x, y2, 0f), Quaternion.identity, transform);
            }
        }

        //Vertical walls & doors
        for (int i = 0; i <= height; i++)
        {
            float y = origin.transform.position.y + unitSize * i;
            float x1 = origin.transform.position.x;
            float x2 = origin.transform.position.x + width * unitSize;
            if (Mathf.Abs(height / 2 - doorSize * i) > doorSize || !room.HasDoorLeft)
            {
                Instantiate(wall, new Vector3(x1, y, 0f), Quaternion.identity, transform);
            }
            if (Mathf.Abs(height / 2 - doorSize * i) > doorSize || !room.HasDoorRight)
            {
                Instantiate(wall, new Vector3(x2, y, 0f), Quaternion.identity, transform);
            }
        }
    }

    private void SetEnemyTargets()
    {

        foreach (Transform child in transform)
        {
            foreach (Transform grandchild in transform.transform)
            {
                if (transform.transform.gameObject.tag == "Spawn")
                {
                    GameObject enemy = Instantiate(enemyPrefab, transform.transform.position, Quaternion.identity);
                    //enemy.GetComponent<Enemy_Movement>().Player = playerRigidbody;
                    Debug.Log("moi");
                }
            }
        }
    }

    public Vector2 GetSize()
    {
        return new Vector2(width * unitSize, height * unitSize);
    }

}
