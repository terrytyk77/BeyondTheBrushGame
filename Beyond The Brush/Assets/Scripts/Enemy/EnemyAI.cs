﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    //Stats       ||
    [Header("Enemy Stats")]
        public string enemyType;
        public float maxHealth;
        public float movementSpeed;
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

    private Pathfinding pathfinding;
    private int pathIndex = 0;
    private List<Vector3Int> currentPath = null;
    private Vector2 currentDestination = Vector2.zero;

    //Collision-----||
    private GameObject collisionObject;
    private Tilemap collisionTilemap;
    private TileBase tile;
    //--------------||

    private GameObject player;
    private Animator Animator;
    private Vector2 startingPosition;
    private Vector2 patrollingPosition;
    private Vector2 enemyDirection;
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
        pathfinding = new Pathfinding();
        Animator = gameObject.GetComponent<Animator>();
        currentHealth = maxHealth;
        currentState = State.Patrolling;
        startingPosition = transform.position;
        patrollingPosition = GetPatrollingPosition();
    }

    // Update is called once per frame
    void FixedUpdate()
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
                FindTarget();
                MoveTo(patrollingPosition);
                if (Vector2.Distance(transform.position, patrollingPosition) < distanceChangePatrol || tile != null || currentPath == null || currentPath.Count <= 0)
                {
                    //Reached Patrolling Position? Get a New One!
                    patrollingPosition = GetPatrollingPosition();
                        Debug.Log(patrollingPosition);
                }
                break;
            }
        case State.Chassing:
            {
                if (Vector2.Distance(transform.position, player.transform.position) < attackRange) 
                {
                    if(enemyType == "Ranged")
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
                    MoveTo(player.transform.position);
                    OutOfChaseRange();
                }
                break;
            }
        case State.Castting:
            {
                Animator.SetBool("Walking", false);
                Animator.SetTrigger("Castting");
                currentState = State.Attacking;
                break;
            }
        case State.Attacking:
            {
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
                MoveTo(startingPosition);
                ResetHP();
                if (Vector2.Distance(transform.position, startingPosition) < distanceChangePatrol)
                {
                    Animator.SetBool("Walking", false);
                    currentState = State.Patrolling;
                }
                break;
            }
        }
    }

    private Vector2 GetPatrollingPosition()
    {
        return startingPosition + GetRandomDirection() * Random.Range(minPatrolRange, maxPatrolRange);
    }

    private Vector2 GetRandomDirection()
    {
        return new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
    }

    private void MoveTo(Vector2 targetPosition)
    {
        
        //transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);

        if (currentPath != null)
        {
            if (pathIndex < currentPath.Count)
            {
                Vector2 targetPositionx = currentDestination;
                transform.position = Vector2.MoveTowards(transform.position, targetPositionx, movementSpeed * Time.deltaTime);
                if (Vector3.Distance(targetPositionx, transform.position) < 0.05)
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

            enemyDirection.x = (targetPosition.x - transform.position.x);
            enemyDirection.y = (targetPosition.y - transform.position.y);
            Animator.SetBool("Walking", true);
        }

        if(currentPath == null)
        {
            tile = collisionTilemap.GetTile(collisionTilemap.WorldToCell(targetPosition));

            if (tile == null)
            {
                Debug.Log("Nowhere to move");
                currentPath = pathfinding.FindPath(collisionTilemap,
                collisionTilemap.WorldToCell(transform.position),
                collisionTilemap.WorldToCell(targetPosition));
                if (currentPath.Count > 0)
                {
                    pathIndex = 0;
                    currentDestination = collisionTilemap.CellToWorld(currentPath[pathIndex]);
                }
            }
        }
    }

    private void FindTarget()
    {
        if ((Vector2.Distance(transform.position, player.transform.position) < aggroRange) && (Vector2.Distance(transform.position, player.transform.position) > attackRange))
        {
            //Player within target Range!
            currentState = State.Chassing;
        }
    }

    private void OutOfChaseRange() {
        if (Vector2.Distance(transform.position, startingPosition) > chaseRange)
        {
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

    public void damage(float damage)
    {
        currentHealth -= damage;
        healthBar.GetComponent<Image>().fillAmount -= damage / maxHealth;

        if (currentHealth <= 0)
        {
            // Delay Death For Animation
            Invoke("Death", 0.2f);
        }
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
        Destroy(gameObject);
        Debug.Log("Enemy Killed!");
    }

    private void createProjectile(Vector2 spawnPosition)
    {
        Instantiate(enemyProjectile, spawnPosition, Quaternion.identity);
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
}
