using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private State curState = 0;

    public Controller controller;

    [Header("Ranges")]
    [SerializeField] private float chaseRange;
    [SerializeField] private float attackRange;
    [SerializeField] private float sightRange;

    [Header("Attack")]
    [SerializeField] private float attackRate;
    private float lastAttackTime;
    bool alreadyAttacked;

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
        target = Player.current;
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
        this.GetComponent<MeshRenderer>().material.color = Color.blue;
        controller.MoveToPosition(goal[goalNumber].position);

        if(Vector3.Distance(agent.transform.position, goal[goalNumber].position) <= distanceThreshold )
        {
            goalNumber++;
            if(goalNumber == goal.Length)
                goalNumber = 0;
        }

        if (targetDistance < chaseRange && targetDistance > attackRange)
            curState = State.chasing;
        if (targetDistance < attackRange)
            curState = State.attack;

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
