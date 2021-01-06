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
		public GameObject playerVertical;
		public GameObject playerHorizontal;

		private Passives playerPassives;
	//_________||

	private void Start()
    {
		playerPassives = player.GetComponent<Passives>();
	}

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
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		Vector2 worldSmallPoint = Camera.main.ScreenToWorldPoint(new Vector2(location.sX, location.sY));
		Vector2 worldBigPoint = Camera.main.ScreenToWorldPoint(new Vector2(location.bX, location.bY));
		Rect drawingZone = Rect.MinMaxRect(worldSmallPoint.x, worldSmallPoint.y, worldBigPoint.x, worldBigPoint.y);

		if (damage == PlayerData.slashDamage)
		{
			PlayerData.slashCooldown = PlayerData.slashCooldownDefault;
		}
		else if (damage == PlayerData.xslashDamage)
		{
			PlayerData.xslashCooldown = PlayerData.xslashCooldownDefault;
		}

		if (enemies.Length != 0)
        {
			foreach (GameObject enemy in enemies)
			{
				BoxCollider2D enemyCollider = enemy.GetComponent<BoxCollider2D>();
				Vector2 enemyCorner = new Vector2(enemy.transform.position.x - (enemyCollider.size.x / 2 * enemy.transform.localScale.x), enemy.transform.position.y - (enemyCollider.size.y / 2 * enemy.transform.localScale.y));
				Rect enemyHitZone = new Rect(enemyCorner.x, enemyCorner.y, enemyCollider.size.x * enemy.transform.localScale.x, enemyCollider.size.y * enemy.transform.localScale.y);

				if (drawingZone.Overlaps(enemyHitZone))
				{
					if(damage == PlayerData.slashDamage)
                    {
						playerPassives.FlashStrike();
						playerPassives.ToArms();
						playerPassives.DemandForAction(damage);
					}
					enemy.GetComponent<EnemyAI>().getDamaged(damage);
                }
			}
		}
	}

	public void HoverObject(DrawingLocation location)
    {
		Vector2 worldSmallPoint = Camera.main.ScreenToWorldPoint(new Vector2(location.sX, location.sY));
		Vector2 worldBigPoint = Camera.main.ScreenToWorldPoint(new Vector2(location.bX, location.bY));
		Rect drawingZone = Rect.MinMaxRect(worldSmallPoint.x, worldSmallPoint.y, worldBigPoint.x, worldBigPoint.y);

		GameObject[] LightObjects = GameObject.FindGameObjectsWithTag("LightObject");
	
		if (LightObjects.Length != 0)
		{
			foreach (GameObject lightObject in LightObjects)
			{
				BoxCollider2D lightObjectCollider = lightObject.GetComponent<BoxCollider2D>();
				Vector2 lightObjectCorner = new Vector2(lightObject.transform.position.x - (lightObjectCollider.size.x / 2 * lightObject.transform.localScale.x), lightObject.transform.position.y - (lightObjectCollider.size.y / 2 * lightObject.transform.localScale.y));

				
				Rect lightObjectHitZone = new Rect(lightObjectCorner.x, lightObjectCorner.y, lightObjectCollider.size.x * lightObject.transform.localScale.x, lightObjectCollider.size.y * lightObject.transform.localScale.y);


				if (drawingZone.Overlaps(lightObjectHitZone))
				{
					if (lightObject.GetComponent<Chest>() == null)
					{
						Destroy(lightObject);
					}
					else
					{
						lightObject.GetComponent<Chest>().DestroyChest();
					}
				}
			}
		}

		if(PlayerData.talentTreeData.node2 == true)
        {
			GameObject[] HeavyObjects = GameObject.FindGameObjectsWithTag("HeavyObject");
			if (HeavyObjects.Length != 0)
			{
				foreach (GameObject heavyObject in HeavyObjects)
				{
					BoxCollider2D heavyObjectCollider = heavyObject.GetComponent<BoxCollider2D>();
					Vector2 heavyObjectCorner = new Vector2(heavyObject.transform.position.x - (heavyObjectCollider.size.x / 2 * heavyObject.transform.localScale.x), heavyObject.transform.position.y - (heavyObjectCollider.size.y / 2 * heavyObject.transform.localScale.y));
					Rect heavyObjectHitZone = new Rect(heavyObjectCorner.x, heavyObjectCorner.y, heavyObjectCollider.size.x * heavyObject.transform.localScale.x, heavyObjectCollider.size.y * heavyObject.transform.localScale.y);

					if (drawingZone.Overlaps(heavyObjectHitZone))
					{
                        if (heavyObject.GetComponent<Chest>() == null)
                        {
							Destroy(heavyObject);
                        }
                        else
                        {
							heavyObject.GetComponent<Chest>().DestroyChest();
						}
					}
				}
			}
		}
	}

	public void SpawnObject(DrawingLocation location, GameObject prefab)
    {
		GameObject newObject;
		GameObject SpawnedObjectParent = GameObject.Find("SpawnedObjects");
		
		Vector2 worldPos = Camera.main.ScreenToWorldPoint(location.middle);
		newObject = Instantiate(prefab, worldPos, Quaternion.identity);

		//Storing The Spawbed Objects in the Spawned Parent Object that exist in each room
		newObject.transform.SetParent(SpawnedObjectParent.transform);
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
                        if (PlayerData.slashCooldown <= 0 && result.score.score >= 0.8f)
                        {

							playerHorizontal.GetComponent<Animator>().SetTrigger("Slash");
							playerVertical.GetComponent<Animator>().SetTrigger("Slash");
							HoverEnemy(location, PlayerData.slashDamage);
						}
	
						break;
                    }
				case "Circle":
                    {
						if (HoverPlayer(location))
                        {
                            if (result.score.score >= 0.7f && PlayerData.shieldCurrentStack > 0)
                            {
								playerHorizontal.GetComponent<Animator>().SetTrigger("Shield");
								playerVertical.GetComponent<Animator>().SetTrigger("Shield");
								if(PlayerData.shieldCurrentStack >= PlayerData.shieldMaxStack)
                                {
									PlayerData.shieldCooldown = PlayerData.shieldCooldownDefault;
									PlayerData.shieldTimer = PlayerData.shieldTimerDefault;
									PlayerData.shieldCurrentStack--;
                                }
                                else if(PlayerData.shieldCurrentStack < PlayerData.shieldMaxStack)
                                {
									PlayerData.shieldTimer = PlayerData.shieldTimerDefault;
									PlayerData.shieldCurrentStack--;
								}
							}
						}
                        else if(PlayerData.rockCooldown <= 0)
                        {
							PlayerData.rockCooldown = PlayerData.rockSpawnCooldownDefault;
							Debug.Log("Stone");
							SpawnObject(location, stone);
						}
						break;
					}
				case "Xspell":
					{
						if (PlayerData.xslashCooldown <= 0)
						{
                            if (playerHorizontal.transform.localScale.x == 1)
                            {
								playerHorizontal.GetComponent<Animator>().SetTrigger("Xspell");
							}
                            else if(playerVertical.transform.localScale.x == 1)
                            {
								playerVertical.GetComponent<Animator>().SetTrigger("Xspell");
							}
							HoverEnemy(location, PlayerData.xslashDamage);
							HoverObject(location);
						}

						break;
					}
				case "Square":
					{
                        if (PlayerData.boxCooldown <= 0)
                        {
							PlayerData.boxCooldown = PlayerData.boxSpawnCooldownDefault;
							Debug.Log("Box");
							SpawnObject(location, box);
                        }

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


