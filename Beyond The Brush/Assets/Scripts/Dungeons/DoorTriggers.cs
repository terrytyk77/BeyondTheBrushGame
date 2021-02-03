using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTriggers : MonoBehaviour
{

    //Variables||
        
        private bool exitDebounce = true;
    //_________||

    private void OnTriggerStay2D(Collider2D collision)
    {

        //Check if it was the player colliding
        if (collision.gameObject.CompareTag("Player") && exitDebounce)
        {
            exitDebounce = false;
            //Get the side name
            string objectName = gameObject.name;


            //Get the procedural data of the dungeon
            GameObject dungeonData = GameObject.FindGameObjectWithTag("proceduralData");

            //Check if it found the object
            if (dungeonData != null)
            {
                //Stop the player movement
                Rigidbody2D playerRB = collision.gameObject.GetComponent<Rigidbody2D>();
                playerRB.constraints = RigidbodyConstraints2D.FreezeAll;

                
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

                    if (gameObject.name == "exit" && !exitDebounce)
                    {
                        
                        DontDestroyOnLoad(dungeonData);                              //Avoid destroying this object to use as reference for the other side teleport
                        sceneTeleport.start(1);                                     //If the door equals to exit then take him to the village
                        //dungeonData.GetComponent<CurrentDungeonData>().changeNextRoom(objectName);
                    }
                    else if(gameObject.name != "exit" && !exitDebounce)
                    {
                        playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
                        dungeonData.GetComponent<CurrentDungeonData>().changeNextRoom(objectName);

                    }

                    exitDebounce = true;

                }

            }

        }

    }


}
