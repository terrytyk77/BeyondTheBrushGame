using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    //Variables||



        GameObject player;
        private Vector3 cameraGoal;

        //player
        private float cameraPercentageLimit = 0.2f;
        private float cameraSpeed = 4f;

    //_________||


    // Start is called before the first frame update
    void Start()
    {
        //Get the player element
        player = GameObject.FindGameObjectWithTag("Player");
        cameraSpeed = player.GetComponent<PlayerMovement>().playerSpeed;
        //Get the player position
        Vector2 playerPos = player.GetComponent<Transform>().position;

        //Set the camera starting position to the player pos
        gameObject.GetComponent<Transform>().position = new Vector3(playerPos.x, playerPos.y, gameObject.transform.position.z);

        //Set the camera goal as the current camera position
        cameraGoal = gameObject.GetComponent<Transform>().position;
    }


    private void UpdatedCameraGoal()
    {
        //Update the camera goal
        Vector3 playerPos = player.GetComponent<Rigidbody2D>().position;
        cameraGoal = new Vector3(playerPos.x, playerPos.y, cameraGoal.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Get the current main camera
        Camera mainC = Camera.main;
        Vector3 cameraPosition = mainC.GetComponent<Transform>().position;

        //Get player position
        Vector2 playerPosition = player.GetComponent<Rigidbody2D>().position;

        //Get the camera absolute sizes
        float height = 2f * mainC.orthographicSize;
        float width = height * mainC.aspect;

        //Camera offset
        float heightOffset = height * cameraPercentageLimit;
        float widthOffset = width * cameraPercentageLimit;

        //Check the borders offsets
        if (playerPosition.y > cameraPosition.y + heightOffset / 2)
        {        
            UpdatedCameraGoal();
            //Camera input
            decimal cameraOffsetInput = decimal.Round((decimal)(cameraSpeed * Time.fixedDeltaTime), 2);

            //Set the camera goal
            Camera.main.GetComponent<Transform>().position = Vector3.MoveTowards(cameraPosition, cameraGoal, (float)cameraOffsetInput);
        } 
        else if (playerPosition.y < cameraPosition.y - heightOffset / 2)
        {
            UpdatedCameraGoal();
            //Camera input
            decimal cameraOffsetInput = decimal.Round((decimal)(cameraSpeed * Time.fixedDeltaTime), 2);

            //Set the camera goal
            Camera.main.GetComponent<Transform>().position = Vector3.MoveTowards(cameraPosition, cameraGoal, (float)cameraOffsetInput);
        } 
        else if (playerPosition.x > cameraPosition.x + widthOffset / 2)
        {
            UpdatedCameraGoal();
            //Camera input
            decimal cameraOffsetInput = decimal.Round((decimal)(cameraSpeed * Time.fixedDeltaTime), 2);

            //Set the camera goal
            Camera.main.GetComponent<Transform>().position = Vector3.MoveTowards(cameraPosition, cameraGoal, (float)cameraOffsetInput);
        }
        else if (playerPosition.x < cameraPosition.x - widthOffset/2)
        {
            UpdatedCameraGoal();
            //Camera input
            decimal cameraOffsetInput = decimal.Round((decimal)(cameraSpeed * Time.fixedDeltaTime), 2);

            //Set the camera goal
            Camera.main.GetComponent<Transform>().position = Vector3.MoveTowards(cameraPosition, cameraGoal, (float)cameraOffsetInput);
        }


        /*
        if (cameraGoal != cameraPosition && playerOffLimits)
        {
            //Camera input
            decimal cameraOffsetInput = decimal.Round( (decimal)(cameraSpeed * Time.fixedDeltaTime) , 2);

            //Set the camera goal
            Camera.main.GetComponent<Transform>().position = Vector3.MoveTowards(cameraPosition, cameraGoal, (float)cameraOffsetInput);

        }
        */





    }
}
