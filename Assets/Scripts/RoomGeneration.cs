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
    [SerializeField] GameObject playerPrefab1;
    [SerializeField] GameObject playerPrefab2;
    [SerializeField] GameObject bossPrefab;

    //Has to be public so Instantiation copies these properties?
    public bool doorUp;
    public bool doorDown;
    public bool doorRight;
    public bool doorLeft;
    public bool start1;
    public bool start2;
    public bool boss;
    

    // Start is called before the first frame update
    void Start()
    {
        //room = GetComponent<Room>();
        //room.SelectDoors();
        //SelectDoors();
        CreateWalls();
        if (start1)
        {
            Instantiate(playerPrefab1, origin.transform.position + new Vector3(width / 2f, height / 2f, 0f), Quaternion.identity);
        }
        else if (start2) {
            Instantiate(playerPrefab2, origin.transform.position + new Vector3(width / 2f, height / 2f, 0f), Quaternion.identity);
        } else if (boss)
        {
            Instantiate(bossPrefab, origin.transform.position + new Vector3(width / 2f, height / 2f, 0f), Quaternion.identity);
        }
    }

    private void CreateWalls()
    {
        //Debug.Log(doorUp);
        for (int i = 0; i <= width; i++)
        {
            float x = origin.transform.position.x + unitSize * i;
            float y1 = origin.transform.position.y;
            float y2 = origin.transform.position.y + width * unitSize;
            
            if (Mathf.Abs(width / 2 - doorSize*i) > doorSize || !hasDoorDown())
            {
                Instantiate(wall, new Vector3(x, y1, 0f), Quaternion.identity, transform);
            }
            if (Mathf.Abs(width / 2 - doorSize * i) > doorSize || !hasDoorUp())
            {
                Instantiate(wall, new Vector3(x, y2, 0f), Quaternion.identity, transform);
            }
        }

        for (int i = 0; i <= height; i++)
        {
            float y = origin.transform.position.y + unitSize * i;
            float x1 = origin.transform.position.x;
            float x2 = origin.transform.position.x + height * unitSize;
            if (Mathf.Abs(height / 2 - doorSize * i) > doorSize || !hasDoorLeft())
            {
                Instantiate(wall, new Vector3(x1, y, 0f), Quaternion.identity, transform);
            }
            if (Mathf.Abs(height / 2 - doorSize * i) > doorSize || !hasDoorRight())
            {
                Instantiate(wall, new Vector3(x2, y, 0f), Quaternion.identity, transform);
            }
        }
    }

    public void SelectDoors(float up=0.5f, float right=0.5f, float down=0.5f, float left=0.5f)
    {
        doorUp = Random.value < up;
        doorDown = Random.value < down;
        doorRight = Random.value < right;
        doorLeft = Random.value < left;
    }

    public Vector2 GetSize()
    {
        return new Vector2(width * unitSize, height * unitSize);
    }

    public bool hasDoorUp()
    {
        return doorUp;
    }

    public bool hasDoorDown()
    {
        return doorDown;
    }

    public bool hasDoorRight()
    {
        return doorRight;
    }

    public bool hasDoorLeft()
    {
        return doorLeft;
    }

}
