using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    //Variables||

        //player
        GameObject player;
        public float cameraPercentageLimit = 0.7f;
    //_________||


    // Start is called before the first frame update
    void Start()
    {
        //Get the player element
        player = GameObject.FindGameObjectWithTag("Player");

        //Get the player position
        Vector2 playerPos = player.GetComponent<Transform>().position;

        //Set the camera starting position to the player pos
        gameObject.GetComponent<Transform>().position = new Vector3(playerPos.x, playerPos.y, gameObject.transform.position.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Get the current main camera
        Camera mainC = Camera.main;
        Vector3 cameraPosition = mainC.GetComponent<Transform>().position;

        //Get player position
        Vector2 playerPosition = player.GetComponent<Transform>().position;

        //Get the camera absolute sizes
        float height = 2f * mainC.orthographicSize;
        float width = height * mainC.aspect;

        //Camera offset
        float heightOffset = height * cameraPercentageLimit + mainC.GetComponent<Transform>().position.y;
        float widthOffset = width * cameraPercentageLimit + mainC.GetComponent<Transform>().position.x;

        if (playerPosition.y > cameraPosition.y + heightOffset / 2)
        {
            //Got out on the topside

        } else if (playerPosition.y < cameraPosition.y - heightOffset / 2)
        //Got out on the botside

        {

        } else if (playerPosition.x > cameraPosition.x + widthOffset / 2){
            //Got out on the right side

        

        }else if (playerPosition.x < cameraPosition.x - widthOffset/2)
        {
            //Got out on the left side

        }







    }
}
