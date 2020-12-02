﻿using System.Collections;
using System.Collections.Generic;
using GestureRecognizer;
using UnityEngine;


public class DrawingLocation{

	//Public variables
	public float sX;
	public float bX;
	public float sY;
	public float bY;
	public Vector2 middle;

	public DrawingLocation(float sX2, float bX2, float sY2, float bY2)
	{
		//Set the points
		sX = sX2;
		bX = bX2;
		sY = sY2;
		bY = bY2;

		Camera cam = Camera.main;
		float height = cam.pixelHeight;
		float width = cam.pixelWidth;

		//Set the middle vector
		middle = new Vector2(((sX2 + bX2) / 2) * width, ((sY2 + bY2) / 2) * height);
	}

}

public class OnDrawEvent : MonoBehaviour
{
	//Variables||

		public GameObject line;
		public GameObject box;
	//_________||


	public DrawingLocation GetDrawingMiddle(UILineRenderer lineData)
    {
		//Check the position of the drawing
		float biggerX = 0f;
		float smallerX = 1f;
		float biggerY = 0f;
		float smallerY = 1f;

		foreach (Vector2 point in lineData.Points)
		{
			//X axis
			if (point.x > biggerX)
			{
				biggerX = point.x;
			}

			if (point.x < smallerX)
			{
				smallerX = point.x;
			}

			//Y axis
			if (point.y > biggerY)
			{
				biggerY = point.y;
			}

			if (point.y < smallerY)
			{
				smallerY = point.y;
			}
		}
			return new DrawingLocation(biggerX, smallerX, biggerY, smallerY);
	}



	public void OnRecognize(RecognitionResult result)
	{
		StopAllCoroutines();

		
		//Handle the shape
		if (result != RecognitionResult.Empty)
		{
			UILineRenderer lineData = line.gameObject.GetComponent<UILineRenderer>();
  
			DrawingLocation Location = GetDrawingMiddle(lineData);

			switch (result.gesture.id)
			{
				case "horizontal":
                    {
						break;
                    }
				case "circle":
                    {
						break;
                    }
				case "Xspell":
					{
						break;
					}
				case "Square":
					{ 
						Vector2 worldPos = Camera.main.ScreenToWorldPoint(Location.middle);
						Instantiate(box, worldPos, Quaternion.identity);
						break;
					}
			}


		}
        else
        {
			Debug.Log("Doesn't know the shape");
        }

	}

}
