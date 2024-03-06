using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Controller : MonoBehaviour
{
    private NavMeshAgent agent;
    public Vector3 movePoint;
    public EnemyState enemyState;
    public bool movePointSet;

    private float searchRange;
    private float enemyDistanceRun;
    private float targetDistance;

    public void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        searchRange = enemyState.searchRange;
        enemyDistanceRun = enemyState.enemyDistanceRun;
        targetDistance = enemyState.targetDistance;
    }

    public void LookTowards(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction);
    }

    public void MoveToTarget(Transform target)
    {
        MoveToPosition(target.position);
    }

    public void MoveToPosition(Vector3 position)
    {
        agent.isStopped = false;
        agent.SetDestination(position);
        LookTowards(position);
    }

    public void StopMovement()
    {
        agent.isStopped = true;
    }

    public void RunAway(Transform target)
    {
        if (targetDistance < enemyDistanceRun)
        {
            Vector3 dirToPlayer = transform.position - target.position;
            Vector3 newPos = transform.position + dirToPlayer.normalized * enemyDistanceRun;
            Debug.Log("New position: " + newPos.ToString());
            MoveToPosition(newPos);
        }
        else
        {
            Debug.Log("Not running away - target distance is greater than enemyDistanceRun");
        }
    }

    public void SearchMovement()
    {
        if (!movePointSet || Vector3.Distance(transform.position, movePoint) < 1f)
            SearchPoint();
        if (movePointSet)
            MoveToPosition(movePoint);
    }

    private void SearchPoint()
    {
        float randomZ = Random.Range(-searchRange, searchRange);
        float randomX = Random.Range(-searchRange, searchRange);

        movePoint = new Vector3(transform.position.x + randomX, 0, transform.position.z + randomZ);

        if (Physics.Raycast(movePoint, -transform.up, 2f))
            movePointSet = true;
    }
}
