using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    //Variables||

        //Hold player elements
        public GameObject playerVertical;
        public GameObject playerHorizontal;
        public GameObject playerOverlay;
        Rigidbody2D playerBody;

        bool HorVerSide = false;
        public static bool verticalDirection = false; //False down, True up

        public GameObject joystick; 

        //Store direction stuff
        private KeyCode? lastPressedKey = null;
        private KeyCode? startingDirection = null;
        private bool differentDirection;

        //hold animation stuff
        private bool justEndedBasicSlash = false;
        private bool justEndedXslash = false;
        private bool justEndedShield = false;
    //_________||

    //Optional variable||

        private const KeyCode upKey = KeyCode.W;
        private const KeyCode downKey = KeyCode.S;
        private const KeyCode rightKey = KeyCode.D;
        private const KeyCode leftKey = KeyCode.A;
    //_________________||

    void Start()
    {   
        //Set the basic stuff for the player||

            playerVerticalPerspective();
            playerBody = gameObject.GetComponent<Rigidbody2D>();
        //__________________________________||
    }

    private void OnDisable()
    {
        playerHorizontal.GetComponent<Animator>().SetBool("Moving", false);
        playerVertical.GetComponent<Animator>().SetBool("Moving", false);
        playerBody.velocity = new Vector3(0, 0, 0);
    }

    void Update()
    {

        //Joystick direction||

            bool goingUp = false;
            bool goingDown = false;
            bool goingRight = false;
            bool goingLeft = false;
        //__________________||

        //Variables for the movement||

            differentDirection = false;                         //check if the player changed his direction
            float movementMagnitude = PlayerData.movementSpeed; //Get the player current movement speed
            Vector2 newForce = new Vector2(0, 0);               //Hold the amount of player movement towards a direction
        //__________________________||

        //Changes the looks of the player depending on direction||

            if (Input.GetKeyDown(upKey))
                playerMovingUp();
            else if (Input.GetKeyDown(downKey))
                playerMovingDown();
            else if (Input.GetKeyDown(rightKey))
                playerMovingRight();
            else if (Input.GetKeyDown(leftKey))
                playerMovingLeft();
        //______________________________________________________||

        if ((Input.GetKeyUp(upKey) && lastPressedKey == upKey) 
            || (Input.GetKeyUp(downKey) && lastPressedKey == downKey) 
            || (Input.GetKeyUp(rightKey) && lastPressedKey == rightKey) 
            || (Input.GetKeyUp(leftKey) && lastPressedKey == leftKey))
            lastPressedKey = null;

        if ((Input.GetKeyUp(upKey) && startingDirection == upKey)
            || (Input.GetKeyUp(downKey) && startingDirection == downKey)
            || (Input.GetKeyUp(rightKey) && startingDirection == rightKey)
            || (Input.GetKeyUp(leftKey) && startingDirection == leftKey))
            startingDirection = null;

        if(startingDirection != null && lastPressedKey == null){
            switch(startingDirection){
                case upKey:
                    playerMovingUp();
                    break;
                case downKey:
                    playerMovingDown();
                    break;

                case leftKey:
                    playerMovingLeft();
                    break;
                case rightKey:
                    playerMovingRight();
                    break;
            }
        }else if(startingDirection == null && lastPressedKey == null){
            if (Input.GetKey(upKey))
                playerMovingUp();
            else if(Input.GetKey(downKey))
                playerMovingDown();
            else if (Input.GetKey(leftKey))
                playerMovingLeft();
            else if (Input.GetKey(rightKey))
                playerMovingRight();
        }

        //Fix animations||

            if(playerHorizontal.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("AttackSide") || playerHorizontal.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("ShieldSide"))
        {
                //The animation is on
                justEndedBasicSlash = true;
            }else{
                //The animation is off
                if(justEndedBasicSlash){
                    justEndedBasicSlash = false;
                    playerVertical.GetComponent<Animator>().Rebind();
                }
            }
        //______________||


        //Handle the velocity of the player body
            if (Input.GetKey(upKey) && lastPressedKey == upKey)
            newForce.y = movementMagnitude;
        else if(Input.GetKey(downKey) && lastPressedKey == downKey)
            newForce.y = -movementMagnitude;
        else if (Input.GetKey(rightKey) && lastPressedKey == rightKey)
            newForce.x = movementMagnitude;
        else if (Input.GetKey(leftKey) && lastPressedKey == leftKey)
            newForce.x = -movementMagnitude;
        else if(!Input.GetKey(upKey) && !Input.GetKey(downKey) && !Input.GetKey(rightKey) && !Input.GetKey(leftKey))
        {
            playerHorizontal.GetComponent<Animator>().SetBool("Moving", false);
            playerVertical.GetComponent<Animator>().SetBool("Moving", false);
        }else{
            playerVertical.GetComponent<Animator>().SetBool("Moving", true);
            playerHorizontal.GetComponent<Animator>().SetBool("Moving", true);
        }

        //Add the velocity to the player body and set the animation direction
        playerBody.velocity = newForce;
        playerVertical.GetComponent<Animator>().SetInteger("Direction", PlayerData.playerDirection);

    }

    private void playerMovingUp()
    {
        //Handle the player perspective
        playerVerticalPerspective();
        PlayerData.playerDirection = 0;
        GameObject.FindGameObjectWithTag("Player").GetComponent<ArmorChange>().changedVerticalDirection(true, ArmorChange.currentDefault);
        HorVerSide = false;

        //Handle the hotkeys
        if (startingDirection == null)
            startingDirection = upKey;

        differentDirection = true;
        lastPressedKey = upKey;
    }

    private void playerMovingDown()
    {
        //Handle the player perspective
        playerVerticalPerspective();
        PlayerData.playerDirection = 1;
        GameObject.FindGameObjectWithTag("Player").GetComponent<ArmorChange>().changedVerticalDirection(false, ArmorChange.currentDefault);
        HorVerSide = false;

        //Handle the hotkeys
        if (startingDirection == null)
            startingDirection = downKey;

        differentDirection = true;
        lastPressedKey = downKey;
    }

    private void playerMovingRight()
    {
        //Handle the player perspective
        playerHorizontalPerspective();
        PlayerData.playerDirection = 3;
        transform.localScale = new Vector3(transform.localScale.y, transform.localScale.y, transform.localScale.z);
        HorVerSide = true;

        //Handle the hotkeys
        if (startingDirection == null)
            startingDirection = rightKey;

        differentDirection = true;
        lastPressedKey = rightKey;
    }

    private void playerMovingLeft()
    {
        //Handle the player perspective
        playerHorizontalPerspective();
        PlayerData.playerDirection = 2;
        transform.localScale = new Vector3(-transform.localScale.y, transform.localScale.y, transform.localScale.z);
        HorVerSide = true;

        //Handle the hotkeys
        if (startingDirection == null)
            startingDirection = leftKey;

        differentDirection = true;
        lastPressedKey = leftKey;
    }


    private void playerVerticalPerspective()
    {
        playerVertical.transform.localScale = new Vector3(1, 1, 0);
        playerHorizontal.transform.localScale = new Vector3(0, 0, 0);
        playerVertical.GetComponent<Animator>().SetBool("Moving", true);
    }

    private void playerHorizontalPerspective()
    {
        playerVertical.transform.localScale = new Vector3(0, 0, 0);
        playerHorizontal.transform.localScale = new Vector3(1, 1, 0);
        playerHorizontal.GetComponent<Animator>().SetBool("Moving", true);
    }
}
