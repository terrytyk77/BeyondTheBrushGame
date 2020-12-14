using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float movementSpeed;

    //Ranges        ||
        public float aggroRange;
        public float chaseRange;
        public float minPatrolRange;
        public float maxPatrolRange;
    //--------------||

    //Enemy State   ||
    private enum State
    {
        Patrolling,
        Chassing,
        Resetting
    }
    private State currentState;
    //--------------||

    private GameObject player;
    private Vector2 startingPosition;
    private Vector2 patrollingPosition;
    private float distanceChangePatrol = 1f;


    // Start is called before the first frame update
    void Start()
    {
        currentState = State.Patrolling;
        startingPosition = transform.position;
        patrollingPosition = GetPatrollingPosition();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Get Player Position
        player = GameObject.FindGameObjectWithTag("Player");

        //State Machine
        switch (currentState)
        {
            default:
            case State.Patrolling:
                {
                    MoveTo(patrollingPosition);

                    if (Vector2.Distance(transform.position, patrollingPosition) < distanceChangePatrol)
                    {
                        //Reached Patrolling Position
                        patrollingPosition = GetPatrollingPosition();
                    }
                    FindTarget();
                    break;
                }
            case State.Chassing:
                {
                    MoveTo(player.transform.position);
                    if (Vector2.Distance(transform.position, startingPosition) > chaseRange)
                    {
                        currentState = State.Resetting;
                    }
                    break;
                }
            case State.Resetting:
                {
                    MoveTo(startingPosition);
                    if (Vector2.Distance(transform.position, startingPosition) < distanceChangePatrol)
                    {
                        currentState = State.Patrolling;
                    }
                    break;
                }
        }
    }

    Vector2 GetPatrollingPosition()
    {
        return startingPosition + GetRandomDirection() * Random.Range(minPatrolRange, maxPatrolRange);
    }

    Vector2 GetRandomDirection()
    {
        return new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
    }

    void MoveTo(Vector2 targetPosition)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
    }

    void FindTarget()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < aggroRange)
        {
            //Player within target Range!
            currentState = State.Chassing;
        }
    }
}
