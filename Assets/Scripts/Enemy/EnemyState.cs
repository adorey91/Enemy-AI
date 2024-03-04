using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [SerializeField] TMP_Text stateText;
    private State curState = 0;

    public Controller controller;
    private MeshRenderer meshRenderer;
    public GameObject HealthBarUI;

    [Header("Range")]
    [SerializeField] private float chaseRange;
    [SerializeField] private float attackRange;

    [Header("Patrol Settings")]
    [SerializeField] private Transform[] patrolWaypoints;
    [SerializeField] int waypointNumber;
    float distanceThreshold = 0.1f;

    [Header("Search Settings")]
    [SerializeField] private float searchTime;
    public float searchRange;
    private float searchTimer;

    [Header("Attack Settings")]
    [SerializeField] private float timeBetweenAttacks;
    [SerializeField] GameObject attackPrefab;
    bool alreadyAttacked;

    [Header("Retreat")]
    [SerializeField] private int healthPanic;

    private float targetDistance;
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
        HealthBarUI.transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
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
        stateText.text = "Patroling";

        if (Vector3.Distance(agent.transform.position, patrolWaypoints[waypointNumber].position) <= distanceThreshold)
        {
            waypointNumber++;
            if (waypointNumber == patrolWaypoints.Length)
                waypointNumber = 0;
        }

        if (targetDistance < chaseRange && targetDistance > attackRange)
            curState = State.Chase;
        if (targetDistance < attackRange)
            curState = State.Attack;
    }

    void ChaseState()
    {
        controller.MoveToTarget(target.transform);
        meshRenderer.material.color = new Color(1, 0.6f, 0);
        stateText.text = "Chasing";

        if (targetDistance < attackRange)
            curState = State.Attack;
        else if (targetDistance > chaseRange)
            curState = State.Search;
        else if (curHp < healthPanic)
            curState = State.Retreat;

        searchTimer = searchTime;
    }

    void SearchState()
    {
        stateText.text = "Searching";
        meshRenderer.material.color = Color.grey;

        searchTimer -= Time.deltaTime;
        controller.SearchMovement();

        if (searchTimer <= 0f)
            curState = State.Patrol;
        if (targetDistance < chaseRange && targetDistance > attackRange)
            curState = State.Chase;
        if (targetDistance < attackRange)
            curState = State.Attack;
        if (curHp < healthPanic)
            curState = State.Retreat;
    }

    void AttackState()
    {
        this.GetComponent<MeshRenderer>().material.color = Color.red;
        stateText.text = "Attacking";
        controller.StopMovement();

        if(!alreadyAttacked)
        {
            Vector3 directionToTarget = target.transform.position - transform.position;
            directionToTarget.Normalize();
            Vector3 spawnPosition = transform.position + directionToTarget * 1f;

            GameObject proj = Instantiate(attackPrefab, spawnPosition, Quaternion.LookRotation(target.transform.position - transform.position));
            proj.GetComponent<Projectile>().Setup(this);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }


        if (targetDistance > chaseRange && targetDistance > attackRange)
            curState = State.Search;
        if (targetDistance > attackRange)
            curState = State.Chase;
        if (curHp < healthPanic)
            curState = State.Retreat;

    }

    void RetreatState()
    {
        meshRenderer.material.color = Color.magenta;
        controller.StopMovement();
        stateText.text = "Retreat";
    }

    void ResetAttack()
    {
        alreadyAttacked = false;
    }

}
