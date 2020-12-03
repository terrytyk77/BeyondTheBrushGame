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

	public DrawingLocation(float smallerX, float biggerX, float smallerY, float biggerY)
	{
		//Set the points
		sX = smallerX;
		bX = biggerX;
		sY = smallerY;
		bY = biggerY;

		Camera cam = Camera.main;
		float height = cam.pixelHeight;
		float width = cam.pixelWidth;

		//Set the middle vector
		middle = new Vector2(((sX + bX) / 2) * width, ((sY + bY) / 2) * height);
	}

}

public class OnDrawEvent : MonoBehaviour
{
	//Variables||

		public GameObject line;
		public GameObject box;
		public GameObject stone;
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

			GameObject player = GameObject.FindWithTag("Player");
			BoxCollider2D playerCollider = player.GetComponent<BoxCollider2D>();

			switch (result.gesture.id)
			{
				case "Horizontal":
                    {
						Debug.Log("Horizontal");
						break;
                    }
				case "Circle":
                    {

						Vector2 worldPos = Camera.main.ScreenToWorldPoint(Location.middle);

						if (HoverPlayer(worldPos, player, playerCollider))
                        {
							Debug.Log("Shielded");
							break;
						}
                        else
                        {
							Instantiate(stone, worldPos, Quaternion.identity);
							Debug.Log(worldPos);
							Debug.Log(player.transform.position.x + playerCollider.size.x / 2);
							Debug.Log("Circle");
							break;
						}
					}
				case "Xspell":
					{
						Debug.Log("Xspell");
						break;
					}
				case "Square":
					{ 
						Vector2 worldPos = Camera.main.ScreenToWorldPoint(Location.middle);
						Instantiate(box, worldPos, Quaternion.identity);
						Debug.Log("Square");
						break;
					}
			}


		}
        else
        {
			Debug.Log("Doesn't know the shape");
        }

	}

	public bool HoverPlayer(Vector2 worldPos, GameObject player, BoxCollider2D playerCollider)
	{
		if(
			(worldPos.x < player.transform.position.x + (playerCollider.size.x / 2) * player.transform.localScale.x) &&
			(worldPos.x > player.transform.position.x - (playerCollider.size.x / 2) * player.transform.localScale.x) &&
			(worldPos.y < player.transform.position.y + (playerCollider.size.y / 2) * player.transform.localScale.y + (playerCollider.offset.y * player.transform.localScale.y)) &&
			(worldPos.y > player.transform.position.y - (playerCollider.size.y / 2) * player.transform.localScale.y + (playerCollider.offset.y * player.transform.localScale.y)))
		{
			return true;
        }
        else
        {
			return false;
        }

	}

}


