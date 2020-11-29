using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMainMenu : MonoBehaviour
{

    //Variables||

    public GameObject[] GameObjects;
    public float cameraSpeed = 8f;

    private List<Vector2> CameraPositions  = new List<Vector2>();
    private Vector3 NextCameraGoal;
    private int currentIndex = 0;
    //_________||



    private void ChangeCameraGoal(Vector2 checkpoint)
    {
        //Set the next goal for the camera
        NextCameraGoal = new Vector3(checkpoint.x, checkpoint.y, gameObject.GetComponent<Transform>().position.z);
    }

    void Start()
    {

        //Get all the camera positions 
        foreach (GameObject point in GameObjects)
        {

            //Get the point position
            Vector2 pointPosition = point.GetComponent<Transform>().position;

            //Add the point
            CameraPositions.Add(pointPosition);
            
        }

        //Set the first position of the array
        ChangeCameraGoal(CameraPositions[0]);

    }

    // Update is called once per frame
    void Update()
    {

        //Check if the camera is correct
        if (Vector3.Distance(gameObject.GetComponent<Transform>().position, NextCameraGoal) > 0.2f)
        {
            //Get the current camera position
            Vector3 currentCameraPos = gameObject.GetComponent<Transform>().position;

            //Make the new camera position
            Vector3 newCameraPosition = (NextCameraGoal - currentCameraPos).normalized * (cameraSpeed * Time.deltaTime);

            //Add the position to the camera
            gameObject.GetComponent<Transform>().position += newCameraPosition;
        }
        else
        {

            //Set the current index
            if (currentIndex >= (CameraPositions.Count - 1))
                currentIndex = 0;
            else
                currentIndex++;

            //Update the current goal
            ChangeCameraGoal(CameraPositions[currentIndex]);
        }

    }
}
