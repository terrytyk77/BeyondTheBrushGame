using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class bigMap : MonoBehaviour, IPointerDownHandler, IScrollHandler
{
    //Variables||

        private bool holdingMap = false;
        private bool updateDataDebounce = true;
        private float maxMapSize = 5f;
        private float miniumMapSize = 1f;

        private Vector2 mapAbsoluteSize;
        private Vector3 storeClickPosition;
        private Vector2 mapPositionOnClick;
    //_________||

    private void Start()
    {
        updateMapSize();
    }

    private void updateMapSize(){
        mapAbsoluteSize = new Vector2(  //Store the absolute size of the map
                                    gameObject.transform.localScale.x * gameObject.GetComponent<RectTransform>().sizeDelta.x - gameObject.transform.parent.GetComponent<RectTransform>().sizeDelta.x,
                                    gameObject.transform.localScale.y * gameObject.GetComponent<RectTransform>().sizeDelta.y - gameObject.transform.parent.GetComponent<RectTransform>().sizeDelta.y);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        holdingMap = true;                                          //Tell the code that the user clicked on the right zone
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonUp(0)) {      //The user is no longer holding the mouse
            holdingMap = false;
        }
        if(Input.GetMouseButton(0) && holdingMap)       //The person is dragging the map
        {
            if(updateDataDebounce)
            {
                storeClickPosition = Input.mousePosition;
                mapPositionOnClick = gameObject.transform.GetComponent<RectTransform>().anchoredPosition;
                updateDataDebounce = false;
            }   

            Vector2 displacement = Input.mousePosition - storeClickPosition;    //Get the mouse displacement as it moves
            Vector2 newMapPosition = mapPositionOnClick + displacement;         //Get the new map position

            updateMapLocaiton(newMapPosition);                                  //Update the map location

        }
        else{
            updateDataDebounce = true;
        }
    }

    private void updateMapLocaiton(Vector2 newMapPosition)
    {
        if (newMapPosition.x > 0)
            newMapPosition = new Vector2(0, newMapPosition.y);                  //Fix the position at the left

        if (newMapPosition.x < -mapAbsoluteSize.x)
            newMapPosition = new Vector2(-mapAbsoluteSize.x, newMapPosition.y); //Fix the position at the right

        if (newMapPosition.y < 0)
            newMapPosition = new Vector2(newMapPosition.x, 0);                  //Fix the top

        if (newMapPosition.y > mapAbsoluteSize.y)
            newMapPosition = new Vector2(newMapPosition.x, mapAbsoluteSize.y);  //Fix the Bottom

        //add a bunch of ifs here
        gameObject.transform.GetComponent<RectTransform>().anchoredPosition = newMapPosition;
    }

    public void OnScroll(PointerEventData eventData)
    {
        RectTransform mapRect = gameObject.transform.GetComponent<RectTransform>();

        if (Input.mouseScrollDelta.y < 0){
            //Scrolling down
            Vector2 newMapScale = new Vector2(mapRect.localScale.x - 0.1f, mapRect.localScale.y - 0.1f);

            if (newMapScale.x < miniumMapSize)
                mapRect.localScale = new Vector2(miniumMapSize, miniumMapSize);
            else
                mapRect.localScale = newMapScale;

            //mapRect.anchoredPosition = new Vector2(mapRect.anchoredPosition.x + (0.1f * mapRect.sizeDelta.x)/2, mapRect.anchoredPosition.y + (0.1f * mapRect.sizeDelta.y)/2);



            //Debug.Log("Moved: " + 0.1f * mapRect.sizeDelta.x + "Pixels");
            Vector2 temeporaryStore = mapAbsoluteSize;

            float percentageX = (100 * mapRect.anchoredPosition.x) / -mapAbsoluteSize.x;
            float percentageY = (100 * mapRect.anchoredPosition.y) / -mapAbsoluteSize.y;

            updateMapSize();

            Vector2 newPos = new Vector2((percentageX * -mapAbsoluteSize.x)/100, (percentageY * -mapAbsoluteSize.y) / 100);
            mapRect.anchoredPosition = newPos;

            updateMapLocaiton(mapRect.anchoredPosition);
        }
        else if(Input.mouseScrollDelta.y > 0){
            //Scrolling up
            Vector2 newMapScale = new Vector2(mapRect.localScale.x + 0.1f, mapRect.localScale.y + 0.1f);

            if (newMapScale.x > maxMapSize)
                mapRect.localScale = new Vector2(maxMapSize, maxMapSize);
            else
                mapRect.localScale = newMapScale;

            float percentageX = (100 * mapRect.anchoredPosition.x) / -mapAbsoluteSize.x;
            float percentageY = (100 * mapRect.anchoredPosition.y) / -mapAbsoluteSize.y;

            updateMapSize();

            Vector2 newPos = new Vector2((percentageX * -mapAbsoluteSize.x) / 100, (percentageY * -mapAbsoluteSize.y) / 100);
            mapRect.anchoredPosition = newPos;

        }

    }
}
