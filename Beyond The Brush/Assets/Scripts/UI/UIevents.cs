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

        if (PlayerData.windowmode)
            options.dropdownWindow.GetComponent<DuloGames.UI.UISelectField>().SelectOption("Fullscreen");
        else
            options.dropdownWindow.GetComponent<DuloGames.UI.UISelectField>().SelectOption("Windowed");
        
        setupProfilesWindow();
        updateOptionsWindowInfo();
    }


    private void setupProfilesWindow()
    {
        DuloGames.UI.UISelectField profileDropdown = profiles.profilesDropDown.GetComponent<DuloGames.UI.UISelectField>();

        //Empty the options
        profileDropdown.ClearOptions();

        //Add the empty profiles
        profileDropdown.AddOption("Default 1");
        profileDropdown.AddOption("Default 2");

        //Select the default one
        profileDropdown.SelectOption("Default 1");

        //Add the items to the dropdown
        foreach (accountInfoResponse.profilesData profile in PlayerData.playerProfiles)
        {
            profileDropdown.AddOption(profile.profile.name);
        }


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
            if (PlayerData.talentTreeData.node3 == true)
            {
                cooldowns.shieldSkill.transform.Find("StackText").GetComponent<Text>().text = PlayerData.shieldCurrentStack.ToString();
            }
            cooldowns.shieldSkill.transform.Find("CooldownText").GetComponent<Text>().text = shieldSkillText;

            //rock spawner
            string rockSkillText = "";
            if (PlayerData.rockCooldown > 0)
            {
                rockSkillText = rockSkillText + decimal.Round((decimal)PlayerData.rockCooldown, 2);
            }
            cooldowns.rockSkill.transform.Find("Cooldown").GetComponent<Image>().fillAmount = (PlayerData.rockCooldown / PlayerData.rockSpawnCooldownDefault);
            cooldowns.rockSkill.transform.Find("CooldownText").GetComponent<Text>().text = rockSkillText;
            
            //box spawner
            string boxSkillText = "";
            if (PlayerData.boxCooldown > 0)
            {
                boxSkillText = boxSkillText + decimal.Round((decimal)PlayerData.boxCooldown, 2);
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

        private void updateTalentTree()
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
                                PlayerData.savePlayerDataRequest(playerInfo, () =>
                                {
                                    Destroy(loadingScreen);
                                });
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
        profiles.profilesWindow.SetActive(!profiles.profilesWindow.activeSelf);
    }

}
