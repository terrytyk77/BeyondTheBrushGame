using System;
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
                //Stop the player movement
                Rigidbody2D playerRB = collision.gameObject.GetComponent<Rigidbody2D>();
                playerRB.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

                //Call for a camera transition
                Camera.main.GetComponent<PostProcessEvents>().transition(MakeTheRoomTeleport, gameObject.name);

                void MakeTheRoomTeleport()
                {
                    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    GameObject[] deadEnemies = GameObject.FindGameObjectsWithTag("DeadEnemy");
                    GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile");

                    //Reset Alive Enemies
                    if (enemies != null)
                    {
                        foreach (GameObject enemy in enemies)
                        {
                            enemy.GetComponent<EnemyAI>().ResetAI();
                        }
                    }

                    //Remove Flying Projectile
                    if (projectiles != null)
                    {
                        foreach (GameObject projectile in projectiles)
                        {
                            Destroy(projectile);
                        }
                    }

                    //Remove Corpes
                    if (deadEnemies != null)
                    {
                        foreach (GameObject deadEnemy in deadEnemies)
                        {
                            Destroy(deadEnemy);
                        }
                    }


                    playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;

                    if (gameObject.name == "exit")
                    {
                        playerRB.constraints = RigidbodyConstraints2D.FreezeAll;
                        sceneTeleport.start(1);
                    }
                    else
                    {
                        dungeonData.GetComponent<CurrentDungeonData>().changeNextRoom(objectName);
                    }
                    

                }

            }

        }

    }


}
