using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Chest : MonoBehaviour
{
    //Coins         ||
        public int MinCoinAmount = 1;
        public int MaxCoinAmount = 1;
        private int CoinAmountRandomizer;
    //--------------||

    //Collision     ||
        public Tile InvisibleCollisionTile;

        private GameObject collisionObject;
        private Tilemap collisionTilemap;

        private Vector3Int collisionTile;

        Vector3 HalfTile;
    //--------------||

    void Start()
    {
        collisionObject = GameObject.FindGameObjectWithTag("CollisionLayer");
        collisionTilemap = collisionObject.GetComponent<Tilemap>();
        HalfTile = new Vector3(collisionTilemap.cellSize.x / 2, collisionTilemap.cellSize.y / 2, 0);

        //To Place The Chest Pixel Accurate With the Tilemap
        transform.position = collisionTilemap.CellToWorld(collisionTilemap.WorldToCell(transform.position)) + HalfTile;

        //Set Invisible Collision Tile Behind Chest
        collisionTile = collisionTilemap.WorldToCell(transform.position);
        collisionTilemap.SetTile(collisionTile, InvisibleCollisionTile);
    }

    private void OnDestroy()
    {
        if(collisionTilemap != null)
        {
            collisionTilemap.SetTile(collisionTile, null);
        }

        //Coins
    }
}
