﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIevents : MonoBehaviour
{
    //Variables||

        //room
        public Sprite DeadMinesRoomSprite;
        public Sprite FrostCaveRoomSprite;
        public GameObject roomPrefab;
        private Vector2Int currentRoom;

        [System.Serializable]
        public class healthbarClass
        {
            public GameObject fill;
            public GameObject text;
        }

        [System.Serializable]
        public class expbarClass
        {
            public GameObject fill;
            public GameObject text;
        }

        [System.Serializable]
        public class skillsCooldowns
        {
            public GameObject slashSkill;
            public GameObject XslashSkill;
            public GameObject shieldSkill;
            public GameObject rockSkill;
            public GameObject boxSkill;
        }

        [System.Serializable]
        public class gameMenuClass
        {
            public GameObject mainWindow;
            public GameObject resume;
            public GameObject logout;
            public GameObject options;
            public GameObject exit;
        }

        //username
        public GameObject usernameDisplay;

        //game menu
        public gameMenuClass mainMenu;
        private bool escapeKeyDebounce = false;
        public KeyCode mainMenuKey = KeyCode.Escape;

        //Skills cooldowns
        public skillsCooldowns cooldowns;

        //Bars
        public healthbarClass healthbar;
        public expbarClass expBar;

        public GameObject levelText;

        //The profiles window
        [System.Serializable]
        public class profilesClass
        {
            public GameObject profilesDropDown;
            public GameObject profilesWindow;
            public GameObject profileName;
        }

        public profilesClass profiles = new profilesClass();

        //Minimap||

            [System.Serializable]
            public class miniMapClass
            {
                public GameObject window;
                public GameObject minimapComponent;
                public GameObject minimapSlider;
                public KeyCode minimapKey = KeyCode.M;
            }

            public miniMapClass minimap = new miniMapClass();

            [System.Serializable]
            public class villageMinimapClass{
                public GameObject camera;
                public GameObject slider;
                public GameObject mapWindow;
            }

            public villageMinimapClass villageMinimap = new villageMinimapClass();

            private Vector2 defaultMinimapPosition;
            private Vector2 defaultMinimapScale;
            private bool minimapOpened = false;
            private bool alreadyChangeMinimap = true;
        //_______||

        //Options||

            [System.Serializable]
            public class OptionsClass
            {
                public GameObject optionsWindow;
                public GameObject dataText;
                public GameObject musicSlider;
                public GameObject sfxSlider;
                public GameObject dropdownWindow;
            }

            public OptionsClass options = new OptionsClass();
        //_______||


        //Talent tree||

            [System.Serializable]
            public class talentTreeClass
            {
                public GameObject talentTreeWindow;
                [System.Serializable]
                public class nodesStruct
                {
                    public GameObject node0;
                    public GameObject node1;
                    public GameObject node2;
                    public GameObject node3;
                    public GameObject node4;
                    public GameObject node5;
                    public GameObject node6;
                    public GameObject node7;
                    public GameObject node8;
                    public GameObject node9;
                    public GameObject node10;
                }

                [System.Serializable]
                public class displayWindowClass
                {
                    public GameObject nodeImage;
                    public GameObject nodeTittle;
                    public GameObject nodeDesc;
                    public GameObject nodeButton;
                    public GameObject resourcesHolder;
                }

                public nodesStruct nodes = new nodesStruct();
                public displayWindowClass displayWindow = new displayWindowClass();
                public GameObject confirmationWindow;
            }

            public talentTreeClass talentTreeData = new talentTreeClass();
            private string currentNode = "";
            private GameObject currentNodeObject;
        //___________||

        //Loading a network instance window
        public GameObject loadingNetworkPrefab;

        [Header("Leave dungeon")]
        public GameObject confirmationWindowDungeon;
    //_________||

    //Sound||

        UIsfx soundEffect;
    //_____||
    private void Awake()
    {
        switch (sceneTeleport.dungeonName)
        {
            case "DeadMines":
                {
                    roomPrefab.GetComponent<Image>().sprite = DeadMinesRoomSprite;
                    break;
                }
            case "FrostCave":
                {
                    roomPrefab.GetComponent<Image>().sprite = FrostCaveRoomSprite;
                    break;
                }
        }
    }

    private void Start()
    {
        //Add the audio instance
        soundEffect = gameObject.GetComponent<UIsfx>();

        //Add the listeners for the talent tree
        talentTreeData.nodes.node0.GetComponent<Button>().onClick.AddListener(delegate{ selectNode(0, talentTreeData.nodes.node0);});
        talentTreeData.nodes.node1.GetComponent<Button>().onClick.AddListener(delegate { selectNode(1, talentTreeData.nodes.node1); });
        talentTreeData.nodes.node2.GetComponent<Button>().onClick.AddListener(delegate { selectNode(2, talentTreeData.nodes.node2); });
        talentTreeData.nodes.node3.GetComponent<Button>().onClick.AddListener(delegate { selectNode(3, talentTreeData.nodes.node3); });
        talentTreeData.nodes.node4.GetComponent<Button>().onClick.AddListener(delegate { selectNode(4, talentTreeData.nodes.node4); });
        talentTreeData.nodes.node5.GetComponent<Button>().onClick.AddListener(delegate { selectNode(5, talentTreeData.nodes.node5); });
        talentTreeData.nodes.node6.GetComponent<Button>().onClick.AddListener(delegate { selectNode(6, talentTreeData.nodes.node6); });
        talentTreeData.nodes.node7.GetComponent<Button>().onClick.AddListener(delegate { selectNode(7, talentTreeData.nodes.node7); });
        talentTreeData.nodes.node8.GetComponent<Button>().onClick.AddListener(delegate { selectNode(8, talentTreeData.nodes.node8); });
        talentTreeData.nodes.node9.GetComponent<Button>().onClick.AddListener(delegate { selectNode(9, talentTreeData.nodes.node9); });
        talentTreeData.nodes.node10.GetComponent<Button>().onClick.AddListener(delegate { selectNode(10, talentTreeData.nodes.node10); });

        //Choose the colors for the talent tree nodes
        updateTalentTree();

        //Adapt to the correct map zoom

        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            changeMinimapZoon();
            currentRoom = GameObject.FindGameObjectWithTag("proceduralData").GetComponent<CurrentDungeonData>().currentRoom;
        }


        //Minimap
        if (minimap != null)
        {
            //Get the default position
            defaultMinimapPosition = minimap.window.GetComponent<RectTransform>().localPosition;
            defaultMinimapScale = minimap.window.GetComponent<RectTransform>().localScale;
        }

        //Change your name
        usernameDisplay.GetComponent<Text>().text = PlayerData.username;

        //Options info
        options.musicSlider.GetComponent<Slider>().value = PlayerData.musicVolume;
        options.sfxSlider.GetComponent<Slider>().value = PlayerData.sfxVolume;

        //Set the audio
        Camera.main.transform.Find("MainMusic").GetComponent<AudioSource>().volume = PlayerData.musicVolume;
        Camera.main.transform.Find("SFX").GetComponent<AudioSource>().volume = PlayerData.sfxVolume;
        Camera.main.transform.Find("UISFX").GetComponent<AudioSource>().volume = PlayerData.sfxVolume;
        Camera.main.transform.Find("PlayerSFX").GetComponent<AudioSource>().volume = PlayerData.sfxVolume;

        if (PlayerData.windowmode)
            options.dropdownWindow.GetComponent<DuloGames.UI.UISelectField>().SelectOption("Fullscreen");
        else
            options.dropdownWindow.GetComponent<DuloGames.UI.UISelectField>().SelectOption("Windowed");
        
        setupProfilesWindow();
        updateOptionsWindowInfo();
    }

    //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    // Method: User level up
    /////
    // Desc: Called when the user levels up in order to display the current info
    //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    public void userLevelUp(){

        gameObject.transform.Find("NotificationLevel").Find("Level Cont").Find("Level Group").GetComponent<HorizontalLayoutGroup>().enabled = true;
        gameObject.transform.Find("NotificationLevel").Find("Level Cont").Find("Level Group").GetComponent<HorizontalLayoutGroup>().enabled = false;
        soundEffect.levelUP();                                  //Plays the level up sound effect
        this.StartCoroutine(levelUPWindow(PlayerData.level));   //Play the level up animation
    }

    IEnumerator levelUPWindow(int oldLevel){

        //Variables||
        
            float fadeSpeed = .05f; //The speed on which the window fades in and out
            int amountOfBounces = 5;
            CanvasGroup mainWindow = gameObject.transform.Find("NotificationLevel").GetComponent<CanvasGroup>();
            GameObject levelNUM = gameObject.transform.Find("NotificationLevel").Find("Level Cont").Find("Level Group").Find("Level Text").gameObject;
            float storedNumberPos = levelNUM.transform.localPosition.x;
        //_________||

        levelNUM.GetComponent<Text>().text = oldLevel.ToString();        //Change the current level number of the window

        //Fade the window in
        while (mainWindow.alpha < 1)                //Check if wheater the window is or is not fully transparent
        {
            mainWindow.alpha += fadeSpeed;          //Remove transparency from the window
            yield return new WaitForSeconds(.05f);   //Yield for a certain amount of time
        }


        //Push the number up
        while(levelNUM.transform.localPosition.y < 30){

            levelNUM.transform.localPosition = new Vector2(levelNUM.transform.localPosition.x, levelNUM.transform.localPosition.y + 3.2f);
            yield return new WaitForSeconds(.05f); 
        }

        int bounceCounter = 0;
        while(bounceCounter < amountOfBounces){

            //Move to the right
            while(levelNUM.transform.localPosition.x - storedNumberPos < 6){
                levelNUM.transform.localPosition = new Vector2(levelNUM.transform.localPosition.x + 2.75f, levelNUM.transform.localPosition.y);
                yield return new WaitForSeconds(.025f);
            }

            //Move to the left
            while (levelNUM.transform.localPosition.x - storedNumberPos > 0)
            {
                levelNUM.transform.localPosition = new Vector2(levelNUM.transform.localPosition.x - 2.75f, levelNUM.transform.localPosition.y);
                yield return new WaitForSeconds(.025f);
            }

            bounceCounter++;
        }

        levelNUM.transform.localPosition = new Vector2(storedNumberPos, levelNUM.transform.localPosition.y);

        yield return new WaitForSeconds(.2f);   //Maybe add like a shake here

        //Push the number down
        while (levelNUM.transform.localPosition.y > 0)
        {

            levelNUM.transform.localPosition = new Vector2(levelNUM.transform.localPosition.x, levelNUM.transform.localPosition.y - 12.8f);
            yield return new WaitForSeconds(.05f);
        }

        levelNUM.transform.localPosition = new Vector2(levelNUM.transform.localPosition.x, 0);

        soundEffect.bling();

        levelNUM.GetComponent<Text>().text = (oldLevel + 1).ToString();

        yield return new WaitForSeconds(2);         //For how long the window should be displayed for the player


        //Fade the window out
        while (mainWindow.alpha > 0)                //Check if wheater the window still has some transparency
        {
            mainWindow.alpha -= fadeSpeed;          //Remove transparency from the window
            yield return new WaitForSeconds(.05f);   //Yield for a certain amount of time
        }


        yield return null;
    }

    public void changeVillageZoom(){
        float defaultValue = 40f;
        float currentValue = villageMinimap.slider.GetComponent<Slider>().value * 2f;
        villageMinimap.camera.GetComponent<Camera>().orthographicSize = defaultValue - (currentValue * 3f);
    }

    private void setupProfilesWindow()
    {
        //Variables||

            string default1Name = "Default 1";
            string default2Name = "Default 2";
        //_________||

        //Get the main componenet
        DuloGames.UI.UISelectField profileDropdown = profiles.profilesDropDown.GetComponent<DuloGames.UI.UISelectField>();

        //Empty the options
        profileDropdown.ClearOptions();

        //Add the empty profiles
        profileDropdown.AddOption(default1Name);
        profileDropdown.AddOption(default2Name);

        //Add the items to the dropdown
        foreach (accountInfoResponse.profilesData profile in PlayerData.playerProfiles)
        {
            profileDropdown.AddOption(profile.profile.name);
        }

        //Select the correct one
        switch (PlayerData.currentProfile)
        {
            case 0:
                profiles.profileName.GetComponent<Text>().text = default1Name;
                profileDropdown.SelectOption(default1Name);
                break;

            case 1:
                profiles.profileName.GetComponent<Text>().text = default2Name;
                profileDropdown.SelectOption(default2Name);
                break;
            default:
                profiles.profileName.GetComponent<Text>().text = PlayerData.playerProfiles[PlayerData.currentProfile - 2].profile.name;
                profileDropdown.SelectOption(PlayerData.playerProfiles[PlayerData.currentProfile - 2].profile.name);
                    break;
        }

    }

    public void profilesListener(int indexValue, string profileName)
    {


        PlayerData.currentProfile = indexValue;
        profiles.profileName.GetComponent<Text>().text = profileName;
        GameObject.FindGameObjectWithTag("Player").GetComponent<ArmorChange>().changeDefaultArmor();
        GameObject.FindGameObjectWithTag("Player").GetComponent<ArmorChange>().changeArmorDrawing();
    }

    public void closeVillageMap(){
        soundEffect.playClick();
        villageMinimap.mapWindow.SetActive(!villageMinimap.mapWindow.activeSelf);
    }

    private void Update()
    {

        if (profiles.profilesWindow.activeSelf)
        {
            Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.localPosition;
            //Set the player camera
            GameObject.FindGameObjectWithTag("UIcamera").transform.localPosition = new Vector3(playerPosition.x, playerPosition.y, -10);
        }

        //Listen to the keyboard keys||
            
            //minimap
            if (Input.GetKeyDown(minimap.minimapKey) && !GameObject.Find("DungeonData")){

                

                if (villageMinimap.mapWindow != null){ 
                    soundEffect.playClick();                                //Play click sound effect
                    if(!villageMinimap.mapWindow.activeSelf){
                        profiles.profilesWindow.SetActive(false);           //Close the profiles window
                        talentTreeData.talentTreeWindow.SetActive(false);   //Close the talent tree
                        options.optionsWindow.SetActive(false);             //Close the options
                    }
                    villageMinimap.mapWindow.SetActive(!villageMinimap.mapWindow.activeSelf);
                }

                
                if (!minimapOpened) { alreadyChangeMinimap = false; } minimapOpened = true; 
            }
            if (Input.GetKeyUp(minimap.minimapKey)) minimapOpened = alreadyChangeMinimap = false;
            
            //main menu
            if (Input.GetKeyDown(mainMenuKey)) { if (!escapeKeyDebounce) { escapeKeyDebounce = true; if (!mainMenu.mainWindow.activeSelf) { Time.timeScale = 0; } else { Time.timeScale = 1; options.optionsWindow.SetActive(false); } mainMenu.mainWindow.SetActive(!mainMenu.mainWindow.activeSelf); } }
            if (Input.GetKeyUp(mainMenuKey)) { if (escapeKeyDebounce) { escapeKeyDebounce = false; } }

            //Open UI windows
            if(Input.GetKeyDown(KeyCode.P)){ closeProfilesWindow(); }   //Profiles window
            if(Input.GetKeyDown(KeyCode.O)){ OnOptions(); }             //Options window
            if(Input.GetKeyDown(KeyCode.I)){ openTalentTree(); }        //Talent tree window
        //___________________________||

        //Minimap||
            if (!alreadyChangeMinimap)
            {
                if (!minimapOpened)
                {
                    //It was closed
                    minimapOpened = false;
                    //Reverse the variables
                    minimap.window.GetComponent<RectTransform>().localScale = defaultMinimapScale;
                    minimap.window.GetComponent<RectTransform>().localPosition = defaultMinimapPosition;
                }
                else
                {
                    //It was opened
                    minimapOpened = true;
                    minimap.window.GetComponent<RectTransform>().localScale = new Vector2(2.5f, 1.8f);
                    minimap.window.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
                }
                alreadyChangeMinimap = true;
            }
        //_______||

        if (options.optionsWindow.activeSelf)
        {
            //If the options window is open then update it at real time
            updateOptionsWindowInfo();
        }

        updateUiElements();

    }

    //Options window||

        public void updateOptionsWindowInfo()
        {
            options.dataText.GetComponent<Text>().text = 
                "\n ID: " + PlayerData.id +
                "\n Email: " + PlayerData.email +
                "\n Name: " + PlayerData.username +
                "\n Level: " + PlayerData.level + 
                "\n Exp: " + PlayerData.exp + "/" + PlayerData.getNeededExp() + 
                "\n Gold: " + PlayerData.gold +
                "\n Resources: " + PlayerData.resources;
        }

        public void musicChanged()
        {
            PlayerData.musicVolume = options.musicSlider.GetComponent<Slider>().value;
            Camera.main.transform.Find("MainMusic").GetComponent<AudioSource>().volume = PlayerData.musicVolume;
        }

        public void sfxChanged()
        {
            PlayerData.sfxVolume = options.sfxSlider.GetComponent<Slider>().value;
            Camera.main.transform.Find("SFX").GetComponent<AudioSource>().volume = PlayerData.sfxVolume;
            Camera.main.transform.Find("UISFX").GetComponent<AudioSource>().volume = PlayerData.sfxVolume;
            Camera.main.transform.Find("PlayerSFX").GetComponent<AudioSource>().volume = PlayerData.sfxVolume;
        }

        public void dropDownChanged(int valeu, string option)
        {
            if (option == "Fullscreen")
            {
                PlayerData.windowmode = true;
                Screen.fullScreen = true;
            }
            else if (option == "Windowed")
            {
                PlayerData.windowmode = false;
                Screen.fullScreen = false;
            }
        }

        public void OnOptions()
        {
            soundEffect.playClick();    //Clicking sound effect
            profiles.profilesWindow.SetActive(false);           //Close the profiles window
            talentTreeData.talentTreeWindow.SetActive(false);   //Close the talent tree
            villageMinimap.mapWindow.SetActive(false);          //Close the map
            //Just enable the options menu
            options.optionsWindow.SetActive(!options.optionsWindow.activeSelf);
        }
    //______________||


    private void updateUiElements()
    {
        //Healthbar
        float healthBarFillAmount = ((float)PlayerData.healthPoints/(float)PlayerData.maxHealthPoints);
        healthbar.fill.GetComponent<Image>().fillAmount = healthBarFillAmount;
        healthbar.text.GetComponent<Text>().text = PlayerData.healthPoints + "/" + PlayerData.maxHealthPoints + " HP";

        //Exp bar
        float expBarFillAmount = ((float)PlayerData.exp/(float)PlayerData.getNeededExp());
        expBar.fill.GetComponent<Image>().fillAmount = expBarFillAmount;

        expBar.text.GetComponent<Text>().text = PlayerData.exp + "/" + PlayerData.getNeededExp();

        //level text
        levelText.GetComponent<Text>().text = PlayerData.level.ToString();

        //Skills cooldowns||

        //slash
        string slashSkillText = "";
            if (PlayerData.slashCooldown > 0 && PlayerData.slashCooldown < 1)
            {
                slashSkillText = slashSkillText + decimal.Round((decimal)PlayerData.slashCooldown, 1);
            }else if(PlayerData.slashCooldown > 0){
                slashSkillText = slashSkillText +  Mathf.RoundToInt(PlayerData.slashCooldown);
            }

            cooldowns.slashSkill.transform.Find("Cooldown").GetComponent<Image>().fillAmount = (PlayerData.slashCooldown/PlayerData.slashCooldownDefault);
            cooldowns.slashSkill.transform.Find("CooldownText").GetComponent<Text>().text = slashSkillText;

            //xslash
            string xslashSkillText = "";
            if (PlayerData.xslashCooldown > 0 && PlayerData.xslashCooldown < 1)
            {
                xslashSkillText = xslashSkillText + decimal.Round((decimal)PlayerData.xslashCooldown, 1);
            }else if(PlayerData.xslashCooldown > 0){
                xslashSkillText = xslashSkillText +  Mathf.RoundToInt(PlayerData.xslashCooldown);
            }
            cooldowns.XslashSkill.transform.Find("Cooldown").GetComponent<Image>().fillAmount = (PlayerData.xslashCooldown / PlayerData.xslashCooldownDefault);
            cooldowns.XslashSkill.transform.Find("CooldownText").GetComponent<Text>().text = xslashSkillText;

            //shield
            string shieldSkillText = "";
            if(PlayerData.shieldCurrentStack < 1) {
                if (PlayerData.shieldCooldown > 0 && PlayerData.shieldCooldown < 1)
                {
                    shieldSkillText = shieldSkillText + decimal.Round((decimal)PlayerData.shieldCooldown, 1);
                }else if(PlayerData.shieldCooldown > 0){
                    shieldSkillText = shieldSkillText +  Mathf.RoundToInt(PlayerData.shieldCooldown);
                }

                cooldowns.shieldSkill.transform.Find("Cooldown").GetComponent<Image>().color = new Color(0f,0f,0f,0.7f);
                cooldowns.shieldSkill.transform.Find("Cooldown").GetComponent<Image>().fillAmount = (PlayerData.shieldCooldown / PlayerData.shieldCooldownDefault);
                
                if (PlayerData.talentTreeData.node3 == true)
                {
                    cooldowns.shieldSkill.transform.Find("StackText").GetComponent<Text>().text = "";
                }
            }
            else
            {
                cooldowns.shieldSkill.transform.Find("Cooldown").GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.25f);
                cooldowns.shieldSkill.transform.Find("Cooldown").GetComponent<Image>().fillAmount = (PlayerData.shieldCooldown / PlayerData.shieldCooldownDefault);
                
                if (PlayerData.talentTreeData.node3 == true)
                {
                    cooldowns.shieldSkill.transform.Find("StackText").GetComponent<Text>().text = PlayerData.shieldCurrentStack.ToString();
                }
            }

            cooldowns.shieldSkill.transform.Find("CooldownText").GetComponent<Text>().text = shieldSkillText;

            //rock spawner
            string rockSkillText = "";
            if (PlayerData.rockCooldown > 0 && PlayerData.rockCooldown < 1)
            {
                rockSkillText = rockSkillText + decimal.Round((decimal)PlayerData.rockCooldown, 1);
            }else if(PlayerData.rockCooldown > 0){
                rockSkillText = rockSkillText +  Mathf.RoundToInt(PlayerData.rockCooldown);
            }

            cooldowns.rockSkill.transform.Find("Cooldown").GetComponent<Image>().fillAmount = (PlayerData.rockCooldown / PlayerData.rockSpawnCooldownDefault);
            cooldowns.rockSkill.transform.Find("CooldownText").GetComponent<Text>().text = rockSkillText;
            
            //box spawner
            string boxSkillText = "";
            if (PlayerData.boxCooldown > 0 && PlayerData.boxCooldown < 1)
            {
                boxSkillText = boxSkillText + decimal.Round((decimal)PlayerData.boxCooldown, 1);
            }else if(PlayerData.boxCooldown > 0){
                boxSkillText = boxSkillText +  Mathf.RoundToInt(PlayerData.boxCooldown);
            }

            cooldowns.boxSkill.transform.Find("Cooldown").GetComponent<Image>().fillAmount = (PlayerData.boxCooldown / PlayerData.boxSpawnCooldownDefault);
            cooldowns.boxSkill.transform.Find("CooldownText").GetComponent<Text>().text = boxSkillText;

        //________________||



    }

    public void changeMinimapZoon()
    {
        //Get the slider value
        float value = minimap.minimapSlider.GetComponent<Slider>().value;

        //Resize the minimap
        minimap.minimapComponent.GetComponent<RectTransform>().localScale = new Vector2(value, value);

        //Get the room size
        int roomSize = (int)roomPrefab.GetComponent<RectTransform>().sizeDelta.x;

        currentRoom = GameObject.FindGameObjectWithTag("proceduralData").GetComponent<CurrentDungeonData>().currentRoom;

        //Change to the correct position
        minimap.minimapComponent.transform.localPosition =
        new Vector2(-currentRoom.x * (roomSize * minimap.minimapComponent.transform.localScale.x),
        -currentRoom.y * (roomSize * minimap.minimapComponent.transform.localScale.y));

    }

    public void OnResume()
    {
        soundEffect.playClick();    //Clicking sound effect

        //Resum the game by closing the window

        //Set the time back to normal
        Time.timeScale = 1;

        //Close the main menu window
        mainMenu.mainWindow.SetActive(false);
    }

    public void OnLogout()
    {
        soundEffect.playClick();    //Clicking sound effect


        //Save data
        if (PlayerData.id != null)
        {

            //Spawn the network loading screen
            GameObject loadingScreen = Instantiate(loadingNetworkPrefab);
            loadingScreen.transform.SetParent(gameObject.transform);
            loadingScreen.transform.localPosition = loadingNetworkPrefab.transform.localPosition;
            loadingScreen.transform.localScale = new Vector3(1, 1, 1);
            loadingScreen.name = loadingNetworkPrefab.name;

            void doLast()
            {
                //Remove the loading screen
                Destroy(loadingScreen);

                //Remove the local data of the player
                PlayerData.ResetPlayerData();

                //Close the main menu
                mainMenu.mainWindow.SetActive(false);

                //Reset the time scale of the game
                Time.timeScale = 1;

                //Move to the main menu
                sceneTeleport.start(0);
            }

            accountInfoResponse.nestedData tosendData = PlayerData.savePlayerData();
            //Open the options panel
            StartCoroutine(PlayerData.savePlayerDataRequest(tosendData, doLast));
        }
        else
        {
            PlayerData.ResetPlayerData();
            mainMenu.mainWindow.SetActive(false);
            Time.timeScale = 1;
            sceneTeleport.start(0);
        }
    }

    public void OnExit()
    {
        soundEffect.playClick();    //Clicking sound effect

        //Save data
        if (PlayerData.id != null)
        {

            //Spawn the network loading screen
            GameObject loadingScreen = Instantiate(loadingNetworkPrefab);
            loadingScreen.transform.SetParent(gameObject.transform);
            loadingScreen.transform.localPosition = loadingNetworkPrefab.transform.localPosition;
            loadingScreen.transform.localScale = new Vector3(1, 1, 1);
            loadingScreen.name = loadingNetworkPrefab.name;

            void doLast()
            {
                //Remove the loading screen
                Destroy(loadingScreen);

                //Make it save before closing
                if (Application.isEditor)
                {
                    //UnityEditor.EditorApplication.isPlaying = false; //Show the quit on the editor as well
                }

                Application.Quit();
            }

            accountInfoResponse.nestedData tosendData = PlayerData.savePlayerData();
            //Open the options panel
            StartCoroutine(PlayerData.savePlayerDataRequest(tosendData, doLast));
        }
        else
        {
            //Make it save before closing
            if (Application.isEditor)
            {
                //UnityEditor.EditorApplication.isPlaying = false; //Show the quit on the editor as well
            }

            Application.Quit();
        }


    }

    //Talent tree related methods||

        public void openTalentTree()
        {
            soundEffect.playClick();    //Clicking sound effect
            profiles.profilesWindow.SetActive(false);           //Close the profiles window
            options.optionsWindow.SetActive(false);             //Close the options
            villageMinimap.mapWindow.SetActive(false);          //Close the map
            talentTreeData.talentTreeWindow.SetActive(!talentTreeData.talentTreeWindow.activeSelf);
        }

        public void updateTalentTree()
        {
            //Update the resources text
            talentTreeData.displayWindow.resourcesHolder.transform.Find("Label").GetComponent<Text>().text = NumAbv.prettyValues(PlayerData.resources);

            for(int i = 0; i < 11; i++)
            {
                //Get the info about this particular node
                (bool, bool) nodeInfo = getNodeInfo(i);

                //Get the correct color
                Color imageColor = new Color(1, 1, 1, 0);
                //Outter Ring color
                Color CircleColor = new Color(1, 1, 1, 1);
                if (nodeInfo.Item1)
                {
                    CircleColor = new Color(0.96f, 0.13f, 0.06f, 1);
                    imageColor = new Color32(1, 1, 1, 0);
                }else if (nodeInfo.Item2)
                {
                    // 54f / 255f, 243 / 255f, 0 / 255f, 40f / 255f
                    CircleColor = new Color(1, 1, 1, 1);
                    imageColor = new Color(0, 0, 0, 0.6f);
                }
                else
                {
                    CircleColor = new Color(1, 1, 1, 1);
                    imageColor = new Color(0, 0, 0,  0.9f);
                }

            switch (i)
                {
                    case 0:
                        talentTreeData.nodes.node0.GetComponent<Image>().color = CircleColor;
                        talentTreeData.nodes.node0.transform.Find("Overlay").GetComponent<Image>().color = imageColor;
                        break;
                    case 1:
                        talentTreeData.nodes.node1.GetComponent<Image>().color = CircleColor;
                        talentTreeData.nodes.node1.transform.Find("Overlay").GetComponent<Image>().color = imageColor;
                        break;

                    case 2:
                        talentTreeData.nodes.node2.GetComponent<Image>().color = CircleColor;
                        talentTreeData.nodes.node2.transform.Find("Overlay").GetComponent<Image>().color = imageColor;
                        break;
                    case 3:
                    talentTreeData.nodes.node3.GetComponent<Image>().color = CircleColor;
                    talentTreeData.nodes.node3.transform.Find("Overlay").GetComponent<Image>().color = imageColor;
                        break;
                    case 4:
                        talentTreeData.nodes.node4.GetComponent<Image>().color = CircleColor;
                        talentTreeData.nodes.node4.transform.Find("Overlay").GetComponent<Image>().color = imageColor;
                        break;
                    case 5:
                        talentTreeData.nodes.node5.GetComponent<Image>().color = CircleColor;
                        talentTreeData.nodes.node5.transform.Find("Overlay").GetComponent<Image>().color = imageColor;
                        break;
                    case 6:
                        talentTreeData.nodes.node6.GetComponent<Image>().color = CircleColor;
                        talentTreeData.nodes.node6.transform.Find("Overlay").GetComponent<Image>().color = imageColor;
                        break;
                    case 7:
                        talentTreeData.nodes.node7.GetComponent<Image>().color = CircleColor;
                        talentTreeData.nodes.node7.transform.Find("Overlay").GetComponent<Image>().color = imageColor;
                        break;
                    case 8:
                        talentTreeData.nodes.node8.GetComponent<Image>().color = CircleColor;
                        talentTreeData.nodes.node8.transform.Find("Overlay").GetComponent<Image>().color = imageColor;
                        break;
                    case 9:
                        talentTreeData.nodes.node9.GetComponent<Image>().color = CircleColor;
                        talentTreeData.nodes.node9.transform.Find("Overlay").GetComponent<Image>().color = imageColor;
                        break;
                    case 10:
                        talentTreeData.nodes.node10.GetComponent<Image>().color = CircleColor;
                        talentTreeData.nodes.node10.transform.Find("Overlay").GetComponent<Image>().color = imageColor;
                        break;
                }
            }
        }

        //talentTreeData.

        void selectNode(int nodeNum, GameObject nodeElement)
        {

            soundEffect.playClick();    //Clicking sound effect

            //Store the current object for later manipulation
            currentNodeObject = nodeElement;

            //Get the current slot image
            Sprite nodeImageElement = nodeElement.transform.Find("Container").Find("Icon").GetComponent<Image>().sprite;

            //Set the image
            talentTreeData.displayWindow.nodeImage.GetComponent<Image>().sprite = nodeImageElement;

            //Get the corresponding node data  
            TalentTree.treeNode selectedNode = new TalentTree.treeNode("", "", "", 0);
            selectedNode = TalentTree.nodes["node" + nodeNum.ToString()];

            //Store the ID for future proccessing
            currentNode = selectedNode.id;

            //Update the UI data
            talentTreeData.displayWindow.nodeTittle.GetComponent<Text>().text = selectedNode.name;
            talentTreeData.displayWindow.nodeDesc.GetComponent<Text>().text = selectedNode.description;


            string newButtonText = "";
            (bool, bool) nodeStatus = getNodeInfo(nodeNum);
            //Check what should be displayed by the button
            if (nodeStatus.Item1)
            {
                //Already owns this node
                newButtonText = "Owned";
                talentTreeData.displayWindow.nodeButton.GetComponent<Button>().interactable = false;
            }
            else if (nodeStatus.Item2)
            {
                //Can unlock this
                newButtonText = "Unlock";
                talentTreeData.displayWindow.nodeButton.GetComponent<Button>().interactable = true;
            }
            else
            {
                //Can't unlock because of the tree order
                newButtonText = "Unavalible";
                talentTreeData.displayWindow.nodeButton.GetComponent<Button>().interactable = false;
            }

            //Setup the button text
            talentTreeData.displayWindow.nodeButton.transform.Find("Text").GetComponent<Text>().text = newButtonText;
        }

        private (bool, bool) getNodeInfo(int nodeNum)
        {
        bool hasNode = false;
        bool canUnlock = false;
        //See if the player can unlock this node or not
        switch (nodeNum)
        {
            //Handle the node 0
            case 0:
                if (PlayerData.talentTreeData.node0)
                {
                    hasNode = true;
                }
                {
                    canUnlock = true;
                }

                break;

            //Handle the node 1
            case 1:
                if (PlayerData.talentTreeData.node1)
                {
                    hasNode = true;
                }
                else if (PlayerData.talentTreeData.node0)
                {
                    canUnlock = true;
                }
                break;

            //Handle the node 2
            case 2:
                if (PlayerData.talentTreeData.node2)
                {
                    hasNode = true;
                }
                else if (PlayerData.talentTreeData.node0)
                {
                    canUnlock = true;
                }

                break;

            //Handle the node 3
            case 3:
                if (PlayerData.talentTreeData.node3)
                {
                    hasNode = true;
                }
                else if (PlayerData.talentTreeData.node0)
                {
                    canUnlock = true;
                }

                break;

            //Handle the node 4
            case 4:
                if (PlayerData.talentTreeData.node4)
                {
                    hasNode = true;
                }
                else if (PlayerData.talentTreeData.node1)
                {
                    canUnlock = true;
                }

                break;

            //Handle the node 5
            case 5:
                if (PlayerData.talentTreeData.node5)
                {
                    hasNode = true;
                }
                else if (PlayerData.talentTreeData.node2)
                {
                    canUnlock = true;
                }

                break;

            //Handle the node 6
            case 6:
                if (PlayerData.talentTreeData.node6)
                {
                    hasNode = true;
                }
                else if (PlayerData.talentTreeData.node2)
                {
                    canUnlock = true;
                }

                break;

            //Handle the node 7
            case 7:
                if (PlayerData.talentTreeData.node7)
                {
                    hasNode = true;
                }
                else if (PlayerData.talentTreeData.node3)
                {
                    canUnlock = true;
                }

                break;

            //Handle the node 8
            case 8:
                if (PlayerData.talentTreeData.node8)
                {
                    hasNode = true;
                }
                else if (PlayerData.talentTreeData.node4)
                {
                    canUnlock = true;
                }

                break;

            //Handle the node 9
            case 9:
                if (PlayerData.talentTreeData.node9)
                {
                    hasNode = true;
                }
                else if (PlayerData.talentTreeData.node7)
                {
                    canUnlock = true;
                }

                break;

            //Handle the node 10
            case 10:
                if (PlayerData.talentTreeData.node10)
                {
                    hasNode = true;
                }
                else if (PlayerData.talentTreeData.node9 || PlayerData.talentTreeData.node6 || PlayerData.talentTreeData.node5 || PlayerData.talentTreeData.node8)
                {
                    canUnlock = true;
                }

                break;
        }

            return (hasNode, canUnlock);
        }

        public void unlockNode()
        {
            soundEffect.playClick();    //Clicking sound effect

            if (currentNode != "")
            {
                //Current node object
                TalentTree.treeNode nodeObject = TalentTree.nodes[currentNode];

                //Update the text and click listeners              
                if (!talentTreeData.confirmationWindow.activeSelf)
                {
                    if (PlayerData.resources >= nodeObject.price)
                    {
                        //Set the buttons and the main text
                        talentTreeData.confirmationWindow.transform.Find("Button Group").Find("Accept").GetComponent<Button>().interactable = true;
                        talentTreeData.confirmationWindow.transform.Find("Content Group").Find("Text").GetComponent<Text>().text =
                        "Do you wish to unlock <b><color=#f5e000>" + nodeObject.name.ToUpper() + "</color></b> for <b><color=#f52500>" + NumAbv.prettyValues(nodeObject.price) + "</color></b> resource points?";


                        //Set the listener for the purchase service
                        talentTreeData.confirmationWindow.transform.Find("Button Group").Find("Accept").GetComponent<Button>().onClick.AddListener(() => {

                            soundEffect.playClick();    //Clicking sound effect

                            //Stop future listeners
                            talentTreeData.confirmationWindow.transform.Find("Button Group").Find("Accept").GetComponent<Button>().onClick.RemoveAllListeners();

                            //Hide the window
                            talentTreeData.confirmationWindow.SetActive(false);

                            //Get the node num in int
                            int currentNodeNum = int.Parse(currentNode.Replace("node", ""));

                            //Make the purchase
                            PlayerData.resources -= nodeObject.price;

                            switch (currentNodeNum)
                            {
                                case 0:
                                    PlayerData.talentTreeData.node0 = true;
                                    break;
                                case 1:
                                    PlayerData.talentTreeData.node1 = true;
                                    break;
                                case 2:
                                    PlayerData.talentTreeData.node2 = true;
                                    break;
                                case 3:
                                    PlayerData.talentTreeData.node3 = true;
                                    break;
                                case 4:
                                    PlayerData.talentTreeData.node4 = true;
                                    break;
                                case 5:
                                    PlayerData.talentTreeData.node5 = true;
                                    break;
                                case 6:
                                    PlayerData.talentTreeData.node6 = true;
                                    break;
                                case 7:
                                    PlayerData.talentTreeData.node7 = true;
                                    break;
                                case 8:
                                    PlayerData.talentTreeData.node8 = true;
                                    break;
                                case 9:
                                    PlayerData.talentTreeData.node9 = true;
                                    break;
                                case 10:
                                    PlayerData.talentTreeData.node10 = true;
                                    break;
                            }

                            //Update the layout of the tree
                            updateTalentTree();

                            //Current selected element updated
                            selectNode(currentNodeNum, currentNodeObject);

                            if (PlayerData.id != null)
                            {
                                //Spawn the network loading screen
                                GameObject loadingScreen = Instantiate(loadingNetworkPrefab);
                                loadingScreen.transform.SetParent(gameObject.transform);
                                loadingScreen.transform.localPosition = loadingNetworkPrefab.transform.localPosition;
                                loadingScreen.transform.localScale = new Vector3(1, 1, 1);
                                loadingScreen.name = loadingNetworkPrefab.name;

                                //Player is logged in
                                //request for a save



                                accountInfoResponse.nestedData playerInfo = PlayerData.savePlayerData();
                                StartCoroutine(PlayerData.savePlayerDataRequest(playerInfo, ()=> { 
                                
                                    Destroy(loadingScreen);

                                }));
                            }

                        });
                    }
                    else
                    {
                        //Set the buttons and the main text
                        talentTreeData.confirmationWindow.transform.Find("Button Group").Find("Accept").GetComponent<Button>().interactable = false;
                        talentTreeData.confirmationWindow.transform.Find("Content Group").Find("Text").GetComponent<Text>().text =
                        "You do not have enough resource points. You're short by <b><color=#f52500>" + (nodeObject.price - PlayerData.resources).ToString() + "</color></b> resource points.";
                    }

                    //Set the listener for the closing button of the window
                    talentTreeData.confirmationWindow.transform.Find("Button Group").Find("Cancel").GetComponent<Button>().onClick.AddListener(() => {

                        soundEffect.playClick();    //Clicking sound effect

                        //Reset the listener to avoid overload
                        talentTreeData.confirmationWindow.transform.Find("Button Group").Find("Cancel").GetComponent<Button>().onClick.RemoveAllListeners();
                        talentTreeData.confirmationWindow.SetActive(false);
                    });

            }    

                //Display the window
                talentTreeData.confirmationWindow.SetActive(!talentTreeData.confirmationWindow.activeSelf);

                //int currentNodeNum = int.Parse(currentNode.Replace("node", ""));
                //Debug.Log(currentNodeNum);
            }

        }
    //___________________________||

    public void closeProfilesWindow()
    {
        soundEffect.playClick();    //Clicking sound effect
        talentTreeData.talentTreeWindow.SetActive(false);   //Close the talent tree
        options.optionsWindow.SetActive(false);             //Close the options
        villageMinimap.mapWindow.SetActive(false);          //Close the map
        profiles.profilesWindow.SetActive(!profiles.profilesWindow.activeSelf);
    }

    //Handle leave dungeon functions||

        public void acceptLeave(){
            GameObject dungeonData = GameObject.FindGameObjectWithTag("proceduralData");
            DontDestroyOnLoad(dungeonData); 
            soundEffect.playClick();    //Clicking sound effect
            confirmationWindowDungeon.SetActive(false);
            sceneTeleport.start(1);
        }

        public void openConfirmation(){
            soundEffect.playClick();    //Clicking sound effect
            if(confirmationWindowDungeon)
                confirmationWindowDungeon.SetActive(!confirmationWindowDungeon.activeSelf);
        }
    //______________________________||

}
