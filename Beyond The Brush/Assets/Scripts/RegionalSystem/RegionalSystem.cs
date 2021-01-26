﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegionalSystem : MonoBehaviour
{
    //Variables||

        public string currentArea = ""; //Holds the last region ID
        public GameObject audioElement; //Holds the audio gameobject
        private MusicPlayer musicLib;   //Holds the library of music

        [System.Serializable]
        public class regionData         //Holds the data for the regions
        {
            public string name;         //Holds the name of the region
            public AudioClip music;     //Holds the music of the region 

            public regionData(string n, AudioClip m)    //Constructor for the class
            {
                name = n;   
                music = m;
            }
        }


        public Dictionary<string, regionData> regionsInfo;


        private void Start()
        {
            musicLib = audioElement.GetComponent<MusicPlayer>();

            //Setup 
            regionsInfo = new Dictionary<string, regionData>
            {
                {
                "dmF", //ID
                new regionData("Deadmines Forest"/*Name*/, musicLib.deadMinesMusic /*Music*/)
                },
            };
        }

    //_________||

    public void changedArea(){

        regionData changedAreaData = regionsInfo[currentArea];  //Get the data from the new area

        StartCoroutine(textAnimation(changedAreaData.name));                    //Call the UI text animation

        audioElement.GetComponent<AudioSource>().Stop();
        audioElement.GetComponent<AudioSource>().clip = changedAreaData.music;  //Change the music file from the thing
        audioElement.GetComponent<AudioSource>().Play();
    }

    private IEnumerator textAnimation(string locationName){

        Text notificationText = gameObject.transform.Find("NewAreaNotification").GetComponent<Text>();  //Get the text component
        notificationText.text = locationName;   //Set the location name

        while (notificationText.color.a < 1){   //Lower transparency until fully visible
            notificationText.color = new Color(notificationText.color.r, notificationText.color.g, notificationText.color.b, notificationText.color.a +0.075f);
            yield return new WaitForSecondsRealtime(0.075f);
        }

        yield return new WaitForSecondsRealtime(1.25f);    //Wait for a little bit

        while (notificationText.color.a > 0){   //Raise transparency until fully invisible
            notificationText.color = new Color(notificationText.color.r, notificationText.color.g, notificationText.color.b, notificationText.color.a - 0.075f);
            yield return new WaitForSecondsRealtime(0.075f);
        }

        yield return null;
    }

}
