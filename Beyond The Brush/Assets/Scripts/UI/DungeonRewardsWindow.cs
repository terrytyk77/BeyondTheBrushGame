using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRewardsWindow : MonoBehaviour
{
    //Variables||

        public float windowMoveSpeed = 25f;                             //The speed at which the window moves down to
        public Vector3 currentWindowPosition = new Vector3(0, 900, 0);  //Set the position on which the window start at
    //_________||

    private void OnEnable()
    {
        currentWindowPosition = new Vector3(0, 900, 0);     //Reset the window position
    }

    void FixedUpdate()
    {
        //DO NOT USE DELTA TIME
        //We want the UI to work while the game is paused
        currentWindowPosition = new Vector3(0, currentWindowPosition.y - windowMoveSpeed, 0);           //Calculate the next window position

        if (currentWindowPosition.y > 0)
            gameObject.GetComponent<RectTransform>().localPosition = currentWindowPosition;             //Update the window position
        else if (currentWindowPosition.y < 0)
            gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);              //Fix the window position if it goes off bounds
    }
}
