using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    //Variables||

        public float playerSpeed = 4f;
    //_________||



    // Start is called before the first frame update
    void Start()
    {

    }

    // Fixed update is called 50 times per frame
    void FixedUpdate()
    {
        //Get the player rigid body component
        Rigidbody2D playerBody = gameObject.GetComponent<Rigidbody2D>();

        //Player movement magnitude
        float movementMagnitude = playerSpeed;

        //New force
        Vector2 newForce = new Vector2(0, 0);

        if (Input.GetKey("w") && !Input.GetKey("s"))
        {
            newForce.y = movementMagnitude;
        }else if (Input.GetKey("s") && !Input.GetKey("w"))
        {
            newForce.y = -movementMagnitude;
        }else if (Input.GetKey("a") && !Input.GetKey("d"))
        {
            newForce.x = -movementMagnitude;

            //Change the image side
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }else if (Input.GetKey("d") && !Input.GetKey("a"))
        {
            newForce.x = movementMagnitude;

            //Change the image side
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }





        //Apply the force on the player's body
        playerBody.velocity = newForce;

    }
}
