using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTriggers : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {

        //Get the side name
        string objectName = gameObject.name;

        //Check if it was the player colliding
        if (collision.gameObject.CompareTag("Player"))
        {
            //Get the procedural data of the dungeon
            GameObject dungeonData = GameObject.FindGameObjectWithTag("proceduralData");

            //Check if it found the object
            if (dungeonData != null)
            {
                dungeonData.GetComponent<CurrentDungeonData>().changeNextRoom(objectName);
            }

        }

    }

}
