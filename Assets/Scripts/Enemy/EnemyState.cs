using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyState : Character
{
    enum State
    {
        Patrol,
        Chase,
        Search,
        Attack,
        Retreat,
    }

    private State curState = 0;

    public Controller controller;
    private MeshRenderer meshRenderer;

    [Header("Range")]
    [SerializeField] private float chaseRange;
    [SerializeField] private float attackRange;

    [Header("Patrol Settings")]
    [SerializeField] private Transform[] patrolWaypoints;
    [SerializeField] int waypointNumber;
    float distanceThreshold = 0.1f;

    [Header("Chase Settings")]
    [SerializeField] float chaseSpeed;

    [Header("Search Settings")]
    [SerializeField] private float searchTime;

    [Header("Attack Settings")]
    [SerializeField] private float timeBetweenAttacks;
    bool alreadyAttacked;

    [Header("Retreat")]
    [SerializeField] private int healthPanic;

    [SerializeField]private float targetDistance;
    NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = Player.current;
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        targetDistance = Vector3.Distance(transform.position, target.transform.position);

        UpdateState();
    }

    void UpdateState()
    {
        switch (curState)
        {
            case State.Patrol:
                PatrolState();
                break;
            case State.Chase:
                ChaseState();
                break;
            case State.Search:
                SearchState();
                break;
            case State.Attack:
                AttackState();
                break;
            case State.Retreat:
                RetreatState();
                break;
        }
    }

    void PatrolState()
    {
        meshRenderer.material.color = Color.blue;
        controller.MoveToPosition(patrolWaypoints[waypointNumber].position);

        if(Vector3.Distance(agent.transform.position, patrolWaypoints[waypointNumber].position) <= distanceThreshold)
        {
            waypointNumber++;
            if (waypointNumber == patrolWaypoints.Length)
                waypointNumber = 0;
        }

        if (targetDistance < chaseRange && targetDistance > attackRange)
            curState = State.Chase;
        if(targetDistance < attackRange)
            curState = State.Attack;
    }

    void ChaseState()
    {
        meshRenderer.material.color = new Color(1, 0.6f, 0);

        controller.MoveToTarget(target.transform);

        if(targetDistance < attackRange && targetDistance > chaseRange)
            curState = State.Attack;
        if (targetDistance > chaseRange)
            curState = State.Search;
        if (curHp < healthPanic)
            curState = State.Retreat;
    }

    void SearchState()
    {
        controller.StopMovement();
        meshRenderer.material.color = Color.grey;

        if (curHp < healthPanic)
            curState = State.Retreat;
    }

    void AttackState()
    {
        this.GetComponent<MeshRenderer>().material.color = Color.red;
        controller.StopMovement();

        if (targetDistance > attackRange)
            curState = State.Chase;

        if (curHp < healthPanic)
            curState = State.Retreat;
    }

    void RetreatState()
    {
        meshRenderer.material.color = Color.magenta;
        controller.StopMovement();
    }

}
