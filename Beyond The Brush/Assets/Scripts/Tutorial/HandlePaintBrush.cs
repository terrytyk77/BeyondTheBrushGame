using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlePaintBrush : MonoBehaviour
{

    //Variables||
        public GameObject paintBrush;

        //shapes
        public GameObject horizontalLine;
        public GameObject xspellLine;
        public GameObject boxLine;
        public GameObject circleLine;

        public string currentlyDrawing = "";
        int currentDot = 0;

        private List<Transform> horizontalDots =  new List<Transform>();
        private List<Transform> xspellDots = new List<Transform>();
        private List<Transform> boxDots = new List<Transform>();
        private List<Transform> circleDots = new List<Transform>();
    //_________||

    private void Start()
    {
        //Store the points||
            //Horizontal
            foreach(Transform dot in horizontalLine.transform.Find("Points"))
            {
                horizontalDots.Add(dot);
            }
            //xslash
            foreach(Transform dot in xspellLine.transform.Find("Points"))
            {
                xspellDots.Add(dot);
            }
            //boxline
            foreach(Transform dot in boxLine.transform.Find("Points"))
            {
                boxDots.Add(dot);
            }
            //circle
            foreach(Transform dot in circleLine.transform.Find("Points"))
            {
                circleDots.Add(dot);
            }
        //________________||

        drawBox();
    }

    public void stopDrawing(){

        paintBrush.SetActive(false);
        currentDot = 0;
        currentlyDrawing = "";

        horizontalLine.SetActive(false);
        xspellLine.SetActive(false);
        boxLine.SetActive(false);
        circleLine.SetActive(false);
    }

    public void drawHorizontal(){
        horizontalLine.SetActive(true);
        paintBrush.SetActive(true);
        paintBrush.transform.position = horizontalDots[0].position;
        currentDot = 0;
        currentlyDrawing = "horizontal";
    }
    public void drawXspell()
    {
        xspellLine.SetActive(true);
        paintBrush.SetActive(true);
        paintBrush.transform.position = xspellDots[0].position;
        currentDot = 0;
        currentlyDrawing = "xspell";
    }

    public void drawBox()
    {
        boxLine.SetActive(true);
        paintBrush.SetActive(true);
        paintBrush.transform.position = boxDots[0].position;
        currentDot = 0;
        currentlyDrawing = "box";
    }

    public void drawCircle()
    {
        circleLine.SetActive(true);
        paintBrush.SetActive(true);
        paintBrush.transform.position = circleDots[0].position;
        currentDot = 0;
        currentlyDrawing = "circle";
    }


    private void Update()
    {
        
        switch(currentlyDrawing)
        {
            case "horizontal":
                moveBrush(horizontalDots);
            break;

            case "xspell":
                moveBrush(xspellDots);
                break;

            case "box":
                moveBrush(boxDots);
                break;

            case "circle":
                moveBrush(circleDots);
                break;
        }

    }

    private void moveBrush(List<Transform> reference)
    {

        Transform nextPoint = reference[currentDot];
        paintBrush.transform.position = Vector3.MoveTowards(paintBrush.transform.position, nextPoint.position, 1.5f);

        if (Vector2.Distance(paintBrush.transform.position, nextPoint.position) < 0.2f)
        {
            if (currentDot >= reference.Count - 1)
            {
                currentDot = 0;
                paintBrush.transform.position = reference[0].position;
            }
            else
                currentDot++;
        }
    }

}
