using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    //Variables||

        public float playerSpeed = 4f;
        Rigidbody2D playerBody;
        GameObject playerVertical;
        GameObject playerHorizontal;
        bool HorVerSide = false;
    //_________||



    // Start is called before the first frame update
    void Start()
    {
        playerVertical  = transform.Find("Vertical").gameObject;
        playerHorizontal = transform.Find("Horizontal").gameObject;
    }

    // Fixed update is called 50 times per frame
    void FixedUpdate()
    {
        //Get the player rigid body component
        playerBody = gameObject.GetComponent<Rigidbody2D>();

        //Player movement magnitude
        float movementMagnitude = playerSpeed;

        //New force
        Vector2 newForce = new Vector2(0, 0);

        if (Input.GetKey("w") && !Input.GetKey("s"))
        {
            playerVerticalPerspective();
            newForce.y = movementMagnitude;
            HorVerSide = false;

        }
        else if (Input.GetKey("s") && !Input.GetKey("w"))
        {
            playerVerticalPerspective();
            newForce.y = -movementMagnitude;
            HorVerSide = false;
        }
        else if (Input.GetKey("a") && !Input.GetKey("d"))
        {
            playerHorizontalPerspective();
            newForce.x = -movementMagnitude;
            HorVerSide = true;

            //Change the image side
            playerHorizontal.transform.rotation = new Quaternion(0, 180, 0, 1);
        }
        else if (Input.GetKey("d") && !Input.GetKey("a"))
        {
            playerHorizontalPerspective();
            newForce.x = movementMagnitude;
            HorVerSide = true;

            //Change the image side
            playerHorizontal.transform.rotation = new Quaternion(0, 0, 0, 1);
        }
        else
        {
            if (HorVerSide)
            {
                playerHorizontal.GetComponent<Animator>().SetBool("Moving", false);
            }
            else
            {
                playerVertical.GetComponent<Animator>().SetBool("Moving", false);
            }
        }

        //Apply the force on the player's body
        playerBody.velocity = newForce;

    }

    private void playerVerticalPerspective()
    {
        playerVertical.SetActive(true);
        playerHorizontal.SetActive(false);
        playerVertical.GetComponent<Animator>().SetBool("Moving", true);
    }

    private void playerHorizontalPerspective()
    {
        playerHorizontal.SetActive(true);
        playerVertical.SetActive(false);
        playerHorizontal.GetComponent<Animator>().SetBool("Moving", true);
    }
}
