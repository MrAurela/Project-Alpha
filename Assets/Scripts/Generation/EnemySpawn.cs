using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    //Any number of enymy types that can be at this location
    [SerializeField] GameObject[] enemyTypes;

    //Weigths for the enemies in enemyTypes.
    [SerializeField] float[] probabilities;

    private RoomGeneration roomGenerator;
    private Room room;

    // Start is called before the first frame update
    void Start()
    {
        //Set information about the room
        this.roomGenerator = transform.root.GetComponent<RoomGeneration>();
        this.room = this.roomGenerator.room;

        //Setup random generation with room seed
        //Random.InitState(this.room.Seed);

        //Spawn enemies if room has NOT BEEN CLEARED
        if (!this.room.IsCleared() && !this.room.IsSafeRoom)
        {
            GameObject enemy = SelectEnemyType();
            if (enemy != null)
            {
                GameObject instantiatedEnemy = Instantiate(enemy, transform.position, Quaternion.identity);
                instantiatedEnemy.transform.parent = this.roomGenerator.transform.Find("Layout").transform; //Put enemies under Layout so they are moved with the room when the room if flipped

                //Set references to correct character object in Enemy_Movement and Enemy_Shooting scripts
                instantiatedEnemy.GetComponent<Enemy_Movement>().Player = this.roomGenerator.playerPrefab.GetComponent<Rigidbody2D>();
                if (instantiatedEnemy.transform.childCount > 0) //Has weapon
                {
                    instantiatedEnemy.transform.GetChild(0).gameObject.GetComponent<Enemy_Shooting>().Player = this.roomGenerator.playerPrefab.GetComponent<Rigidbody2D>();
                }

                //Counts the number of enemies added to keep count how many are left in a room
                roomGenerator.IncreaseNumberOfEnemies();
            }
        }
    }

    //Randomly selects the enemy type based on the weights given.
    //If enemy types are: enemy1, enemy2, enemy3 and
    //probabilities are: 0.4, 0.2, 0.25
    //Then the enemy is selected with following probabilities:
    //    enemy1: 40%
    //    enemy2: 20%
    //    enemy3: 25%
    //    no enemy: 15%
    //
    //Returns: GameObject of the enemy or null
    private GameObject SelectEnemyType()
    {
        float random = roomGenerator.RandomFloat(0f, 1f);
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

}
