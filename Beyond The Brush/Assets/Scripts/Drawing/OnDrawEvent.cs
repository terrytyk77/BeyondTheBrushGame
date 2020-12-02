using System.Collections;
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
		float height = cam.pixelWidth;
		float width = cam.pixelHeight;

		Debug.Log(height + " " + width);

		//Set the middle vector
		middle = new Vector2(((sX2 + bX2) * width) / 2, ((sY2 + bY2) * height) / 2);
	}

}

public class OnDrawEvent : MonoBehaviour
{
	//Variables||

		public GameObject line;
		public GameObject rock;
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

            if (result.gesture.id == "Square"){

				Vector2 worldPos = Camera.main.ScreenToWorldPoint(Location.middle);

				Debug.Log(worldPos.x + " , " + worldPos.y);
				//TODO
				Instantiate(	rock, worldPos, Quaternion.identity);
            }

		}
        else
        {
			Debug.Log("Doesn't know the shape");
        }

	}

}
