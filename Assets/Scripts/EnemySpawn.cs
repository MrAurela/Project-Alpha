using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] GameObject[] enemyTypes;
    [SerializeField] float[] probabilities;

    private RoomGeneration roomGenerator;
    private Room room;

    // Start is called before the first frame update
    void Start()
    {
        this.roomGenerator = transform.root.GetComponent<RoomGeneration>();
        this.room = this.roomGenerator.room;

        //Setup random generation
        Random.InitState(this.room.seed);

        //Spawn enemies if room has not been cleared already
        if (!this.room.IsCleared)
        {
            GameObject enemy = SelectEnemyType();
            if (enemy != null)
            {
                GameObject instantiatedEnemy = Instantiate(enemy, transform.position, Quaternion.identity);
                instantiatedEnemy.GetComponent<Enemy_Movement>().Player = this.roomGenerator.playerPrefab.GetComponent<Rigidbody2D>();
                instantiatedEnemy.transform.GetChild(0).gameObject.GetComponent<Enemy_Shooting>().Player = this.roomGenerator.playerPrefab.GetComponent<Rigidbody2D>();
            }
        }
    }

    private GameObject SelectEnemyType()
    {
        float random = Random.Range(0f, 1f);
        float cumulativeProbability = 0f;
        for (int i = 0; i < probabilities.Length; i++)
        {
            cumulativeProbability += probabilities[i];
            if (cumulativeProbability >= random)
            {
                return enemyTypes[i];
            }
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
