using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Random = UnityEngine.Random;

public class RandomRoomPart : MonoBehaviour
{
    [SerializeField] GameObject[] roomParts;
    [SerializeField] float[] probabilities;
    [SerializeField] float[] flipProbabilities; //x, y, both, otherwise no flip

    // Start is called before the first frame update
    void Start()
    {
        RoomGeneration roomGenerator = transform.root.GetComponent<RoomGeneration>();
        Room room = roomGenerator.room;
        QRand qrand = FindObjectOfType<QRand>();
       // qrand.InitState(room.Seed); //Change the seed based on the location
        float random = qrand.NextFloat();

        //Random.InitState(room.Seed); //TODO something addition to make new combinations
        //float random = Random.value;

        //Debug.Log(random);
        float cumulativeProbability = 0f;
        for (int i = 0; i < probabilities.Length; i++)
        {
            cumulativeProbability += probabilities[i];
            if (cumulativeProbability >= random)
            {
                GameObject roomPart = Instantiate(roomParts[i], transform.position, Quaternion.identity, transform.parent);
                //roomPart.transform.parent = roomGenerator.transform.Find("Layout").transform;
                roomPart.transform.parent = gameObject.transform;
                break;
            }
        }

        random = qrand.NextFloat();
        cumulativeProbability = 0f;
        bool x = false;
        bool y = false;
        for (int i = 0; i < flipProbabilities.Length; i++)
        {
            cumulativeProbability += flipProbabilities[i];
            if (cumulativeProbability >= random)
            {
                if (i == 0 || i == 2) x = true;
                if (i == 1 || i == 2) y = true;
                Debug.Log("Probs: "+i);
            }
        }

        Debug.Log(x + ", " + y);

        int j = 0;
        if (transform.childCount > 0) {
            foreach (Transform child in transform.GetChild(0))
            {
                if (x) child.localPosition = new Vector3(-child.localPosition.x, child.localPosition.y, child.localPosition.z);
                if (y) child.localPosition = new Vector3(child.localPosition.x, -child.localPosition.y, child.localPosition.z);
                child.parent = roomGenerator.transform.Find("Layout").transform;
                Debug.Log("Childs: " + j);
                j += 1;
            }
        }
        

        //This game object is not neede anymore
        Destroy(gameObject);
    }

}
