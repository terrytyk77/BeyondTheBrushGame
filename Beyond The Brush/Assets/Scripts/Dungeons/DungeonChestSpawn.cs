using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonChestSpawn : MonoBehaviour
{
    private GameObject chestSpawnPoint;
    private Transform[] childrenChestSpawnPoint;
    private GameObject spawnedChest;
    private int chestRandomPosition;
    private Vector3 chestPosition;

    public GameObject WoodenChestPrefab;

    void Start()
    {
        chestSpawnPoint = GameObject.Find("ChestSpawnPoint");

        if (chestSpawnPoint != null)
        {
            //Initialize child Array
            childrenChestSpawnPoint = chestSpawnPoint.GetComponentsInChildren<Transform>();
            
            //Get Random Position
            chestRandomPosition = Random.Range(1, childrenChestSpawnPoint.Length);
            
            //Get Position To Spawn Chest
            chestPosition = childrenChestSpawnPoint[chestRandomPosition].transform.position;

            //Spawn Chest
            spawnedChest = Instantiate(WoodenChestPrefab, chestPosition, Quaternion.identity);
            spawnedChest.transform.SetParent(transform);
        }
    }
}
