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
    [SerializeField] RoomGeneration room;
    [SerializeField] int levelWidth;
    [SerializeField] int levelHeight;

    // Start is called before the first frame update
    void Start()
    {
        Generate();
    }

    private void Generate()
    {
        Instantiate(camera1, world1Origin + cameraOffset, Quaternion.identity);
        Instantiate(camera2, world2Origin + cameraOffset, Quaternion.identity);
        
        for (int x = 0; x < levelWidth; x++)
        {
            for (int y = 0; y < levelHeight; y++)
            {
                Vector3 offset = new Vector3(x * room.GetSize().x, y * room.GetSize().y);
                Instantiate(room, world1Origin + offset, Quaternion.identity);
                Instantiate(room, world2Origin + offset, Quaternion.identity);
            }
        }
    }
}
