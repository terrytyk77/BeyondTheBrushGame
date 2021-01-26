using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMarkerAnimation : MonoBehaviour
{
    //Variables||

        private Vector2 startingPoint;
        private bool direction = true;

        private float sizeLimit = 3.5f;
        private float speed = 0.09f;
    //_________||

    private void Start()
    {
        startingPoint = gameObject.GetComponent<Transform>().localScale;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Transform markerSize = gameObject.GetComponent<Transform>();
        if (direction)
        {
            if(markerSize.localScale.x + speed > startingPoint.x + sizeLimit)
                markerSize.localScale = new Vector2(startingPoint.x + sizeLimit, startingPoint.x + sizeLimit);
            else
                markerSize.localScale = new Vector2(markerSize.localScale.x + speed, markerSize.localScale.y + speed);
        }
        else
        {
            if (markerSize.localScale.x - speed < startingPoint.x - sizeLimit)
                markerSize.localScale = new Vector2(startingPoint.x - sizeLimit, startingPoint.x - sizeLimit);
            else
                markerSize.localScale = new Vector2(markerSize.localScale.x - speed, markerSize.localScale.y - speed);
        }

        if (markerSize.localScale.y <= startingPoint.x - sizeLimit)
            direction = true;
        else if (markerSize.localScale.y >= startingPoint.x + sizeLimit)
            direction = false;

    }
}
