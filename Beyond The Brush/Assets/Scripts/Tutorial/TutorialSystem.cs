using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSystem : MonoBehaviour
{

    //Variables||

        public GameObject mainNPC;              //Hold the main NPC used on the tutorial
        public GameObject tutorialCanvas;       //Holds the tutorial canvas
        
        private HandlePaintBrush canvasBrush;   //Holds the canvas drawings funcitons
        private Rigidbody2D playerRB;           //Hold the player rigid body

        private NPCsystem npcSystem;            //Hold the main NPC system
    //_________||

    //Tutorial stages||

        private int currentStage = 0;       //Stores the current tutorial stage

        //For stage 1
        private bool alreadMoved = false;   //Check if the player already moved
        private float timeForStage = 2f;    //The amount of time the player has to move
        private float timeCounter = 0;      //Count time after walking

        //For stage 3
        private bool alreadyReachedNPC = false; //Stores weather the camera has already reached the NPC or not
    //_______________||

    //Hold all the new messages||

        private List<NPCsystem.dialogShape> stage1Dialog = new List<NPCsystem.dialogShape>();
        private List<NPCsystem.dialogShape> stage2Dialog = new List<NPCsystem.dialogShape>();
    //_________________________||

    //Stages info||
    /*
     *Stage 0 -> Introduction
     *Stage 1 -> Movement
     *Stage 2 -> Talk about NPCS
     *Stage 3 -> Move camera to NPC
     *Stage 4 -> ended showin where the npc is, now the player needs to interect with him and displays the horizontal line
     *Stage 5 -> forces the player to drawn an horizontal line
    */
    //___________||

    private void Start()
    {
        playerRB = GameObject.FindGameObjectWithTag("Player")
                             .GetComponent<Rigidbody2D>();              //Get the player rigidbody
        NPCsystem.convChanged += whatToDo;                              //Add the function into the listeners
        npcSystem = mainNPC.GetComponent<NPCsystem>();                  //Get the NPC component

        canvasBrush = tutorialCanvas.transform.parent.GetComponent<HandlePaintBrush>();  //Store the canvas of the brush

        //Setup the dialogs||

            //stage 1
            stage1Dialog.Add(new NPCsystem.dialogShape("", "All you have to do is move towards the NPC and then hold <E> to interect with him..."));
            stage1Dialog.Add(new NPCsystem.dialogShape("npcCamera", "Lets try with that one!"));

            //stage 4
            stage2Dialog.Add(new NPCsystem.dialogShape("", "I think it's time for us to learn the most important part of them all..."));
            stage2Dialog.Add(new NPCsystem.dialogShape("", "Asides from walking, all the other actions are done by drawing!"));
            stage2Dialog.Add(new NPCsystem.dialogShape("", "You can draw on your screen by dragging your mouse. This can be used to attack, defend yourself and even spawn objects!"));
            stage2Dialog.Add(new NPCsystem.dialogShape("horizontal", "Let's start drawing. First we will be learning the most basic and important shape"));
            stage2Dialog.Add(new NPCsystem.dialogShape("", "This is the basic attack..."));
            stage2Dialog.Add(new NPCsystem.dialogShape("", "You cast it by drawing a horizontal line over the enemies..."));
            stage2Dialog.Add(new NPCsystem.dialogShape("", "all your skills have cooldowns that are displayed at the bottom of the screen. Remember to keep track of them as you play!"));
            stage2Dialog.Add(new NPCsystem.dialogShape("drawHorizontal", "either way, it's a way more fun to learn by trying so let's give it a try. Draw a horizontal line"));
        //_________________||

        npcSystem.StartNPCdialog();
    }

    private void whatToDo(string id){
        switch(id){
            case "a":
                currentStage = 1;
            break;

            case "npcCamera":
                Camera.main.gameObject.GetComponent<CameraBehavior>().enabled = false;
                currentStage = 3;
                break;

            case "horizontal":

                tutorialCanvas.SetActive(true); //Displays the canvas
                canvasBrush.drawHorizontal();   //Draws the horizontal line
                break;

            case "drawHorizontal":

                currentStage = 5;
                canvasBrush.stopDrawing();       //Stop drawing on the canvas
                tutorialCanvas.SetActive(false); //Displays the canvas
                break;
        }
    }

    private void Update()
    {
        //Handle stage 1
        if(currentStage == 1 && alreadMoved)
        {
            if(timeCounter > timeForStage)
            {
                //Set everything up for the next stage
                currentStage = 2;
                npcSystem.startingMessage = "Great! Now that you know on how to walk we'll be teaching you on how to interect with NPCs";
                npcSystem.dialogMessages = stage1Dialog;
                npcSystem.StartNPCdialog();
            }
            else
            timeCounter += Time.deltaTime;
        }else if(currentStage == 1)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
                alreadMoved = true;
        }
            
        //Handle stage 3
        if(currentStage == 3)
        {
            Transform cameraData = Camera.main.gameObject.GetComponent<Transform>();

            Vector3 holdNewPos = Vector3.MoveTowards(cameraData.position, mainNPC.transform.position, 0.1f);
            Vector3 holdNewPos2 = Vector3.MoveTowards(cameraData.position, playerRB.position, 0.1f);

            if (Vector2.Distance(cameraData.position, mainNPC.transform.position) < 0.2f)
            {
                alreadyReachedNPC = true;
            } 

            if(alreadyReachedNPC && Vector2.Distance(cameraData.position, playerRB.position) < 0.2f)
            {
                Camera.main.gameObject.GetComponent<CameraBehavior>().enabled = true;
                currentStage = 4;
                npcSystem.startingMessage = "Great job! You now know on how to interect with NPCs...";
                npcSystem.dialogMessages = stage2Dialog;
            }

            if(!alreadyReachedNPC)
            {
                cameraData.position = new Vector3(holdNewPos.x, holdNewPos.y, cameraData.position.z);
            }else{
                cameraData.position = new Vector3(holdNewPos2.x, holdNewPos2.y, cameraData.position.z);
            }

        }

        //Stage 5-> horizontal drawing
        if(currentStage == 5){

        }

    }

}
