using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private float targetDistance;

    private void Start()
    {
        
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
