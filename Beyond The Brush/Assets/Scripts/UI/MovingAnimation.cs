using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovingAnimation : MonoBehaviour
{
    Vector2 markerStartingPos;
    public bool xAxis = false;

    // Start is called before the first frame update
    void OnEnable()
    {
        markerStartingPos = new Vector2(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y);

        if(xAxis)
            StartCoroutine(markerMovementX());
        else
            StartCoroutine(markerMovementY());
    }


    private IEnumerator markerMovementY()
    {

        Image markerComponent = gameObject.GetComponent<Image>();
        markerComponent.color = new Color(255, 255, 255, 255);
        bool direction = false; //False for down, true for up

        while (true)
        {
            if (direction)
            {
                markerComponent.transform.localPosition += new Vector3(0, 0.475f, 0);
                if (markerComponent.transform.localPosition.y > markerStartingPos.y + 12)
                    direction = false;

                yield return new WaitForSeconds(0.025f);
            }
            else
            {
                markerComponent.transform.localPosition -= new Vector3(0, 0.475f, 0);

                if (markerComponent.transform.localPosition.y < markerStartingPos.y - 12)
                    direction = true;

                yield return new WaitForSeconds(0.025f);
            }
        }

    }

    private IEnumerator markerMovementX()
    {

        Image markerComponent = gameObject.GetComponent<Image>();
        markerComponent.color = new Color(255, 255, 255, 255);
        bool direction = false; //False for down, true for up

        while (true)
        {
            if (direction)
            {
                markerComponent.transform.localPosition += new Vector3(0.475f, 0, 0);
                if (markerComponent.transform.localPosition.x > markerStartingPos.x + 12)
                    direction = false;

                yield return new WaitForSeconds(0.025f);
            }
            else
            {
                markerComponent.transform.localPosition -= new Vector3(0.475f, 0, 0);

                if (markerComponent.transform.localPosition.x < markerStartingPos.x - 12)
                    direction = true;

                yield return new WaitForSeconds(0.025f);
            }
        }

    }
}
