using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CurrentDungeonData : MonoBehaviour
{
    //Variables||

    //UI elements||

            //Handle the dungeon rewards||

            [System.Serializable]
            public class dungeonResult
            {
                public GameObject window;
                public GameObject textLabel;
                public GameObject acceptButton;
                public GameObject cancelButton;
            }

            public dungeonResult DungeonResultWindow = new dungeonResult(); 
            //__________________________||


            [System.Serializable]
            public class uiElements
            {
                [System.Serializable]
                public class minimap{

                    public GameObject textElement;
                    public GameObject mask;
                    public GameObject roomPrefab;

                    public Color completedRoom;
                    public Color uncompletedRoom;
                    public Color unexploredRoom;


                }

                public minimap miniMap = new minimap();

            }

            public uiElements UIelements = new uiElements();
        //___________||
        

        //Main village id number
        public int mainVillageID = 1;

        //Dungeons List
        public GameObject dungeonsData;

        //Starting room
        private GameObject startingRoom;

        //The current room coordinates
        public Vector2Int currentRoom;

        //The next room teleport point
        public string nextRoomSide;

        //The player
        Rigidbody2D playerRB;

        //Current played dungeon
        public Dungeon currentDungeon;

        //Store inactive rooms
        private List<GameObject> inactiveRooms = new List<GameObject>();
               
        private List<roomPos> map = new List<roomPos>();

        //Counters||

            public int amountOfChests = 0;
            public int amountOfDeaths = 0;
        //________||

    //_________||

    //Class to handle rooms position
    class roomPos
    {
        public Vector2Int position = new Vector2Int();
        public Dungeon.room room;

        public roomPos(int x, int y, Dungeon.room room2)
        {
            position.x = x;
            position.y = y;
            room = room2;
        }

    }

    public Dungeon.room getRoomViaCords(Vector2Int cords)
    {
        Dungeon.room foundRoom = new Dungeon.room();

        bool foundAroom = false;

        //Loop through all the dungeon rooms that exist
        foreach (roomPos room in map)
        {

            if (room.position == cords)
            {
                foundRoom = room.room;
                foundAroom = true;
                return room.room;
            }
        }

        if (!foundAroom)
        {
            foundRoom.roomPrefab = null;
            return foundRoom;
        }
        else
        {
            return foundRoom;
        }


    }


    public void CompletedRoom()
    {
        //Get the current room
        Dungeon.room RoomWeAreIn = getRoomViaCords(currentRoom);
        RoomWeAreIn.setCompleted(true);
        updateMap();
    }

    public void changeNextRoom(string roomSide)
    {
        //Same a cloned version of the current room
        GameObject CurrentRoomReference;
        
        //Get the current active room
        foreach(GameObject child in GameObject.FindGameObjectsWithTag("dungeonRoom"))
        {
            if (child.activeSelf)
            {
                CurrentRoomReference = child;
            }
        }
        
        //string LocalPath = "Assets/Dungeons/CurrentRooms/" + getRoomViaCords(currentRoom).roomPrefab.name + currentRoom.x + currentRoom.y + ".prefab";
        //getRoomViaCords(currentRoom).roomPrefab = PrefabUtility.SaveAsPrefabAssetAndConnect(CurrentRoomReference, LocalPath, InteractionMode.AutomatedAction);

        //Calculate on which side from the next room should the player be spawned in
        switch (roomSide)
        {
            case "right":
                nextRoomSide = "left";
                currentRoom.x++;
                break;

            case "left":
                nextRoomSide = "right";
                currentRoom.x--;
                break;

            case "top":
                nextRoomSide = "bottom";
                currentRoom.y++;
                break;

            case "bottom":
                nextRoomSide = "top";
                currentRoom.y--;
                break;

            case "exit":
                sceneTeleport.start(mainVillageID);
                break;

            default:
                nextRoomSide = null;
                break;
        }

        //Update discord presence
        DiscordPresence.PresenceManager.instance.presence.state = "Room: " + "["+ currentRoom.x+ ", "+ currentRoom .y+ "]";
        DiscordPresence.PresenceManager.UpdatePresence(null);

        //Destroy all the current existing dungeon rooms
        foreach (GameObject room in GameObject.FindGameObjectsWithTag("dungeonRoom"))
        {
            if (room.activeSelf)
            {
                room.SetActive(false);
                inactiveRooms.Add(room);
            }
            //Destroy(room);
        }

        //Gets the room
        Dungeon.room roomToCreate = getRoomViaCords(currentRoom);

        //Check if the next rooms already exist and if not create them
        createNextRooms(currentRoom);

        if (roomToCreate.roomPrefab != null)
        {
            //Check if it was already spawned, if not spawn it!
            bool roomWasSpawned = false;

            for (int index = 0; index < inactiveRooms.Count; index++)
            {
                GameObject room = inactiveRooms[index];
                if (room != null && room.name == roomToCreate.roomPrefab.name + currentRoom.x + currentRoom.y)
                {
                    roomWasSpawned = true;
                    room.SetActive(true);
                    inactiveRooms.Remove(room);
                }
            }

            if (!roomWasSpawned)
            {
                //Then create a new instance
                //Spawn the next room into the map
                GameObject roomThatDidntExist = Instantiate(roomToCreate.roomPrefab, roomToCreate.roomPrefab.transform.position, Quaternion.identity);
                roomThatDidntExist.name = roomToCreate.roomPrefab.name + currentRoom.x + currentRoom.y;
            }


        }


        //Teleport the player to the correct side
        foreach (Transform child in roomToCreate.roomPrefab.transform)
        {
            if (child.gameObject.name == "TeleportLocations")
            {
                foreach(Transform child2 in child)
                {
                    if (child2.gameObject.name == nextRoomSide)
                    {
                        playerRB.position = child2.position;

                        //Reset the camera position
                        Camera.main.transform.position = new Vector3(child2.position.x, child2.position.y, Camera.main.transform.position.z);
                    }
                }
            }
        }

        //Change the explored state
        foreach (CurrentDungeonData.roomPos mappyroommy in map)
        {
            if (currentRoom == mappyroommy.position)
            {
                mappyroommy.room.setExplored(true);
            }
        }

        //Update the UI
        updateMap();

    }

    private void createNextRooms(Vector2Int roomLocation)
    {

        void addNewRoom(string hasOposingSide)
        {

            //Check if the next shape can be created
            //Store possible sides
            List<Dungeon.room> possibleSides = new List<Dungeon.room>();
            List<Dungeon.room> possibleSidesFiltered = new List<Dungeon.room>();

            Vector2Int newRoomDirection = roomLocation;

            //Get direction of the new room
            switch (hasOposingSide)
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

            //loop through all possible rooms
            for(int i = 0; i < currentDungeon.rooms.Count; i++)
            {
                Dungeon.room room = currentDungeon.rooms[i];

                bool hasTheSide = false;

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

                if (hasTheSide)
                {

                    //If the room has an opposing side then add to the list
                    possibleSides.Add(room);
                }
            }

            //ADD THE FILTER||

            List<Dungeon.room> toBeRemovedRooms = new List<Dungeon.room>();

            //Loop through the list of rooms that u have
            for (int i = 0; i < possibleSides.Count; i++)
            {
                //Checks each side of the room to see if something should be removed or not

                Dungeon.room room = possibleSides[i];

                //Bottom room
                Vector2Int nextHallPositionBottom = (newRoomDirection + Vector2Int.down);
                Dungeon.room nextHallBottom = getRoomViaCords(nextHallPositionBottom);

                //Top room
                Vector2Int nextHallPositionTop = (newRoomDirection + Vector2Int.up);
                Dungeon.room nextHallTop = getRoomViaCords(nextHallPositionTop);

                //Right room
                Vector2Int nextHallPositionRight = (newRoomDirection + Vector2Int.right);
                Dungeon.room nextHallRight = getRoomViaCords(nextHallPositionRight);

                //Left room
                Vector2Int nextHallPositionLeft = (newRoomDirection + Vector2Int.left);
                Dungeon.room nextHallLeft = getRoomViaCords(nextHallPositionLeft);

                //Check if there is a room in each of this sides
                if(nextHallBottom.roomPrefab != null) {
                    //There is a room at its bottom
                    bool HasConection = nextHallBottom.roomSides.top;

                    if (HasConection && !room.roomSides.bottom || !HasConection && room.roomSides.bottom)
                    {

                        //They cannot connect
                        toBeRemovedRooms.Add(room);
                        continue;
                    }
                }

                if (nextHallTop.roomPrefab != null)
                {
                    //There is a room at its top
                    bool HasConection = nextHallTop.roomSides.bottom;

                    if (HasConection && !room.roomSides.top || !HasConection && room.roomSides.top)
                    {

                        //They cannot connect
                        toBeRemovedRooms.Add(room);
                        continue;
                    }
                }

                if (nextHallRight.roomPrefab != null)
                {
                    //There is a room at its right
                    bool HasConection = nextHallRight.roomSides.left;

                    if (HasConection && !room.roomSides.right || !HasConection && room.roomSides.right)
                    {

                        //They cannot connect
                        toBeRemovedRooms.Add(room);
                        continue;
                    }
                }

                if (nextHallLeft.roomPrefab != null)
                {
                    //There is a room at its left
                    bool HasConection = nextHallLeft.roomSides.right;

                    if (HasConection && !room.roomSides.left || !HasConection && room.roomSides.left)
                    {
                        //They cannot connect
                        toBeRemovedRooms.Add(room);
                        continue;
                    }
                }


            }

            //Remove the rooms that are extra
            for (int i = 0; i < toBeRemovedRooms.Count; i++)
            {
                possibleSides.Remove(toBeRemovedRooms[i]);
            }

            //this means there are rooms that are not just one sided
            //it means we can remove the one sided rooms to avoid dead ends
            if (possibleSides.Count > 1)
            {

                foreach(Dungeon.room filteredRoom2 in possibleSides)
                {
                    int sidesCounter = 0;
                    if (filteredRoom2.roomSides.top)
                    {
                        sidesCounter++;
                    }
                    if (filteredRoom2.roomSides.bottom)
                    {
                        sidesCounter++;
                    }
                    if (filteredRoom2.roomSides.left)
                    {
                        sidesCounter++;
                    }
                    if (filteredRoom2.roomSides.right)
                    {
                        sidesCounter++;
                    }

                    if (sidesCounter > 1)
                    {
                        possibleSidesFiltered.Add(filteredRoom2);
                    }
                }

                //Add the new room
                int chooseRandomRoom = Random.Range(0, possibleSidesFiltered.Count);
                CreateNewRoom(newRoomDirection.x, newRoomDirection.y, possibleSidesFiltered[chooseRandomRoom]);
            }
            else
            {
                //Add the new room
                int chooseRandomRoom = Random.Range(0, possibleSides.Count);
                CreateNewRoom(newRoomDirection.x, newRoomDirection.y, possibleSides[chooseRandomRoom]);
            }
            //______________||

        }



        //Get the room you're in
        Dungeon.room receivedRoom = getRoomViaCords(roomLocation);

        //Get the sides avalible
        Dungeon.room.sides receivedSides = receivedRoom.roomSides;

        if (receivedSides.top)
        {
            //Has topside
            Vector2Int nextRoomPos = new Vector2Int(roomLocation.x, roomLocation.y + 1);
            Dungeon.room nextRoomElement = getRoomViaCords(nextRoomPos);
            
            if (nextRoomElement.roomPrefab == null)
            {
                //The room still did not exist
                addNewRoom("top");
            }

        }
        if (receivedSides.bottom)
        {
            //Has bottomside
            Vector2Int nextRoomPos = new Vector2Int(roomLocation.x, roomLocation.y - 1);
            Dungeon.room nextRoomElement = getRoomViaCords(nextRoomPos);

            if (nextRoomElement.roomPrefab == null)
            {
                addNewRoom("bottom");
            }

        }
        if (receivedSides.left)
        {
            //Has left side
            Vector2Int nextRoomPos = new Vector2Int(roomLocation.x - 1, roomLocation.y);
            Dungeon.room nextRoomElement = getRoomViaCords(nextRoomPos);

            if (nextRoomElement.roomPrefab == null)
            {
                addNewRoom("left");
            }

        }
        if (receivedSides.right)
        {
            //Has right side
            Vector2Int nextRoomPos = new Vector2Int(roomLocation.x + 1, roomLocation.y);
            Dungeon.room nextRoomElement = getRoomViaCords(nextRoomPos);

            if (nextRoomElement.roomPrefab == null)
            {
                addNewRoom("right");
            }

        }


    }









    private void getCorrectDungeon(List<Dungeon> theList, string wantedResult)
    {
        //Check if the list is empty
        if (theList.Count > 0)
        {
            //Loop through all the dungeons
            foreach(Dungeon dungeon in theList)
            {
                if (dungeon.DungeonName == wantedResult)
                {
                    currentDungeon = dungeon;

                    //Spawn the starting room
                    startingRoom = Instantiate(dungeon.startingRoom, Vector2.zero, Quaternion.identity);
                    startingRoom.name = dungeon.startingRoom.name + "00";

                    //ADD STARTING ROOM TO MAP||

                    //Add the starting room

                        Dungeon.room startingRoomObject = new Dungeon.room("Starting Room", dungeon.startingRoom, new Dungeon.room.sides(true, false, false, false));
                        
                        CreateNewRoom(0, 0, startingRoomObject);

                        List<Dungeon.room> acceptedRooms = new List<Dungeon.room>();

                        //Add the following room
                        foreach (Dungeon.room room in currentDungeon.rooms)
                        {

                            int roomAmountOfSides = 0;
                            
                            //Check if the room has a bottom dooor
                            if (room.roomSides.bottom)
                            {
                                roomAmountOfSides++;
                                
                            }
                            if (room.roomSides.top)
                            {
                                roomAmountOfSides++;
                            }
                            if (room.roomSides.right)
                            {
                                roomAmountOfSides++;
                            }
                            if (room.roomSides.left)
                            {
                                roomAmountOfSides++;
                            }

                            if (roomAmountOfSides > 1 && room.roomSides.bottom)
                            {
                                acceptedRooms.Add(room);
                            }
                            
                        }

                        int chooseRandom = Random.Range(0, acceptedRooms.Count);

                    CreateNewRoom(0, 1, acceptedRooms[chooseRandom]);
                    //________________________||


                    //Set the current coordinates
                    currentRoom = new Vector2Int(0, 0);

                    return;
                }
            }
        }
        else
        {
            Debug.Log("No dungeons found");
        }

    }


    public void updateMap()
    {

        //Change the minimap text
        UIelements.miniMap.textElement.GetComponent<Text>().text = "Current room: " + currentRoom.x + "," + currentRoom.y;

        //Get the room size
        int roomSize = (int)UIelements.miniMap.roomPrefab.GetComponent<RectTransform>().sizeDelta.x;


        //Redo the minimap colors 
        foreach (Transform child in UIelements.miniMap.mask.transform)
        {
            //The room position
            Vector2Int childRoomPos = new Vector2Int((int)child.localPosition.x / roomSize, (int)child.localPosition.y / roomSize);

            //Get the room
            Dungeon.room minimapRoom = getRoomViaCords(childRoomPos);

            //Get the player pointer
            Transform playerElement = child.Find("player");

            //Found the correct room
            if (childRoomPos == currentRoom)
            {

                //Also show the player marker
                playerElement.gameObject.SetActive(true);
            }
            else
            {
                //Disable the player marker just in case
                playerElement.gameObject.SetActive(false);
            }

            //Change the room color to the correct one
            if (minimapRoom.getCompleted())
                child.GetComponent<Image>().color = UIelements.miniMap.completedRoom;
            else if (minimapRoom.getExplored())
                child.GetComponent<Image>().color = UIelements.miniMap.uncompletedRoom;
            else
                child.GetComponent<Image>().color = UIelements.miniMap.unexploredRoom;


        }

        //Repositionate the map
        UIelements.miniMap.mask.transform.localPosition = 
            new Vector2(-currentRoom.x  * (roomSize * UIelements.miniMap.mask.transform.localScale.x), 
            -currentRoom.y * (roomSize * UIelements.miniMap.mask.transform.localScale.y));

    }


    private void CreateNewRoom(int x, int y, Dungeon.room room)
    {

        //Maybe attach this to the map???

        //Add to the minimap||

                int roomSize = (int)UIelements.miniMap.roomPrefab.GetComponent<RectTransform>().sizeDelta.x;

                //Create the element on the room
                GameObject newRoom = Instantiate(UIelements.miniMap.roomPrefab);

                //Change it's nesting position
                newRoom.transform.SetParent(UIelements.miniMap.mask.transform);
                newRoom.GetComponent<RectTransform>().localScale = new Vector2(1, 1);

                //Put at correct position
                newRoom.transform.localPosition = new Vector2(x * roomSize, y * roomSize);


            //Set correct color
            if (room.getCompleted())
                newRoom.GetComponent<Image>().color = UIelements.miniMap.completedRoom;
            else
                newRoom.GetComponent<Image>().color = UIelements.miniMap.unexploredRoom;

            //If it is the entrance room
            if (new Vector2Int(x, y) == Vector2Int.zero)
            {
                Transform entranceElement = newRoom.transform.Find("entrance");
                entranceElement.gameObject.SetActive(true);
            }


        //Change door displaying
        foreach (Transform door in newRoom.transform)
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


        //__________________||

        //The room cannot be explored
        room.setExplored(false);

        //Add to the list
        map.Add(new roomPos(x, y, 
            new Dungeon.room(room.roomName, room.roomPrefab, room.roomSides, room.getCompleted(), false)));
    }

    private void SpawnPlayer()
    {
        foreach (Transform child in startingRoom.transform)
        {
            if(child.gameObject.name == "TeleportLocations")
            {
                foreach (Transform location in child)
                {
                    if (location.gameObject.name == "exit")
                    {
                        playerRB.position = location.position;

                        //Reset the camera position
                        Camera.main.transform.position = new Vector3(location.position.x, location.position.y, Camera.main.transform.position.z);
                    }
                }
            }
        }

    }

    private void Start()
    {

        //Get the list of dungeons
        List<Dungeon> dungeonsList = dungeonsData.GetComponent<DungeonsAPI>().dungeons;

        //Get the player body
        playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();

        GameObject dungeonAPI = GameObject.FindGameObjectWithTag("sceneAPI");
        if (dungeonAPI != null)
        {
            //Change the second argument when needed
            getCorrectDungeon(dungeonsList, sceneTeleport.dungeonName);
            SpawnPlayer();

            //Avoid duplicates
            Destroy(dungeonAPI);

            //Update the minimap
            updateMap();
        }
        else
        {
            //deadmines is the current default dungeon
            //Change the second argument when needed
            getCorrectDungeon(dungeonsList, "Deadmines");
            SpawnPlayer();

            //Update the minimap
            updateMap();
        }

    }


    //Variables for the loading screen||

        public GameObject background;
        public GameObject loadingScreen;
        public GameObject loadingBar;
    //________________________________||

    public void callMapLoad() {

        //Save the object for the next scene
        DontDestroyOnLoad(gameObject);

        sceneTeleport.start(1);

        //StartCoroutine("LoadMainMap", 1);
    }
}
