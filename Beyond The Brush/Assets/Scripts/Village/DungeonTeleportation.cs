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
            //This means that the user who entered is the played
            //Start the teleportation of scene proccess
            GameObject sceneAPI = GameObject.FindGameObjectWithTag("sceneAPI");



            if (sceneAPI)
            {
                //Store the next dungeon name
                sceneAPI.GetComponent<sceneAPI>().nextDungeon = dungeonName;

                //Freeze the player to avoid errors
                collision.gameObject.GetComponent<Rigidbody2D>().constraints = 
                RigidbodyConstraints2D.FreezePositionX | 
                RigidbodyConstraints2D.FreezePositionY | 
                RigidbodyConstraints2D.FreezeRotation;

                //Send the data to the other side
                GameObject newDungeon = Instantiate(sceneAPI, Vector2.zero, Quaternion.identity);

                DontDestroyOnLoad(sceneAPI);

                //load the new scene
                sceneAPI.GetComponent<sceneAPI>().teleportToNewScene(2);
            }
        }
    }


}
