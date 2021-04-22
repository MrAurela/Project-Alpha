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

    // Start is called before the first frame update
    void Start()
    {
        Generate();
    }

    private void Generate()
    {
        Instantiate(camera1, world1Origin + cameraOffset, Quaternion.identity);
        Instantiate(camera2, world2Origin + cameraOffset, Quaternion.identity);
    }
}
