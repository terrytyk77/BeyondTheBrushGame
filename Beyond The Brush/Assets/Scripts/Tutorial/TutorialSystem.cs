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

        private int currentStage = 1;       //Stores the current tutorial stage

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
        private bool drawnRock = false;  //false
        private bool drawnCrate = false; //false

        //For stage 13
        public GameObject point1;
        public GameObject enemiesSwarm;

        //For stage 15
        public GameObject healthBarArrow;
        public GameObject minimapArrow;
        public GameObject talentTreeArrow;
        public GameObject expbarArrow;

        //For stage 16
        public GameObject bigmapWindow;

        //For stage 17
        public GameObject skullIcon;
        public GameObject skullArrow;

        //For stage 18
        public GameObject point2;

        //For stage 19
        private float amountOfTime19;
        public GameObject invisibleWall;
    //_______________||

    //Hold all the new messages||

        private List<NPCsystem.dialogShape> stage1Dialog = new List<NPCsystem.dialogShape>();
        private List<NPCsystem.dialogShape> stage2Dialog = new List<NPCsystem.dialogShape>();
        private List<NPCsystem.dialogShape> stage5Dialog = new List<NPCsystem.dialogShape>();
        private List<NPCsystem.dialogShape> stage8Dialog = new List<NPCsystem.dialogShape>();
        private List<NPCsystem.dialogShape> stage10Dialog = new List<NPCsystem.dialogShape>();
        private List<NPCsystem.dialogShape> stage12Dialog = new List<NPCsystem.dialogShape>();
        private List<NPCsystem.dialogShape> stage15Dialog = new List<NPCsystem.dialogShape>();
        private List<NPCsystem.dialogShape> stage17Dialog = new List<NPCsystem.dialogShape>();
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
     *Stage 12 -> Shake the screen
     *Stage 13 -> Spawn the enemy
     *Stage 14 -> The player has to kill the enemies
     *Stage 15 -> You just killed all the enemies, talk about the UI now 
     *Stage 16 -> wait for minimap click
     *Stage 17 -> explain dungeons entrance
     *Stage 18 -> Move the camera to the bushes
     *Stage 19 -> Wait for a few second and then move to the player
     *Stage 20 -> move the camera back to the player
     *Stage 21 -> It ended! :D
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
            stage12Dialog.Add(new NPCsystem.dialogShape("stopShake", "...!"));
            stage12Dialog.Add(new NPCsystem.dialogShape("moveCameraEnemies", "what was that?"));
            stage12Dialog.Add(new NPCsystem.dialogShape("teleportCamera", "..."));
            stage12Dialog.Add(new NPCsystem.dialogShape("", "a swarm of enemies!"));
            stage12Dialog.Add(new NPCsystem.dialogShape("killEnemies", "this is a great opportunity for you to test your new skills!"));

            //stage 15
            stage15Dialog.Add(new NPCsystem.dialogShape("", "Thank you for your help " + PlayerData.username + "!"));
            stage15Dialog.Add(new NPCsystem.dialogShape("healthbar", "There's just a few more things that we need to talk about. One of them is your UI!"));
            stage15Dialog.Add(new NPCsystem.dialogShape("", "at the top left you can find your health bar, it displays your current amount of health points..."));
            stage15Dialog.Add(new NPCsystem.dialogShape("expbar", "once it reaches zero you die. If you haven't killed all the enemies then the ones that are dead will respawn..."));
            stage15Dialog.Add(new NPCsystem.dialogShape("", "this is the exp bar, it fills itself as you kill enemies..."));
            stage15Dialog.Add(new NPCsystem.dialogShape("talentTree", "once it reaches 100% you'll level up. Leveling up increases your max hp and damage dealt to enemies..."));
            stage15Dialog.Add(new NPCsystem.dialogShape("", "you can open your talent tree by clicking the book button..."));
            stage15Dialog.Add(new NPCsystem.dialogShape("minimap", "you can upgrade your own abilities thru it..."));
            stage15Dialog.Add(new NPCsystem.dialogShape("", "last but not least this is your minimap. You can use it to locate yourself..."));
            stage15Dialog.Add(new NPCsystem.dialogShape("bigmap", "you can also click it in order to open a big map!"));

            //stage 17
            stage17Dialog.Add(new NPCsystem.dialogShape("", "they resemble dungeon entrances. These dungeons is where you grind and progress thru the game..."));
            stage17Dialog.Add(new NPCsystem.dialogShape("endTutorial", "and that is pretty much it! Thank you for playing the tutorial and feel free to move into the next region..."));
        //_________________||

        npcSystem.StartNPCdialog();
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
                
            case "cameraShake":

                StartCoroutine(cameraShake(1.4f));
                break;
                
            case "stopShake":

                StopAllCoroutines();
                break;
                
            case "moveCameraEnemies":
                Camera.main.gameObject.GetComponent<CameraBehavior>().enabled = false;
                enemiesSwarm.SetActive(true);
                currentStage = 13;
                break;
                
            case "teleportCamera":
                Camera.main.gameObject.transform.position = point1.transform.position;
                
                break;
                
            case "killEnemies":
                Camera.main.gameObject.transform.position = new Vector3( playerRB.position.x, playerRB.position.y, -10);
                Camera.main.gameObject.GetComponent<CameraBehavior>().enabled = true;
                currentStage = 14;
                break;
                
            case "healthbar":
                healthBarArrow.SetActive(true);
                break;
                
            case "expbar":
                healthBarArrow.SetActive(false);
                expbarArrow.SetActive(true);
                break;
                
            case "talentTree":
                talentTreeArrow.SetActive(true);
                expbarArrow.SetActive(false);
                break;
                
            case "minimap":
                talentTreeArrow.SetActive(false);
                minimapArrow.SetActive(true);
                break;
                
            case "bigmap":
                currentStage = 16;
                break;

            case "endTutorial":
                currentStage = 18;
                Camera.main.gameObject.GetComponent<CameraBehavior>().enabled = false;
                skullIcon.SetActive(false);
                skullArrow.SetActive(false);
                bigmapWindow.SetActive(false);
                playerRB.constraints = RigidbodyConstraints2D.FreezeAll;
                break;
        }
    }

    private IEnumerator cameraShake(float duration){

        //Settings||

            Vector3 cameraStart = Camera.main.transform.position;
            float shakingAmount = 0.25f;
            float shakedAmount = 0f;
            float updateGap = 0.075f;
            bool direction = false;
        //________||

        while(shakedAmount < duration)
        {

            if (direction)
                Camera.main.transform.position = cameraStart + new Vector3(0, shakingAmount, 0);
            else
                Camera.main.transform.position = cameraStart - new Vector3(0, shakingAmount, 0);

            direction = !direction;

            //Handle the time variables
            shakedAmount += updateGap;
            yield return new WaitForSeconds(updateGap);
        }

        yield return null;
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


        //Stage 13
        if(currentStage == 13)
        {
            Transform cameraData = Camera.main.gameObject.GetComponent<Transform>();
            
            if(Vector2.Distance(cameraData.position, point1.transform.position) > 0.2f){
                cameraData.position = Vector3.MoveTowards(cameraData.position, point1.transform.position, 0.4f);
            }
        }

        //Stage 14
        if(currentStage == 14)
        {
            int aliveEnemies = 0;
            foreach(Transform enemy in enemiesSwarm.transform){
                if(enemy.GetComponent<EnemyAI>().getHealth > 0){
                    aliveEnemies++;
                }
            }

            if(aliveEnemies == 0){
                npcSystem.startingMessage = "Uff, that was close...";
                npcSystem.dialogMessages = stage15Dialog;
                npcSystem.StartNPCdialog();
                currentStage = 15;
            }
            
        }

        //Stage 16
        if(currentStage == 16 && bigmapWindow.activeSelf)
        {
            currentStage = 17;
            skullIcon.SetActive(true);
            skullArrow.SetActive(true);
            minimapArrow.SetActive(false);
            npcSystem.startingMessage = "Maps usually have this skull icons on them...";
            npcSystem.dialogMessages = stage17Dialog;
            npcSystem.StartNPCdialog();
        }

        //Stage 18
        if(currentStage == 18){
            Transform cameraData = Camera.main.gameObject.GetComponent<Transform>();
            if (Vector2.Distance(cameraData.position, point2.transform.position) < 0.2f)
            {
                currentStage = 19;
                
            }else{
                cameraData.position = Vector3.MoveTowards(cameraData.position, point2.transform.position, 0.4f);
            }
        }

        if(currentStage == 19){
            amountOfTime19 += Time.deltaTime;
            if(amountOfTime19 > 1.5f)
            {
                Transform cameraData = Camera.main.gameObject.GetComponent<Transform>();
                invisibleWall.SetActive(false);
                currentStage = 20;
            }
        }

        if (currentStage == 20)
        {
            Transform cameraData = Camera.main.gameObject.GetComponent<Transform>();
            if (Vector2.Distance(cameraData.position, playerRB.position) < 2f)
            {
                currentStage = 21;
                Camera.main.gameObject.GetComponent<CameraBehavior>().enabled = true;
                playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            else{
                cameraData.position = Vector3.MoveTowards(cameraData.position, playerRB.position, 0.4f);
            }
        }
    }

}
