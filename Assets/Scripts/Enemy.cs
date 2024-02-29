using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    enum State
    {
        patrol,
        chasing,
        search,
        attack,
        retreat,
    }
    private State curState;

    [Header("Ranges")]
    [SerializeField] private float chaseRange;
    [SerializeField] private float attackRange;

    [Header("Attack")]
    [SerializeField] private float attackRate;
    private float lastAttackTime;

    [Header("Searching")]
    [SerializeField] private float searchTime;

    [Header("Retreat")]
    [SerializeField] int healthPanic;

    [Header("Patrol")]
    public Transform[] goal;
    public int goalNumber = 0;
    float distanceThreshold = 0.1f;

    private float targetDistance;
    NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }


    private void Update()
    {
        switch (curState)
        {
            case State.patrol:
                PatrolState();
                break;
            case State.chasing:
                ChasingState();
                break;
            case State.search:
                SearchingState();
                break;
            case State.attack:
                AttackState();
                break;
            case State.retreat:
                RetreatState();
                break;

        }
    }

    void PatrolState()
    {
        agent.destination = goal[goalNumber].position;

        if(Vector3.Distance(agent.transform.position, goal[goalNumber].position) <= distanceThreshold )
        {
            goalNumber++;
            if(goalNumber == goal.Length)
                goalNumber = 0;
        }

    }

    void ChasingState()
    {

    }

    void SearchingState()
    {

    }

    void AttackState()
    {

    }

    void RetreatState()
    {

    }
}
