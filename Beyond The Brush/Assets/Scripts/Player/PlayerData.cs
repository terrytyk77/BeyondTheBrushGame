using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerData : MonoBehaviour
{
    //Variables||

        //Instance
        static public PlayerData instance;

        private void Awake()
        {
            instance = this;
        }

        //Database data
        static private string _id = null;
        static private string _username = "Offline Player";
        static private int _level = 1;
        static private int _exp = 0;
        static private int _resources = 20000;
        static private int _gold = 0;
        static private string _email;


        //options database data
        static private bool _windowmode = true;
        static private float _musicVolume = 0.3f;
        static private float _sfxVolume = 0.3f;

        //local data
        static private int _healthPoints = 100;
        static private int _maxHealthPoints = 100;
        static private float _movementSpeed = 4f;

        //Spells Damage/Shield
        static public int _slashDamage = 20;
        static public int _xslashDamage = 50;
        static public int _shieldDamageReduction = 50;
        static public int _shieldCurrentStack = 1;
        static public int _shieldMaxStack = 1;
        static public bool _isShielded = false;

        //Cooldowns
        static public float slashCooldownDefault = 1.5f;
        static public float xslashCooldownDefault = 10f;
        static public float shieldCooldownDefault = 12f;
        static public float boxSpawnCooldownDefault = 10f;
        static public float rockSpawnCooldownDefault = 10f;
    
        static public float shieldTimerDefault = 4f;
        

    private class cooldowsClass{
            public float _slashCooldown = 0;
            public float _xslashCooldown = 0;
            public float _shieldCooldown = 0;
            public float _rockCooldown = 0;
            public float _boxCooldown = 0;
            public float _shieldTimer = 0;
    }

        static private cooldowsClass cooldowns = new cooldowsClass();

        //Talent tree
        [System.Serializable]
        public class talentTreeClass
        {
            public bool node0 = false;
            public bool node1 = false;
            public bool node2 = false;
            public bool node3 = false;
            public bool node4 = false;
            public bool node5 = false;
            public bool node6 = false;
            public bool node7 = false;
            public bool node8 = false;
            public bool node9 = false;
            public bool node10 = false;
        }

    static private talentTreeClass _talentTreeData = new talentTreeClass();
    public static talentTreeClass talentTreeData { get { return _talentTreeData; } set { _talentTreeData = value; } }


    //_________||

    //Get the database data
    public static string id {get{return _id;} set { _id = value; } }
    public static string email { get { return _email; } }
    public static string username { get { return _username; } set { _username = value; } }
    public static int level { get { return _level; } set { _level = value; } }
    public static int exp { get { return _exp; } set { _exp = value; } }
    public static int resources { get { return _resources; } set { _resources = value; } }
    public static int gold { get { return _gold; } set { _gold = value; } }
    public static float musicVolume { get { return _musicVolume; } set { _musicVolume = value; } }
    public static float sfxVolume { get { return _sfxVolume; } set { _sfxVolume = value; } }
    public static bool windowmode { get { return _windowmode; } set { _windowmode = value; } }

    //Get the local data
    public static int healthPoints { get { return _healthPoints; } set { _healthPoints = value; } }
    public static int maxHealthPoints { get { return _maxHealthPoints; } set { _maxHealthPoints = value; } }
    public static float movementSpeed { get { return _movementSpeed; } set { _movementSpeed = value; } }

    //Spells Damage/Shield
    static public int slashDamage { get { return _slashDamage; } set { _slashDamage = value; } }
    static public int xslashDamage { get { return _xslashDamage; } set { _xslashDamage = value; } }
    static public int shieldDamageReduction { get { return _shieldDamageReduction; } set { _shieldDamageReduction = value; } }
    static public int shieldCurrentStack { get { return _shieldCurrentStack; } set { _shieldCurrentStack = value; } }
    static public int shieldMaxStack { get { return _shieldMaxStack; } set { _shieldMaxStack = value; } }
    static public bool isShielded { get { return _isShielded; } set { _isShielded = value; } }

    //Cooldowns
    public static float slashCooldown { get { return cooldowns._slashCooldown; } set { cooldowns._slashCooldown = value; } }
    public static float xslashCooldown { get { return cooldowns._xslashCooldown; } set { cooldowns._xslashCooldown = value; } }
    public static float shieldCooldown { get { return cooldowns._shieldCooldown; } set { cooldowns._shieldCooldown = value; } }
    public static float shieldTimer { get { return cooldowns._shieldTimer; } set { cooldowns._shieldTimer = value; } }
    public static float rockCooldown { get { return cooldowns._rockCooldown; } set { cooldowns._rockCooldown = value; } }
    public static float boxCooldown { get { return cooldowns._boxCooldown; } set { cooldowns._boxCooldown = value; } }

    public static int getNeededExp()
    {
        int result = (level * 100) + 50;

        return result;
    }

    public static void resetCooldowns()
    {
        //Slash
        if (cooldowns._slashCooldown > 0)
            cooldowns._slashCooldown -= Time.deltaTime;
        else
            cooldowns._slashCooldown = 0;

        //XSlash
        if (cooldowns._xslashCooldown > 0)
            cooldowns._xslashCooldown -= Time.deltaTime;
        else
            cooldowns._xslashCooldown = 0;

        //Shield
        if (cooldowns._shieldCooldown > 0 && _shieldCurrentStack < _shieldMaxStack)
        {
            cooldowns._shieldCooldown -= Time.deltaTime;
        }
        else if(_shieldCurrentStack < _shieldMaxStack && cooldowns._shieldCooldown <= 0)
        {
            _shieldCurrentStack++;
            cooldowns._shieldCooldown = shieldCooldownDefault;
        }
        else if (_shieldCurrentStack >= _shieldMaxStack)
        {
            cooldowns._shieldCooldown = 0;
        }

        //Rock spawn
        if (cooldowns._rockCooldown > 0)
            cooldowns._rockCooldown -= Time.deltaTime;
        else
            cooldowns._rockCooldown = 0;

        //Box spawn
        if (cooldowns._boxCooldown > 0)
            cooldowns._boxCooldown -= Time.deltaTime;
        else
            cooldowns._boxCooldown = 0;

        //ShieldTimer
        if (cooldowns._shieldTimer > 0)
        {
            _isShielded = true;
            cooldowns._shieldTimer -= Time.deltaTime;
        }
        else
        {
            _isShielded = false;
            cooldowns._shieldTimer = 0;
        }

    }

    public static void playerDied()
    {
        //Handle the player death
        //Variables||

            //players body
            Rigidbody2D playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();

            //dungeon data
            CurrentDungeonData dungeonInfo = GameObject.FindGameObjectWithTag("proceduralData").GetComponent<CurrentDungeonData>();

            //The interface
            GameObject mainInterface = GameObject.FindGameObjectWithTag("mainUI");
        //_________||

        //Freeze the player movement
        playerRB.constraints = RigidbodyConstraints2D.FreezeAll;

        //Play the death animation

        //Respawn the player
        void doLast()
        {
            //Make him able to move itself again
            playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;

            //Reset his cooldowns
            cooldowns._shieldCooldown = 0;
            cooldowns._slashCooldown = 0;
            cooldowns._xslashCooldown = 0;
            cooldowns._shieldTimer = 0;

            //Reset the room||

                //Find the current room instance
                string roomName = dungeonInfo.getRoomViaCords(dungeonInfo.currentRoom).roomPrefab.name + dungeonInfo.currentRoom.x + dungeonInfo.currentRoom.y;
                GameObject roomInstance = GameObject.Find(roomName);
                
                //If it finds the instance then respawn it
                if (roomInstance && !dungeonInfo.getRoomViaCords(dungeonInfo.currentRoom).getCompleted())
                {
                    //Get the prefab version
                    GameObject prefabVersion = dungeonInfo.getRoomViaCords(dungeonInfo.currentRoom).roomPrefab;

                    //Disable the current one
                    roomInstance.SetActive(false);

                    //New room
                    GameObject newVersion = Instantiate(roomInstance);

                    List<GameObject> childsReference = new List<GameObject>();

                    //Recreate the childs
                    foreach(Transform child in newVersion.transform)
                    {
                        foreach (Transform child2 in roomInstance.transform)
                        {
                            if (child.name == child2.name)
                            {
                                GameObject newElement = Instantiate(child.gameObject);
                                Destroy(child2.gameObject);
                                newElement.name = child2.name;
                                childsReference.Add(newElement);
                            }
                        }
                    }

                    //Finish the proccess
                    Destroy(newVersion);

                    foreach (GameObject childRef in childsReference)
                    {
                        childRef.transform.SetParent(roomInstance.transform);
                    }

                    roomInstance.SetActive(true);

                }
            //______________||


            void teleportPlayer(string doorName)
            {
                //Get the current room uwu
                GameObject roomYoureIn = dungeonInfo.getRoomViaCords(dungeonInfo.currentRoom).roomPrefab;
                Vector2 teleportPosition = roomYoureIn.transform.Find("TeleportLocations").Find(doorName).localPosition;

                playerRB.position = teleportPosition;
            }

            //Teleport the player
            if (dungeonInfo.nextRoomSide == "")
                teleportPlayer("exit");
            else
                teleportPlayer(dungeonInfo.nextRoomSide);

            //Add do the death counters
            dungeonInfo.amountOfDeaths += 1;

            //Reset his HP
            _healthPoints = _maxHealthPoints;

            //Reset the camera
            Camera.main.transform.position = new Vector3( playerRB.position.x, playerRB.position.y, Camera.main.transform.position.z);
        }

        //Make the screen go dark
        instance.StartCoroutine(screenFade(mainInterface.transform.Find("ShadowOverlay").gameObject, doLast));



    }

    public static IEnumerator screenFade(GameObject darkOverlay, Action dolast)
    {

        float speed = .1f;

        void addTransparency(float transparency)
        {
            //Get the default color of the image
            Color defaultColor = darkOverlay.GetComponent<Image>().color;

            darkOverlay.GetComponent<Image>().color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, defaultColor.a += transparency);
        }

        //Fade in
        while (darkOverlay.GetComponent<Image>().color.a < 1)
        {
            addTransparency(speed);
            yield return new WaitForSeconds(.1f);
        }

        //Yield for a bit to end the death animation
        yield return new WaitForSeconds(.5f);

        //Play the function after the death loading
        dolast();

        //Fade in
        while (darkOverlay.GetComponent<Image>().color.a > 0)
        {
            addTransparency(-speed);
            yield return new WaitForSeconds(.1f);
        }
    }

    public static void damagePlayer(int damage)
    {

        if (healthPoints >= damage)
        {
            if (isShielded == true)
            {
                healthPoints -= (damage - (damage * shieldDamageReduction / 100));
                shieldTimer = 0f;
                isShielded = false;
            }
            else
            {
                healthPoints -= damage;
            }
        }
        else
        {
            healthPoints = 0;
        }

        if (healthPoints == 0)
        {
            playerDied();
        }
    }

    public static void addPlayerExp(int enemyExp)
    {
        int missingExp = getNeededExp() - PlayerData.exp;
        if (enemyExp >= missingExp)
        {
            int restExpToAddNextLevel = enemyExp  - missingExp;
            PlayerData.level++;
            PlayerData.exp = restExpToAddNextLevel;
        }
        else
        {
            PlayerData.exp = PlayerData.exp + enemyExp;
        }
    }

    static public accountInfoResponse.nestedData savePlayerData()
    {
        accountInfoResponse.nestedData data = new accountInfoResponse.nestedData();

        data.stats = new accountInfoResponse.nestedData.nested2Data();

        //Set the data to be sent
        data._id = _id;
        data.musicVolume = _musicVolume;
        data.sfxVolume = _sfxVolume;
        data.windowMode = _windowmode;
        data.stats.level = _level;
        data.stats.exp = _exp;
        data.stats.gold = _gold;
        data.stats.ressources = _resources;
        data.talentTree = _talentTreeData;

        return data;
    }

    public static IEnumerator savePlayerDataRequest(accountInfoResponse.nestedData data, Action doLast)
    {
        Debug.Log("Trying to save player data");

        //Set the cookie
        WebServices.CookieString = null;

        var request = WebServices.Post("save/fullSave", JsonUtility.ToJson(data));

        //Make the code wait until the server responds
        yield return request.SendWebRequest();

        if (request.isNetworkError)
        {
            Debug.Log("Network Error");
        }
        else
        {
            //Get the header for proccessing
            WebServices.CookieString = request.GetResponseHeader("set-cookie");

            //The server response
            string result = request.downloadHandler.text;

            doLast();

            Debug.Log("Completed");
        }
    }



    static public void SetPlayerData(accountInfoResponse json)
    {
        if (json.body._id != null){_id = json.body._id;}
        if (json.body.name != null){ _username = json.body.name;}
        if (json.body.email != null) { _email = json.body.email; }

        if (json.body.stats.level < 1)
        {
            _level = 1;
        }
        else
        {
            _level = json.body.stats.level;
        }

        //Setup the talent tree uwu
        if(json.body.talentTree != null)
        {
            _talentTreeData = json.body.talentTree;
        }

        //Set the options
        _windowmode = json.body.windowMode;
        _musicVolume = json.body.musicVolume;
        _sfxVolume = json.body.sfxVolume;

        //Set the correct window mode
        Screen.fullScreen = _windowmode;

        _exp = json.body.stats.exp;
        _resources = json.body.stats.ressources;
        _gold = json.body.stats.gold;

        //Calculate the max health
        _maxHealthPoints = 100;

        //Reset the player hp
        _healthPoints = _maxHealthPoints;
    }

    static public void ResetPlayerData()
    {
        _id = null;
        _username = "Offline Player";
        _level = 1;
        _exp = 0;
        _resources = 0;
        _gold = 0;
        _email = "";
        _movementSpeed = 4;
        _healthPoints = 100;
        _maxHealthPoints = 100;
        _talentTreeData = new talentTreeClass();
    }

}
