using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatedObjects : MonoBehaviour
{
    public GameObject Instantiate(GameObject instance, Vector3 location)
    {
        GameObject instantiated = Instantiate(instance, location, Quaternion.identity);
        instantiated.transform.parent = gameObject.transform;
        return instantiated;
    }

    public void RemoveAll()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
