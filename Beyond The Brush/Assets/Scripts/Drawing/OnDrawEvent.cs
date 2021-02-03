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
    public delegate void DrawnShape(string id);
    public static event DrawnShape shapeDrawn;

    //Variables||

		public GameObject line;
		public GameObject box;
		public GameObject stone;
		public GameObject player;
		public GameObject drawingCollider;
		public GameObject playerVertical;
		public GameObject playerHorizontal;
		public GameObject DamagePopUpPrefab;

		private Passives playerPassives;

		private UIsfx soundEffect;
		//_____||

	private void Start()
	 {
		playerPassives = player.GetComponent<Passives>();

		//Add the audio instance
		soundEffect = gameObject.GetComponentInParent<UIsfx>();
	}

    private void Update()
    {
		PlayerData.resetCooldowns();
	}

    public bool HoverPlayer(DrawingLocation location)
	{
		Vector2 worldPos = Camera.main.ScreenToWorldPoint(location.middle);
        Vector2 worldSmallPoint = Camera.main.ScreenToWorldPoint(new Vector2(location.sX, location.sY));
        Vector2 worldBigPoint = Camera.main.ScreenToWorldPoint(new Vector2(location.bX, location.bY));
		
		BoxCollider2D playerCollider = drawingCollider.GetComponent<BoxCollider2D>();
        Vector2 playerPos = player.transform.position;
        Vector2 playerSize = new Vector2(playerCollider.size.x * Mathf.Abs(player.transform.localScale.x) , (playerCollider.size.y + playerCollider.offset.y) * Mathf.Abs(player.transform.localScale.y));

		Rect drawingZone = Rect.MinMaxRect(worldSmallPoint.x, worldSmallPoint.y, worldBigPoint.x, worldBigPoint.y);
		Rect playerHitZone = Rect.MinMaxRect(playerPos.x - playerSize.x/2, playerPos.y - playerSize.y / 2, playerPos.x + playerSize.x / 2, playerPos.y + playerSize.y / 2);

		return
			drawingZone.Overlaps(playerHitZone);

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
					DamagePopUp.Create(DamagePopUpPrefab, enemy.transform.position, damage);
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
						soundEffect.breakChest(lightObject);
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
		newObject.AddComponent<ObjectDespawn>();
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
                            if(shapeDrawn != null)
                                shapeDrawn(result.gesture.id);

							soundEffect.slash();
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
                                if (shapeDrawn != null)
                                    shapeDrawn("shield");

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
                        else 
                        {
							if (PlayerData.rockCooldown <= 0)
                            {
                                if (shapeDrawn != null)
                                    shapeDrawn("rock");

								soundEffect.rockSpawn();
                                PlayerData.rockCooldown = PlayerData.rockSpawnCooldownDefault;
								SpawnObject(location, stone);
							}
						}
						break;
					}
				case "Xspell":
					{
						if (PlayerData.xslashCooldown <= 0)
						{
                            if (shapeDrawn != null)
                                shapeDrawn(result.gesture.id);

							soundEffect.xSlash();

                            switch (PlayerData.playerDirection)
                            {
								case 0:
                                    {
										playerVertical.GetComponent<Animator>().SetTrigger("XspellBack");
										break;
									}
								case 1:
                                    {
										playerVertical.GetComponent<Animator>().SetTrigger("XspellFront");
										break;
                                    }
								case 2:
                                    {
										playerHorizontal.GetComponent<Animator>().SetTrigger("Xspell");
										break;
                                    }
								case 3:
                                    {
										playerHorizontal.GetComponent<Animator>().SetTrigger("Xspell");
										break;
                                    }
                            }
	
							HoverEnemy(location, PlayerData.xslashDamage);
							HoverObject(location);
						}

						break;
					}
				case "Square":
					{
                        if (PlayerData.boxCooldown <= 0 && !HoverPlayer(location))
                        {
                            if (shapeDrawn != null)
                                shapeDrawn(result.gesture.id);
                            PlayerData.boxCooldown = PlayerData.boxSpawnCooldownDefault;
							SpawnObject(location, box);
                        }

						break;
					}
			}
		}
	}
}


