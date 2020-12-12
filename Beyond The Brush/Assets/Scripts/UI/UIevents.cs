using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIevents : MonoBehaviour
{
    //Variables||
        public GameObject minimapComponent;
        public GameObject roomPrefab;
        public GameObject minimapSlider;

        private Vector2Int currentRoom;
    //_________||

    private void Start()
    {
        //Adapt to the correct map zoom
        changeMinimapZoon();
        currentRoom = GameObject.FindGameObjectWithTag("proceduralData").GetComponent<CurrentDungeonData>().currentRoom;
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
