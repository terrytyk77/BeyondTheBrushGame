using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonTeleportation : MonoBehaviour
{

    //Variables||
        public string dungeonName = "";
    //_________||


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
                //Store the next dungeon name
                sceneTeleport.dungeonName = dungeonName;

                //Freeze the player to avoid errors
                collision.gameObject.GetComponent<Rigidbody2D>().constraints = 
                RigidbodyConstraints2D.FreezePositionX | 
                RigidbodyConstraints2D.FreezePositionY | 
                RigidbodyConstraints2D.FreezeRotation;

                //load the new scene
                sceneTeleport.start(2);
        }
    }
}
