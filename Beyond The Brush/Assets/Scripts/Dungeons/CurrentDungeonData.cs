using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentDungeonData : MonoBehaviour
{
    //All the script classes||

        [System.Serializable]
        public class dungeonResult              //class holder for the dungeon rewards window
        {
            public GameObject window;           //holds the main frame of the window
            public GameObject textLabel;        //holds the display text label
            public GameObject acceptButton;     //holds the accept button (continue the dungeon)
            public GameObject cancelButton;     //holds the cancel button (leave the dungeon)
        }
        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        //||||||||||||||||||||||||||||||||||||||||||||||||||| This is a class breaker|||||||||||||||||||||||||||||||||||||||||||||||||||
        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        [System.Serializable]
        public class uiElements                 //class holder for UI elements - holds minimap and its counters
        {
            [System.Serializable]
            public class minimap                //class holder for the minimap box
            {

                public GameObject textElement;  //Holds the minimap text label
                public GameObject mask;         //Holds the mask that overlays the map
                public GameObject roomPrefab;   //Holds the room prefab that is used on the minimap to describe each room

                public Color completedRoom;     //Holds the color used for completed rooms on the minimap
                public Color uncompletedRoom;   //Holds the color for uncompleted rooms on the minimap
                public Color unexploredRoom;    //Holds the color for unexplored rooms on the minimap


            }

            public minimap miniMap = new minimap(); //Creates the variable holder of the minimap

        }
        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||\\
        //||||||||||||||||||||||||||||||||||||||||||||||||||| This is a class breaker|||||||||||||||||||||||||||||||||||||||||||||||||||
        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||//
        class roomPos                                           //Holds the room positions class
        {
            public Vector2Int position = new Vector2Int();      //Stores the room position in vector2 format
            public Dungeon.room room;                           //Stores the room itself

            public roomPos(int x, int y, Dungeon.room room2)    //Enter method of the class
            {
                position.x = x;
                position.y = y;
                room = room2;
            }

        }
    //______________________||

    //Class holder variables||

        public dungeonResult DungeonResultWindow = new dungeonResult(); //Holds the result window class
        public uiElements UIelements = new uiElements();                //Holds the UI elements (minimap + counters) class
    //______________________||

    //Variables||

        //Data that won't change middle runtime
        public int mainVillageID = 1;       //Holds the village scene number
        public GameObject dungeonsData;     //Holds the list of all dungeons that exist (extracted from the dungeon API)
        
        public GameObject background;       //Loading screen background
        public GameObject loadingScreen;    //Loading screen window 
        public GameObject loadingBar;       //Loading screen loading bar

        //Data that will change once during runtime
        public Dungeon currentDungeon;      //Holds the current dungeon object
        private GameObject startingRoom;    //Holds the starting room of the dungeon prefab
        private Rigidbody2D playerRB;       //Holds the player rigid body
        private UIsfx UIsoundEffect;        //Holds the sound effects callbacks

        //Data that might change during runtime
        public Vector2Int currentRoom;                                      //Holds the players current room cords
        public string nextRoomSide;                                         //The opposite door to the last one that the player took
        private List<GameObject> inactiveRooms = new List<GameObject>();    //Stores innactive rooms (rooms on which the player is not present)
        private List<roomPos> map = new List<roomPos>();                    //Stores the rooms already used on the minimap to avoid duplicates

        //Counters
        public int amountOfChests = 0;                                      //Keeps track of the amount of time that the player has died
        public int amountOfDeaths = 0;                                      //Keeps track of the amount of deaths that the player has
        public int amountOfCompletedRooms = 0;                              //Keeps track of the amount of completed rooms that the player has
        //_________||



    /*      Unity life cycle methods    */
    private void Start()                                                                    //This method gets called at the start of the dungeon
        {
            UIsoundEffect = GameObject.FindGameObjectWithTag("mainUI").GetComponent<UIsfx>();   //Get the UI library for the sound effects
            List<Dungeon> dungeonsList = dungeonsData.GetComponent<DungeonsAPI>().dungeons;     //Get the list of dungeons
            playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();  //Get the player rigid body
            GameObject dungeonAPI = GameObject.FindGameObjectWithTag("sceneAPI");               //Get the dungeon API (holds the dungeon we want to generate)
            if (sceneTeleport.dungeonName != "" && sceneTeleport.dungeonName != null)                                                             //Check if there is a dungeon to spawn
            {
                getCorrectDungeon(dungeonsList, sceneTeleport.dungeonName);                     //Ask for the starting area to be generated
                Destroy(dungeonAPI);                                                            //Avoid duplicates of the dungeon API
            }
            else                                                                                //It couldn't find any dungeon request
                getCorrectDungeon(dungeonsList, "DeadMines");                                   //Go with the default and load "Deadmines"

            if(currentDungeon.mainMusic != null){
                AudioSource mainMusic = Camera.main.transform.Find("MainMusic").GetComponent<AudioSource>();
                mainMusic.clip = currentDungeon.mainMusic;
                mainMusic.Play();
            }
            SpawnPlayer();                                                                      //Spawn the player on the starting area
            updateMap();                                                                        //Ask for a dungeon update
        }

    /*                                  */

    


    //Code below holds all the methods used on the dungeon algorithm||

    //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    // Method: Get room via cords
    /////
    // Desc: Grabs on a vector and returns the corresponding room with equal coordinates
    //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    public Dungeon.room getRoomViaCords(Vector2Int cords)   //Gets a room via cords
    {
        Dungeon.room foundRoom = new Dungeon.room();        //Create an empty room

        foreach (roomPos room in map)                       //Loop through all the dungeon rooms that exist
        {
            if (room.position == cords)                     //Check if the room has the same cords
                return room.room;                           //if it does then return the room
        }

        foundRoom.roomPrefab = null;                        //If it doesn't find the room then empty the new one
        return foundRoom;                                   //and send it bacck to the player
    }

    //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    // Method: Set completed reward
    /////
    // Desc: Displays the reward window to the player with all its animations and functionalities such as the dungeon leave
    //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    private void setupCompletedReward(int resourcesAmount, int completedRooms)
    {
        //Handle player movemenet||

            playerRB.constraints = RigidbodyConstraints2D.FreezeAll;                                        //Freeze the player
        //_______________________||

        float currentTimeScale = Time.timeScale;                                                            //Store the current time

        Time.timeScale = 1;                                                                                 //Set the time scale to normal value

        //Handle the rewards window show up||
        DungeonResultWindow.window.GetComponent<RectTransform>().localPosition = new Vector3(0, 900, 0);    //Window position off the screen
        DungeonResultWindow.window.SetActive(true);                                                         //Enable the window

        //Handle the buttons listeners
        Button LeaveDungeonBTN = DungeonResultWindow.cancelButton.GetComponent<Button>();                   //Get the button component of the leave
        Button ContinueDungeonBTN = DungeonResultWindow.acceptButton.GetComponent<Button>();                //Get the button component of the continue

        LeaveDungeonBTN.onClick.RemoveAllListeners();       //Clear the listeners
        ContinueDungeonBTN.onClick.RemoveAllListeners();    //Clear the listeners

        void leaveFunction()
        {
            UIsoundEffect.playClick();                      //Play a click sound effect
            DontDestroyOnLoad(gameObject);                  //Preserve the current dungeon data
            Time.timeScale = currentTimeScale;              //Reset back to the time it had
            sceneTeleport.start(1);                         //Teleport back to the village
        }


        void continueFunction()
        {
            UIsoundEffect.playClick();                                      //Play a click sound effect
            Time.timeScale = currentTimeScale;                              //Reset back to the time it had
            DungeonResultWindow.window.SetActive(false);                    //Close the window
            playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;   //Unfreeze the player
            this.StopCoroutine(cancelButtonEffect());                       //Stop the cancel button coroutine
            this.StopCoroutine(writeDungeonResult());                       //Stop the counting coroutine
            ContinueDungeonBTN.interactable = false;                        //Make the continue button non interactable
            LeaveDungeonBTN.interactable = false;                           //Make the leave button non interectable
        }
      
        LeaveDungeonBTN.onClick.AddListener(leaveFunction);         //Add the leave method to the listener
        ContinueDungeonBTN.onClick.AddListener(continueFunction);   //Add the continue method to the listener
        LeaveDungeonBTN.interactable = false;                       //Make sure this button is not interectable

        IEnumerator writeDungeonResult()
        {
            //Hold the phrases texts
            string completeMessage = "";
            string message1 = "Found Chests: ";
            string message2 = "Amount of deaths: ";
            string message3 = "Completed rooms: ";
            string message4 = "Total reward: ";
            string message5 = " resources";

            string message1Built = "";
            string message2Built = "";
            string message3Built = "";
            string message4Built = "";
            string message5Built = "";

            //Count timers
            int chestsHolder = -1;
            int deathsHolder = -1;
            int roomsCompletedHolder = -1;
            int resourcesHolder = -1;

            //Add the first section
            while(message1Built.Length < message1.Length)
            {
                UIsoundEffect.playType();                                                   //Plays the typing sound effect per character
                //Add to the message
                message1Built = message1Built + message1[message1Built.Length];             //Gets the next letter from the main message
                completeMessage = message1Built;                                            //Update the over all message
                DungeonResultWindow.textLabel.GetComponent<Text>().text = completeMessage;  //Update the text label data
                yield return new WaitForSeconds(0.05f);                                     //Wait between characters
            }

            //Calculate the amount of chests
            while(chestsHolder < amountOfChests){

                UIsoundEffect.playType();                                                   //Plays the typing sound effect per character
                chestsHolder++;
                DungeonResultWindow.textLabel.GetComponent<Text>().text = completeMessage + "<color=#cedb1a>" + chestsHolder + "</color>";
                yield return new WaitForSeconds(0.1f);
            }

            completeMessage = completeMessage + "<color=#cedb1a>" + chestsHolder + "</color>\n";  //Add the yellow color to the text

            //Add the second section
            while (message2Built.Length < message2.Length)
            {
                UIsoundEffect.playType();                                                   //Plays the typing sound effect per character
                //Add to the message
                message2Built = message2Built + message2[message2Built.Length];                             //Gets the next letter from the main message
                DungeonResultWindow.textLabel.GetComponent<Text>().text = completeMessage + message2Built;  //Update the text label data
                yield return new WaitForSeconds(0.05f);                                                     //Wait between characters
            }

            completeMessage = completeMessage + message2Built;

            //Calculate the amount of deaths
            while (deathsHolder < amountOfDeaths)
            {
                UIsoundEffect.playType();                                                   //Plays the typing sound effect per character
                deathsHolder++;
                DungeonResultWindow.textLabel.GetComponent<Text>().text = completeMessage + "<color=#9e0e1d>" + deathsHolder + "</color>";
                yield return new WaitForSeconds(0.1f);
            }

            completeMessage = completeMessage + "<color=#9e0e1d>" + deathsHolder + "</color>\n";  //Add the red color to the text

            //Add the third section
            while (message3Built.Length < message3.Length)
            {
                UIsoundEffect.playType();                                                   //Plays the typing sound effect per character
                //Add to the message
                message3Built = message3Built + message3[message3Built.Length];                             //Gets the next letter from the main message
                DungeonResultWindow.textLabel.GetComponent<Text>().text = completeMessage + message3Built;  //Update the text label data
                yield return new WaitForSeconds(0.05f);                                                     //Wait between characters
            }

            completeMessage = completeMessage + message3Built;

            //Calculate the amount of completed rooms
            while (roomsCompletedHolder < completedRooms)
            {
                UIsoundEffect.playType();                                                   //Plays the typing sound effect per character
                roomsCompletedHolder++;
                DungeonResultWindow.textLabel.GetComponent<Text>().text = completeMessage + "<color=#45ba06>" + roomsCompletedHolder + "</color>";
                yield return new WaitForSeconds(0.1f);
            }

            completeMessage = completeMessage + "<color=#45ba06>" + roomsCompletedHolder + "</color>\n\n";  //Add the green color to the text

            //Add the fourth section
            while (message4Built.Length < message4.Length)
            {
                UIsoundEffect.playType();                                                   //Plays the typing sound effect per character
                //Add to the message
                message4Built = message4Built + message4[message4Built.Length];                             //Gets the next letter from the main message
                DungeonResultWindow.textLabel.GetComponent<Text>().text = completeMessage + "<size=25><b>" + message4Built+ "</b></size>";  //Update the text label data
                yield return new WaitForSeconds(0.05f);                                                     //Wait between characters
            }

            completeMessage = completeMessage + "<size=25><b>" + message4Built+ "</b></size>";

            //Calculate the amount of resources
            while (resourcesHolder < resourcesAmount)
            {
                UIsoundEffect.playType();                                                   //Plays the typing sound effect per character
                resourcesHolder++;
                DungeonResultWindow.textLabel.GetComponent<Text>().text = completeMessage + "<size=25><b><color=#eeff00>" + resourcesHolder + "</color></b></size>";
                yield return new WaitForSeconds(0.005f);
            }

            completeMessage = completeMessage + "<size=25><b><color=#eeff00>" + resourcesHolder + "</color></b></size>";  //Add the golden color to the text

            //Add the fifth section
            while (message5Built.Length < message5.Length)
            {
                UIsoundEffect.playType();                                                   //Plays the typing sound effect per character
                //Add to the message
                message5Built = message5Built + message5[message5Built.Length];                             //Gets the next letter from the main message
                DungeonResultWindow.textLabel.GetComponent<Text>().text = completeMessage + "<size=25><b>" + message5Built + "</b></size>";  //Update the text label data
                yield return new WaitForSeconds(0.05f);                                                     //Wait between characters
            }

            yield return null;
        }

        //Declare coroutines for the buttons
        IEnumerator cancelButtonEffect()
        {
            int waitTime = 3; //The amount of seconds to enable the button

            DungeonResultWindow.cancelButton.transform.Find("Text").GetComponent<Text>().text = waitTime.ToString();        //Set the starting text

            while (waitTime > 0)
            {
                yield return new WaitForSeconds(1);                                                                         //Force code yield
                waitTime--;                                                                                                 //Subtract to the counter
                DungeonResultWindow.cancelButton.transform.Find("Text").GetComponent<Text>().text = waitTime.ToString();    //Update to the current time number
            }

            //The wait time already ended
            DungeonResultWindow.cancelButton.transform.Find("Text").GetComponent<Text>().text = "Leave";                    //Change the label to "leave"
            LeaveDungeonBTN.interactable = true;                                                                            //Make the button interectable for the player         

            yield return null; //Break the coroutine
        }

        ContinueDungeonBTN.interactable = true;         //Make the continue button interectable
        StartCoroutine(writeDungeonResult());           //Make the coroutine that handles the dungeon display info
        StartCoroutine(cancelButtonEffect());           //Start the cancel button countdown
    }

    //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    // Method: Completed room
    /////
    // Desc: Checks if the player already completed a room and if yes handle the room rewards + animations and feedback
    //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    public void CompletedRoom()
    {
        Dungeon.room RoomWeAreIn = getRoomViaCords(currentRoom);    //Get the current room
        if (RoomWeAreIn.getCompleted())                             //Check if this room has already been completed
            return;                                                 //If this room has already been completed then stop the rest of the code from running

        amountOfCompletedRooms++;                                   //Augment the counter of completed rooms
        RoomWeAreIn.setCompleted(true);                             //Change the current room completion status
        updateMap();                                                //Update the minimap

        //Calculate the rewards window info||

        //some math here xd
        int totalAmountOfResources = currentDungeon.baseReward;

            if(amountOfCompletedRooms % 3 == 0)
            {
                totalAmountOfResources = totalAmountOfResources + (int)(0.2f * ( (float)amountOfChests * 5f - (float)amountOfDeaths * 3f + (float)amountOfCompletedRooms * 5f) );
                PlayerData.resources += totalAmountOfResources;                         //Add the amount of player resources
                setupCompletedReward(totalAmountOfResources, amountOfCompletedRooms);   //Handle the rewards window info
            }
        //_________________________________||
    }



    //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    // Method: Change next room
    /////
    // Desc: Handle the change between rooms: update active room, current player position and camera
    //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    public void changeNextRoom(string roomSide)
    {
        GameObject CurrentRoomReference = GameObject.FindGameObjectWithTag("dungeonRoom");  //A reference to the players current room


        switch (roomSide)                                                                   //Calculate on which side from the next room should the player be spawned in
        {
            case "right":
                nextRoomSide = "left";                                                      //Right -> Left
                currentRoom.x++;
                break;

            case "left":
                nextRoomSide = "right";                                                     //Left -> Right
                currentRoom.x--;
                break;

            case "top":
                nextRoomSide = "bottom";                                                    //Top -> Bottom
                currentRoom.y++;
                break;

            case "bottom":
                nextRoomSide = "top";                                                       //Bottom -> Top
                currentRoom.y--;
                break;

            case "exit":
                DontDestroyOnLoad(gameObject);                                              //Avoid destroying this object to use as reference for the other side teleport
                sceneTeleport.start(mainVillageID);                                         //If the door equals to exit then take him to the village
                break;

            default:
                nextRoomSide = null;                                                        //Just in case it can't tell what the door is
                break;
        }

        //Update discord presence
        if (DiscordPresence.PresenceManager.instance != null)                                                                       //Check if the player is using discord
        {
            DiscordPresence.PresenceManager.instance.presence.state = "Room: " + "[" + currentRoom.x + ", " + currentRoom.y + "]";  //Update his room status
            DiscordPresence.PresenceManager.UpdatePresence(null);                                                                   //Send a discord update request
        }

        inactiveRooms.Add(CurrentRoomReference);    //Add the current active room to the inactive list
        CurrentRoomReference.SetActive(false);      //Turn the current room into an inactive room


        Dungeon.room roomToCreate = getRoomViaCords(currentRoom);       //Gets the room that will be activated
        createNextRooms(currentRoom);                                   //Create all conected rooms if possible

        if (roomToCreate.roomPrefab != null)                            //Make sure if the room has a prefab to be used
        {
            
            bool roomWasSpawned = false;                                //Check if it was already spawned, if not spawn it!

            for (int index = 0; index < inactiveRooms.Count; index++)   //Loop through the inactive rooms and remove the room from there
            {
                GameObject room = inactiveRooms[index];
                if (room != null && room.name == roomToCreate.roomPrefab.name + currentRoom.x + currentRoom.y)
                {
                    roomWasSpawned = true;      //Hold on the result to the variable
                    room.SetActive(true);       //Make the room active
                    inactiveRooms.Remove(room); //Remove the room from the list
                    break;                      //No point to continue the loop
                }
            }

            if (!roomWasSpawned)    //If the room didn't exist in the inactive list (a room that has never been generated)     
            {
                //Spawn the room and change its name
                GameObject roomThatDidntExist = Instantiate(roomToCreate.roomPrefab, roomToCreate.roomPrefab.transform.position, Quaternion.identity);
                roomThatDidntExist.name = roomToCreate.roomPrefab.name + currentRoom.x + currentRoom.y;
            }


        }

        //Teleport the player to the correct side
        foreach (Transform child in roomToCreate.roomPrefab.transform)  //Loop through all the teleport locations of the room
        {
            if (child.gameObject.name == "TeleportLocations")           //Filter correct game object
            {
                foreach(Transform child2 in child)
                {
                    if (child2.gameObject.name == nextRoomSide)         //Filter the correct room side
                    {
                        playerRB.position = child2.position;            //Change the player position to the spawn point

                        //Reset the camera position
                        Camera.main.transform.position = new Vector3(   //Change the camera position to match the players
                                                                    child2.position.x, 
                                                                    child2.position.y, 
                                                                    Camera.main.transform.position.z);
                    }
                }
            }
        }

        foreach (CurrentDungeonData.roomPos mappyroommy in map) //Loop through the minimap list
        {
            if (currentRoom == mappyroommy.position)            //If the room position equals to the players current room
            {
                mappyroommy.room.setExplored(true);             //then turn the explored element into true
                break;
            }
        }

        updateMap();                                            //update the minimap to match to the players new position
    }

    //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    // Method: Create rooms 
    /////
    // Desc: Creates the rooms (if needed) connected to the one you just moved into
    //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    private void createNextRooms(Vector2Int roomLocation)
    {
        void addNewRoom(string hasOposingSide)
        {
            List<Dungeon.room> possibleSides = new List<Dungeon.room>();        //Stores all possible rooms (with a door that connects)
            List<Dungeon.room> possibleSidesFiltered = new List<Dungeon.room>();//Filtered version of the the previous list to account for problems
            Vector2Int newRoomDirection = roomLocation;                         //Holder for this new room position
            
            switch (hasOposingSide)                                             //Get direction of the new room
            {
                case "top":
                    newRoomDirection.y++;
                    break;

                case "bottom":
                    newRoomDirection.y--;
                    break;

                case "right":
                    newRoomDirection.x++;
                    break;

                case "left":
                    newRoomDirection.x--;
                    break;
            }
            
            for(int i = 0; i < currentDungeon.rooms.Count; i++) //loop through all possible rooms
            {
                Dungeon.room room = currentDungeon.rooms[i];    //Get room instance
                bool hasTheSide = false;                        //Holder to check if the rooms can connect with each other

                switch (hasOposingSide)
                {
                    case "top":
                        hasTheSide = room.roomSides.bottom;
                        break;

                    case "bottom":
                        hasTheSide = room.roomSides.top;
                        break;

                    case "right":
                        hasTheSide = room.roomSides.left;
                        break;

                    case "left":
                        hasTheSide = room.roomSides.right;
                        break;
                }

                if (hasTheSide)                 //Check if the room has an opposing side
                    possibleSides.Add(room);    //If it does then add it to the possible sides list
            }

            List<Dungeon.room> toBeRemovedRooms = new List<Dungeon.room>(); //A list of rooms that will need to be removed

            for (int i = 0; i < possibleSides.Count; i++)                   //Loop through the list of rooms that u have
            {
                Dungeon.room room = possibleSides[i];                       //Get the instance of the room

                Dungeon.room nextHallBottom = getRoomViaCords((newRoomDirection + Vector2Int.down));    //Room at the bottom
                Dungeon.room nextHallTop = getRoomViaCords((newRoomDirection + Vector2Int.up));         //Room at the top
                Dungeon.room nextHallRight = getRoomViaCords((newRoomDirection + Vector2Int.right));    //Room at the right
                Dungeon.room nextHallLeft = getRoomViaCords((newRoomDirection + Vector2Int.left));      //Room at the left

                //Check if there is a room in each of this sides||
                if(nextHallBottom.roomPrefab != null) 
                {
                    bool HasConection = nextHallBottom.roomSides.top;                                       //Connection Holder
                    if (HasConection && !room.roomSides.bottom || !HasConection && room.roomSides.bottom)   //If it passes this filter then they can't connect
                    {
                        toBeRemovedRooms.Add(room);                                                         //Add the room to the ones that need to be removed
                        continue;                                                                           //Reduce loop over work
                    }
                }

                if (nextHallTop.roomPrefab != null)
                {
                    bool HasConection = nextHallTop.roomSides.bottom;                                       //Connection Holder
                    if (HasConection && !room.roomSides.top || !HasConection && room.roomSides.top)         //If it passes this filter then they can't connect
                    {
                        toBeRemovedRooms.Add(room);                                                         //Add the room to the ones that need to be removed
                        continue;                                                                           //Reduce loop over work
                    }
                }

                if (nextHallRight.roomPrefab != null)
                {
                    bool HasConection = nextHallRight.roomSides.left;                                       //Connection Holder
                    if (HasConection && !room.roomSides.right || !HasConection && room.roomSides.right)     //If it passes this filter then they can't connect
                    {
                        toBeRemovedRooms.Add(room);                                                         //Add the room to the ones that need to be removed
                        continue;                                                                           //Reduce loop over work
                    }
                }

                if (nextHallLeft.roomPrefab != null)
                {
                    bool HasConection = nextHallLeft.roomSides.right;                                       //Connection Holder
                    if (HasConection && !room.roomSides.left || !HasConection && room.roomSides.left)       //If it passes this filter then they can't connect
                    {
                        toBeRemovedRooms.Add(room);                                                         //Add the room to the ones that need to be removed
                        continue;                                                                           //Reduce loop over work
                    }
                }

            }

            for (int i = 0; i < toBeRemovedRooms.Count; i++)    //Loop through rooms that need to be removed
            {
                possibleSides.Remove(toBeRemovedRooms[i]);      //Remove them from the possible rooms via value
            }

            if (possibleSides.Count > 1)                        //Check if it is possible to generate a non dead end way
            {
                foreach(Dungeon.room filteredRoom2 in possibleSides)    //Loop through possible sides list
                {
                    int sidesCounter = 0;                               //Holder to count the amount of sides each room has

                    if (filteredRoom2.roomSides.top)
                        sidesCounter++;
                    if (filteredRoom2.roomSides.bottom)
                        sidesCounter++;
                    if (filteredRoom2.roomSides.left)
                        sidesCounter++;
                    if (filteredRoom2.roomSides.right)
                        sidesCounter++;

                    if (sidesCounter > 1)                                               //If the room has more than 1 door then it isn't a dead end
                        possibleSidesFiltered.Add(filteredRoom2);                       //Add the non dead end to the new list
                }

                int chooseRandomRoom = Random.Range(0, possibleSidesFiltered.Count);    //Generate a random number 
                CreateNewRoom(newRoomDirection.x, newRoomDirection.y, possibleSidesFiltered[chooseRandomRoom]); //Create the new room
            }
            else                                                                                                //If the code goes through here it means it will need to generate a dead end
            {
                int chooseRandomRoom = Random.Range(0, possibleSides.Count);                                    //Generate a random number
                CreateNewRoom(newRoomDirection.x, newRoomDirection.y, possibleSides[chooseRandomRoom]);         //Add the dead end to the map
            }
            //______________||

        }

        Dungeon.room receivedRoom = getRoomViaCords(roomLocation);                                              //Get your current room
        Dungeon.room.sides receivedSides = receivedRoom.roomSides;                                              //Get current room sides

        if (receivedSides.top)                                                                                  //Check if this room has a topside
        {
            Vector2Int nextRoomPos = new Vector2Int(roomLocation.x, roomLocation.y + 1);                        //Get the next room position
            Dungeon.room nextRoomElement = getRoomViaCords(nextRoomPos);                                        //Get the next room element
            
            if (nextRoomElement.roomPrefab == null)                                                             //Check if there is a room at the top
                addNewRoom("top");                                                                              //If there isn't then request for one
        }

        if (receivedSides.bottom)
        {
            Vector2Int nextRoomPos = new Vector2Int(roomLocation.x, roomLocation.y - 1);                        //Get the next room position
            Dungeon.room nextRoomElement = getRoomViaCords(nextRoomPos);                                        //Get the next room element

            if (nextRoomElement.roomPrefab == null)                                                             //Check if there is a room at the bottom
                addNewRoom("bottom");                                                                           //If there isn't then request for one
        }
        if (receivedSides.left)
        {
            Vector2Int nextRoomPos = new Vector2Int(roomLocation.x - 1, roomLocation.y);                        //Get the next room position
            Dungeon.room nextRoomElement = getRoomViaCords(nextRoomPos);                                        //Get the next room element

            if (nextRoomElement.roomPrefab == null)                                                             //Check if there is a room at the left
                addNewRoom("left");                                                                             //If there isn't then request for one
        }   
        if (receivedSides.right)
        {
            Vector2Int nextRoomPos = new Vector2Int(roomLocation.x + 1, roomLocation.y);                        //Get the next room position
            Dungeon.room nextRoomElement = getRoomViaCords(nextRoomPos);                                        //Get the next room element

            if (nextRoomElement.roomPrefab == null)                                                             //Check if there is a room at the right
                addNewRoom("right");                                                                            //If there isn't then request for one
        }

    }

    //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    // Method: Get correct dungeon
    /////
    // Desc: Gets the dungeon that is gonna be used as the current one
    //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    private void getCorrectDungeon(List<Dungeon> theList, string wantedResult)
    {
                    
        foreach(Dungeon dungeon in theList)                 //Loop through all the dungeons on the API
        {
            if (dungeon.DungeonName == wantedResult)        //Check if the dungeon name matches the wanted one
            {
                currentDungeon = dungeon;                   //Changed the current dungeon holder to the found one
                
                //Add the starting room 
                startingRoom = Instantiate(dungeon.startingRoom, Vector2.zero, Quaternion.identity);    //Spawn the starting room
                startingRoom.name = dungeon.startingRoom.name + "00";                                   //Changed the starting room name
                Dungeon.room startingRoomObject = new Dungeon.room(                                     //Create a starting room class element
                                                "Starting Room",                                        //Sets the starting room name
                                                dungeon.startingRoom,                                   //Sets the starting room prefab
                                                new Dungeon.room.sides(                                 //Sets the starting room direction
                                                dungeon.startingRoomSides.top, 
                                                dungeon.startingRoomSides.right, 
                                                dungeon.startingRoomSides.left, 
                                                dungeon.startingRoomSides.bottom));    
                        
                    CreateNewRoom(0, 0, startingRoomObject);                                            //Creates the starting room at the minimap

                    //Add the room connected to the starting room
                    List<Dungeon.room> acceptedRooms = new List<Dungeon.room>();    //Create a filter list for the connected room

                    foreach (Dungeon.room room in currentDungeon.rooms)             //Loop through all the existing rooms of that new dungeon
                    {

                        int roomAmountOfSides = 0;                                  //Hold the amount of sides that this room has
                            
                        if (room.roomSides.bottom)
                            roomAmountOfSides++;
                        if (room.roomSides.top)
                            roomAmountOfSides++;
                        if (room.roomSides.right)
                            roomAmountOfSides++;
                        if (room.roomSides.left)
                            roomAmountOfSides++;

                        if (roomAmountOfSides > 1 && room.roomSides.bottom)        //If the room has more than 1 side (avoid one way rooms) then add to the filter list
                            acceptedRooms.Add(room);
                            
                    }

                    int chooseRandom = Random.Range(0, acceptedRooms.Count);       //Generate a random integer

                    CreateNewRoom(0, 1, acceptedRooms[chooseRandom]);              //Randomly choose from the filtered list one room
                    currentRoom = new Vector2Int(0, 0);                            //Set the current room cords

                return;                                                            //Reset the whole function
            }
        }
    }

    //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    // Method: Update the minimap
    /////
    // Desc: Updates the rooms that are displayed by the minimap
    //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    public void updateMap()
    {
        UIelements.miniMap.textElement.GetComponent<Text>().text = "Current room: " + currentRoom.x + "," + currentRoom.y;  //Change the text label of the minimap
        int roomSize = (int)UIelements.miniMap.roomPrefab.GetComponent<RectTransform>().sizeDelta.x;                        //Get the room icon prefab size
 
        foreach (Transform child in UIelements.miniMap.mask.transform)                                                      //Loop through minimap rooms
        {
            Vector2Int childRoomPos = new Vector2Int(                       //The room position
                                    (int)child.localPosition.x / roomSize, 
                                    (int)child.localPosition.y / roomSize); 
            Dungeon.room minimapRoom = getRoomViaCords(childRoomPos);       //Get a reference to the actual room
            Transform playerElement = child.Find("player");                 //Get the player pointer

            if (childRoomPos == currentRoom)                                //Check if the minimap room == to the real room
                playerElement.gameObject.SetActive(true);                   //If yes enable the player pointer
            else
                playerElement.gameObject.SetActive(false);                  //If no then disable the player pointer
            
            //handle room color
            if (minimapRoom.getCompleted())                                //If the room has already been completed
                child.GetComponent<Image>().color = UIelements.miniMap.completedRoom;
            else if (minimapRoom.getExplored())                            //If the room has already been explored 
                child.GetComponent<Image>().color = UIelements.miniMap.uncompletedRoom;
            else                                                           //If the room hasn't been completed nor explored
                child.GetComponent<Image>().color = UIelements.miniMap.unexploredRoom;


        }

        //Repositionate the map just to make sure that it is always centered
        UIelements.miniMap.mask.transform.localPosition = 
            new Vector2(-currentRoom.x  * (roomSize * UIelements.miniMap.mask.transform.localScale.x), 
            -currentRoom.y * (roomSize * UIelements.miniMap.mask.transform.localScale.y));
    }

    //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    // Method: Create minimap room
    /////
    // Desc: Create a new room prefab on the minimap
    //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    private void CreateNewRoom(int x, int y, Dungeon.room room)
        {
        int roomSize = (int)UIelements.miniMap.roomPrefab.GetComponent<RectTransform>().sizeDelta.x;//Get the room prefab size
        GameObject newRoom = Instantiate(UIelements.miniMap.roomPrefab);                            //Create the room element
        newRoom.transform.SetParent(UIelements.miniMap.mask.transform);                             //Change the room parent into the minimap
        newRoom.GetComponent<RectTransform>().localScale = new Vector2(1, 1);                       //Set the room size
        newRoom.transform.localPosition = new Vector2(x * roomSize, y * roomSize);                  //Put at correct position inside of the minimap

        newRoom.GetComponent<Image>().color = UIelements.miniMap.unexploredRoom;                    //Set its color to an unexplored room
        room.setExplored(false);                                                                    //The room has not been explored

        if (new Vector2Int(x, y) == Vector2Int.zero)                                                //Check if it is the entrance room
            newRoom.transform.Find("entrance").gameObject.SetActive(true);                          //If yes then enable the entrance icon

        foreach (Transform door in newRoom.transform)                                               //Show the doors that should be displayed
                {
                    if (door.gameObject.name == "top")
                        door.gameObject.SetActive(room.roomSides.top);
                    if (door.gameObject.name == "bottom")
                        door.gameObject.SetActive(room.roomSides.bottom);
                    if (door.gameObject.name == "right")
                        door.gameObject.SetActive(room.roomSides.right);
                    if (door.gameObject.name == "left")
                        door.gameObject.SetActive(room.roomSides.left);
                }

        //Add to the list of minimap rooms
        map.Add(new roomPos(x, y, new Dungeon.room(room.roomName, room.roomPrefab, room.roomSides, room.getCompleted(), false)));
    }

    //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    // Method: Spawn the player
    /////
    // Desc: Spawns the player when he joins the dungeon instance
    //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    private void SpawnPlayer()
    {
        foreach (Transform child in startingRoom.transform) //Loop through the starting area childs
        {
            if(child.gameObject.name == "TeleportLocations")    //Filter the correct child
            {
                foreach (Transform location in child)           //Loop through all possible teleportation points
                {
                    if (location.gameObject.name == "exit")     //Check if the child is the exit
                    {
                        playerRB.position = location.position;  //Move the player to this teleport point
                        Camera.main.transform.position = new Vector3(location.position.x, location.position.y, Camera.main.transform.position.z); //Fix the camera
                        break;
                    }
                }
                break;
            }
        }

    }
}
