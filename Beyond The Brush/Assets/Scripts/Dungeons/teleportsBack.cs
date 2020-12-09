using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleportsBack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        GameObject dungeonData = GameObject.FindGameObjectWithTag("proceduralData");


        if (dungeonData != null)
        {
            //Variables||
                Dungeon dungeonUsed = dungeonData.GetComponent<CurrentDungeonData>().currentDungeon;
                string dungeonName = dungeonUsed.DungeonName;
            //_________||

            foreach (Transform location in gameObject.transform)
            {

                if (dungeonName.ToLower() == location.gameObject.name.ToLower())
                {
                    GameObject player = GameObject.FindGameObjectWithTag("Player");

                    if(player != null)
                    {

                        player.GetComponent<Transform>().position = new Vector2(location.position.x, location.position.y);
                    }
                }
            }


        }        
    }

}
