using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Chest : MonoBehaviour
{
    //Coins         ||
        public int MinCoinAmount = 1;
        public int MaxCoinAmount = 1;
        public GameObject coinPrefab;

        private CircleCollider2D coinCollider;
        private GameObject coinSpawned;
        private int CoinAmountRandomizer;
    //--------------||

    //Collision     ||
        public Tile InvisibleCollisionTile;

        private GameObject collisionObject;
        private Tilemap collisionTilemap;

        private Vector3Int collisionTile;
        private Vector3 startingPosition;

        Vector3 HalfTile;
    //--------------||

        private GameObject spawnedObject;

    void Start()
    {
        startingPosition = gameObject.transform.position;
        //Collisions
        if (collisionTilemap != null)
        {
            collisionObject = GameObject.FindGameObjectWithTag("CollisionLayer");
            collisionTilemap = collisionObject.GetComponent<Tilemap>();

            HalfTile = new Vector3(collisionTilemap.cellSize.x / 2, collisionTilemap.cellSize.y / 2, 0);

            coinCollider = coinPrefab.GetComponent<CircleCollider2D>();

            //Parent GameObject
            spawnedObject = GameObject.Find("SpawnedObjects");

            //To Place The Chest Pixel Accurate With the Tilemap
            transform.position = collisionTilemap.CellToWorld(collisionTilemap.WorldToCell(startingPosition)) + HalfTile;

            //Set Invisible Collision Tile Behind Chest
            collisionTile = collisionTilemap.WorldToCell(transform.position);
            collisionTilemap.SetTile(collisionTile, InvisibleCollisionTile);

            //Set Amount of Coins In Chest
            CoinAmountRandomizer = Random.Range(MinCoinAmount, MaxCoinAmount + 1);
        }
    }

    public void DestroyChest()
    {        
        if (collisionTilemap != null)
        {
            collisionObject = GameObject.FindGameObjectWithTag("CollisionLayer");
            collisionTilemap = collisionObject.GetComponent<Tilemap>();
            collisionTilemap.SetTile(collisionTile, null);
        }

        //Coins
        if (spawnedObject == null)
        {
            spawnedObject = gameObject.transform.parent.parent.Find("SpawnedObjects").gameObject;
        }

        if (CoinAmountRandomizer == 0)
        {
            CoinAmountRandomizer = Random.Range(MinCoinAmount, MaxCoinAmount + 1);
        }

        if (coinCollider == null)
        {
            coinCollider = coinPrefab.GetComponent<CircleCollider2D>();
        }

        for (int i = 0; i < CoinAmountRandomizer; i++)
        {
            //Make the Coins Spawn Inside the Tile
            Vector3 CoinPosition = startingPosition + new Vector3(
                Random.Range(-HalfTile.x + coinCollider.radius / 2 * coinPrefab.transform.localScale.x, HalfTile.x - coinCollider.radius / 2 * coinPrefab.transform.localScale.x),
                Random.Range(-HalfTile.y + coinCollider.radius / 2 * coinPrefab.transform.localScale.y, HalfTile.y - coinCollider.radius / 2 * coinPrefab.transform.localScale.y),
                0);

            coinSpawned = Instantiate(coinPrefab, CoinPosition, Quaternion.identity);
            coinSpawned.transform.SetParent(spawnedObject.transform);
        }

        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }
}
