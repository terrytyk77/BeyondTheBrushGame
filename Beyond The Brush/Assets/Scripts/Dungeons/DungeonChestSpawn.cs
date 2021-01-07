using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonChestSpawn : MonoBehaviour
{
    private GameObject chestSpawnPoint;
    private GameObject chestParent;

    private Transform[] childrenChestSpawnPoint;
    private GameObject spawnedChest;
    private int chestRandomPosition;
    private Vector3 chestPosition;

    public GameObject WoodenChestPrefab;
    public GameObject IronChestPrefab;

    private int ChestTypeRandomizer;

    private GameObject EnemyParent;

    void Start()
    {
        chestSpawnPoint = GameObject.Find("ChestSpawnPoint");
        chestParent = GameObject.Find("Chests");

        if (chestSpawnPoint != null)
        {
            //Initialize child Array
            childrenChestSpawnPoint = chestSpawnPoint.GetComponentsInChildren<Transform>();
            
            //Get Random Position
            chestRandomPosition = Random.Range(1, childrenChestSpawnPoint.Length);
            
            //Get Position To Spawn Chest
            chestPosition = childrenChestSpawnPoint[chestRandomPosition].transform.position;

            //Type of Chest
            ChestTypeRandomizer = Random.Range(1, 11);

            //Spawn Chest 30% Iron 70% Wood!
            if (ChestTypeRandomizer % 3 == 0)
            {
                spawnedChest = Instantiate(IronChestPrefab, chestPosition, Quaternion.identity);
            }
            else
            {
                spawnedChest = Instantiate(WoodenChestPrefab, chestPosition, Quaternion.identity);
            }
            
            //Adjust Parent
            spawnedChest.transform.SetParent(chestParent.transform);
        }
    }

    void Update()
    {
        //Checks If Room Is Completed!
        EnemyParent = GameObject.Find("Enemies");
        chestParent = GameObject.Find("Chests");

        if (EnemyParent.transform.childCount == 0 && chestParent.transform.childCount == 0)
        {
            CurrentDungeonData currentData = GameObject.FindGameObjectWithTag("proceduralData").GetComponent<CurrentDungeonData>();
            currentData.getRoomViaCords(currentData.currentRoom).setCompleted(true);
        }
    }
}
