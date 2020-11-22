using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{

    //Variables||

        public bool AdjustResolution = true;
        public float startingCameraSize = 6;
        private float defaultWidth = 1920;
        private float defaultHeight = 1080;
    //_________||

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Camera camera = gameObject.GetComponent<Camera>();

        //Set the camera size
        if (AdjustResolution)
        {
            camera.orthographicSize = ((defaultWidth * startingCameraSize) /Screen.width); 
        }
    }
}
