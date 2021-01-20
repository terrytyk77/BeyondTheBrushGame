﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    //Variables||
        public GameObject playerVertical;
        public GameObject playerHorizontal;
        public GameObject playerOverlay;
        Rigidbody2D playerBody;
        bool HorVerSide = false;
        public static bool verticalDirection = false; //False down, True up

        public GameObject joystick; 
    //_________||



    // Start is called before the first frame update
    void Start()
    {
        playerVerticalPerspective();
    }

    // Fixed update is called 50 times per frame
    void FixedUpdate()
    {

        //Joystick direction||

            bool goingUp = false;
            bool goingDown = false;
            bool goingRight = false;
            bool goingLeft = false;
        //__________________||

        DuloGames.UI.UIJoystick joystickComp = joystick.GetComponent<DuloGames.UI.UIJoystick>();
        if (joystickComp != null)
        {
            Vector2 axis = joystickComp.JoystickAxis;

            //-0.8X   0.5Y >

            //Going up
            if (axis.y > 0 && axis.x <= 0.75f && axis.x >= -0.75f)
                goingUp = true;

            //Going down
            if (axis.y < 0 && axis.x < 0.75f && axis.x > -0.75f)
                goingDown = true;

            //Going left
            if (axis.x < 0 && axis.y <= 0.75f && axis.y >= -0.75f)
                goingLeft = true;

            //Going right
            if (axis.x > 0 && axis.y < 0.75f && axis.y > -0.75f)
                goingRight = true;

        }

        //Get the player rigid body component
        playerBody = gameObject.GetComponent<Rigidbody2D>();

        //Player movement magnitude
        float movementMagnitude = PlayerData.movementSpeed;

        //New force
        Vector2 newForce = new Vector2(0, 0);

        if ((Input.GetKey("w") && !Input.GetKey("s")) || goingUp)
        {
            PlayerData.playerDirection = 0;

            playerVerticalPerspective();
            newForce.y = movementMagnitude;
            HorVerSide = false;
            verticalDirection = true;
            transform.localScale = new Vector3(transform.localScale.y, transform.localScale.y, transform.localScale.z);
            GameObject.FindGameObjectWithTag("Player").GetComponent<ArmorChange>().changedVerticalDirection(verticalDirection, ArmorChange.currentDefault);

        }
        else if ((Input.GetKey("s") && !Input.GetKey("w")) || goingDown)
        {
            PlayerData.playerDirection = 1;

            playerVerticalPerspective();
            newForce.y = -movementMagnitude;
            HorVerSide = false;
            verticalDirection = false;
            transform.localScale = new Vector3(transform.localScale.y, transform.localScale.y, transform.localScale.z);
            GameObject.FindGameObjectWithTag("Player").GetComponent<ArmorChange>().changedVerticalDirection(verticalDirection, ArmorChange.currentDefault);
        }
        else if ((Input.GetKey("a") && !Input.GetKey("d")) || goingLeft)
        {
            PlayerData.playerDirection = 2;

            playerHorizontalPerspective();
            newForce.x = -movementMagnitude;
            HorVerSide = true;
            //Change the image side
            transform.localScale = new Vector3(-transform.localScale.y, transform.localScale.y, transform.localScale.z);
            GameObject.FindGameObjectWithTag("Player").GetComponent<ArmorChange>().changeHorizontalDirection(true);
        }
        else if ((Input.GetKey("d") && !Input.GetKey("a")) || goingRight)
        {
            PlayerData.playerDirection = 3;

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

        if(playerOverlay)
        {
            playerOverlay.transform.localPosition = new Vector2(playerBody.position.x, playerBody.position.y);
        }

        //Apply the force on the player's body
        playerBody.velocity = newForce;
        playerVertical.GetComponent<Animator>().SetInteger("Direction", PlayerData.playerDirection);
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
