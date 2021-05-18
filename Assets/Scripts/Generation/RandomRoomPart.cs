using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Random = UnityEngine.Random;

public class RandomRoomPart : MonoBehaviour
{
    [SerializeField] GameObject[] roomParts;
    [SerializeField] float[] probabilities;

    [SerializeField] float horizontalFlipProbability = 0.5f; float verticalFlipProbability = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        RoomGeneration roomGenerator = transform.root.GetComponent<RoomGeneration>();
        Room room = roomGenerator.room;
        QRand qrand = FindObjectOfType<QRand>();
        qrand.InitState(room.Seed + (int)transform.position.x * 10000000 + (int)transform.position.y * 1000000000); //Change the seed based on the location
        float random = qrand.NextFloat();

        //Random.InitState(room.Seed); //TODO something addition to make new combinations
        //float random = Random.value;

        Debug.Log(random);
        float cumulativeProbability = 0f;
        for (int i = 0; i < probabilities.Length; i++)
        {
            cumulativeProbability += probabilities[i];
            if (cumulativeProbability >= random)
            {
                GameObject roomPart = Instantiate(roomParts[i], transform.position, Quaternion.identity, transform.parent);
                //roomPart.transform.parent = roomGenerator.transform.Find("Layout").transform;
                break;
            }
        }

        /*for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            GameObject child = gameObject.transform.GetChild(i).gameObject;
            if (x) child.transform.localPosition = new Vector3(-child.transform.localPosition.x, child.transform.localPosition.y, child.transform.localPosition.z);
            if (y) child.transform.localPosition = new Vector3(child.transform.localPosition.x, -child.transform.localPosition.y, child.transform.localPosition.z);
            child.transform.parent = roomGenerator.transform.Find("Layout").transform;
        }*/

        //This game object is not neede anymore
        Destroy(gameObject);
    }

}
