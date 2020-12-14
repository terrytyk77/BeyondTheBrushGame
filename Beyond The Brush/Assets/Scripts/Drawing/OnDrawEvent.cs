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

	public DrawingLocation(float biggerX, float smallerX, float biggerY, float smallerY)
	{
		//Set the points
		sX = smallerX;
		bX = biggerX;
		sY = smallerY;
		bY = biggerY;

		//Set the middle vector
		middle = new Vector2(((sX + bX) / 2), ((sY + bY) / 2));
	}

}

public class OnDrawEvent : MonoBehaviour
{
	//Variables||

		public GameObject line;
		public GameObject box;
		public GameObject stone;
		public GameObject player;
		public GameObject drawingCollider;
    //_________||


    private void Update()
    {
		PlayerData.resetCooldowns();
	}

    public bool HoverPlayer(DrawingLocation location)
	{
		Vector2 worldPos = Camera.main.ScreenToWorldPoint(location.middle);
		BoxCollider2D playerCollider = drawingCollider.GetComponent<BoxCollider2D>();

		if (
			(worldPos.x < player.transform.position.x + (playerCollider.size.x / 2) * player.transform.localScale.x) &&
			(worldPos.x > player.transform.position.x - (playerCollider.size.x / 2) * player.transform.localScale.x) &&
			(worldPos.y < player.transform.position.y + (playerCollider.size.y / 2 + playerCollider.offset.y) * player.transform.localScale.y) &&
			(worldPos.y > player.transform.position.y - (playerCollider.size.y / 2 + playerCollider.offset.y) * player.transform.localScale.y))
		{
			return true;
		}
		else
		{
			return false;
		}

	}

	public void HoverEnemy(DrawingLocation location, int damage)
    {
		var enemies = GameObject.FindGameObjectsWithTag("Enemy");
		Vector2 worldSmallPoint = Camera.main.ScreenToWorldPoint(new Vector2(location.sX, location.sY));
		Vector2 worldBigPoint = Camera.main.ScreenToWorldPoint(new Vector2(location.bX, location.bY));

		foreach (var enemy in enemies)
		{
			BoxCollider2D enemyCollider = enemy.GetComponent<BoxCollider2D>();
			Vector2 enemyCorner = new Vector2(enemy.transform.position.x - (enemyCollider.size.x / 2 * enemy.transform.localScale.x), enemy.transform.position.y - (enemyCollider.size.y / 2 * enemy.transform.localScale.y));

			Rect drawingZone = Rect.MinMaxRect(worldSmallPoint.x, worldSmallPoint.y, worldBigPoint.x, worldBigPoint.y);
			Rect enemyHitZone = new Rect(enemyCorner.x, enemyCorner.y, enemyCollider.size.x * enemy.transform.localScale.x, enemyCollider.size.y * enemy.transform.localScale.y);


			if (drawingZone.Overlaps(enemyHitZone))
            {
				enemy.GetComponent<Enemy>().damage(damage);
			}
		}
	}

	public void SpawnObject(DrawingLocation location, GameObject prefab)
    {
		Vector2 worldPos = Camera.main.ScreenToWorldPoint(location.middle);
		Instantiate(prefab, worldPos, Quaternion.identity);
	}

	public DrawingLocation GetDrawingMiddle(UILineRenderer lineData)
    {
		//Check the position of the drawing
		float biggerX = 0f;
		float smallerX = 1f;
		float biggerY = 0f;
		float smallerY = 1f;

		Camera cam = Camera.main;
		float height = cam.pixelHeight;
		float width = cam.pixelWidth;

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
			return new DrawingLocation(biggerX * width, smallerX * width, biggerY * height, smallerY * height);
	}


	public void OnRecognize(RecognitionResult result)
	{
		StopAllCoroutines();

		
		//Handle the shape
		if (result != RecognitionResult.Empty)
		{
			UILineRenderer lineData = line.gameObject.GetComponent<UILineRenderer>();
			DrawingLocation location = GetDrawingMiddle(lineData);


			switch (result.gesture.id)
			{
				case "Horizontal":
                    {
                        if (PlayerData.slashCooldown <= 0)
                        {
							Debug.Log("Slash");
							HoverEnemy(location, 20);
							PlayerData.slashCooldown = PlayerData.slashCooldownDefault;
                        }
	
						break;
                    }
				case "Circle":
                    {
						if (HoverPlayer(location))
                        {
                            if (PlayerData.shieldCooldown <= 0)
                            {
								Debug.Log("Shield");
								PlayerData.shieldCooldown = PlayerData.shieldCooldownDefault;
							}

						}
                        else
                        {
							Debug.Log("Stone");
							SpawnObject(location, stone);
						}
						break;
					}
				case "Xspell":
					{
						if (PlayerData.xslashCooldown <= 0)
						{
							Debug.Log("Xspell");
							HoverEnemy(location, 50);
							PlayerData.xslashCooldown = PlayerData.xslashCooldownDefault;
						}

						break;
					}
				case "Square":
					{
						Debug.Log("Box");
						SpawnObject(location, box);
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


