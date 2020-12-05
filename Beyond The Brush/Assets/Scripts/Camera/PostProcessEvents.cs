using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PostProcessEvents : MonoBehaviour
{

    //Variables||
        public GameObject darkOverlay;
    //_________||

    public void transition(Action fun)
    {
        StartCoroutine("TransitionAnimation", fun);
    }

    IEnumerator TransitionAnimation(Action fun)
    {
        Image imageElement = darkOverlay.GetComponent<Image>();

        //Make the screen get darker    
        while (imageElement.color.a < 1)
        {
            float transparencyVolume = imageElement.color.a;
            imageElement.color = new Color(0, 0, 0, transparencyVolume + 0.05f);

            yield return new WaitForSeconds(0.05f);
        }

        //Call the teleport
        fun();

        yield return new WaitForSeconds(0.5f);
 
        while (imageElement.color.a > 0)
        {
            float transparencyVolume = imageElement.color.a;
            imageElement.color = new Color(0, 0, 0, transparencyVolume - 0.05f);

            yield return new WaitForSeconds(0.05f);
        }


        yield return 0;
    }
}
