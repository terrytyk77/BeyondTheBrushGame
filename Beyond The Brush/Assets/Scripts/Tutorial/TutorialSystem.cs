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

        private int currentStage = 11;       //Stores the current tutorial stage

        //For stage 1
        private bool alreadMoved = false;   //Check if the player already moved
        private float timeForStage = 2f;    //The amount of time the player has to move
        private float timeCounter = 0;      //Count time after walking

        //For stage 3
        private bool alreadyReachedNPC = false; //Stores weather the camera has already reached the NPC or not

        //For stage 5
        private bool drawnHorizontal = false;
        private bool drawnXspell = false;
        private bool drawnShield = false;
        private bool drawnRock = true;
        private bool drawnCrate = true;
    //_______________||

    //Hold all the new messages||

        private List<NPCsystem.dialogShape> stage1Dialog = new List<NPCsystem.dialogShape>();
        private List<NPCsystem.dialogShape> stage2Dialog = new List<NPCsystem.dialogShape>();
        private List<NPCsystem.dialogShape> stage5Dialog = new List<NPCsystem.dialogShape>();
        private List<NPCsystem.dialogShape> stage8Dialog = new List<NPCsystem.dialogShape>();
        private List<NPCsystem.dialogShape> stage10Dialog = new List<NPCsystem.dialogShape>();
        private List<NPCsystem.dialogShape> stage12Dialog = new List<NPCsystem.dialogShape>();
    //_________________________||

    //Stages info||
    /*
     *Stage 0 -> Introduction
     *Stage 1 -> Movement
     *Stage 2 -> Talk about NPCS
     *Stage 3 -> Move camera to NPC
     *Stage 4 -> ended showin where the npc is, now the player needs to interect with him and displays the horizontal line
     *Stage 5 -> forces the player to draw an horizontal line
     *Stage 6 -> Talks to the player about the xspell
     *Stage 7 -> forces the player to draw the xspell
     *Stage 8 -> talks to the player about the shield
     *Stage 9 -> forces the player to do the shield
     *Stage 10 -> talks to the player about object spawning
     *Stage 11 -> forces the player to spawn a rock and a crate
     *Stage 12 -> Ended the drawing, now the player will fight against a mob
    */
    //___________||

    private void Start()
    {
        playerRB = GameObject.FindGameObjectWithTag("Player")
                             .GetComponent<Rigidbody2D>();              //Get the player rigidbody
        NPCsystem.convChanged += whatToDo;                              //Add the function into the listeners
        OnDrawEvent.shapeDrawn += drawnShape;                           //Add the function into the drawing listener
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

            //Stage 6
            stage5Dialog.Add(new NPCsystem.dialogShape("showXspell", "Now for the next spell..."));
            stage5Dialog.Add(new NPCsystem.dialogShape("", "this is the xspell! You cast it by drawing an X in a single stroke..."));
            stage5Dialog.Add(new NPCsystem.dialogShape("", "it works exactly the same way as the basic slash with the difference that it also breaks objects and chests!"));
            stage5Dialog.Add(new NPCsystem.dialogShape("drawX", "Let's give it a try! Draw an X"));

            //Stage 8
            stage8Dialog.Add(new NPCsystem.dialogShape("showCircle", "the player can shield himself by drawing a circle above itself!"));
            stage8Dialog.Add(new NPCsystem.dialogShape("", "however you need to be careful as this shape is also used to spawn objects..."));
            stage8Dialog.Add(new NPCsystem.dialogShape("", "while the player has his shield on he will take reduced damage from the next attack that hits him..."));
            stage8Dialog.Add(new NPCsystem.dialogShape("drawShield", "let's give it a try!"));

            //Stage 10
            stage10Dialog.Add(new NPCsystem.dialogShape("", "that wraps out the combat system..."));
            stage10Dialog.Add(new NPCsystem.dialogShape("", "there are only 2 more things that you need to learn about the drawings..."));
            stage10Dialog.Add(new NPCsystem.dialogShape("showSquare", "as you might have already noticed, you can spawn a rock by drawing the circle outside of the player..."));
            stage10Dialog.Add(new NPCsystem.dialogShape("", "you can also draw an open square in order to spawn a create..."));
            stage10Dialog.Add(new NPCsystem.dialogShape("", "you might be wondering on why would we need to spawn these objects..."));
            stage10Dialog.Add(new NPCsystem.dialogShape("", "sometimes you'll encounter these <gate doors>. They can be opened by spawning an object on top of a pressure plate..."));
            stage10Dialog.Add(new NPCsystem.dialogShape("", "pressure plates always have a drawing on top of them. This drawing is either a circle or a square..."));
            stage10Dialog.Add(new NPCsystem.dialogShape("", "in case they have a circle then you'll need to spawn a rock, if they have a square on the other hand you'll need a crate..."));
            stage10Dialog.Add(new NPCsystem.dialogShape("drawObjects", "before we continue let's spawn both a rock and a crate!"));

            //Stage 12
            stage12Dialog.Add(new NPCsystem.dialogShape("cameraShake", "..."));
            stage12Dialog.Add(new NPCsystem.dialogShape("moveCamera", "what was that?"));
            stage12Dialog.Add(new NPCsystem.dialogShape("", "a wave of enemies!"));
            stage12Dialog.Add(new NPCsystem.dialogShape("killEnemies", "this is a great opportunity for you to test your new skills!"));
        //_________________||

        //npcSystem.StartNPCdialog();
    }

    private void drawnShape(string id){
        if(currentStage == 5 && id == "Horizontal"){
            currentStage = 6;
            drawnHorizontal = true;
            npcSystem.dialogMessages = stage5Dialog;
            npcSystem.startingMessage = "Good job! This is an extremely spammable ability that you'll use quite often as you progress.";
            npcSystem.StartNPCdialog();
        }

        if (currentStage == 7 && id == "Xspell")
        {
            drawnXspell = true;
            currentStage = 8;
            npcSystem.dialogMessages = stage8Dialog;
            npcSystem.startingMessage = "You're good at this! Now we'll be going for the defensive part...";
            npcSystem.StartNPCdialog();
        }

        if(currentStage == 9 && id == "shield"){
            drawnShield = true;
            currentStage = 10;
            npcSystem.dialogMessages = stage10Dialog;
            npcSystem.startingMessage = "There you go!";
            npcSystem.StartNPCdialog();
        }

        if (currentStage == 11)
        { 
            if(id == "rock"){
                drawnRock = true;
            }
            if(id == "Square")
            {
                drawnCrate = true;
            }
        }
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

            case "showXspell":

                tutorialCanvas.SetActive(true); //Displays the canvas
                canvasBrush.drawXspell();       //Stop drawing on the canvas
                break;

            case "drawX":

                currentStage = 7;
                canvasBrush.stopDrawing();       //Stop drawing on the canvas
                tutorialCanvas.SetActive(false); //Displays the canvas
                break;
                
            case "showCircle":

                tutorialCanvas.SetActive(true); //Displays the canvas
                canvasBrush.drawCircle();       //Stop drawing on the canvas
                break;
                
            case "drawShield":

                currentStage = 9;
                canvasBrush.stopDrawing();       //Stop drawing on the canvas
                tutorialCanvas.SetActive(false); //Displays the canvas
                break;
                
            case "showSquare":

                tutorialCanvas.SetActive(true); //Displays the canvas
                canvasBrush.drawBox();          //Stop drawing on the canvas
                break;

            case "drawObjects":

                currentStage = 11;
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

        //Stage 11
        if(currentStage == 11 && drawnCrate && drawnRock){
            currentStage = 12;
            npcSystem.startingMessage = "Alright. That's enough drawing...";
            npcSystem.dialogMessages = stage12Dialog;
            npcSystem.StartNPCdialog();
        }

    }

}
