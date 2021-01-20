using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin2 : MonoBehaviour
{
    private int direction = 1;

    // Update is called once per frame
    void Update()
    {
        float circleSpeed = (0.5f * Time.deltaTime * direction);

        gameObject.GetComponent<CutoutMaskUI>().fillAmount += circleSpeed;

        if (gameObject.GetComponent<CutoutMaskUI>().fillAmount >= 1)
        {
            direction = -1;
            gameObject.GetComponent<CutoutMaskUI>().fillClockwise = true;
        }
        else if (gameObject.GetComponent<CutoutMaskUI>().fillAmount <= 0)
        {
            direction = 1;
            gameObject.GetComponent<CutoutMaskUI>().fillClockwise = false;
        }

    }
}
