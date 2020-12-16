using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PostProcessEvents : MonoBehaviour
{

    //Variables||
        public GameObject darkOverlay;
        private string storeName = "";
    //_________||

    public void transition(Action fun, string name)
    {
        storeName = name;
        StartCoroutine("TransitionAnimation", fun);
    }

    IEnumerator TransitionAnimation(Action fun)
    {

        if (storeName == "exit")
        {
            fun();
            yield return 0;
        }
        else
        {



        Image imageElement = darkOverlay.GetComponent<Image>();

        //Make the screen get darker    
        while (imageElement.color.a < 1)
        {
            float transparencyVolume = imageElement.color.a;
            imageElement.color = new Color(0, 0, 0, transparencyVolume + 0.05f);

            yield return new WaitForSeconds(0.015f);
        }

        //Call the teleport
        fun();

        yield return new WaitForSeconds(0.5f);
 
        while (imageElement.color.a > 0)
        {
            float transparencyVolume = imageElement.color.a;
            imageElement.color = new Color(0, 0, 0, transparencyVolume - 0.05f);

            yield return new WaitForSeconds(0.015f);
        }


        yield return 0;

        }

    }
}
