using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    public Tile doorTile;

    private GameObject collisionObject;
    private Tilemap collisionTilemap;

    private Vector3Int collisionTile;

    // Start is called before the first frame update
    void Start()
    {
        collisionObject = GameObject.FindGameObjectWithTag("CollisionLayer");
        collisionTilemap = collisionObject.GetComponent<Tilemap>();
        collisionTile = collisionTilemap.WorldToCell(transform.position);
        collisionTilemap.SetTile(collisionTile, doorTile);
    }

    public void DestroyDoor()
    {
        if (!collisionTilemap)
        {
            collisionObject = GameObject.FindGameObjectWithTag("CollisionLayer");
            collisionTilemap = collisionObject.GetComponent<Tilemap>();
        }
        collisionTilemap.SetTile(collisionTile, null);
        gameObject.SetActive(false);
    }
}
