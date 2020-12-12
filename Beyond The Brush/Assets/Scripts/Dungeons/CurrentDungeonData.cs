using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CurrentDungeonData : MonoBehaviour
{
    //Variables||

    //UI elements||
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
        private Vector2Int currentRoom;

        //The next room teleport point
        private string nextRoomSide;

        //The player
        Rigidbody2D playerRB;

        //Current played dungeon
        public Dungeon currentDungeon;
               
        private List<roomPos> map = new List<roomPos>();
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

    private Dungeon.room getRoomViaCords(Vector2Int cords)
    {
        Dungeon.room foundRoom = new Dungeon.room();

        //Loop through all the dungeon rooms that exist
        foreach (roomPos room in map)
        {

            if (room.position == cords)
            {
                foundRoom = room.room;
                return room.room;
            }
        }

        return foundRoom;
    }


    public void changeNextRoom(string roomSide)
    {

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
                StartCoroutine("LoadMainMap", mainVillageID);
                break;

            default:
                nextRoomSide = null;
                break;
        }

        //Destroy all the current existing dungeon rooms
        foreach (GameObject room in GameObject.FindGameObjectsWithTag("dungeonRoom"))
        {
            Destroy(room);
        }

        //Gets the room
        Dungeon.room roomToCreate = getRoomViaCords(currentRoom);

        //Check if the next rooms already exist and if not create them
        createNextRooms(currentRoom);

        //Spawn the next room into the map
        Instantiate(roomToCreate.roomPrefab, roomToCreate.roomPrefab.transform.position, Quaternion.identity);

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
                    }
                }
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
            List<Dungeon.room> filteredList = new List<Dungeon.room>();
            List<Dungeon.room> extraFilteredList = new List<Dungeon.room>();

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
            foreach (Dungeon.room room in currentDungeon.rooms)
            {
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

            //Loop through the list of rooms that u have
            foreach (Dungeon.room room in possibleSides)
            {
                bool roomShouldBeRemoved = false;

                //Check what sides does this room has
                //it will then check if there already is a room on that direction or not
                //In case there is it will check if it is possible to make a connection
                //if it isn't then this room cannot exist
                //this avoids rooms whose doors would reach another room wall
                //it will obviously increase the odds of the development of an impossible dungeon thoe
                //a more specific algorithm will need to be added in a near future
                if (room.roomSides.bottom)
                {
                    Dungeon.room nextHall = getRoomViaCords(newRoomDirection + Vector2Int.down);
                    if (nextHall.roomPrefab != null)
                    {

                        //There is already a room here
                        if (!nextHall.roomSides.top)
                        {
                            //There was no door here
                            roomShouldBeRemoved = true;

                        }
                    }
                }

                if (room.roomSides.top)
                {
                    Dungeon.room nextHall = getRoomViaCords(newRoomDirection + Vector2Int.up);
                    if (nextHall.roomPrefab != null)
                    {
                        //There is already a room here
                        if (!nextHall.roomSides.bottom)
                        {
                            //There was no door here
                            roomShouldBeRemoved = true;

                        }
                    }
                }

                if (room.roomSides.right)
                {
                    Dungeon.room nextHall = getRoomViaCords(newRoomDirection + Vector2Int.right);
                    if (nextHall.roomPrefab != null)
                    {
                        //There is already a room here
                        if (!nextHall.roomSides.left)
                        {
                            //There was no door here
                            roomShouldBeRemoved = true;

                        }
                    }
                }

                if (room.roomSides.left)
                {
                    Dungeon.room nextHall = getRoomViaCords(newRoomDirection + Vector2Int.left);
                    if (nextHall.roomPrefab != null)
                    {
                        //There is already a room here
                        if (!nextHall.roomSides.right)
                        {
                            //There was no door here
                            roomShouldBeRemoved = true;

                        }
                    }
                }


                if (!roomShouldBeRemoved)
                {
                    filteredList.Add(room);
                }

            }

            //check if there are only one side rooms
            bool thereAreNotOneSidedRooms = false;

            foreach (Dungeon.room filteredRoom in filteredList)
            {
                int sidesCounter = 0;
                if (filteredRoom.roomSides.top)
                {
                    sidesCounter++;
                }
                if (filteredRoom.roomSides.bottom)
                {
                    sidesCounter++;
                }
                if (filteredRoom.roomSides.left)
                {
                    sidesCounter++;
                }
                if (filteredRoom.roomSides.right)
                {
                    sidesCounter++;
                }

                if (sidesCounter > 1)
                {
                    thereAreNotOneSidedRooms = true;
                }
            }


            //this means there are rooms that are not just one sided
            //it means we can remove the one sided rooms to avoid dead ends
            if (thereAreNotOneSidedRooms)
            {
                foreach(Dungeon.room filteredRoom in filteredList)
                {
                    int sidesCounter = 0;
                    if (filteredRoom.roomSides.top)
                    {
                        sidesCounter++;
                    }
                    if (filteredRoom.roomSides.bottom)
                    {
                        sidesCounter++;
                    }
                    if (filteredRoom.roomSides.left)
                    {
                        sidesCounter++;
                    }
                    if (filteredRoom.roomSides.right)
                    {
                        sidesCounter++;
                    }

                    if (sidesCounter > 1)
                    {
                        extraFilteredList.Add(filteredRoom);
                    }
                }
            }
            //______________||

            if (extraFilteredList.Count < 1)
            {
                //Add the new room
                int chooseRandomRoom = Random.Range(0, filteredList.Count);
                CreateNewRoom(newRoomDirection.x, newRoomDirection.y, filteredList[chooseRandomRoom]);
            }
            else
            {
                //Add the new room
                int chooseRandomRoom = Random.Range(0, extraFilteredList.Count);
                CreateNewRoom(newRoomDirection.x, newRoomDirection.y, extraFilteredList[chooseRandomRoom]);
            }


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


                    //ADD STARTING ROOM TO MAP||

                    //Add the starting room

                        Dungeon.room startingRoomObject = new Dungeon.room("Starting Room", dungeon.startingRoom, new Dungeon.room.sides(true, false, false, false));
                        startingRoomObject.setCompleted(true);

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


    private void updateMap()
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

        //Check explored status
        foreach (CurrentDungeonData.roomPos pos in map)
        {
            Debug.Log(pos.position.x + ":" + pos.position.y + " - " + pos.room.getExplored());
        }

    }


    private void CreateNewRoom(int x, int y, Dungeon.room room)
    {

        //Maybe attach this to the map???

        //Add to the minimap||

            bool alreadyExists = false;
            int roomSize = (int)UIelements.miniMap.roomPrefab.GetComponent<RectTransform>().sizeDelta.x;

            //Check if it already exists
            foreach (Transform child in UIelements.miniMap.mask.transform)
            {

                if (child.position.x == x * roomSize && child.position.y == y * roomSize)
                {
                    alreadyExists = true;
                }
            }

            if (!alreadyExists)
            {
                //Create the element on the room
                GameObject newRoom = Instantiate(UIelements.miniMap.roomPrefab);

                //Change it's nesting position
                newRoom.transform.parent = UIelements.miniMap.mask.transform;
                newRoom.GetComponent<RectTransform>().localScale = new Vector2(1, 1);

                //Put at correct position
                newRoom.transform.localPosition = new Vector2(x * roomSize, y * roomSize);

                //Set correct color
                if (room.getCompleted())
                    newRoom.GetComponent<Image>().color = UIelements.miniMap.completedRoom;
                else
                    newRoom.GetComponent<Image>().color = UIelements.miniMap.unexploredRoom;
                

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

            }
        //__________________||

        //The room cannot be explored
        room.setExplored(false);

        //Add to the list
        map.Add(new roomPos(x, y, 
            new Dungeon.room(room.roomName, room.roomPrefab, room.roomSides)));
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
            getCorrectDungeon(dungeonsList, dungeonAPI.GetComponent<sceneAPI>().nextDungeon);
            SpawnPlayer();

            //Avoid duplicates
            Destroy(dungeonAPI);
        }
        else
        {
            //deadmines is the current default dungeon
            //Change the second argument when needed
            getCorrectDungeon(dungeonsList, "Deadmines");
            SpawnPlayer();
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

        StartCoroutine("LoadMainMap", 1);
    }


    IEnumerator LoadMainMap(int scene)
    {
            
        //Get the loading screen transparency
        CanvasGroup backAlpha = background.GetComponent<CanvasGroup>();
        backAlpha.alpha = 0;

        //Turn the loading screen on
        loadingScreen.SetActive(true);

        //Add some sort of transition
        while (backAlpha.alpha < 1)
        {
            backAlpha.alpha += 0.05f;
            yield return new WaitForSeconds(0.05f);
        }



        loadingBar.SetActive(true);

        //Just wait
        yield return new WaitForSeconds(1);


        //Start actually loading the new scene
        AsyncOperation opereration = SceneManager.LoadSceneAsync(scene);

        //Track the new scene progress
        while (!opereration.isDone)
        {
            //Calculate current progress
            float CurrentProgress = Mathf.Clamp(opereration.progress / 0.9f, 1, 2);

            //Get the bar fill object
            GameObject barFill = loadingBar.GetComponent<Transform>().GetChild(0).gameObject;

            //Change the bar size
            barFill.GetComponent<Image>().fillAmount = CurrentProgress;

            //Restart the loop
            yield return null;
        }

    }

}
