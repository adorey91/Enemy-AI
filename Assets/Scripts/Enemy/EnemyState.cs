using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyState : Character
{
    public enum State
    {
        Patrol,
        Chase,
        Search,
        Attack,
        Retreat,
        RunAway,
    }

    private State enemyState;
    [SerializeField] float moveSpeed;
    [SerializeField] TMP_Text stateText;

    private Controller controller;
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
    public float runSpeed;
    public float enemyDistanceRun;

    public float targetDistance;
    NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = Player.current;
        meshRenderer = GetComponent<MeshRenderer>();
        controller = GetComponent<Controller>();
        agent.speed = moveSpeed;
    }

    private void Update()
    {
        targetDistance = Vector3.Distance(transform.position, target.transform.position);

        HealthBarUI.transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);

        UpdateState();
    }

    void UpdateState()
    {
        if (curHp < healthPanic)
            State.RunAway;
        else
        {
            switch (enemyState)
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
    }

    void PatrolState()
    {
        agent.speed = moveSpeed;
        meshRenderer.material.color = Color.blue;
        controller.MoveToPosition(patrolWaypoints[waypointNumber].position);
        stateText.text = "Patroling";

        if (Vector3.Distance(agent.transform.position, patrolWaypoints[waypointNumber].position) <= distanceThreshold)
        {
            waypointNumber++;
            if (waypointNumber == patrolWaypoints.Length)
                waypointNumber = 0;
        }

        if (targetDistance < chaseRange)
            enemyState = State.Chase;
    }

    void ChaseState()
    {
        agent.speed = moveSpeed;
        controller.MoveToTarget(target.transform);
        meshRenderer.material.color = new Color(1, 0.6f, 0);
        stateText.text = "Chasing";

        if (targetDistance < attackRange)
            enemyState = State.Attack;
        else if (targetDistance > chaseRange)
            enemyState = State.Search;
        else if (curHp < healthPanic)
            enemyState = State.Retreat;

        searchTimer = searchTime;
    }

    void SearchState()
    {
        agent.speed = moveSpeed;
        stateText.text = "Searching";
        meshRenderer.material.color = Color.grey;

        searchTimer -= Time.deltaTime;
        controller.SearchMovement();

        if (searchTimer <= 0f)
            enemyState = State.Patrol;
        if (targetDistance < chaseRange && targetDistance > attackRange)
            enemyState = State.Chase;
        if (targetDistance < attackRange)
            enemyState = State.Attack;
        if (curHp < healthPanic)
            enemyState = State.Retreat;
    }

    void AttackState()
    {
        agent.speed = moveSpeed;
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
            enemyState = State.Search;
        if (targetDistance > attackRange)
            enemyState = State.Chase;
        if (curHp < healthPanic)
            enemyState = State.Retreat;

    }

    void RetreatState()
    {
        agent.speed = moveSpeed * runSpeed;
        meshRenderer.material.color = Color.magenta;
        controller.RunAway(target.transform);
        stateText.text = "Retreat";
    }

    void RunAway()
    {
        agent.speed = moveSpeed * runSpeed;
        meshRenderer.material.color = new Color(0.6f, 0.3f, 0);
        controller.RunAway(target.transform);
        stateText.text = "Run Away";
    }

    void ResetAttack()
    {
        alreadyAttacked = false;
    }

}
