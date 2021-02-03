using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    //Stats       ||
    [Header("Enemy Stats")]
    public string enemyType;
    public float damage;
    public int level;
    public float maxHealth;
    public int experience;
    public float walkingMovementSpeed;
    public float fleeingMovementSpeed;
    //--------------||

    //Ranges        ||
    [Header("Enemy Ranges")]
    public float aggroRange;
    public float chaseRange;
    public float attackRange;
    public float minPatrolRange;
    public float maxPatrolRange;
    //--------------||

    //Image         ||
    public GameObject healthBar;
    public Transform levelText;
    public GameObject enemyProjectile;
    //--------------||

    //Enemy State   ||
    private enum State
    {
        Patrolling,
        Chassing,
        Attacking,
        Castting,
        Resetting,
        Diying
    }
    private State currentState;
    //--------------||


    //Collision-----||
    private GameObject collisionObject;
    private Tilemap collisionTilemap;
    Vector3 HalfTile;
    //--------------||

    //Path----------||
    private Vector3 currentDestination = Vector3.zero;
    private Pathfinding pathfinding;
    private List<Vector3Int> currentPath = null;
    private int pathIndex = 0;
    //--------------||

    //Timer---------||
    private float CheckPlayerTimer = 0f;
    //--------------||

    private GameObject player;
    private Animator Animator;
    private Vector3 startingPosition;
    private Vector3 patrollingPosition;
    private Vector3 enemyDirection;
    private float currentMovementSpeed;
    private float currentHealth;
    private float distanceChangePatrol = 1f;
    private bool firing;
    private bool attackEnded;
    private bool castEnded;

    public float getHealth { get { return currentHealth; } }

    // Start is called before the first frame update
    void Start()
    {
        //Set Stats Depending on Level
        if(GameObject.FindGameObjectWithTag("proceduralData"))
            setStats();

        collisionObject = GameObject.FindGameObjectWithTag("CollisionLayer");
        collisionTilemap = collisionObject.GetComponent<Tilemap>();
        HalfTile = new Vector3(collisionTilemap.cellSize.x / 2, collisionTilemap.cellSize.y / 1.25f, 0);

        currentDestination = collisionTilemap.CellToWorld(collisionTilemap.WorldToCell(transform.position));
        pathfinding = new Pathfinding();

        Animator = gameObject.GetComponent<Animator>();
        currentMovementSpeed = walkingMovementSpeed;
        currentHealth = maxHealth;
        currentState = State.Patrolling;
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Get Player Position
        player = GameObject.FindGameObjectWithTag("Player");

        GetEnemyDirection();
        LayeringUpdate();

        //State Machine
        switch (currentState)
        {
            default:
            case State.Patrolling:
                {
                    if (currentPath == null)
                    {
                        getPath(GetPatrollingPosition());
                    }
                    else
                    {
                        MoveTo();
                    }

                    FindTarget();
                    break;
                }
            case State.Chassing:
                {
                    TargetDead();

                    if (Vector3.Distance(transform.position, player.transform.position) < attackRange)
                    {
                        currentPath = null;
                        if (enemyType == "Ranged")
                        {
                            currentState = State.Castting;
                        }
                        else
                        {
                            currentState = State.Attacking;
                        }
                    }
                    else
                    {
                        // Move to Player If Not in Range for Attack
                        if (currentPath == null)
                        {
                            getPath(player.transform.position - new Vector3(0, player.GetComponent<BoxCollider2D>().size.y/2 * player.transform.localScale.y, 0));
                        }
                        else
                        {
                            if(pathIndex >= 2)
                            {
                                getPath(player.transform.position - new Vector3(0, player.GetComponent<BoxCollider2D>().size.y / 2 * player.transform.localScale.y, 0));
                            }
                            MoveTo();
                        }
                        //Check if out of range!
                        OutOfChaseRange();
                    }
                    break;
                }
            case State.Castting:
                {
                    TargetDead();

                    Animator.SetBool("Walking", false);
                    Animator.SetTrigger("Castting");
                    currentState = State.Attacking;
                    break;
                }
            case State.Attacking:
                {
                    TargetDead();

                    Animator.SetBool("Walking", false);
                    if (enemyType == "Ranged")
                    {
                        if (castEnded)
                        {
                            if (firing)
                            {
                                castEnded = false;
                                firing = false;
                                createProjectile(transform.position);
                            }
                        }
                    }
                    else
                    {
                        Animator.SetTrigger("Attacking");
                    }

                    if (attackEnded)
                    {
                        attackEnded = false;
                        currentState = State.Chassing;
                    }
                    break;
                }
            case State.Resetting:
                {
                    if (currentPath == null)
                    {
                        getPath(startingPosition);
                        currentMovementSpeed = fleeingMovementSpeed;
                    }
                    else
                    {
                        MoveTo();
                        ResetHP();
                    }
                    if (Vector3.Distance(transform.position, startingPosition) < distanceChangePatrol)
                    {
                        Animator.SetBool("Walking", false);
                        currentMovementSpeed = walkingMovementSpeed;
                        currentState = State.Patrolling;
                    }
                    break;
                }
            case State.Diying:
                {
                    break;
                }
        }
    }

    private Vector3 GetPatrollingPosition()
    {
        return startingPosition + GetRandomDirection() * Random.Range(minPatrolRange, maxPatrolRange);
    }

    private Vector3 GetRandomDirection()
    {
        return new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), 0f).normalized;
    }

    private void getPath(Vector3 position)
    {
        if (collisionTilemap == null)
        {
            collisionObject = GameObject.FindGameObjectWithTag("CollisionLayer");
            collisionTilemap = collisionObject.GetComponent<Tilemap>();
        }

        TileBase tile = collisionTilemap.GetTile(collisionTilemap.WorldToCell(position));
        if (tile == null)
        {
            currentPath = pathfinding.FindPath(collisionTilemap, collisionTilemap.WorldToCell(transform.position), collisionTilemap.WorldToCell(position));
            if (currentPath.Count > 0)
            {
                pathIndex = 0;
                currentDestination = collisionTilemap.CellToWorld(currentPath[pathIndex]);
            }
        }
    }


    private void MoveTo()
    {
        //add movement
        if (pathIndex < currentPath.Count)
        {
            Vector3 targetPosition = new Vector3(currentDestination.x, currentDestination.y, transform.position.z) + HalfTile;
            enemyDirection.x = (targetPosition.x - transform.position.x);
            enemyDirection.y = (targetPosition.y - transform.position.y);
            Animator.SetBool("Walking", true);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentMovementSpeed * Time.deltaTime);
            if (Vector3.Distance(targetPosition, transform.position) < 0.05)
            {
                pathIndex++;
                if (pathIndex < currentPath.Count)
                {
                    currentDestination = collisionTilemap.CellToWorld(currentPath[pathIndex]);
                }
                else
                {
                    currentPath = null; 
                }
            }
        }
        else
        {
            currentPath = null;
        }

    }

    private void FindTarget()
    {
        if (enemyType == "Ranged")
        {
            if ((Vector3.Distance(transform.position, player.transform.position) < aggroRange) && (Vector3.Distance(transform.position, player.transform.position) > attackRange / 2) && PlayerData.healthPoints > 0)
            {
                //Player within target Range!
                currentState = State.Chassing;
            }
        }
        else if (enemyType == "Melee")
        {
            if ((Vector3.Distance(transform.position, player.transform.position) < aggroRange) && (Vector3.Distance(transform.position, player.transform.position) > attackRange) && PlayerData.healthPoints > 0)
            {
                //Player within target Range!
                currentState = State.Chassing;
            }
        }
        
    }
    
    private void TargetDead()
    {
        if (PlayerData.healthPoints <= 0 ||  player == null)
        {
            //Debug.Log(player);
            currentPath = null;
            currentState = State.Patrolling;
        }
    }


    private void OutOfChaseRange() {
        if (Vector3.Distance(transform.position, startingPosition) > chaseRange)
        {
            currentPath = null;
            currentState = State.Resetting;
        }
    }

    private void GetEnemyDirection()
    {
        if(currentState == State.Castting || currentState == State.Attacking)
        {
            enemyDirection.x = (player.transform.position.x - transform.position.x);
            enemyDirection.y = (player.transform.position.y - transform.position.y);
        }

        if (enemyDirection.x < 0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if(enemyDirection.x > 0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    public int getDamaged(float damage)
    {
        //Scale the damage depending on players level
        damage += damage * (0.1f * PlayerData.level);

        if (currentHealth > damage)
        {
            currentHealth -= damage;
            healthBar.GetComponent<Image>().fillAmount -= damage / maxHealth;
            return (int)damage;
        }
        else
        {
            float storeHealth = currentHealth;
            if(damage == PlayerData.xslashDamage)
            {
                player.GetComponent<Passives>().Overkill();
                player.GetComponent<Passives>().BattleThrist();
            }
            
            healthBar.GetComponent<Image>().fillAmount = 0;
            currentHealth = 0;
            Death();
            return (int)storeHealth;
        }

        

    }

    public void dealDamage()
    {
        //Send the damage request to the player
        PlayerData.damagePlayer((int)Mathf.Ceil(damage));
    }

    private void ResetHP()
    {
        if(currentHealth < maxHealth)
        {
            float hpTicks = 10f;
            currentHealth += hpTicks;
            healthBar.GetComponent<Image>().fillAmount += hpTicks / maxHealth;
        }
        else
        {
            currentHealth = maxHealth;
            healthBar.GetComponent<Image>().fillAmount = maxHealth / maxHealth;
        }
    }

    private void Death()
    {
       
        //Set New Tag, So Enemy Are Not Detected On Draw While Dead
        transform.gameObject.tag = "DeadEnemy";

        //Check if the room is complete
        if (gameObject.transform.parent.name == "Enemies")
        {
            gameObject.transform.parent.parent.GetComponent<DungeonChestSpawn>().checkIfRoomComplete();
        }

        //State to Dead
        currentState = State.Diying;

        //Adjusting Sorting Layer
        gameObject.GetComponent<SortingGroup>().sortingOrder = -10;

        //Runing Death Animation
        Animator.SetTrigger("Diying");

        //Removing HP Bar
         gameObject.transform.Find("Canvas").gameObject.SetActive(false);

        //Remove Box Collider
        gameObject.transform.Find("ActualHitBox").GetComponent<BoxCollider2D>().enabled = false;
        
        //Grant Exp to the Player
        PlayerData.addPlayerExp(experience);

        //Dispawn AI after 5s to 10s
        Invoke("RemoveAI", UnityEngine.Random.Range(5f, 10f));
    }

    private void RemoveAI()
    {
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }

    private void createProjectile(Vector3 spawnPosition)
    {
        GameObject projectile = Instantiate(enemyProjectile, spawnPosition, Quaternion.identity);
        projectile.transform.SetParent(transform);
    }

    private void LayeringUpdate()
    {
        if(currentHealth > 0)
        {
            if (gameObject.transform.position.y > player.transform.position.y)
            {
                gameObject.GetComponent<SortingGroup>().sortingOrder = -10;
            }
            else
            {
                gameObject.GetComponent<SortingGroup>().sortingOrder = 10;
            }
        }
    }

    private void Fire()
    {
        firing = true;
    }

    private void AttackEnded()
    {
        attackEnded = true;
    }

    private void CastEnded()
    {
        castEnded = true;
    }

    public void ResetAI()
    {
        currentHealth = maxHealth;
        healthBar.GetComponent<Image>().fillAmount = 1;
        transform.position = startingPosition;
        currentPath = null;
        currentState = State.Patrolling;
    }

    public void setStats()
    {
        float incrementEveryLevel = 0.1f;
        int amountOfRoomsCompleted = GameObject.FindGameObjectWithTag("proceduralData").GetComponent<CurrentDungeonData>().amountOfCompletedRooms;

        //Augment level every 3 room completed
        level += (int)amountOfRoomsCompleted / 3;

        //Change Level Text
        levelText.GetComponent<Text>().text = level.ToString();

        //Calculate Stats per level
        for(int i = 0; i < level - 1; i++)
        {
            damage += damage * incrementEveryLevel;
            maxHealth += maxHealth * incrementEveryLevel;
        }
    }

    private void OnDrawGizmos()
    {
        if(gameObject.tag != "DeadEnemy")
        {
            Gizmos.color = Color.green;

            if (currentPath != null)
            {
                for (int i = 0; i < currentPath.Count; i++)
                {
                    if (i + 1 < currentPath.Count)
                        if (currentPath[i] != null)
                            Gizmos.DrawLine(currentPath[i] + HalfTile, currentPath[i + 1] + HalfTile);
                }
            }


            //Radius For Detecting Player
            Gizmos.color = Color.red;
            float theta = 0;
            float x = aggroRange * Mathf.Cos(theta);
            float y = aggroRange * Mathf.Sin(theta);
            Vector3 pos = transform.position + new Vector3(x, y, 0);
            Vector3 newPos = pos;
            Vector3 lastPos = pos;
            for (theta = 0.1f; theta < Mathf.PI * 2; theta += 0.1f)
            {
                x = aggroRange * Mathf.Cos(theta);
                y = aggroRange * Mathf.Sin(theta);
                newPos = transform.position + new Vector3(x, y, 0);
                Gizmos.DrawLine(pos, newPos);
                pos = newPos;
            }
            Gizmos.DrawLine(pos, lastPos);
            //Gizmos.DrawWireSphere(transform.position, aggroRange);
        }
    }
}
