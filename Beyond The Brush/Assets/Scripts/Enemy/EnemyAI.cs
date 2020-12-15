using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    //Stats       ||
    [Header("Enemy Stats")]
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
    //--------------||

    //Enemy State   ||
    private enum State
    {
        Patrolling,
        Chassing,
        Attacking,
        Resetting
    }
    private State currentState;
    //--------------||

    private GameObject player;
    private Animator Animator;
    private Vector2 startingPosition;
    private Vector2 patrollingPosition;
    private Vector2 enemyDirection;
    private float currentHealth;
    private float distanceChangePatrol = 1f;


    // Start is called before the first frame update
    void Start()
    {
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
                    MoveTo(patrollingPosition);
                    if (Vector2.Distance(transform.position, patrollingPosition) < distanceChangePatrol)
                    {
                        //Reached Patrolling Position? Get a New One!
                        patrollingPosition = GetPatrollingPosition();
                    }
                    FindTarget();
                    break;
                }
            case State.Chassing:
                {
                    MoveTo(player.transform.position);
                    if (Vector2.Distance(transform.position, player.transform.position) < attackRange) 
                    {
                        currentState = State.Attacking;
                    }
                    OutOfChaseRange();
                    break;
                }
            case State.Attacking:
                {
                    Animator.SetTrigger("Attacking");
                    Animator.SetBool("Walking", false);
                    FindTarget();
                    OutOfChaseRange();
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
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
        enemyDirection.x = (targetPosition.x - transform.position.x);
        enemyDirection.y = (targetPosition.y - transform.position.y);
        Animator.SetBool("Walking", true);
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
            Invoke("death", 0.2f);
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

    private void death()
    {
        Destroy(gameObject);
        Debug.Log("Enemy Killed!");
    }
}
