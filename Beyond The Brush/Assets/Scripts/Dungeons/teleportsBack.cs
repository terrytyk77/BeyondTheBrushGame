using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleportsBack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        GameObject dungeonData = GameObject.FindGameObjectWithTag("proceduralData");

        //Check if the player came from a dungeonn
        if (dungeonData != null)
        {
            //Variables||
                Dungeon dungeonUsed = dungeonData.GetComponent<CurrentDungeonData>().currentDungeon;
                string dungeonName = dungeonUsed.DungeonName;
            //_________||

            //loop through all teleports
            foreach (Transform location in gameObject.transform)
            {
                //check if the teleport back exists
                if (dungeonName.ToLower() == location.gameObject.name.ToLower())
                {
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    
                    //check if the player exists
                    if(player != null)
                    {

                        //teleports the player to the correct location
                        player.GetComponent<Transform>().position = new Vector2(location.position.x, location.position.y);
                        Camera.main.GetComponent<Transform>().position = new Vector3(location.position.x, location.position.y, Camera.main.transform.position.z);
                    }
                }
            }

            //Get rid of the game object as it is no longer used
            Destroy(dungeonData);

        }        
    }

}
