using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    //Variables||
        public GameObject playerVertical;
        public GameObject playerHorizontal;
        Rigidbody2D playerBody;
        bool HorVerSide = false;
        public static bool verticalDirection = false; //False down, True up
    //_________||



    // Start is called before the first frame update
    void Start()
    {
        playerVerticalPerspective();
    }

    // Fixed update is called 50 times per frame
    void FixedUpdate()
    {
        //Get the player rigid body component
        playerBody = gameObject.GetComponent<Rigidbody2D>();

        //Player movement magnitude
        float movementMagnitude = PlayerData.movementSpeed;

        //New force
        Vector2 newForce = new Vector2(0, 0);

        if (Input.GetKey("w") && !Input.GetKey("s"))
        {
            playerVerticalPerspective();
            newForce.y = movementMagnitude;
            HorVerSide = false;
            verticalDirection = true;
            transform.localScale = new Vector3(transform.localScale.y, transform.localScale.y, transform.localScale.z);
            GameObject.FindGameObjectWithTag("Player").GetComponent<ArmorChange>().changedVerticalDirection(verticalDirection, ArmorChange.currentDefault);

        }
        else if (Input.GetKey("s") && !Input.GetKey("w"))
        {
            playerVerticalPerspective();
            newForce.y = -movementMagnitude;
            HorVerSide = false;
            verticalDirection = false;
            transform.localScale = new Vector3(transform.localScale.y, transform.localScale.y, transform.localScale.z);
            GameObject.FindGameObjectWithTag("Player").GetComponent<ArmorChange>().changedVerticalDirection(verticalDirection, ArmorChange.currentDefault);
        }
        else if (Input.GetKey("a") && !Input.GetKey("d"))
        {
            playerHorizontalPerspective();
            newForce.x = -movementMagnitude;
            HorVerSide = true;
            //Change the image side
            transform.localScale = new Vector3(-transform.localScale.y, transform.localScale.y, transform.localScale.z);
            GameObject.FindGameObjectWithTag("Player").GetComponent<ArmorChange>().changeHorizontalDirection(true);
        }
        else if (Input.GetKey("d") && !Input.GetKey("a"))
        {
            playerHorizontalPerspective();
            newForce.x = movementMagnitude;
            HorVerSide = true;
            //Change the image side
            transform.localScale = new Vector3(transform.localScale.y, transform.localScale.y, transform.localScale.z);
            GameObject.FindGameObjectWithTag("Player").GetComponent<ArmorChange>().changeHorizontalDirection(false);
        }
        else
        {
                playerHorizontal.GetComponent<Animator>().SetBool("Moving", false);
                playerVertical.GetComponent<Animator>().SetBool("Moving", false);
        }

        //Apply the force on the player's body
        playerBody.velocity = newForce;

    }

    private void playerVerticalPerspective()
    {
        playerVertical.transform.localScale = new Vector3(1, 1, 0);
        playerHorizontal.transform.localScale = new Vector3(0, 0, 0);
        //playerVertical.SetActive(true);
        //playerHorizontal.SetActive(false);
        playerVertical.GetComponent<Animator>().SetBool("Moving", true);
    }

    private void playerHorizontalPerspective()
    {
        playerVertical.transform.localScale = new Vector3(0, 0, 0);
        playerHorizontal.transform.localScale = new Vector3(1, 1, 0);

        //playerHorizontal.SetActive(true);
        //playerVertical.SetActive(false);
        playerHorizontal.GetComponent<Animator>().SetBool("Moving", true);
    }
}
