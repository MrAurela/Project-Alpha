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

    private bool doorUp;
    private bool doorDown;
    private bool doorRight;
    private bool doorLeft;

    // Start is called before the first frame update
    void Start()
    {
        SelectDoors();
        CreateWalls();
    }

    private void SelectDoors()
    {
        doorUp = Random.value > 0.5f;
        doorDown = Random.value > 0.5f;
        doorRight = Random.value > 0.5f;
        doorLeft = Random.value > 0.5f;
    }

    private void CreateWalls()
    {
        for (int i = 0; i <= width; i++)
        {
            float x = origin.transform.position.x + unitSize * i;
            float y1 = origin.transform.position.y;
            float y2 = origin.transform.position.y + width * unitSize;
            
            if (Mathf.Abs(width / 2 - doorSize*i) > doorSize || !doorDown)
            {
                Instantiate(wall, new Vector3(x, y1, 0f), Quaternion.identity);
            }
            if (Mathf.Abs(width / 2 - doorSize * i) > doorSize || !doorUp)
            {
                Instantiate(wall, new Vector3(x, y2, 0f), Quaternion.identity);
            }
        }

        for (int i = 0; i <= height; i++)
        {
            float y = origin.transform.position.y + unitSize * i;
            float x1 = origin.transform.position.x;
            float x2 = origin.transform.position.x + height * unitSize;
            if (Mathf.Abs(height / 2 - doorSize * i) > doorSize || !doorLeft)
            {
                Instantiate(wall, new Vector3(x1, y, 0f), Quaternion.identity);
            }
            if (Mathf.Abs(height / 2 - doorSize * i) > doorSize || !doorRight)
            {
                Instantiate(wall, new Vector3(x2, y, 0f), Quaternion.identity);
            }
        }
    }

}
