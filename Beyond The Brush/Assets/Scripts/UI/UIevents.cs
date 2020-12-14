using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        }

        [System.Serializable]
        public class expbarClass
        {
            public GameObject fill;
            public GameObject text;
        }

        //username
        public GameObject usernameDisplay;

        //Bars
        public healthbarClass healthbar;
        public expbarClass expBar;

        //minimap
        public GameObject minimap;
        public GameObject minimapComponent;
        public GameObject minimapSlider;
        public KeyCode minimapKey = KeyCode.M;

        private Vector2 defaultMinimapPosition;
        private Vector2 defaultMinimapScale;
        private bool minimapOpened = false;
        private bool alreadyChangeMinimap = true;
    //_________||

    private void Start()
    {

        //Adapt to the correct map zoom
        changeMinimapZoon();
        currentRoom = GameObject.FindGameObjectWithTag("proceduralData").GetComponent<CurrentDungeonData>().currentRoom;

        //Minimap
        if (minimap != null)
        {
            //Get the default position
            defaultMinimapPosition = minimap.GetComponent<RectTransform>().localPosition;
            defaultMinimapScale = minimap.GetComponent<RectTransform>().localScale;
        }

        //Change your name
        usernameDisplay.GetComponent<Text>().text = PlayerData.username;
    }

    private void Update()
    {
        //Listen to the keyboard keys||
            if (Input.GetKeyDown(minimapKey)){ if (!minimapOpened) { alreadyChangeMinimap = false; } minimapOpened = true; }
            if (Input.GetKeyUp(minimapKey)) minimapOpened = alreadyChangeMinimap = false;
        //___________________________||

        //Minimap||
            if (!alreadyChangeMinimap)
            {
                if (!minimapOpened)
                {
                    //It was closed
                    minimapOpened = false;
                    //Reverse the variables
                    minimap.GetComponent<RectTransform>().localScale = defaultMinimapScale;
                    minimap.GetComponent<RectTransform>().localPosition = defaultMinimapPosition;
                }
                else
                {
                    //It was opened
                    minimapOpened = true;
                    minimap.GetComponent<RectTransform>().localScale = new Vector2(2.5f, 1.8f);
                    minimap.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
                }
                alreadyChangeMinimap = true;
            }
        //_______||

        updateUiElements();

    }

    private void updateUiElements()
    {
        //Healthbar
        float healthBarFillAmount = ((float)PlayerData.healthPoints/(float)PlayerData.maxHealthPoints);
        healthbar.fill.GetComponent<Image>().fillAmount = healthBarFillAmount;

        //Exp bar
        float expBarFillAmount = ((float)PlayerData.exp/(float)PlayerData.getNeededExp());
        expBar.fill.GetComponent<Image>().fillAmount = expBarFillAmount;

        expBar.text.GetComponent<Text>().text = PlayerData.exp + "/" + PlayerData.getNeededExp();

    }

    public void changeMinimapZoon()
    {
        //Get the slider value
        float value = minimapSlider.GetComponent<Slider>().value;

        //Resize the minimap
        minimapComponent.GetComponent<RectTransform>().localScale = new Vector2(value, value);

        //Get the room size
        int roomSize = (int)roomPrefab.GetComponent<RectTransform>().sizeDelta.x;

        currentRoom = GameObject.FindGameObjectWithTag("proceduralData").GetComponent<CurrentDungeonData>().currentRoom;

        //Change to the correct position
        minimapComponent.transform.localPosition =
        new Vector2(-currentRoom.x * (roomSize * minimapComponent.transform.localScale.x),
        -currentRoom.y * (roomSize * minimapComponent.transform.localScale.y));

    }

}
