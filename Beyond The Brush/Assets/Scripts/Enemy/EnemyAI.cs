using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    //Stats       ||
    [Header("Enemy Stats")]
        public string enemyType;
        public int damage;
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
        public GameObject enemyProjectile;
    //--------------||

    //Enemy State   ||
    private enum State
    {
        Patrolling,
        Chassing,
        Attacking,
        Castting,
        Resetting
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

    // Start is called before the first frame update
    void Start()
    {
        collisionObject = GameObject.FindGameObjectWithTag("CollisionLayer");
        collisionTilemap = collisionObject.GetComponent<Tilemap>();
        HalfTile = new Vector3(collisionTilemap.cellSize.x / 2, collisionTilemap.cellSize.y / 2, 0);

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
                            getPath(player.transform.position);
                        }
                        else
                        {
                            if(pathIndex >= 2)
                            {
                                getPath(player.transform.position);
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
        if ((Vector3.Distance(transform.position, player.transform.position) < aggroRange) && (Vector3.Distance(transform.position, player.transform.position) > attackRange) && PlayerData.healthPoints > 0)
        {
            //Player within target Range!
            currentState = State.Chassing;
        }
    }
    
    private void TargetDead()
    {
        if (PlayerData.healthPoints <= 0)
        {
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

    public void getDamaged(float damage)
    {
        currentHealth -= damage;
        healthBar.GetComponent<Image>().fillAmount -= damage / maxHealth;

        if (currentHealth <= 0)
        {
            // Delay Death For Animation
            Invoke("Death", 0.2f);
        }
    }

    public void dealDamage()
    {
        PlayerData.healthPoints -= damage;
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
        PlayerData.addPlayerExp(experience);
        Destroy(gameObject);
    }

    private void createProjectile(Vector3 spawnPosition)
    {
        GameObject projectile = Instantiate(enemyProjectile, spawnPosition, Quaternion.identity);
        projectile.transform.SetParent(transform);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (currentPath != null)
        {
            for (int i = 0; i < currentPath.Count; i++)
            {
                if (i + 1 < currentPath.Count)
                    if (currentPath[i] != null)
                        Gizmos.DrawLine(currentPath[i] + HalfTile, currentPath[i + 1] + HalfTile);
            }
        }
    }
}
