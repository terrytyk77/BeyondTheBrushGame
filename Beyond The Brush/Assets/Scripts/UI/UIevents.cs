using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIevents : MonoBehaviour
{
    //Variables||

        //room
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
                }

                public nodesStruct nodes = new nodesStruct();
                public displayWindowClass displayWindow = new displayWindowClass();
            }

            public talentTreeClass talentTreeData = new talentTreeClass();
            private string currentNode = "";
        //___________||

        //Loading a network instance window
        public GameObject loadingNetworkPrefab;


    //_________||

    private void Start()
    {
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

        updateOptionsWindowInfo();
    }

    private void Update()
    {
        //Listen to the keyboard keys||
            
            //minimap
            if (Input.GetKeyDown(minimap.minimapKey)){ if (!minimapOpened) { alreadyChangeMinimap = false; } minimapOpened = true; }
            if (Input.GetKeyUp(minimap.minimapKey)) minimapOpened = alreadyChangeMinimap = false;
            
            //main menu
            if (Input.GetKeyDown(mainMenuKey)) { if (!escapeKeyDebounce) { escapeKeyDebounce = true; if (!mainMenu.mainWindow.activeSelf) { Time.timeScale = 0; } else { Time.timeScale = 1; options.optionsWindow.SetActive(false); } mainMenu.mainWindow.SetActive(!mainMenu.mainWindow.activeSelf); } }
            if (Input.GetKeyUp(mainMenuKey)) { if (escapeKeyDebounce) { escapeKeyDebounce = false; } }
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
        }

        public void sfxChanged()
        {
            PlayerData.sfxVolume = options.sfxSlider.GetComponent<Slider>().value;
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
            //Just enable the options menu
            options.optionsWindow.SetActive(!options.optionsWindow.activeSelf);
        }

        public void closeOption()
        {
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
            if (PlayerData.slashCooldown > 0)
            {
                slashSkillText = slashSkillText + decimal.Round((decimal)PlayerData.slashCooldown, 2);
            }
            cooldowns.slashSkill.transform.Find("Cooldown").GetComponent<Image>().fillAmount = (PlayerData.slashCooldown/PlayerData.slashCooldownDefault);
            cooldowns.slashSkill.transform.Find("CooldownText").GetComponent<Text>().text = slashSkillText;

            //xslash
            string xslashSkillText = "";
            if (PlayerData.xslashCooldown > 0)
            {
                xslashSkillText = xslashSkillText + decimal.Round((decimal)PlayerData.xslashCooldown, 2);
            }
            cooldowns.XslashSkill.transform.Find("Cooldown").GetComponent<Image>().fillAmount = (PlayerData.xslashCooldown / PlayerData.xslashCooldownDefault);
            cooldowns.XslashSkill.transform.Find("CooldownText").GetComponent<Text>().text = xslashSkillText;

            //shield
            string shieldSkillText = "";
            if (PlayerData.shieldCooldown > 0)
            {
                shieldSkillText = shieldSkillText + decimal.Round((decimal)PlayerData.shieldCooldown, 2);
            }
            cooldowns.shieldSkill.transform.Find("Cooldown").GetComponent<Image>().fillAmount = (PlayerData.shieldCooldown / PlayerData.shieldCooldownDefault);
            cooldowns.shieldSkill.transform.Find("CooldownText").GetComponent<Text>().text = shieldSkillText;
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
        //Resum the game by closing the window

        //Set the time back to normal
        Time.timeScale = 1;

        //Close the main menu window
        mainMenu.mainWindow.SetActive(false);
    }

    public void OnLogout()
    {



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
            talentTreeData.talentTreeWindow.SetActive(!talentTreeData.talentTreeWindow.activeSelf);
        }

        //talentTreeData.

        void selectNode(int nodeNum, GameObject nodeElement)
        {

            //Get the current slot image
            Sprite nodeImageElement = nodeElement.transform.Find("Container").Find("Icon").GetComponent<Image>().sprite;

            //Set the image
            talentTreeData.displayWindow.nodeImage.GetComponent<Image>().sprite = nodeImageElement;

            //Get the corresponding node data  
            TalentTree.treeNode selectedNode = new TalentTree.treeNode("", "", "");
            selectedNode = TalentTree.nodes["node" + nodeNum.ToString()];

            //Store the ID for future proccessing
            currentNode = selectedNode.id;

            //Update the UI data
            talentTreeData.displayWindow.nodeTittle.GetComponent<Text>().text = selectedNode.name;
            talentTreeData.displayWindow.nodeDesc.GetComponent<Text>().text = selectedNode.description;
        }
    //___________________________||


}
